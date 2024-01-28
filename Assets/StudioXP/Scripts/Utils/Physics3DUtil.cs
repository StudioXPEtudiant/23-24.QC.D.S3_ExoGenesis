using Sirenix.Utilities;
using UnityEngine;

namespace StudioXP.Scripts.Utils
{
    public class Physics3DUtil
    {
        public static Color ColorPositive => Color.green;
        public static Color ColorNegative => Color.red;
        
        public static bool DebugMode { get; set; }
        public static bool VerboseMode { get; set; }

        private const int MAXResults = 5;
        private static readonly RaycastHit[] Hits = new RaycastHit[MAXResults];
        private static int _nbCollision = 0;
        
        private static int _currentResult = 0;

        static Physics3DUtil()
        {
#if DEBUG
            DebugMode = true;
#endif
        }
        
        public static Collider GetRayCast(Vector3 center, Vector3 direction, float distance, LayerMask layer,
            float adjustment = 0.02f)
        {
            return TestRayCast(center, direction, distance, layer, adjustment) ? Hits[0].collider : null;
        }

        public static Collider GetNextResult()
        {
            _currentResult++;
            if (_currentResult >= MAXResults || _currentResult >= _nbCollision)
                return null;

            return Hits[_currentResult].collider;
        }

        public static bool TestRayCast(Vector3 center, Vector3 direction, float distance, LayerMask layer, 
            float adjustment = 0.02f)
        {
            _currentResult = 0;
            float distanceTotal = distance + adjustment;
            _nbCollision = Physics.RaycastNonAlloc(center, direction, Hits, distanceTotal, layer);
            Hits.Sort((hit1, hit2) => Vector3.Distance(hit1.point, center).CompareTo(Vector3.Distance(hit2.point, center)));

            bool isColliding = _nbCollision > 0;
            DrawRay(center, direction, distanceTotal, isColliding);

            return isColliding;
        }

        private static void DrawRay(Vector3 center, Vector3 direction, float distance, bool isColliding)
        {
            if (DebugMode)
            {
                Debug.DrawLine(center, center + direction * distance, isColliding ? ColorPositive : ColorNegative);
            }
        }
    }
}
