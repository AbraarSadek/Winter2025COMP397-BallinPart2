/*
 * 
 * Script Name: playerJumpController
 * Created By: Abraar Sadek
 * Date Created: 02/10/2025
 * Last Modified: 02/10/2025
 * 
 * Script Purpose: To control the basic upwards jump ability of the player...
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//playerJumpController Class
public class playerJumpController : MonoBehaviour {

    //Referance To Other Scripts
    playerMovementController playerMovement; //Referance To The playerMovementController Script

    //Public Variables
    [Header("Player Jump: ")]
    public float jumpForce; //Float variable that will hold the players jump force
    public float jumpCooldown; //Float variable that will hold the players jump cooldown
    public float airMultiplier; //Float variable that will hold the players jump air multiplier
    
    //Private Variables
    private bool readyToJump = true; //Bool variable that will check if the player is ready to jump

    //Private Variables
    private Rigidbody rb; //Reference to Rigidbody Attached To Player

    //playerJumpController Class Getter & Setter
    public bool GetReadyToJump { get { return readyToJump; } }
    public void SetReadyToJump(bool value) { readyToJump = value; }
    public float GetJumpCoooldown { get { return jumpCooldown; } }
    public float GetAirMultiplier { get { return airMultiplier; } }
    //End of playerJumpController Class Getter & Setter

    //Start Method
    void Start() {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<playerMovementController>();
    } //End of Start Method

    //Jump Method
    private void Jump() {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); //Reset players 'y' velocity to zero
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); //Add force to the player to make them jump
    } //End of Jump Method

    //PreformJump Method
    public void PerformJump() { 
        Jump();
    } //End of PreformJump Method

    //ResetJump Method
    private void ResetJump() {
        readyToJump = true; //Set the 'readyToJump' variable to true
    } //End of ResetJump Method

    //CallResetJump Method
    public void CallResetJump() {
        //ResetJump(); 
        StartCoroutine(ResetJumpAfterDelay());
    } //End of CallResetJump Method

    //ResetJumpAfterDelay IEnumerator
    private IEnumerator ResetJumpAfterDelay() {
        yield return new WaitForSeconds(jumpCooldown);
        ResetJump();
    } //End of ResetJumpAfterDelay IEnumerator

} //End of playerJumpController Class