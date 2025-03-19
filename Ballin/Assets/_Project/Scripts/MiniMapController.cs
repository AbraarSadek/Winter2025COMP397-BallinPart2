/*
 * 
 * Script Name: MiniMapController
 * Created By: Abraar Sadek
 * Date Created: 02/21/2025
 * Last Modified: 02/21/2025
 * 
 * Script Purpose: To control the minimap's movement based on the player's position...
 * 
 */

using UnityEngine;

//MiniMapController Class
public class MiniMapController : MonoBehaviour {

    //Private Variables
    [SerializeField] private Transform player; //Transform variable to reference the player's transform

    //Update Method - Is Called Once Per Frame 
    void Update() {

        Vector3 newPosition = player.position; //Vector3 variable that will get the player's current position
        newPosition.y = transform.position.y; //Set the 'y' position of the minimap unchanged (preventing vertical movement)
        transform.position = newPosition; //Set the minimap's position to follow the player's position

    } //End of Update Method

} //End of MiniMapController Class
