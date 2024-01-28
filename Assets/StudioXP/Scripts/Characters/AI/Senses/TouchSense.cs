using System;
using UnityEngine;

namespace StudioXP.Scripts.Characters.AI.Senses
{
    public class TouchSense : ProximitySense
    {
        private void OnDrawGizmos()
        {
            DrawGizmosRange(Color.black);
        }
    }
}
