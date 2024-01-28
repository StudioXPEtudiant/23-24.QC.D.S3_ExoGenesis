using System.Linq;
using StudioXP.Scripts.Objects;
using UnityEngine;

namespace StudioXP.Scripts.Characters
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class Hand : MonoBehaviour
    {
        [SerializeField] private Tool defaultTool;
        [SerializeField] private Inventory inventory;

        private AudioSource _audioSource;
        private int _animatorHit;

        private Interactable _interactableLookedAt;
        
        public Inventory Inventory => inventory;

        public Tool CurrentTool {
            get
            {
                var objectInHand = inventory.GetCurrentItem();
                Tool toolInHand = null;

                if (objectInHand)
                    toolInHand = objectInHand.GetComponent<Tool>();

                if (!toolInHand)
                    toolInHand = defaultTool;

                return toolInHand;
            }
        }

        public Animator Animator { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _animatorHit = Animator.StringToHash("Hit");
        }

        public void SetInteractable(Interactable interactable)
        {
            _interactableLookedAt = interactable;
        }

        public void ExecuteAction()
        {
            if (CurrentTool.PlayAnimationOnAction)
            {
                AnimateAction();
                return;
            }

            AnimationTrigger();
        }

        public void ExecuteSecondaryAction()
        {
            if (CurrentTool.SecondaryActions.Any(action => action.Execute(_interactableLookedAt))) 
                return;
            
            foreach (var action in defaultTool.SecondaryActions)
            {
                if (action.Execute(_interactableLookedAt))
                    break;
            }
        }

        public void AnimationTrigger()
        {
            foreach (var action in CurrentTool.Actions)
            {
                if (action.Execute(_interactableLookedAt))
                    break;
            }
        }
        
        private void AnimateAction()
        {
            if (Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == _animatorHit) return;
            Animator.SetTrigger(_animatorHit);
            _audioSource.Play();
        }
    }
}
