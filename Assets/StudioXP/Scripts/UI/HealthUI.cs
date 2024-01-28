using UnityEngine;

namespace StudioXP.Scripts.UI
{
    public class HealthUI : RadialValueUI
    {
        public static HealthUI Instance { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            
            if (Instance == null)
                Instance = this;
        }
    }
}
