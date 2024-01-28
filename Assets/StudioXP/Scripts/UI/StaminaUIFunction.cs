using UnityEngine;

namespace StudioXP.Scripts.UI
{
    public class StaminaUIFunction : RadialValueUIFunction
    {
        public override void SetValue(float value)
        {
            StaminaUI.Instance.Value = value;
        }

        public override void SetMaxValue(float maxValue)
        {
            StaminaUI.Instance.MaxValue = maxValue;
        }
    }
}
