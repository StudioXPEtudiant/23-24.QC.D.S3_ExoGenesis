using System.Collections;
using System.Collections.Generic;
using StudioXP.Scripts.Characters;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace StudioXP.Scripts.Objects
{
    public class Activable : MonoBehaviour
    {
        [FormerlySerializedAs("onActivation")] [SerializeField] private UnityEvent onActivation;

        public void Activate()
        {
            onActivation.Invoke();
        }
    }
}
