using Sirenix.OdinInspector;
using StudioXP.Scripts.Objects;
using UnityEngine;

namespace StudioXP.Scripts.Characters.Actions
{
    public class DropAction : CharacterAction
    {
        [SerializeField] private Hand hand;

        private void Start()
        {
            if (hand == null)
                hand = gameObject.GetComponent<Hand>();
        }

        public override bool Execute(Interactable interactable)
        {
            var pickable = hand.Inventory.GetCurrentItem();
            if (pickable == null) return false;
            
            if (interactable) 
            {
                var storageSlot = interactable.GetComponent<StorageSlot>();
                var storable = pickable.GetComponent<Storable>();
                if (storageSlot && storable)
                {
                    return Store(pickable, storable, storageSlot); ;
                }
            }
            Drop(pickable);
            return true;
        }

        private void Drop(Pickable pickable)
        {
            if (pickable == hand.Inventory.GetCurrentItem())
                hand.Inventory.RemoveCurrentSlot();
            if (pickable == null) return;
            
            var pickableRigidbody = pickable.GetComponent<Rigidbody>();
            pickableRigidbody.useGravity = true;
            pickableRigidbody.isKinematic = false;

            foreach (var col in pickable.GetComponents<Collider>()) 
                col.enabled = true;

            pickable.transform.parent = null;
            pickable.Drop();
        }
        
        private bool Store(Pickable pickable, Storable storable, StorageSlot slot)
        {
            if (!storable || !slot) return false;

            if (slot.GetComponentInChildren<Storable>()) return false;

            if (!slot.canStore(storable)) return false;

            Drop(pickable);

            var objectTransform = storable.transform;
            objectTransform.parent = slot.transform;
            objectTransform.localRotation = storable.StorageRotation;
            objectTransform.localPosition = storable.StoragePosition;
            objectTransform.localScale = storable.StorageScale;

            var objectRigidbody = storable.GetComponent<Rigidbody>();
            objectRigidbody.useGravity = false;
            objectRigidbody.isKinematic = true;

            if (hand.Animator)
                hand.Animator.runtimeAnimatorController = null;

            foreach (var col in storable.GetComponents<Collider>())
                col.enabled = true;

            
            return true;
        }
    }
}
