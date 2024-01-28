using System.Collections;
using System.Collections.Generic;
using StudioXP.Scripts.Characters;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


namespace StudioXP.Scripts.Objects
{
    public class Pickable : MonoBehaviour
    {

        [SerializeField] private Vector3 pickupPosition;
        [SerializeField] private Quaternion pickupRotation;
        [FormerlySerializedAs("onPickup")] [SerializeField] private UnityEvent pickedUp;
        [FormerlySerializedAs("onDrop")] [SerializeField] private UnityEvent dropped;

        public Vector3 PickupPosition => pickupPosition;

        public Quaternion PickupRotation => pickupRotation;
        
        private bool _isPickedUp;

        public bool IsOnHand()
        {
            return _isPickedUp;
        }

        private void Awake()
        {
            if (transform.parent == null)
                return;

            _isPickedUp = transform.parent.GetComponent<Hand>();
        }

        public void Pickup()
        {
            _isPickedUp = true;
            pickedUp.Invoke();
        }

        public void Drop()
        {
            _isPickedUp = false;
            dropped.Invoke();
        }
    }
}
