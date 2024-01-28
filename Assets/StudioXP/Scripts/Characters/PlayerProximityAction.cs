using System;
using UnityEngine;
using UnityEngine.Events;

namespace StudioXP.Scripts.Characters
{
    public class PlayerProximityAction : MonoBehaviour
    {
        [SerializeField] private float distance;
        [SerializeField] private UnityEvent playerEnter;
        [SerializeField] private UnityEvent playerExit;

        private GameObject _player;
        private bool _playerIsInside;
        
        void Start()
        {
            _player = GameObject.FindWithTag("Player");
        }
        
        void Update()
        {
            var playerIsClose =
                Vector3.Distance(gameObject.transform.position, _player.transform.position) <= distance;
            
            if (!_playerIsInside)
            {
                if (!playerIsClose) return;
                
                _playerIsInside = true;
                playerEnter.Invoke();
            }
            else
            {
                if (playerIsClose) return;
                
                _playerIsInside = false;
                playerExit.Invoke();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, distance);
        }
    }
}
