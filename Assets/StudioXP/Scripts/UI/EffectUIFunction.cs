using UnityEngine;

namespace StudioXP.Scripts.UI
{
    public class EffectUIFunction : MonoBehaviour
    {
        public void Play(string identifier)
        {
            EffectUIController.Instance.Play(identifier);
        }

        public void Play(string identifier, float duration)
        {
            EffectUIController.Instance.Play(identifier, duration);
        }
    }
}
