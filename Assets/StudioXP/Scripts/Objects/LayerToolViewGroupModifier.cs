using System.Collections.Generic;
using UnityEngine;

namespace StudioXP.Scripts.Objects
{
    public class LayerToolViewGroupModifier : MonoBehaviour
    {
        [SerializeField] private LayerToolView[] layerToolViews;
        
        public void Activate()
        {
            foreach(var layerToolView in layerToolViews)
                layerToolView.Activate();
        }
        
        public void Deactivate()
        {
            foreach(var layerToolView in layerToolViews)
                layerToolView.Deactivate();
        }
    }
}
