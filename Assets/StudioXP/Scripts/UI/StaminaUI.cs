using UnityEngine;

namespace StudioXP.Scripts.UI
{
    public class StaminaUI : RadialValueUI
    {
        public static StaminaUI Instance { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            
            if (Instance == null)
                Instance = this;
        }
    }
}
