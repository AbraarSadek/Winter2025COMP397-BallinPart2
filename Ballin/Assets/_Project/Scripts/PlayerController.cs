/*
 * 
 * Script Name: PlayerController
 * Created By: Abraar Sadek
 * Date Created: 02/23/2025
 * Last Modified: 02/23/2025
 * 
 * Script Purpose: To control the players movement...
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    //Movement Variables
    [Header("Player Movement Settings: ")]
    [SerializeField]
    public Transform playerCamera;
    [Range(0.0f, 0.5f)] public float mouseSmoothTime = 0.03f;
    public bool isCursorLocked = true;
    public float mouseSensitivity = 3.5f;
    public float walkSpeed = 6.0f;
    [Range(0.0f, 0.5f)] public float moveSmoothTime = 0.03f;
    public float gravity = -30f;
    InputAction moveAction;
    InputAction lookAction;
    //todo: InputAction jumpAction;
    

    //Ground Check Variables
    [Header("Player Ground Check Settings: ")]
    public Transform groundCheck;
    public LayerMask ground;

    //Jump Variables
    [Header("Player Jump Settings: ")]
    public float jumpHeight = 6.0f;
    float velocityY;
    bool isGrounded;

    //Variables For Player Camera
    float cameraCap;
    UnityEngine.Vector2 currentMouseDelta;
    UnityEngine.Vector2 currentMouseDeltaVelocity;

    //Variables For Character Controller Object
    CharacterController controller;
    UnityEngine.Vector2 currentDirection;
    UnityEngine.Vector2 currentDirectionVelocity;
    Vector3 velocity;

    //Variables For Player Dash
    [Header("Player Dash Settings: ")]
    [SerializeField] public float dashSpeed = 15.0f; // Dash speed
    [SerializeField] public float dashDuration = 0.2f; // Duration of the dash
    [SerializeField] public float dashTime = 0f;
    public bool isDashing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        controller = GetComponent<CharacterController>();

        if (isCursorLocked) {
            //Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouse();
        UpdateMovement();
    }

    public void UpdateMouse()
    {

        UnityEngine.Vector2 targetMouseDelta = lookAction.ReadValue<UnityEngine.Vector2>();

        currentMouseDelta = UnityEngine.Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraCap -= currentMouseDelta.y * mouseSensitivity;
        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);

        playerCamera.localEulerAngles = UnityEngine.Vector3.right * cameraCap;

        transform.Rotate(UnityEngine.Vector3.up * currentMouseDelta.x * mouseSensitivity);

    }

    public void UpdateMovement()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);

        UnityEngine.Vector2 targetDirection = moveAction.ReadValue<UnityEngine.Vector2>();
        targetDirection.Normalize();

        currentDirection = UnityEngine.Vector2.SmoothDamp(currentDirection, targetDirection, ref currentDirectionVelocity, moveSmoothTime);

        velocityY += gravity * 2f * Time.deltaTime;

        //If-Statement - (dashing)
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            isDashing = true;
            dashTime = Time.time + dashDuration;
        }

        //If-Statement - (dashing)
        if (isDashing && Time.time < dashTime)
        {
            //Apply dash speed to the player while dashing
            velocity =  (transform.forward * currentDirection.y + transform.right * currentDirection.x) * dashSpeed + Vector3.up * velocityY;
        } else
        {
            isDashing = false;
            velocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x) * walkSpeed + Vector3.up * velocityY;

        }

        controller.Move(velocity * Time.deltaTime);

        if (isGrounded && Input.GetButtonDown("Jump")) {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (!isGrounded && controller.velocity.y < -1f)
        {
            velocityY = -8f;
        }


    }

}
