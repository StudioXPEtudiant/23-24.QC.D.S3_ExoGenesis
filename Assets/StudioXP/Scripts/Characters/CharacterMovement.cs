using StudioXP.Scripts.Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace StudioXP.Scripts.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private GameObject head;
        [SerializeField] private GameObject mainHand;
        [SerializeField] private bool hasGravity = true;
        [FormerlySerializedAs("minSpeed")] [SerializeField] private float walkSpeed = 4;
        [FormerlySerializedAs("maxSpeed")] [SerializeField] private float runSpeed = 6;
        [SerializeField] private float maxStamina = 50;
        [SerializeField] private float staminaUsePerSec = 10;
        [SerializeField] private float staminaRegenPerSec = 5;
        [SerializeField] private float minTilt = 30;
        [SerializeField] private float maxTilt = 150;
        [SerializeField] private float mouseSensibility = 1;
        [SerializeField] private FloatEvent movedWithSpeed;
        [SerializeField] private FloatEvent staminaChanged;
        [SerializeField] private FloatEvent maxStaminaChanged;

        private CharacterController _characterController;

        private float _headTilt = 90;
        private float _stamina;
        
        private Vector3 _movementX;
        private Vector3 _movementZ;

        private bool _isRunning;
        private bool _canRun;
        
        void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _stamina = maxStamina;
            _canRun = true;
            movedWithSpeed.Invoke(walkSpeed);
            maxStaminaChanged.Invoke(maxStamina);
            staminaChanged.Invoke(_stamina);
        }
        
        private void Update()
        {
            if(hasGravity)
                _characterController.Move(Physics.gravity * Time.deltaTime);

            if (!_isRunning)
            {
                if (_stamina < maxStamina)
                {
                    var staminaRegen = Time.deltaTime * staminaRegenPerSec;
                    _stamina += staminaRegen;
                    if (_stamina >= maxStamina)
                    {
                        _stamina = maxStamina;
                        _canRun = true;
                    }
                    staminaChanged.Invoke(_stamina);
                }
            }

            var movement = (_movementX + _movementZ).normalized;
            if (movement.magnitude == 0)
            {
                movedWithSpeed.Invoke(0);
                return;
            }

            var currentSpeed = walkSpeed;
            if (_isRunning)
            {
                var staminaCost = Time.deltaTime * staminaUsePerSec;
                if (staminaCost <= _stamina)
                {
                    _stamina -= staminaCost;
                    currentSpeed = runSpeed;
                }
                else
                {
                    _canRun = false;
                    _stamina = 0;
                }
                
                staminaChanged.Invoke(_stamina);
            }
            
            _characterController.Move(movement * (currentSpeed * Time.deltaTime));
            movedWithSpeed.Invoke(currentSpeed);
        }

        public void SetRunning(bool isRunning)
        {
            _isRunning = isRunning;
            if (_isRunning && !_canRun)
                _isRunning = false;
        }

        public void SetWalkSpeed(float speed)
        {
            if (speed < 0)
                walkSpeed = 0;
            else if (speed > runSpeed)
                walkSpeed = runSpeed;
            else
                walkSpeed = speed;
        }

        public void SetRunSpeed(float speed)
        {
            runSpeed = speed < walkSpeed ? walkSpeed : speed;
        }

        public void SetMovementX(float movement)
        {
            _movementX = transform.right * movement;
        }
        
        public void SetMovementZ(float movement)
        {
            _movementZ = transform.forward * movement;
        }

        public void TiltHead(float tilt)
        {
            if (tilt == 0)
                return;

            tilt = -tilt;
            
            _headTilt += tilt * mouseSensibility;
            
            _headTilt %= 360;
            if (_headTilt < 0)
                _headTilt += 360;
            
            if (_headTilt > maxTilt)
                _headTilt = maxTilt;
            else if (_headTilt < minTilt)
                _headTilt = minTilt;

            head.transform.localRotation = Quaternion.Euler(_headTilt - 90, 0, 0);
        }

        public void RotateY(float rotation)
        {
            transform.Rotate(0, rotation * mouseSensibility, 0);
        }
    }
}
