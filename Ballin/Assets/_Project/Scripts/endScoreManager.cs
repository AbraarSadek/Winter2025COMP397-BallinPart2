using UnityEngine;
using TMPro;
using System;
using static WinEvent;

public class EndScoreManager : MonoBehaviour {

    [SerializeField] private GameObject starOne;
    [SerializeField] private GameObject starTwo;
    [SerializeField] private GameObject starThree;

    int remainingHealthBars;
    int coinsCollected;
    float remainingTime;

    //References to Other Scripts
    private PlayerHealthController playerHealthController;
    private CoinCollectionController coinCollectionController;
    private TimerController timerController;

    void Awake() {

        remainingHealthBars = WinEvent.GameData.remainingHealthBars; //Set The Remaining Health Bars To The Value Stored In The Game Data
        coinsCollected = WinEvent.GameData.coinsCollected; //Set The Coins Collected To The Value Stored In The Game Data
        remainingTime = WinEvent.GameData.remainingTime; //Set The Remaining Time To The Value Stored In The Game Data

    }

    // Update is called once per frame
    void Update() {
        AwardStars();
    }

    private void AwardStars() {

        //If-Statement - That Will Award The Player Stars If They Have Met The Requirements
        if (remainingHealthBars == 3) { starOne.SetActive(true); /*Set Game Object To Active*/ }
        if (remainingTime >= 60f) { starTwo.SetActive(true); /*Set Game Object To Active*/ }
        if (coinsCollected == 3) { starThree.SetActive(true); /*Set Game Object To Active*/ }

    }

}