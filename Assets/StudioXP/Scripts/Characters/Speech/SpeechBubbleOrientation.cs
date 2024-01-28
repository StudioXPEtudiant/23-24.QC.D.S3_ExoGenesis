using UnityEngine;

namespace StudioXP.Scripts.Characters.Speech
{
    public class SpeechBubbleOrientation : MonoBehaviour
    {
        private Transform _cameraTransform;
        
        void Start()
        {
            var main = UnityEngine.Camera.main;
            if (main)
                _cameraTransform = main.transform;
        }
        
        void Update()
        {
            transform.forward = _cameraTransform.forward;
        }
    }
}
