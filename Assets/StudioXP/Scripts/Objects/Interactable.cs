using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


namespace StudioXP.Scripts.Objects
{
    public class Interactable : MonoBehaviour
    {
        [FormerlySerializedAs("onStartLookAt")] [SerializeField] private UnityEvent startedLookingAt;
        [FormerlySerializedAs("onStopLookAt")] [SerializeField] private UnityEvent stoppedLookingAt;
        
        public void StartLookingAt()
        {
            var pick = GetComponent<Pickable>();
            if (!pick || !pick.IsOnHand())
                startedLookingAt.Invoke();
        }

        public void StopLookingAt()
        {
            var pick = GetComponent<Pickable>();
            if (!pick || !pick.IsOnHand())
                stoppedLookingAt.Invoke();
        }
    }
}
