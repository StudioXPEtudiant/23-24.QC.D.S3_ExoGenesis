using System.Linq;
using StudioXP.Scripts.Game;
using UnityEngine;
using UnityEngine.Events;

namespace StudioXP.Scripts.Objects
{
    public class ConditionalAction : MonoBehaviour
    {
        [SerializeField] private GameFlag[] requiredFlags;
        [SerializeField] private UnityEvent action;
        
        public bool IsAvailable => requiredFlags.All(flag => GameFlagCollection.Instance.IsTriggered(flag));

        public void Execute()
        {
            if (!IsAvailable) return;
            action.Invoke();
        }
    }
}
