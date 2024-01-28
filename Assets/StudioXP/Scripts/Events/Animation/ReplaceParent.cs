using Sirenix.OdinInspector;
using StudioXP.Scripts.Game;
using UnityEngine;

namespace StudioXP.Scripts.Events.Animation
{
    public class ReplaceParent : MonoBehaviour
    {
        [AssetSelector(Paths = Paths.Prefabs, ExpandAllMenuItems = false)]
        [AssetsOnly]
        [SerializeField] private GameObject prefab;

        public void Replace()
        {
            var parent = transform.parent;
            GameObject go = Instantiate(prefab, parent.parent);
            go.transform.position = parent.transform.position;
            Destroy(parent.gameObject);
        }
    }
}
