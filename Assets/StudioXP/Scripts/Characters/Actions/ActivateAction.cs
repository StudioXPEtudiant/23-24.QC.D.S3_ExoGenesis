using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StudioXP.Scripts.Objects;

namespace StudioXP.Scripts.Characters.Actions
{
    public class ActivateAction : CharacterAction
    {
        public override bool Execute(Interactable interactable)
        {
           
            if (interactable)
            {
                var activable = interactable.GetComponent<Activable>();
                
                if (activable)
                {
                    activable.Activate();
                    return true;
                }
            }
            return false;
        }
    }
}
