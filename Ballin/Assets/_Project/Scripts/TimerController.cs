/*
 * 
 * Script Name: TimerController
 * Created By: Abraar Sadek
 * Date Created: 02/27/2025
 * Last Modified: 02/27/2025
 * 
 * Script Purpose: To control game duration timer...
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using Unity.Mathematics;
using Platformer397;

//TimerController Class
public class TimerController : MonoBehaviour {

    [SerializeField] private FloatSO star;
    private bool hasTime = false;
    private bool noTime = false;

    //Public Component Reference Variables
    [Header("Component References: ")]
    public TMP_Text timerText;

    //Public Variables
    [Header("Timer Controler Variables: ")]
    [SerializeField]
    public float timeRemaining = 90f; //Float variable to store the remaining time 
    public bool timerIsRunning = false; //Bool variable to hold if the timer is running or not

    //Private Variables
    private Color greenTextColor = Color.green;
    private Color yellowTextColor = Color.yellow;
    private Color redTextColor = Color.red;

    //Start Method - Is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
        timerIsRunning = true; //Set the 'timerIsRunning' variable to true

    } //End of Start Method

    //Update Method - Is called once per frame
    void Update() {
        
        //If-Else Statement - That Will Check If The Timer Is Running
        if (timerIsRunning) {

            //Nested If-Statement - That Will Check If The 'timeRemaining' Variable Is Equal To '0'
            if (timeRemaining > 0) {

                timeRemaining -= Time.deltaTime; //Add the time passed since the last frame to the 'timeRemaining' variable
                DisplayTimer(timeRemaining); //Call the 'DisplayTimer' method

            } else {

                timeRemaining = 0; //Set the value of the 'timeRemaining' variable to '0'
                timerIsRunning = false; //Set the value of the 'timerIsRunning' variable to 'false'
                DisplayTimer(timeRemaining); //Call the 'DisplayTimer' method

            }//End of Nested If-Else Statement

        } //End of If-Statement


        if (timeRemaining >= 60 && !hasTime)
        {
            star.Value++;
            hasTime = true;
            Debug.Log("Star Collected");

        }
        else if (timeRemaining < 60 && !noTime)
        {
            star.Value--;
            noTime = true;
            hasTime = false;
            Debug.Log("No star");
        }

    } //End of Update Method

    //DisplayTimer Method - That Will Display The Timer
    public void DisplayTimer(float timeToDisplay) {

        timeToDisplay = Mathf.Max(timeToDisplay, 0); //Set the value of the 'timeToDisplay' variable to '0' to prevent negative values

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); //Int variable to store the remaining minutes
        float seconds = Mathf.FloorToInt(timeToDisplay % 60); //Int variable to store the remaining seconds

        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);

        UpdateTimerTextColor(timeRemaining); //Call the 'UpdateTimerTextColor' method and pass it the value of the 'timeRemaining' variable

    } //End of DisplayTimer Method

    //UpdateTimerTextColor Method - That Will Change The Timers Text Color Depending On The Remaining Time
    public void UpdateTimerTextColor(float remainingTime) {

        StopAllCoroutines(); //Stop all coroutines

        //Else-If Statement - That Will Check How Much Time Is Left On The Timer
        if (remainingTime > 60) {
            timerText.color = greenTextColor; //Set the 'timerText.color' variable to the value of the 'greenTextColor' variable
            timerText.transform.localScale = Vector3.one; //Reset text scale
        } else if (remainingTime > 30) {
            timerText.color = yellowTextColor; //Set the 'timerText.color' variable to the value of the 'yellowTextColor' variable
            StartCoroutine(PulsateText(1.1f, 2.0f)); //Call the 'PulsateText' method and pass it the values of '1.1f' and '2.0f'
        } else {
            timerText.color = redTextColor; //Set the 'timerText.color' variable to the value of the 'redTextColor' variable
            StartCoroutine(PulsateText(1.3f, 4.0f)); //Call the 'PulsateText' method and pass it the values of '1.3f' and '4.0f'
        } //End of Else-If Statement

    } //End of ChangeTimersTextColor Method

    //PulsateText Method - That Will Make The Timers Text Pulsate Depending On The Remaining Time
    private IEnumerator PulsateText(float maxScale, float speed) {

        //While Loop - That Will Run As Long As The 'timerIsRunning' Is 'true'
        while (timerIsRunning) {

            float scale = 1.0f + Mathf.Sin(Time.time * speed) * (maxScale - 1.0f); //Float variable to store the scale of the text
            timerText.transform.localScale = new Vector3(scale, scale, 1); //Set the scale of the text to the value of the 'scale' variable
            yield return null; //Wait for the next frame and then return value of 'null'

        } //End of While Loop

        /*
         * float time = 0f; //Float variable to store the time

        //While Loop - That Will Run As Long As The 'timerIsRunning' Is 'true'
        while (timerIsRunning) {

            float scale = Mathf.Lerp(1.0f, maxScale, Mathf.PingPong(time * speed, 1.0f)); //Float variable to store the scale of the text
            timerText.transform.localScale = new Vector3(scale, scale, 1); //Set the scale of the text to the value of the 'scale' variable
            time += Time.deltaTime; //Add the time passed since the last frame to the 'time' variable
            yield return null; //Wait for the next frame and then return value of 'null'

        } //End of While Loop
         */

    } //End of PulsateText Method

} //End of TimerController Class
