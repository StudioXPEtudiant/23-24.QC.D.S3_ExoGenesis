using UnityEngine;

namespace StudioXP.Scripts.Characters.AI.Senses
{
    public class SmellSense : ProximitySense
    {
        private void OnDrawGizmos()
        {
            DrawGizmosRange(Color.yellow);
        }
    }
}
