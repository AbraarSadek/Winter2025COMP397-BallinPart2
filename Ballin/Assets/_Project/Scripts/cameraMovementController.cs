/*
 * 
 * Script Name: cameraMovementController
 * Created By: Abraar Sadek
 * Date Created: 02/13/2025
 * Last Modified: 02/13/2025
 * 
 * Script Purpose: To control the player cameras movement...
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//cameraMovementController Class
public class cameraMovementController : MonoBehaviour {

    public Transform cameraPosition; //Transform variable to store the position of the camera

    //Update Method
    void Update() {
        transform.position = cameraPosition.position;
    } //End of Update Method

} //End of cameraMovementController Class
