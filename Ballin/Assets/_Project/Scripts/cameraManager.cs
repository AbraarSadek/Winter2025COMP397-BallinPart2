/*
 * 
 * Script Name: cameraManager
 * Created By: Abraar Sadek
 * Date Created: 02/10/2025
 * Last Modified: 02/10/2025
 * 
 * Script Purpose: To control the game camera...
 * 
 */

using UnityEngine;
using Unity.Cinemachine;

//namespace cameraManager
namespace cameraManager {

    //cameraManager Class
    public class cameraManager : MonoBehaviour {

        //Private Variables - Referencing The Cinemachine Virtual Camera And The Transform of The Player
        [SerializeField] private CinemachineCamera freeLookCam; // The camera to be managed
        [SerializeField] private Transform player; // The player's transform

        //Awake Method - Called when the script is loaded
        private void Awake() {

            #if UNITY_EDITOR && !UNITY_ANDROID
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            #endif

            //If-Statement - That Will Check If The Player Transform Is Already Assigned, And If Not, Assign It
            if (player == null) {
                GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
                //Nested If-Statement
                if (foundPlayer != null) {
                    player = foundPlayer.transform;
                } else {
                    Debug.LogError("Player object not found! Make sure the player has the correct tag."); //Print To Log
                } //End of Nested If-Statement
            }//End of If-Statement

        } //End of Awake Method

        //OnEnable Method - Called when the script is enabled
        private void OnEnable() {
            //Set The Player's Transform As The Target For The Cinemachine Camera
            freeLookCam.Target.TrackingTarget = player;
        } //End of OnEnable Method

    } //End of cameraManager Class

} //End of namespace cameraManager
