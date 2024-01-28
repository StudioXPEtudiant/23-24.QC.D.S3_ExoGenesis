using System.Collections.Generic;
using StudioXP.Scripts.Characters.Actions;
using UnityEngine;
using UnityEngine.Serialization;

namespace StudioXP.Scripts.Objects
{
    public class Tool : MonoBehaviour
    {
        [FormerlySerializedAs("hasAnimation")] [SerializeField] private bool playAnimationOnAction = true;
        [SerializeField] private RuntimeAnimatorController animatorController;
        
        [SerializeField] private CharacterAction[] actions;
        [SerializeField] private CharacterAction[] secondaryActions;

        public IEnumerable<CharacterAction> Actions => actions;

        public IEnumerable<CharacterAction> SecondaryActions => secondaryActions;
        public bool PlayAnimationOnAction => playAnimationOnAction;
        public RuntimeAnimatorController AnimatorController => animatorController;
    }
}
