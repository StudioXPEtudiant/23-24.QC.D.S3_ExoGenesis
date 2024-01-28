using UnityEngine;
using UnityEngine.Rendering;

namespace StudioXP.Scripts.Game
{
    public class DaylightCycle : MonoBehaviour
    {
        [SerializeField] private Light sun;
        [SerializeField] private Light moon;
        [SerializeField] private float dayLength;
        [SerializeField] private float nightLength;
        [SerializeField] private float dawnRotation = -20;
        [SerializeField] private float duskRotation = 200;

        private float _dayIncPerMS;
        private float _nightIncPerMS;

        private LensFlareCommonSRP _lensFlare;
        
        void Start()
        {
            _dayIncPerMS = duskRotation - dawnRotation;
        }
        
        void Update()
        {
            
        }
    }
}
