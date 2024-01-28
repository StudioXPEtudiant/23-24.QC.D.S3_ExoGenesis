using System;
using System.Linq;
using Sirenix.OdinInspector;
using StudioXP.Scripts.Game;
using StudioXP.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace StudioXP.Scripts.Characters.Speech
{
    [Serializable]
    public class NPCDialogue
    {
        [FoldoutGroup("Configuration"), SerializeField] private bool isRepeatable;
        [FoldoutGroup("Configuration"), SerializeField] private bool isRandomized;
        [FoldoutGroup("Configuration"), SerializeField] private GameFlag[] requiredFlags;
        
        [FoldoutGroup("Dialogue"), SerializeField] private NPCDialogueLine[] lines;
        
        [FoldoutGroup("Events"), SerializeField] private UnityEvent dialogueStart;
        [FoldoutGroup("Events"), SerializeField] private UnityEvent dialogueContinue;
        [FoldoutGroup("Events"), SerializeField] private UnityEvent dialogueEnd;

        private int _currentLine;
        private bool _isActive;
        private bool _isCompleted;

        public bool IsAvailable => !_isCompleted && requiredFlags.All(flag => GameFlagCollection.Instance.IsTriggered(flag));

        public bool ShowNextLine(string defaultNPCName)
        {
            if (!IsAvailable)
                return false;
            
            if (!_isActive)
                Init();

            if (_currentLine >= lines.Length)
            {
                if (!isRepeatable)
                    _isCompleted = true;

                _isActive = false;
                DialogueUIController.Instance.Hide();
                dialogueEnd.Invoke();
                return false;
            }

            if (isRandomized)
            {
                _currentLine = lines.Length;
                dialogueContinue.Invoke();
                ShowLine(defaultNPCName, lines[Random.Range(0, lines.Length)]);
            }
            else
            {
                if(_currentLine > 0)
                    dialogueContinue.Invoke();
                
                ShowLine(defaultNPCName, lines[_currentLine++]);
            }
            
            return true;
        }

        public void Cancel()
        {
            _isActive = false;
            DialogueUIController.Instance.Hide();
        }

        private void Init()
        {
            _currentLine = 0;
            _isActive = true;
            dialogueStart.Invoke();
        }

        private static void ShowLine(string defaultNPCName, NPCDialogueLine line)
        {
            DialogueUIController.Instance.Show(string.IsNullOrEmpty(line.name) ? defaultNPCName : line.name, line.line);
        }
    }
}
