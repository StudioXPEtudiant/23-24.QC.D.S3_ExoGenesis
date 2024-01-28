using System;
using Sirenix.OdinInspector;
using StudioXP.Scripts.Objects;
using UnityEngine;

namespace StudioXP.Scripts.Characters.Actions
{
    public class PickupAction : CharacterAction
    {
        [SerializeField] private Hand hand;

        private void Start()
        {
            if (hand == null)
                hand = gameObject.GetComponent<Hand>();
        }

        public override bool Execute(Interactable interactable)
        {
            if (!interactable) return false;
            
            var pickable = interactable.GetComponent<Pickable>();
            if (!pickable) return false;
            
            Pickup(pickable);
            return true;
        }
        
        public void Pickup(Pickable pickable)
        {
            var objectTransform = pickable.transform;
            if (!hand.Inventory.TrySetItemInEmptySlot(pickable.GetComponent<Pickable>()))
                return;
            
            objectTransform.parent = hand.Inventory.Root.transform;
            objectTransform.localRotation = pickable.PickupRotation;
            objectTransform.localPosition = pickable.PickupPosition;
            objectTransform.localScale = Vector3.one;

            var objectRigidbody = pickable.GetComponent<Rigidbody>();
            objectRigidbody.useGravity = false;
            objectRigidbody.isKinematic = true;

            if (hand.Animator)
            {
                var tool = pickable.GetComponent<Tool>();
                if(tool)
                    hand.Animator.runtimeAnimatorController = tool.AnimatorController;
            }
            
            foreach (var col in pickable.GetComponents<Collider>())
                col.enabled = false;
            
            pickable.Pickup();
        }
    }
}
