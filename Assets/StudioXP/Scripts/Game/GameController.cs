using UnityEngine;
using UnityEngine.SceneManagement;

namespace StudioXP.Scripts.UI
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void ReloadActiveScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void SetPause(bool pause)
        {
            Time.timeScale = pause ? 0 : 1;
        }
    }
}
