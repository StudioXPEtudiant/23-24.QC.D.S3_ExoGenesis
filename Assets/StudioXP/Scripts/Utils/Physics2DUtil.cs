using UnityEngine;

namespace StudioXP.Scripts.Utils
{
    public static class Physics2DUtil
    {
        public static Color ColorPositive => Color.green;
        public static Color ColorNegative => Color.red;
        
        public static bool DebugMode { get; set; }
        public static bool VerboseMode { get; set; }

        private const int MAXResults = 5;
        private static readonly RaycastHit2D[] Hits = new RaycastHit2D[MAXResults];
        private static int _nbCollision = 0;
        
        private static int _currentResult = 0;

        static Physics2DUtil()
        {
#if DEBUG
            DebugMode = true;
#endif
        }
        
        public static Collider2D GetRayCast(Vector2 center, Vector2 direction, float distance, LayerMask layer,
            float adjustment = 0.02f)
        {
            return TestRayCast(center, direction, distance, layer, adjustment) ? Hits[0].collider : null;
        }
        
        public static Collider2D GetBoxCast(Vector2 center, Vector2 size, Vector2 direction, float distance, LayerMask layer, 
            float adjustment = 0.02f)
        {
            return TestBoxCast(center, size, direction, distance, layer, adjustment) ? Hits[0].collider : null;
        }

        public static Collider2D GetNextResult()
        {
            _currentResult++;
            if (_currentResult >= MAXResults || _currentResult >= _nbCollision)
                return null;

            return Hits[_currentResult].collider;
        }

        public static bool TestRayCast(Vector2 center, Vector2 direction, float distance, LayerMask layer, 
            float adjustment = 0.02f)
        {
            _currentResult = 0;
            float distanceTotal = distance + adjustment;
            _nbCollision = Physics2D.RaycastNonAlloc(center, direction, Hits, distanceTotal, layer);

            bool isColliding = _nbCollision > 0;
            DrawRay(center, direction, distanceTotal, isColliding);

            return isColliding;
        }
        
        public static bool TestBoxCast(Vector2 center, Vector2 size, Vector2 direction, float distance, LayerMask layer, 
            float adjustment = 0.02f)
        {
            _currentResult = 0;
            float distanceTotal = distance + adjustment;
        
            _nbCollision = Physics2D.BoxCastNonAlloc(center, size, 0, direction, Hits, distanceTotal, 
                layer);

            bool isColliding = _nbCollision > 0;
            DrawBox(center, size, direction, distanceTotal, isColliding);

            return isColliding;
        }
        
        private static void DrawBox(Vector2 center, Vector2 size, Vector2 direction, float distance, bool isColliding)
        {
            if (DebugMode)
            {
                Color color = isColliding ? ColorPositive : ColorNegative;
            
                Vector2 perpDir = new Vector2(direction.y, -direction.x) * size / 2f;
                Vector2 dirDistance = direction * distance;
                Vector2 start = center - perpDir, end = center + perpDir;
                Vector3 start2 = start + dirDistance, end2 = end + dirDistance;
            
                Debug.DrawLine(start, end, color);
                Debug.DrawLine(start, start2, color);
                Debug.DrawLine(end, end2, color);
                Debug.DrawLine(start2, end2, color);
            }
        }

        private static void DrawRay(Vector2 center, Vector2 direction, float distance, bool isColliding)
        {
            if (DebugMode)
            {
                Debug.DrawLine(center, center + direction * distance, isColliding ? ColorPositive : ColorNegative);
            }
        }
    }
}
