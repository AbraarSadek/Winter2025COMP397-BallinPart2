/*
 * 
 * Script Name: playerCameraController
 * Created By: Abraar Sadek
 * Date Created: 02/13/2025
 * Last Modified: 02/13/2025
 * 
 * Script Purpose: To control the players camera...
 * 
 */

using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
using DG.Tweening;

//playerCameraController Class
public class playerCameraController : MonoBehaviour {


    //Public Variables
    public float sensitivityX; //Float variable to store the sensitivity of the camera on the X-Axis
    public float sensitivityY; //Float variable to store the sensitivity of the camera on the Y-Axis

    public Transform orientation; //Transform variable to store the transform component of the player object

    float xRotation; //Float variable to store the rotation of the camera on the X-Axis
    float yRotation; //Float variable to store the rotation of the camera on the Y-Axis

    //Start Method - 
    private void Start() {
        //Lock The Mouse Cursor To The Center of The Game View And Hide It
        //Cursor.lockState = CursorLockMode.Locked;
       // Cursor.visible = false;
    } //End of Start Method

    //Update Method - 
    private void Update() {
       
        //Update Method Varibles 
        float mouseInputX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivityX; //Float variable to store the input of the mouse on the X-Axis
        float mouseInputY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivityX; //Float variable to store the input of the mouse on the Y-Axis

        yRotation += mouseInputX; //Update the 'yRotation' variable with the value of the 'mouseInputX' variable
        xRotation -= mouseInputY; //Update the 'xRotation' variable with the value of the 'mouseInputY' variable

        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Clamp the 'xRotation' variable to a range of -90 to 90 degrees

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); //Update the 'transform.rotation' variable with the value of the 'xRotation' and 'yRotation' variables
        orientation.rotation = Quaternion.Euler(0, yRotation, 0); //Update the 'orientation.rotation' variable with the value of the 'yRotation' variable

    } //End of Update Method

    //DoFOV Method
    public void DoFOV(float endValue) {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f); 
    } //End of DoFOV Method

} //End of playerCameraController Class
