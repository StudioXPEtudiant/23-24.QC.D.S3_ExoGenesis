using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace StudioXP.Scripts.Events
{
    [Serializable]
    public class InputEvent
    {
        [LabelText("Axe")]
        [SerializeField] private string axis;
        
        [LabelText("Est un bouton")]
        [SerializeField] private bool isButton;
        
        [FoldoutGroup("Events", false)]
        [LabelText("Bouton Appuyé")]
        [ShowIf("isButton")]
        [SerializeField] private UnityEvent buttonDown;
        
        [FoldoutGroup("Events")]
        [LabelText("Bouton Relâché")]
        [ShowIf("isButton")]
        [SerializeField] private UnityEvent buttonUp;
        
        [FoldoutGroup("Events")]
        [LabelText("Bouton")]
        [ShowIf("isButton")]
        [SerializeField] private UnityEvent button;
        
        [FoldoutGroup("Events")]
        [LabelText("Envoyer la valeur de l'axe")]
        [HideIf("isButton")]
        [SerializeField] private FloatEvent sendAxisValue;

        public void Update()
        {
            if (isButton)
            {
                if(Input.GetButtonDown(axis))
                    buttonDown.Invoke();
                
                if(Input.GetButton(axis))
                    button.Invoke();
                
                if(Input.GetButtonUp(axis))
                    buttonUp.Invoke();
            }
            else
            {
                sendAxisValue.Invoke(Input.GetAxis(axis));
            }
        }
    }
}
