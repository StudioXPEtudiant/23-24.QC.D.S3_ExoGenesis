using StudioXP.Scripts.Events;
using StudioXP.Scripts.Objects;
using StudioXP.Scripts.Utils;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable MergeConditionalExpression

namespace StudioXP.Scripts.Characters
{
    public class LookAt : MonoBehaviour
    {
        [SerializeField] private float reach = 3;
        [SerializeField] private LayerMask layers;
        [FormerlySerializedAs("onLookAtChanged")] [SerializeField] private InteractableEvent lookAtChanged;

        private Interactable _interactable;

        private void Update()
        {
            var goTransform = transform;
            var rayRes = Physics3DUtil.GetRayCast(goTransform.position, goTransform.forward, reach, layers);

            var lookable = ReferenceEquals(rayRes, null) ? null : rayRes.GetComponent<Interactable>();

            if (lookable == _interactable) return;
            
            if (_interactable)
                _interactable.StopLookingAt();

            _interactable = lookable;

            if (_interactable)
                _interactable.StartLookingAt();

            lookAtChanged.Invoke(_interactable);
        }
    }
}
