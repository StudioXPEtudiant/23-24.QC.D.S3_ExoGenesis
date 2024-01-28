using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace StudioXP.Scripts.Camera
{
    public class Bobbing : MonoBehaviour
    {   
        [SerializeField] private bool idleBobbing = false;
        [SerializeField] private float idleVelocity = 1;
        [SerializeField] private float movingMultiplicator = 1;

        [SerializeField] private Vector3 positionAmplitude = Vector3.zero;
        [SerializeField] private Vector3 positionSpeed = Vector3.zero;
        [SerializeField] private Vector3 rotationAmplitude = Vector3.zero;
        [SerializeField] private Vector3 rotationSpeed = Vector3.zero;

        [SerializeField] private bool launchMinAmplitudeEvent = false;
        [SerializeField] private bool launchMaxAmplitudeEvent = false;
        [SerializeField] private UnityEvent targetXAmplitudeReached;
        [SerializeField] private UnityEvent targetYAmplitudeReached;
        [SerializeField] private UnityEvent targetZAmplitudeReached;

        private float _bobbingTime;
        private Vector3 _positionBobbingCos = Vector3.zero;

        public void Apply(float velocity)
        {
            if (velocity == 0)
            {
                if (!idleBobbing)
                    return;
                
                velocity = idleVelocity;
            }
            else
                velocity *= movingMultiplicator;
            
            _bobbingTime += Time.deltaTime;
            var positionBobbingSin = GetBobbingSin(positionAmplitude, positionSpeed, velocity);
            var positionBobbingCos = GetBobbingCos(positionAmplitude, positionSpeed, velocity);
            
            transform.localPosition = positionBobbingSin;
            transform.localRotation = Quaternion.Euler(GetBobbingSin(rotationAmplitude, rotationSpeed, velocity));

            if (_positionBobbingCos.x * positionBobbingCos.x < 0)
            {
                if((launchMinAmplitudeEvent && positionBobbingSin.x < 0) || (launchMaxAmplitudeEvent && positionBobbingSin.x > 0))
                    targetXAmplitudeReached.Invoke();
            }
            
            if (_positionBobbingCos.y * positionBobbingCos.y < 0)
            {
                if((launchMinAmplitudeEvent && positionBobbingSin.y < 0) || (launchMaxAmplitudeEvent && positionBobbingSin.y > 0))
                    targetYAmplitudeReached.Invoke();
            }
            
            if (_positionBobbingCos.z * positionBobbingCos.z < 0)
            {
                if((launchMinAmplitudeEvent && positionBobbingSin.z< 0) || (launchMaxAmplitudeEvent && positionBobbingSin.z > 0))
                    targetZAmplitudeReached.Invoke();
            }
            
            _positionBobbingCos = positionBobbingCos;
        }

        private Vector3 GetBobbingSin(Vector3 amplitude, Vector3 speed, float velocity)
        {
            return new Vector3(
                amplitude.x * Mathf.Sin(_bobbingTime * velocity * speed.x),
                amplitude.y * Mathf.Sin(_bobbingTime * velocity * speed.y),
                amplitude.z * Mathf.Sin(_bobbingTime * velocity * speed.z)
            );
        }
        
        private Vector3 GetBobbingCos(Vector3 amplitude, Vector3 speed, float velocity)
        {
            return new Vector3(
                amplitude.x * Mathf.Cos(_bobbingTime * velocity * speed.x),
                amplitude.y * Mathf.Cos(_bobbingTime * velocity * speed.y),
                amplitude.z * Mathf.Cos(_bobbingTime * velocity * speed.z)
            );
        }
    }
}
