using Sirenix.OdinInspector;
using UnityEngine;

namespace StudioXP.Scripts.Characters.AI.Senses
{
    public class VisionSense : ProximitySense
    {
        [SerializeField, MinValue(0), MaxValue(360)] private float fieldOfView = 90;

        public float FieldOfView => fieldOfView;
        
        protected override bool OnIsSensing(AITarget target)
        {
            if (target.gameObject == gameObject) return false;

            var thisTransform = transform;
            var targetDirection = target.transform.position - thisTransform.position;
            
            if (targetDirection.magnitude > Range)
                return false;
            
            var visionVector = Quaternion.Euler(thisTransform.up) * thisTransform.forward;
            var dotProduct = Vector3.Dot(targetDirection.normalized, visionVector);

            return dotProduct >= (180 - fieldOfView) / 180;
        }

        private void OnDrawGizmos()
        {
            if (!ShowDebugInfo) return;
            
            var position = transform.position;
            var thisTransform = gameObject.transform;
            var forward = thisTransform.forward;
            var up = thisTransform.up;
            var right = thisTransform.right;
            var visionVector = Quaternion.Euler(up) * forward * Range;

            var halfRotation = fieldOfView / 2;

            var minVisionVector = Quaternion.Euler(-up * halfRotation) * visionVector;
            var maxVisionVector = Quaternion.Euler(up * halfRotation) * visionVector;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(position, minVisionVector);
            Gizmos.DrawRay(position, maxVisionVector);

            var startPos = position + minVisionVector;
            var appliedSteps = Mathf.Max(1, (int)(0.1f * fieldOfView));
            var stepRotation = fieldOfView / appliedSteps;
            
            for (var i = 0; i < appliedSteps; i++)
            {
                var nextPos = position + (Quaternion.Euler(up * stepRotation * (i + 1)) * minVisionVector);
                Gizmos.DrawLine(startPos, nextPos);
                startPos = nextPos;
            }
        }
    }
}
