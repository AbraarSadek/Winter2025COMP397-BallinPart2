using UnityEngine;

namespace Platformer397
{
    public class DeathSceneManager : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void Retry()
        {
            string lastScene = PlayerPrefs.GetString("LastPlayedScene");
            if (!string.IsNullOrEmpty(lastScene))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(lastScene);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level");
            }
        }

        public void BaskToMainMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

    }
}
