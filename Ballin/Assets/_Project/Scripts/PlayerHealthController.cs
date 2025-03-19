/*
 * 
 * Script Name: playerJumpController
 * Created By: Abraar Sadek
 * Date Created: 02/22/2025
 * Last Modified: 02/22/2025
 * 
 * Script Purpose: To control the players health bar...
 * 
 */

using Unity.VisualScripting;
using UnityEngine;

//PlayerHealthController Class
public class PlayerHealthController : MonoBehaviour {

    private bool hasHearts = false;
    private bool noHearts = false;
    //Pulic Variables
    [SerializeField] private FloatSO score;
    [SerializeField] private FloatSO star;
    public GameObject[] healthBars; //Game Object array variables that will hold the players health bars
    public bool isDead; //Bool variable that will hold if the player is dead or not
    public int remainingHealthBars; //Int variable that will hold the players remaining health bars

    //Getters & Setters
    public bool GetIsDead { get { return isDead; } }
    private bool SetIsDead { set { isDead = value; } }
    public int GetRemainingHealthBars { get { return remainingHealthBars; } }
    public int SetRemainingHealthBars { set { remainingHealthBars = value; } }

    //Start Method - Is Called Once Before The First Execution of Update Method
    void Start() {

        remainingHealthBars = healthBars.Length; //Set the 'remainingHealthBars' variable to the length of the 'healthBars' array

    } //End of Start Method

    //Update Method - Is Called Once Per Frame
    void Update() {

        //If-Statement - That Will Check If The Player Is Dead
        if (isDead) {

            //ADD PLAYER DEATH CODE BELOW

            Debug.Log("Player Is Dead!!!"); //Temporarly death message

        } //End of If-Statement


        if (remainingHealthBars == 3 && !hasHearts)
        {
            star.Value++;
            hasHearts = true;
            Debug.Log("Star Collected");

        }
        else if(remainingHealthBars == 2 && !noHearts)
        {
            star.Value--;
            noHearts = true;
            hasHearts = false;
            Debug.Log("No Stars");
        }

    } //End of Update Method

    //DamageTaked Method - That Will Keep Track Of The Damage The Player Has Taken And Will Remove Corresponding Health Bars
    public void DamageTaked(int damage) {

        //If-Statement - That Will Check If The Player Has More Then Or Equal To 1 (0) Health Bar Remaining
        if (remainingHealthBars >=  1) {
            remainingHealthBars -= damage; //Deal damage to the player depending on the value of the 'damage' variable
            Destroy(healthBars[remainingHealthBars].gameObject); //Destroy the health bar game object that corresponds to health bar that the player has lost
            Debug.Log("Damage Taken, Remaining Health: " + remainingHealthBars);
        } //End of If-Statement

        //If-Statement - That Will Check If The Player Has Less The 1 (0) Health Bar Remaining
        if (remainingHealthBars < 1) {
            isDead = true; //Set the 'isDead' variable to true
        } //End of If-Statement

        if (remainingHealthBars < 3)
        {
            score.Value -= 200;
            Debug.Log("I lost 200 points");
        }
        else if (remainingHealthBars < 2)
        {
            score.Value -= 500;
            Debug.Log("I lost 500 points");
        }


    } //End of DamageTaked Method

} //End of PlayerHealthController Class
