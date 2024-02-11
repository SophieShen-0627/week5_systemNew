using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed of the character

    private PlayerControls controls;
    private Vector2 moveInput;

    private void Awake()
    {
        // Initialize the PlayerControls
        controls = new PlayerControls();

        // Bind the movement action
        controls.Player.Walk.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Walk.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        // Apply the movement input to the character
        Vector3 move = new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;
        transform.position += move;
    }
}