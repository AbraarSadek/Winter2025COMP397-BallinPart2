/*
 * 
 * Script Name: CoinCollectionController
 * Created By: Abraar Sadek
 * Date Created: 02/21/2025
 * Last Modified: 02/21/2025
 * 
 * Script Purpose: To control players collection of coins...
 * 
 */

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

//CoinCollectionController Class
public class CoinCollectionController : MonoBehaviour {

    //Priavte Variables
    private bool hasStar = false;
    public int coinsCollected = 0; //Int variable that will hold the number of coins collected by the player

    [SerializeField] private FloatSO score;
    [SerializeField] private FloatSO star;

    [SerializeField] private bool hasCollectedGoldCoin = false; //Bool variable that will hold if the player has collected the gold coin
    [SerializeField] private bool hasCollectedSilverCoin = false; //Bool variable that will hold if the player has collected the silver coin
    [SerializeField] private bool hasCollectedBronzeCoin = false; //Bool variable that will hold if the player has collected the bronze coin

    [SerializeField] private Image goldCoinImage; //Image variable that will hold the gold coin image
    [SerializeField] private Image silverCoinImage; //Image variable that will hold the silver coin image
    [SerializeField] private Image bronzeCoinImage; //Image variable that will hold the bronze coin image

    //OnTriggerEnter Method - Will Be Called When The Player Collides With The Coin Objects
    private void OnTriggerEnter(Collider other) {

        //If-Statement - That Will Check If The Player Has Collided With An Object That Is Tagged 'whatIsGoldCoin'
        if (other.CompareTag("whatIsGoldCoin")) {

            coinsCollected++; //Increment the 'coinsCollected' variable by 1
            hasCollectedGoldCoin = true; //Set the 'hasCollectedGoldCoin' variable to true
            score.Value += 1000;
            Debug.Log("Player Has Collected The Gold Coin!");
            Debug.Log("Coins Collected: " + coinsCollected);
            Destroy(other.gameObject); //Destroy the object that the player has collided with

            //Nested If-Statement - That Will Make The Gold Coin Appear In The Player's Inventory Once The Gold Coin Has Been Collected
            if (goldCoinImage != null) {
                goldCoinImage.enabled = true;
            } //End of Nested If-Statement

        } //End of If-Statement

        //If-Statement - That Will Check If The Player Has Collided With An Object That Is Tagged 'whatIsSilverCoin'
        if (other.CompareTag("whatIsSilverCoin")) {

            coinsCollected++; //Increment the 'coinsCollected' variable by 1
            hasCollectedSilverCoin = true; //Set the 'hasCollectedSilverCoin' variable to true
            score.Value += 500;
            Debug.Log("Player Has Collected The Silver Coin!");
            Debug.Log("Coins Collected: " + coinsCollected);
            Destroy(other.gameObject); //Destroy the object that the player has collided with

            //Nested If-Statement - That Will Make The Gold Coin Appear In The Player's Inventory Once The Gold Coin Has Been Collected
            if (silverCoinImage != null) {
                silverCoinImage.enabled = true;
            } //End of Nested If-Statement

        } //End of If-Statement

        //If-Statement - That Will Check If The Player Has Collided With An Object That Is Tagged 'whatIsBronzeCoin'
        if (other.CompareTag("whatIsBronzeCoin")) {

            coinsCollected++; //Increment the 'coinsCollected' variable by 1
            hasCollectedBronzeCoin = true; //Set the 'hasCollectedBronzeCoin' variable to true
            score.Value += 100;
            Debug.Log("Player Has Collected The Bronze Coin!");
            Debug.Log("Coins Collected: " + coinsCollected);
            Destroy(other.gameObject); //Destroy the object that the player has collided with

            //Nested If-Statement - That Will Make The Gold Coin Appear In The Player's Inventory Once The Gold Coin Has Been Collected
            if (bronzeCoinImage != null) {
                bronzeCoinImage.enabled = true;
            } //End of Nested If-Statement

        } //End of If-Statement

    } //End of OnTriggerEnter Method

    //Updates to check if all 3 coints are collected to move into the WinScene
    private void Update()
    {
        
        if (coinsCollected == 3 && !hasStar)
        {
            star.Value++;
            hasStar = true;
            Debug.Log("Star Collected");
            
        }
    
        
    }

    private void Start()
    {
        score.Value = 0;
        star.Value = 0;
    }

} //End of CoinCollectionController Class
