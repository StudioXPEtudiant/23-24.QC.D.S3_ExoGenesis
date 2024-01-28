using UnityEngine;

namespace StudioXP.Scripts.Utils
{
    public class CoroutineInvoker : MonoBehaviour
    {
        public static CoroutineInvoker Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }
    }
}
