using UnityEngine;

namespace StudioXP.Scripts.Objects
{
    public class LayerToolView : MonoBehaviour
    {
        [SerializeField] private int layer = 3;

        private int _previousLayer;
            
        public void Activate()
        {
            if (gameObject.layer == layer) return;
            _previousLayer = gameObject.layer;
            gameObject.layer = layer;
        }
        
        public void Deactivate()
        {
            if (gameObject.layer != layer) return;
            gameObject.layer = _previousLayer;
        }
    }
}
