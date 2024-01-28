using System;
using UnityEngine;

namespace StudioXP.Scripts.Animation
{
    public class ShakeAnimation : MonoBehaviour
    {
        [SerializeField] private float duration = 2;
        [SerializeField] private Vector3 positionAmplitude = Vector3.zero;
        [SerializeField] private Vector3 positionSpeed = Vector3.zero;
        [SerializeField] private Vector3 rotationAmplitude = Vector3.zero;
        [SerializeField] private Vector3 rotationSpeed = Vector3.zero;

        private Vector3 _initialPosition;
        private Quaternion _initialRotation;
        private bool _isPlaying;
        private float _shakeTime;
        private float _shakeDuration;

        public void Play()
        {
            if (!_isPlaying)
                _shakeTime = 0;
            
            _isPlaying = true;
            _shakeDuration = 0;
        }

        private void Awake()
        {
            _initialPosition = transform.localPosition;
            _initialRotation = transform.localRotation;
        }

        private void Update()
        {
            if (!_isPlaying) return;
            
            _shakeTime += Time.deltaTime;
            _shakeDuration += Time.deltaTime;

            if (_shakeDuration >= duration)
            {
                _isPlaying = false;
                _shakeTime = 0;
                _shakeDuration = 0;
            }
            
            transform.localPosition = _initialPosition + GetShake(positionAmplitude, positionSpeed);
            transform.localRotation = _initialRotation;
            transform.Rotate(GetShake(rotationAmplitude, rotationSpeed));
        }

        private Vector3 GetShake(Vector3 amplitude, Vector3 speed)
        {
            return new Vector3(
                amplitude.x * Mathf.Sin(_shakeTime * speed.x),
                amplitude.y * Mathf.Sin(_shakeTime * speed.y),
                amplitude.z * Mathf.Sin(_shakeTime * speed.z)
            );
        }
    }
}
