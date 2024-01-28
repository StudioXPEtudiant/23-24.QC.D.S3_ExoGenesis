using UnityEngine;

namespace StudioXP.Scripts.UI
{
    public class HealthUIFunction : RadialValueUIFunction
    {
        public override void SetValue(float value)
        {
            HealthUI.Instance.Value = value;
        }

        public override void SetMaxValue(float maxValue)
        {
            HealthUI.Instance.MaxValue = maxValue;
        }
    }
}
