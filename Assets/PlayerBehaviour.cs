using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Bolt;
using TMPro;
using System;
using System.Linq;

public class PlayerBehaviour : EntityEventListener<IPlayerState>
{
    public static Vector3 spawnPosition = new Vector3(0, 1, 0);
    public static Quaternion spawnRotation = Quaternion.identity;

    public Camera firstPersonCamera;
    public CharacterController character;
    public TMP_Text nameplate;
    public Transform groundCheck;
    public LayerMask groundMask;
    public GameObject faceBox;
    public BoxCollider hitbox;

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

            state.Id = System.Guid.NewGuid();
            state.Name = PlayerPrefs.GetString("name", "Player Name");
            state.IsTagged = false;
            ChangeColor();
        }

        state.AddCallback("Name", NameChanged);
        state.AddCallback("Color", ColorChanged);
        state.AddCallback("IsTagged", TagChanged);
    }

    public void ChangeColor()
    {
        state.Color = Color.HSVToRGB(UnityEngine.Random.value, 1.0f, 0.5f);
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

    public override void OnEvent(TagEvent evnt)
    {
        if (evnt.TargetId != state.Id) return;
        state.IsTagged = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<PlayerBehaviour>();
        if (player && state.IsTagged)
        {
            var evnt = TagEvent.Create();
            evnt.TargetId = player.state.Id;
            evnt.Send();
        }
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
    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnRespawn(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        velocity = Vector3.zero;
        Physics.SyncTransforms();
    }

    public void OnColorChange(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        ChangeColor();
    }

    public void OnToggleTag(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        state.IsTagged = !state.IsTagged;
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

    void TagChanged()
    {
        faceBox.SetActive(state.IsTagged);
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
            (isJumping, "Jump"),
            (state.IsTagged, "Tag")
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
