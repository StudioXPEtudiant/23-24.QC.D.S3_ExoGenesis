using System;
using UnityEngine;

namespace StudioXP.Scripts.Characters.AI.Actions
{
    public class AIChaseAction : MonoBehaviour
    {
        [SerializeField] private float slowSpeed = 2;
        [SerializeField] private float fastSpeed = 3;
        
        private AIMovement _aiMovement;

        private void Awake()
        {
            _aiMovement = GetComponent<AIMovement>();
        }

        public void ChaseSlow(AITarget target)
        {
            _aiMovement.SetSpeed(slowSpeed);
            _aiMovement.MoveTo(target.transform.position);
        }

        public void ChaseFast(AITarget target)
        {
            _aiMovement.SetSpeed(fastSpeed);
            _aiMovement.MoveTo(target.transform.position);
        }

        public void Stop(AITarget target)
        {
            _aiMovement.Stop();
        }
    }
}
