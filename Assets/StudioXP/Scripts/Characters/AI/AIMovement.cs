using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace StudioXP.Scripts.Characters.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIMovement : MonoBehaviour
    {
        [SerializeField] private float navMeshSearchRange = 1.0f;
        [SerializeField] private string walkAnimation = "isWalking";
        
        private Animator _animator;
        private int _walkAnimationId;
        
        private NavMeshAgent _agent;
        private NavMeshHit _hit;

        public bool IsMoving() => _agent.hasPath && !_agent.isStopped;

        private bool _previousWalkingAnimation = false;

        public void Pause()
        {
            _agent.isStopped = true;
            _previousWalkingAnimation = _animator.GetBool(_walkAnimationId);
            _animator.SetBool(_walkAnimationId, false);
        }

        public void Resume()
        {
            _agent.isStopped = false;
            _animator.SetBool(_walkAnimationId, _previousWalkingAnimation);
        }

        public void Stop()
        {
            _agent.isStopped = true;
            _animator.SetBool(_walkAnimationId, false);
        }

        public void MoveToRandom(Vector3 center, float range)
        {
            for (var i = 0; i < 30; i++)
            {
                var randomPoint = center + Random.insideUnitSphere * range;
                if (!NavMesh.SamplePosition(randomPoint, out _hit, navMeshSearchRange, NavMesh.AllAreas)) continue;
                
                MoveTo(_hit.position);
                return;
            }
        }

        public void MoveTo(Vector3 position)
        {
            _agent.SetDestination(position);
            _agent.isStopped = false;
            
            if(_animator)
                _animator.SetBool(_walkAnimationId, true);
        }

        public void SetSpeed(float speed)
        {
            _agent.speed = speed;
        }
        
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _walkAnimationId = Animator.StringToHash(walkAnimation);
            _animator = GetComponent<Animator>();
        }

        private void OnDrawGizmosSelected()
        {
            if (_agent == null) return;
            if (!IsMoving()) return;
            
            var corners = _agent.path.corners;
            var size = corners.Length;

            for (var i = 1; i < size; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(corners[i - 1], corners[i]);
            }
        }
    }
}
