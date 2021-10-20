using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Bolt;
using TMPro;
using System;
using System.Linq;

public class PlayerBehaviour : EntityBehaviour<IPlayerState>
{
    public static Vector3 spawnPosition = new Vector3(0, 1, 0);
    public static Quaternion spawnRotation = Quaternion.identity;

    public Camera firstPersonCamera;
    public CharacterController character;
    public TMP_Text nameplate;
    public Transform groundCheck;
    public LayerMask groundMask;

    public float moveSpeed;
    public float gravity;
    public float groundDistance;
    public float jumpHeight;
    public float lookSpeed;

    private float xRotation;
    private Vector3 velocity;
    private bool isGrounded;

    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);

        if (entity.IsOwner)
        {
            firstPersonCamera.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;

            state.Name = PlayerPrefs.GetString("name", "Player Name");
            state.Color = Color.HSVToRGB(UnityEngine.Random.value, 1.0f, 0.5f);
        }

        state.AddCallback("Name", NameChanged);
        state.AddCallback("Color", ColorChanged);
    }

    public override void SimulateOwner()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        var moveVector =
            transform.right * walkVector.x
            + transform.forward * walkVector.y;

        velocity.y += gravity * Time.deltaTime;

        if (isJumping && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        character.Move(
            (moveVector.normalized * moveSpeed * Time.deltaTime) + (velocity * Time.deltaTime)
        );

        xRotation -= lookVector.y * lookSpeed;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        firstPersonCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * lookVector.x * lookSpeed);
    }

    private Vector2 walkVector;
    public void OnWalk(InputAction.CallbackContext context)
    {
        if (isPaused) return;
        walkVector = context.ReadValue<Vector2>();
    }

    private Vector2 lookVector;
    public void OnLook(InputAction.CallbackContext context)
    {
        if (isPaused) return;
        lookVector = context.ReadValue<Vector2>();
    }

    private bool isJumping;
    public void OnJump(InputAction.CallbackContext context)
    {
        if (isPaused) return;
        isJumping = context.ReadValue<float>() != 0;
    }

    private bool isPaused;
    public void OnPause()
    {
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnUnpause()
    {
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnRespawn()
    {
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        velocity = Vector3.zero;
        Physics.SyncTransforms();
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
        if (!entity.IsOwner) return;

        var style = new GUIStyle(GUI.skin.box);
        style.alignment = TextAnchor.UpperLeft;

        GUI.backgroundColor = Color.black;
        GUI.color = Color.white;
        GUILayout.Box(debug(), style);
    }

    private string debug()
    {
        var flagsDefs = new[] {
            (isPaused, "Pause"),
            (isGrounded, "Ground"),
            (isJumping, "Jump")
        };

        return String.Join(
            Environment.NewLine,
            $"Name \"{state.Name}\" ",
            $"Color {(Vector3)(Vector4)state.Color}",
            $"Pos {transform.position}",
            $"Vel {velocity}",
            String.Join(
                " ",
                from flag in flagsDefs
                where flag.Item1
                select flag.Item2
            )
        );
    }
}
