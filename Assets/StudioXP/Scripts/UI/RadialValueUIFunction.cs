using UnityEngine;

namespace StudioXP.Scripts.UI
{
    public abstract class RadialValueUIFunction : MonoBehaviour
    {
        public abstract void SetValue(float value);
        public abstract void SetMaxValue(float maxValue);
    }
}
