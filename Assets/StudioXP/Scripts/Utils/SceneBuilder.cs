using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace StudioXP.Scripts.Utils
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
    public class SceneBuilder : MonoBehaviour
    { 
        [SerializeField] private SceneConfiguration configuration;
        
        /*public void Awake() 
        {
            if (Application.isEditor && !Application.isPlaying)
            {
                //SceneManager.LoadScene()
                //Debug.Log();
            }
        }*/

       // [MenuItem("Studio XP/Exporter le projet")]
        /*public static void ExportPackage()
        {
            //AssetDatabase.ExportPackage();
        }
        
        [MenuItem("Studio XP/Configurer la scène")]
        public static void ConfigureActiveScene()
        {
            SceneConfiguration configuration =
                AssetDatabase.LoadAssetAtPath<SceneConfiguration>("Assets/StudioXP/Configuration/SceneConfiguration.asset");

            var instances = SceneManager.GetActiveScene().GetRootGameObjects().ToList();
            foreach (var prefab in configuration.BaseSceneObjects)
            {
                bool isInstantiated = false;
                foreach (var go in SceneManager.GetActiveScene().GetRootGameObjects())
                {
                    if (PrefabUtility.GetCorrespondingObjectFromSource(go) == prefab)
                        isInstantiated = true;
                }

                if (!isInstantiated)
                    PrefabUtility.InstantiatePrefab(prefab);
            }
        }*/

        [MenuItem("Studio XP/Export Project")]
        public static void ExportProject()
        {
            const ExportPackageOptions flags = ExportPackageOptions.Default 
                                               | ExportPackageOptions.Recurse  
                                               | ExportPackageOptions.Interactive;

            var savePath = EditorUtility.SaveFilePanel("Save Project Package", "", "", "unitypackage");
            var projectContent = new string[]
            {
                "Assets",
                "PackageUpdater",
                "ProjectSettings/EditorBuildSettings.asset",
                "ProjectSettings/DynamicsManager.asset",
                "ProjectSettings/EditorSettings.asset",
                "ProjectSettings/GraphicsSettings.asset",
                "ProjectSettings/InputManager.asset",
                "ProjectSettings/ProjectSettings.asset",
                "ProjectSettings/QualitySettings.asset",
                "ProjectSettings/TagManager.asset",
            };

            if (!string.IsNullOrEmpty(savePath))
                AssetDatabase.ExportPackage(projectContent, savePath, flags);
        }
    }
#endif
}
