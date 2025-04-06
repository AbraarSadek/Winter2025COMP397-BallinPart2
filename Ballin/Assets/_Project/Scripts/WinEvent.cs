using UnityEngine;
using UnityEngine.SceneManagement;

public class WinEvent : MonoBehaviour {

    [SerializeField] private PlayerHealthController playerHealthController;
    [SerializeField] private CoinCollectionController coinCollectionController;
    [SerializeField] private TimerController timerController;

    public static class GameData {
        public static int remainingHealthBars;
        public static int coinsCollected;
        public static float remainingTime;
    }

    private void OnTriggerEnter(Collider other) {

        GameData.remainingHealthBars = playerHealthController.remainingHealthBars;
        GameData.coinsCollected = coinCollectionController.coinsCollected;
        GameData.remainingTime = timerController.timeRemaining;

        if (other.CompareTag("Player")) {

            SceneManager.LoadScene("WinScene");

        }

    }

}
