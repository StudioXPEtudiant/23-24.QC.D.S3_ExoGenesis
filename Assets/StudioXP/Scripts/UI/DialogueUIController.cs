using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace StudioXP.Scripts.UI
{
    public class DialogueUIController : MonoBehaviour
    {
        public static DialogueUIController Instance { get; private set; }

        [SerializeField] private TextMeshProUGUI characterName;
        [SerializeField] private TextMeshProUGUI dialogue;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void Hide()
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        public void Show(string nameToDisplay, string dialogueToDisplay)
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(true);

            characterName.text = nameToDisplay;
            dialogue.text = dialogueToDisplay;
        }
    }
}
