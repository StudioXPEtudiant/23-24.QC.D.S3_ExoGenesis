using UnityEngine;

namespace StudioXP.Scripts.UI
{
    public class DialogueUIFunction : MonoBehaviour
    {
        private string _tmpName = "Inconnu";
        
        public void Hide()
        {
            DialogueUIController.Instance.Hide();
        }
        
        public void SetName(string name)
        {
            _tmpName = name;
        }

        public void Show(string dialogue)
        {
            DialogueUIController.Instance.Show(_tmpName, dialogue);
        }
    }
}
