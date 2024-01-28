using UnityEngine;

namespace StudioXP.Scripts.Events.Animation
{
    public class DestroyParent : MonoBehaviour
    {
        public void Execute()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
