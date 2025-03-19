/*
 * 
 * Script Name: playerJumpController
 * Created By: Abraar Sadek
 * Date Created: 02/13/2025
 * Last Modified: 02/13/2025
 * 
 * Script Purpose: To control the dash ability of the player...
 * 
 */

using UnityEngine;
using System.Collections;


//playerDashController Class
public class playerDashController : MonoBehaviour {

    [Header("Rederence: ")]
    public Transform orientation; //Transform variable to store the orientation of the player
    public Transform playerCamera; //Transform variable to store the camera of the player
    private Rigidbody rb; //Rigidbody variable to store the rigidbody of the player

    //Referance To Other Scripts
    private playerMovementController playerMovement; //Referance To The playerMovementController Script

    //Public Variables
    [Header("Dashing Settings: ")]
    public float dashingForce; //Float variable to store the force of the dash
    public float dashingUpwardsForce; //Float variable to store the upwards force of the dash
    public float maxDashingYSpeed; //Float variable to store the maximum y-speed of the dash
    public float dashingDuration; //Float variable to store the duration of the dash

    public float dashingCooldown; //Float variable to store the cooldown between dashes

    [Header("Settings: ")]
    public bool useCameraForward = true; //Bool variable to store if the dash will be in the direction of the camera
    public bool allowAllDirections = true; //Bool variable to store if the player can dash in all directions
    public bool disableGravity = false; //Bool variable to store if the 
    public bool resetVelocity = true; //Bool variable to store if the

    [Header("Camera Effects: ")]
    public playerCameraController playerCameraController; //Referance To The playerCameraController Script
    public float dashingFOV; //Float variable to store the fov of the dash

    //Private Variables
    private float dashingCooldownTimer; //Float variable to store the timer for the dash cooldown

    //Start Method
    private void Start () {
        rb = GetComponent<Rigidbody>(); //Get the 'Rigidbody' component attached to the player
        playerMovement = GetComponent<playerMovementController>(); //Get the 'playerMovementController' component attached to the player
    } //End of Start Method

    //Update Method
    private void Update() {

        //If-Statement - That Will Check If The Dash Cooldown Timer Is Still Counting Down
        if (dashingCooldownTimer > 0) {
            dashingCooldownTimer -= Time.deltaTime;
        } //End of If-Statement

        //If-Statement - That Will Check If The 'Left Shift-Key' Is Being Pressed, And If So, Call The 'Dashing' Method
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            Dashing(); //Method Call
        } //End of If-Statement 

    } //End of Update Method

    //Dashing Method
    private void Dashing() {

        //If-Else Statement - That Will Check If The Dash Cooldown Timer Is Still Counting Down
        if (dashingCooldownTimer > 0) {
            return;
        } else {
            dashingCooldownTimer = dashingCooldown;
        } //End of If-Elsse Statement

        playerMovement.isDashing = true; //Set the 'isDashing' variable to true
        playerMovement.maxYSpeed = maxDashingYSpeed; //Set the 'maxYSpeed' variable to the value of the 'maxDashingYSpeed' variable

        playerCameraController.DoFOV(dashingFOV); //Call the 'DoFOV' Method

        Transform forwardT; //Transform variable to store the forward direction of the player

        //If-Else Statement
        if (useCameraForward) { 
            forwardT = playerCamera; //Set the 'forwardT' variable to the value of the 'playerCamera' variable
        } else {
            forwardT = orientation; //Set the 'forwardT' variable to the value of the 'orientation' variable
        } //End of If-Else Statement

        Vector3 direction = GetDirection(forwardT); //Call the 'GetDirection' Method

        //If-Statement - That Will Check If The 'Left Shift-Key' Is Being Pressed
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb.AddForce(direction * 30f, ForceMode.Impulse);
        } //End of If-Statement

        if (disableGravity) { 
            rb.useGravity = false; //Disable gravity
        } //End of If-Statement

        Invoke(nameof(ResetDash), dashingDuration); //Invoke the 'ResetDash' Method

    } //End of Dashing Method

    private Vector3 delayedForcedToApplyDashing;

    //DelayedDashingForce Method
    private void DelayedDashingForce() {

        //If-Statement - That Will Reset The Player's Velocity
        if (resetVelocity) { 
            rb.linearVelocity = Vector3.zero;
        } //End of If-Statement

        rb.AddForce(delayedForcedToApplyDashing, ForceMode.Impulse); //Delaying adding dashing force to the player
    } //End of DelayedDashingForce Method

    //ResetDash Method
    private void ResetDash() {

        //playerMovement.isDashing = false; //Set the 'isDashing' variable to false
        //playerMovement.maxYSpeed = 0; //Set the 'maxYSpeed' variable to zero

        //playerCameraController.DoFOV(70); //Call the 'DoFOV' Method

        ////If-Statement
        //if (disableGravity)
        //{
        //    rb.useGravity = false; //Enable gravity
        //} //End of If-Statement

        playerMovement.isDashing = false;
        playerMovement.maxYSpeed = 0;

        if (disableGravity)
        {
            rb.useGravity = true; // Re-enable gravity after dash
        }

        if (resetVelocity)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0); // Keep Y velocity if jumping
        }

        playerCameraController.DoFOV(70);

    } //End of ResetDash Method

    // Method
    private Vector3 GetDirection(Transform forwardT) {

        //GetDirection Method Local Variables
        float horizontalInput = Input.GetAxisRaw("Horizontal"); //Float variable to store the horizontal input
        float verticalInput = Input.GetAxisRaw("Vertical"); //Float variable to store the vertical input

        Vector3 direction = new Vector3(); //Create a new Vector3 with the horizontal and vertical input

        //If-Else Statement - That Will Check If The Player Can Dash In All Directions
        if (allowAllDirections) {
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput; //Set the 'direction' variable to the value of the 'forwardT.forward' and 'forwardT.right' variables
        } else {
            direction = forwardT.forward; //Set the 'direction' variable to the value of the 'forwardT.forward' variable
        } //End of If-Else Statement

        if (verticalInput == 0 && horizontalInput ==0) {
            direction = forwardT.forward; //Set the 'direction' variable to the value of the 'forwardT.forward' variable
        } //End of If-Statement

        return direction.normalized; //Return the 'direction' variable

    } //End of GetDirection Method

} //End of playerDashController Class