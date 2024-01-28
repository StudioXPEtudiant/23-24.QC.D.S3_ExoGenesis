using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StudioXP.Scripts.Characters.AI.Actions
{
    public class AIWanderAction : MonoBehaviour
    {
        [SerializeField] private float minSpeed = 1;
        [SerializeField] private float maxSpeed = 2;
        [SerializeField] private float minRange = 2;
        [SerializeField] private float maxRange = 5;
        [SerializeField] private float minRestTime = 0.5f;
        [SerializeField] private float maxRestTime = 5;
        
        private AIMovement _aiMovement;
        private RestingPhase _resting = RestingPhase.RestingStart;
        
        private bool _isPaused;

        private void Awake()
        {
            _aiMovement = GetComponent<AIMovement>();
        }

        public void Wander()
        {
            if (_isPaused || _aiMovement.IsMoving()) return;

            switch (_resting)
            {
                case RestingPhase.RestingStart:
                    _resting = RestingPhase.Resting;
                    _aiMovement.Stop();
                    StartCoroutine(StopRest());
                    break;
                case RestingPhase.RestingEnd:
                    _resting = RestingPhase.RestingStart;
                
                    _aiMovement.SetSpeed(Random.Range(minSpeed, maxSpeed));
                    _aiMovement.MoveToRandom(transform.position, Random.Range(minRange, maxRange));
                    break;
                case RestingPhase.Resting:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void Pause()
        {
            _isPaused = true;
            _aiMovement.Pause();
        }
        
        public void Resume()
        {
            _isPaused = false;
            _aiMovement.Resume();
        }

        public void Stop()
        {
            _aiMovement.Stop();
        }
        
        private IEnumerator StopRest()
        {
            yield return new WaitForSeconds(Random.Range(minRestTime, maxRestTime));
            _resting = RestingPhase.RestingEnd;
        }
        
        
        private void OnDrawGizmos()
        {
            var position = transform.position;
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(position, minRange);
            Gizmos.DrawWireSphere(position, maxRange);
        }
        
        private enum RestingPhase
        {
            RestingStart,
            Resting,
            RestingEnd
        }
    }
}
