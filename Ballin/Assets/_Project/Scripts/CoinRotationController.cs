/*
 * 
 * Script Name: playerJumpController
 * Created By: Abraar Sadek
 * Date Created: 02/21/2025
 * Last Modified: 02/21/2025
 * 
 * Script Purpose: To control the rotation of gold coins...
 * 
 */

using UnityEngine;

//GoldCoinRotationController Class
public class CoinRotationController : MonoBehaviour {

    //Public Variables
    [SerializeField] public Vector3 rotationSpeed; //Vector3 variable to store the rotation speed of the gold coin

    //Update Method - Update is called once per frame
    void Update() {
        transform.transform.Rotate(rotationSpeed * Time.deltaTime); //Calulate the rotation speed of the gold coin
    } //End of Update Method

} //End of GoldCoinRotationController Class
