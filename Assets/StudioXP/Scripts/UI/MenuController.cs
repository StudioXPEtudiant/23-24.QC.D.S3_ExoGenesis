using UnityEngine;

namespace StudioXP.Scripts.UI
{
    public class MenuController : MonoBehaviour
    {
        public static MenuController Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        
        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void ReloadActiveScene()
        {
            GameController.Instance.ReloadActiveScene();
        }

        public void Quit()
        {
            GameController.Instance.Quit();
        }
    }
}
