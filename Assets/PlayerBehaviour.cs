using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Bolt;
using TMPro;

public class PlayerBehaviour : EntityBehaviour<IPlayerState>
{
    public Camera firstPersonCamera;
    public CharacterController character;
    public TMP_Text nameplate;
    public float moveSpeed = 10;

    private float xRotation = 0;

    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);

        if (entity.IsOwner)
        {
            firstPersonCamera.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;

            state.Name = PlayerPrefs.GetString("name", "Player Name");
            state.Color = Color.HSVToRGB(Random.value, 1.0f, 0.5f);
        }

        state.AddCallback("Name", NameChanged);
        state.AddCallback("Color", ColorChanged);
    }

    public override void SimulateOwner()
    {

        character.Move((
                transform.right * walkVector.x
                + transform.forward * walkVector.y
            ).normalized
            * moveSpeed
            * Time.deltaTime);

        xRotation -= lookVector.y * 0.5f;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        firstPersonCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * lookVector.x);
    }

    private Vector2 walkVector;
    public void OnWalk(InputAction.CallbackContext context)
    {
        walkVector = context.ReadValue<Vector2>();
    }

    private Vector2 lookVector;
    public void OnLook(InputAction.CallbackContext context)
    {
        lookVector = context.ReadValue<Vector2>();
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    void NameChanged()
    {
        nameplate.text = state.Name;
    }

    void ColorChanged()
    {
        GetComponent<Renderer>().material.color = state.Color;
        nameplate.color = state.Color;
    }

    void OnGUI()
    {
        if (entity.IsOwner)
        {
            GUI.color = state.Color;
            GUILayout.Label(state.Name);
            GUI.color = Color.white;
        }
    }
}
