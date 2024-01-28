using UnityEngine;

namespace StudioXP.Scripts.Characters.Speech
{
    public class NPCDialogueCollection : MonoBehaviour
    {
        [SerializeField] private string defaultNPCName = "Inconnu";
        [SerializeField] private NPCDialogue[] dialogues;

        private NPCDialogue _currentDialogue;

        public void Execute()
        {
            foreach (var dialogue in dialogues)
            {
                if (!dialogue.IsAvailable)
                {
                    _currentDialogue = null;
                    continue;
                }
                
                dialogue.ShowNextLine(defaultNPCName);
                _currentDialogue = dialogue;
                return;
            }
        }

        public void Cancel()
        {
            _currentDialogue?.Cancel();
        }
    }
}
