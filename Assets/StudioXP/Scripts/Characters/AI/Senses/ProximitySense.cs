using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace StudioXP.Scripts.Characters.AI.Senses
{
    public class ProximitySense : Sense
    {
        [SerializeField] private bool showDebugInfo = false;
        [SerializeField, MinValue(0)] private float range = 20;

        public bool ShowDebugInfo => showDebugInfo;
        public float Range => range;

        protected override bool OnIsSensing(AITarget target)
        {
            if (target.gameObject == gameObject) return false;

            var targetDirection = target.transform.position - transform.position;

            return targetDirection.magnitude <= range;
        }

        protected void DrawGizmosRange(Color color)
        {
            if (!showDebugInfo) return;

            Gizmos.color = color;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
