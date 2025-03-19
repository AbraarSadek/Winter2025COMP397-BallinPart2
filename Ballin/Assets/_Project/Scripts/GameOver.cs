using System;
using UnityEngine;

namespace Platformer397
{
    public class GameOver : MonoBehaviour {

        [SerializeField]
        public string gameOverSceneName = "DeathScene";

        public Transform player;

        private Vector3 playerPosition;

        //Reference To 'PlayerHealthController' Script
        public PlayerHealthController playerHealthController;
        public TimerController timerController;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start() {

            //playerHealthController = GetComponent<PlayerHealthController>();

            if (player == null)
            {
                Debug.LogError("Player not found in the scene. Please add a player to the scene.");
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (player != null)
            {
                playerPosition = player.transform.position;
                
                if (playerPosition.y < -10 || playerHealthController.isDead == true)
                {
                    LoadDeath();
                } 
                if (timerController.timeRemaining <= 0)
                {
                    Debug.Log("death from timer");
                    LoadDeath();
                }
            }
        }

        public void LoadDeath()
        {
            Time.timeScale = 1f;
            Debug.Log("Game Over");
            UnityEngine.SceneManagement.SceneManager.LoadScene(gameOverSceneName);
        }
    }
}
