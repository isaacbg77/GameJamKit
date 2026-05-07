using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJamKit.Utilities
{
    public static class MonoBehaviourExtensions
    {
        public static T? FindObjectInMyScene<T>(this MonoBehaviour self)
        {
            return self.FindObjectsInMyScene<T>().FirstOrDefault();
        }

        public static T[] FindObjectsInMyScene<T>(this MonoBehaviour self)
        {
            var scene = self.gameObject.scene;
            var results = new List<T>();

            foreach (var root in scene.GetRootGameObjects())
            {
                results.AddRange(root.GetComponentsInChildren<T>(true));
            }

            return results.ToArray();
        }

        public static T InstantiateInScene<T>(this T self, Scene scene) where T : Component
        {
            var instance = Object.Instantiate(self);
            MoveToSceneSafe(instance.gameObject, scene);
            return instance;
        }

        public static T InstantiateInScene<T>(this T self, Vector3 position, Quaternion rotation, Scene scene) where T : Component
        {
            var instance = Object.Instantiate(self, position, rotation);
            MoveToSceneSafe(instance.gameObject, scene);
            return instance;
        }

        private static void MoveToSceneSafe(GameObject instance, Scene scene)
        {
            if (scene.IsValid() && scene.isLoaded)
            {
                SceneManager.MoveGameObjectToScene(instance, scene);
            }
            else
            {
                Debug.LogWarning($"{nameof(InstantiateInScene)}: target scene '{scene.name}' is invalid or not loaded. Object remains in Active Scene.");
            }
        }
    }
}
