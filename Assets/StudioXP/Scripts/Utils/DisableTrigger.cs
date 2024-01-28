using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace StudioXP.Scripts.Utils
{

   

    public class DisableTrigger : MonoBehaviour
    {
        [FormerlySerializedAs("onDisable")] [SerializeField] private UnityEvent onDisable;

        private void OnDisable()
        {
            onDisable.Invoke();
        }
    }
}
