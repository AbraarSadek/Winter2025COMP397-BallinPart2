/*
 * 
 * Script Name: playerMovementController
 * Created By: Abraar Sadek
 * Date Created: 02/10/2025
 * Last Modified: 02/10/2025
 * 
 * Script Purpose: To control the basic movement of the player, forward, backword, left and right...
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;


//playerMovementController Class
public class playerMovementController : MonoBehaviour {

    //Referance To Other Scripts
    playerJumpController playerJump; //Reference to the 'playerJumpController' script

    //Public Variables
    [Header("Player Movement: ")]
    private float movementSpeed; //Float variable that will control the movement speed of the player
    public float walkingSpeed; //Float variable that will control the walking speed of the player
    public float dashingSpeed; //Float variable that will control the dashing speed of the player
    public float dashingSpeedChangeFactor; //Float variable that will control the dashing speed change factor of the player

    public float maxYSpeed; //Float variable that will control the maximum 'y' speed of the player

    public float groundDrag; //Float variable that will control the ground drag of the player

    [Header("Ground Check: ")]
    public float playerHeight; //Float variable that will hold the height of the player
    public LayerMask whatIsGround; //LayerMask variable that will hold the layer of the ground
    bool isGrounded; //Bool variable that will hold if the player is on the ground or not

    public Transform orientation; //Transform variable that will hold the orientation of the player

    float horizontalInput; //Float variable that will hold how much input is being sent for the player's horizontal input keys
    float verticalInput; //Float variable that will hold how much input is being sent for the player's vertical input keys

    Vector3 movementDirection; //Vector3 variable that will hold the direction the player is moving in

    [Header("Rederence: ")]
    Rigidbody rb; //Reference to Rigidbody Attached To Player

    public MovementState state;
    public enum MovementState {
        Walking, Jumping, Dashing 
    }

    public bool isWalking; //Bool variable that will hold if the player is walking or not
    public bool isDashing; //Bool variable that will hold if the player is dashing or not

    //Start Method
    private void Start() {
        rb = GetComponent<Rigidbody>(); //Get the 'Rigidbody' component attached to the player
        rb.freezeRotation = true; //Freeze the rotation of the player
        playerJump = GetComponent<playerJumpController>(); //Get the 'playerJumpController' component attached to the player
    } //End of Start Method

    //Update Method
    private void Update() {

        //Check If The Player Is On The Ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        //Method Calls
        KeyboardInput(); 
        PlayerSpeedControl();
        PlayerStateHandeler();

        //If-Else Statement - That Will Apply Drag To The Player If They Are On The Ground
        if (state == MovementState.Walking) {
            rb.linearDamping = groundDrag;
        } else {
            rb.linearDamping = 0;
        } //End of If-Else Statement

    } //End of Update Method

    //FixedUpdate Method
    private void FixedUpdate() {
        //Method Calls
        MovePlayer(); 
    } //End of FixedUpdate Method

    //KeyboardInput Method
    private void KeyboardInput() {
        //Get The 'Horizontal' and 'Vertical' Inputs For The Player
        horizontalInput = Input.GetAxisRaw("Horizontal"); //Get the input for the player's horizontal input keys
        verticalInput = Input.GetAxisRaw("Vertical"); //Get the input for the player's vertical input keys

        //If- Statement - That Will Make The Player Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true && playerJump.GetReadyToJump == true) {
            playerJump.SetReadyToJump(false); //Set the 'readyToJump' variable to false
            playerJump.PerformJump(); //Call the 'PreformJump' Method
            //Invoke(nameof(playerJump.CallResetJump), playerJump.GetJumpCoooldown); //Invoke the 'ResetJump' Method
            playerJump.CallResetJump();
        } //End of If-Statement
    } //End of KeyboardInput Method

    //MovePlayer Method
    private void MovePlayer() {

        movementDirection = orientation.forward * verticalInput + orientation.right* horizontalInput; //Calculate the direction the player is moving in

        //Else-If Statement - That Will 
        if (isGrounded) {
            rb.AddForce(movementDirection.normalized * movementSpeed * 10f, ForceMode.Force); //Add force to the player to make them move while on the ground
        } else if (!isGrounded) {
            rb.AddForce(movementDirection.normalized * movementSpeed * 10f * playerJump.GetAirMultiplier, ForceMode.Force); //Add force to the player to make them move while in the air
        } //End of Else-If Statement

    } //End of MovePlayer Method

    //PlayerSpeedControl Method
    private void PlayerSpeedControl() {

        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); //Create a new Vector3 with only the X and Z components of the player's velocity

        if (isDashing == true) { return; }

        //If- Statement - That Will Limit The Player's Speed, If They Exceed The Maximum Speed
        if (flatVelocity.magnitude > movementSpeed) {
            Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed; //Create a new Vector3 with the same direction as the player's velocity, but with a magnitude of the maximum speed
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z); //Set the player's velocity to the limited velocity
        } //End of If-Statement

        //If-Statement - That Will Limit The Players 'y' Speed
        if (maxYSpeed != 0 && rb.linearVelocity.y > maxYSpeed) {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, maxYSpeed, rb.linearVelocity.z); //Set the player's velocity to the limited velocity
        } //end of If-Statement

    } //End of PlayerSpeedControll Method

    //Private Variables
    private float desiredMoveSpeed; //Float variable that will hold the desired movement speed of the player
    private float lastDesiredMoveSpeed; //Float variable that will hold the last desired movement speed of the player
    private MovementState lastState; //MovementState variable that will hold the last state of the player
    private bool keepMomentum; //Bool variable that will hold if the player should keep their momentum or not

    //PlayerStateHandeler Method
    private void PlayerStateHandeler() {

        //If-Statement - That Will Check If The Player Is Walking, Jumping Or Dashing
        if (isDashing) {
            state = MovementState.Dashing; //Set the 'state' variable to 'walking'
            desiredMoveSpeed = dashingSpeed; //Set the 'movementSpeed' variable to the value of the 'walkingSpeed' variable
            speedChangeFactor = dashingSpeedChangeFactor; //Set the 'speedChangeFactor' variable to the value of the dashingSpeedChangeFactor
        }
        else if (isGrounded) {
            state = MovementState.Walking; //Set the 'state' variable to 'dashing'
            desiredMoveSpeed = walkingSpeed; //Set the 'movementSpeed' variable to the value of the 'dashingSpeed' variable
        } else { 
            state = MovementState.Jumping; //Set the 'state' variable to 'jumping'

            //Nested If-Else Statement - 
            if (desiredMoveSpeed < walkingSpeed) {
                desiredMoveSpeed = walkingSpeed; //Set the 'desiredMoveSpeed' variable to the value of the 'walkingSpeed' variable
            } else {
                desiredMoveSpeed = dashingSpeed; //Set the 'desiredMoveSpeed' variable to the value of the 'dashingSpeed' variable
            }//End of Nested If-Else Statement

        } //End of Else-If Statement

        bool desriredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed; //Bool variable that will hold if the desired movement speed has changed or not

        //If-Statement
        if (lastState == MovementState.Dashing) { 
            keepMomentum = true; //Set the 'keepMomentum' variable to true
        } //End of If-Statement

        //If-Statement
        if (desriredMoveSpeedHasChanged) {

            //Nested Else-If Statement
            if (keepMomentum) {
                StopAllCoroutines(); //Stop all coroutines
                StartCoroutine(SmoothlyLerpMoveSpeed()); //Start the 'SmoothlyLerpMoveSpeed' IEnumerator
            } else {
                StopAllCoroutines(); //Stop all coroutines
                movementSpeed = desiredMoveSpeed;
            } //End of Nested Else-If Statement

        } //End of If-Statement

        lastDesiredMoveSpeed = desiredMoveSpeed; //Set the 'lastDesiredMoveSpeed' variable to the value of the 'desiredMoveSpeed' variable
        lastState = state; //Set the 'lastState' variable to the value of the 'state' variable

    } //End of PlayerStateHandeler Method

    //Private Variable
    private float speedChangeFactor;

    //SmoothlyLerpMoveSpeed IEnomerator 
    private IEnumerator SmoothlyLerpMoveSpeed () {

        //IEnumerator SmoothlyLerpMoveSpeed Local Variables
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - movementSpeed);
        float startValue = movementSpeed;

        float boostFactor = speedChangeFactor;

        //While-Loop
        while (time < difference) { 
            movementSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime * boostFactor;
            yield return null;
        } //End of While-Loop

        movementSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;

    } //End of SmoothlyLerpMoveSpeed IEnumerator

} //End of playerMovementController Class


/*
 * //Public Variables
    public float movementSpeed = 5f; //Float variable that will control the movement speed of the player
    public float stopLinearDamping = 1f; //Float variable that will control the 'Drag' of the player when it is not moving
    public float movementLinearDamping = 0f; //Float variable that will control the 'Drag' of the player when it is moving

    //Private Variables
    private Rigidbody rb; //Reference to Rigidbody Attached To Player
    private float horizontalInput; //Will hold how much input is being sent for the player's horizontal input keys
    private float verticalInput; //Will hold how much input is being sent for the player's vertical input keys

    //Awake Method - Called when the script is loaded
    private void Awake() {
        rb = GetComponent<Rigidbody>(); //Get the 'Rigidbody' component attached to the player
        rb.linearDamping = stopLinearDamping; //Set the 'Drag' of the player to the value of the 'stopLinearDamping' variable
    } //End of Awake Method

    //FixedUpdate Method - Called once per frame
    void FixedUpdate() {

        //Get The Input From The Player (WASD or Arrow keys)
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput * movementSpeed, 0, verticalInput * movementSpeed); //Calculate the movement of the player

        //If-Else Statement - To Check If The Player Is Moving Or Not
        if (movement.magnitude > 0f)
        {
            rb.linearDamping = movementLinearDamping; //Set the 'Drag' of the player to the value of the 'movementLinearDamping' variable
            rb.AddForce(movement * movementSpeed, ForceMode.Acceleration); //Apply force to the Rigidbody to move the player
        } else {
            rb.linearDamping = stopLinearDamping; //Set the 'Drag' of the player to the value of the 'stopLinearDamping' variable
        } //End of If-Else Statement

    } //End of FixedUpdate Method
 */