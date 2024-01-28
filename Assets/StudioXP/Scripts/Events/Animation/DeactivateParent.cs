using UnityEngine;

namespace StudioXP.Scripts.Events.Animation
{
    public class DeactivateParent : MonoBehaviour
    {
        public void Execute()
        {
            transform.parent.gameObject.SetActive(false);
        }
    }
}
