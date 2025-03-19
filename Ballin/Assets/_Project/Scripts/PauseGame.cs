using UnityEngine;

namespace Platformer397
{
    public class PauseGame : MonoBehaviour
    {
        [SerializeField] GameObject PauseMenu;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            PauseMenu.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (PauseMenu.activeSelf)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        // Update is called once per frame
        public void Pause()
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0;
        }

        public void Resume()
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

        public void BaskToMainMenu() {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

    }
}
