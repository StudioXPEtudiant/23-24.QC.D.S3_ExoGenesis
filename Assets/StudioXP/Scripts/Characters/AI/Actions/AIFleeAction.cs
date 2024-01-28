using UnityEngine;

namespace StudioXP.Scripts.Characters.AI.Actions
{
    public class AIFleeAction : MonoBehaviour
    {
        [SerializeField] private float minSpeed = 3;
        [SerializeField] private float maxSpeed = 3.5f;
        [SerializeField] private float minRange = 10;
        [SerializeField] private float maxRange = 15;
        [SerializeField] private float randomLocationFactor = 4;
        
        private AIMovement _aiMovement;

        private void Awake()
        {
            _aiMovement = GetComponent<AIMovement>();
        }
        
        public void Flee(AITarget target)
        {
            if (!target || _aiMovement.IsMoving()) return;

            var position = transform.position;
            var oppositeDirection = (position - target.transform.position).normalized;
                
            _aiMovement.SetSpeed(Random.Range(minSpeed, maxSpeed));
            _aiMovement.MoveToRandom(position + oppositeDirection * Random.Range(minRange, maxRange), randomLocationFactor);
        }

        public void Stop(AITarget target)
        {
            _aiMovement.Stop();
        }
    }
}
