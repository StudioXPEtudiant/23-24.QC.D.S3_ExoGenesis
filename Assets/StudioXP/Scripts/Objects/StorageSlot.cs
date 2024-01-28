using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace StudioXP.Scripts.Objects
{
    

    public class StorageSlot : MonoBehaviour
    {

        public List<String> valideType;
        [SerializeField] private UnityEvent onSlotChange;
        [SerializeField] private UnityEvent onSlotIn;
        [SerializeField] private UnityEvent onSlotOut;

        [SerializeField] private Transform dropPosition;

        private int oldChildCount;

        public void Start()
        {
            oldChildCount = transform.childCount;
        }

        public bool canStore(Storable item)
        {
            if (valideType.Count > 0)
            {
                foreach (String type in item.StorageTypes)
                {
                    if (valideType.Contains(type))
                    {
                        return true;
                    }
                }

                return false;
            }
            return true;
        }

        public void DropSlot()
        {
            foreach (Storable child in GetComponentsInChildren<Storable>())
            {
                var pickableRigidbody = child.GetComponent<Rigidbody>();
                pickableRigidbody.useGravity = true;
                pickableRigidbody.isKinematic = false;

                foreach (var col in child.GetComponents<Collider>())
                    col.enabled = true;

                child.transform.localScale = Vector3.one;
                
                child.transform.SetParent( dropPosition,true);
                if (dropPosition)
                    child.transform.localPosition = Vector3.zero;

             //   child.transform.SetParent(null, true);



            }
        }

        public void ClearSlot()
        {
            foreach (Storable child in GetComponentsInChildren<Storable>())
            { 
                    GameObject.Destroy(child.gameObject);
            }
        }

        private void OnTransformChildrenChanged()
        {
            onSlotChange.Invoke();

            if(oldChildCount < transform.childCount)
            {
                onSlotIn.Invoke();
            }
            else if (oldChildCount > transform.childCount)
            {
                onSlotOut.Invoke();
            }
            oldChildCount = transform.childCount;

        }



    }
}
