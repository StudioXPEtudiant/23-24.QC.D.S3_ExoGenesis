using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StudioXP.Scripts.Utils
{
    #if UNITY_EDITOR
    [CreateAssetMenu(fileName = "SceneConfiguration", menuName = "Studio XP/Scene Configuration", order = 1)]
    public class SceneConfiguration : ScriptableObject
    {
        public SceneAsset controlScene;
        public SceneAsset baseScene;
    }
    #endif
}
