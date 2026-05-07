using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJamKit.Utilities
{
    public static class GameObjectExtensions
    {
        public static T? FindObjectInMyScene<T>(this GameObject self)
        {
            return self.FindObjectsInMyScene<T>().FirstOrDefault();
        }

        public static T[] FindObjectsInMyScene<T>(this GameObject self)
        {
            var scene = self.scene;
            var results = new List<T>();

            foreach (var root in scene.GetRootGameObjects())
            {
                results.AddRange(root.GetComponentsInChildren<T>(true));
            }

            return results.ToArray();
        }

        public static GameObject InstantiateInScene(this GameObject original, Scene scene)
        {
            var instance = Object.Instantiate(original);
            MoveToSceneSafe(instance, scene);
            return instance;
        }

        public static GameObject InstantiateInScene(this GameObject original, Vector3 position, Quaternion rotation, Scene scene)
        {
            var instance = Object.Instantiate(original, position, rotation);
            MoveToSceneSafe(instance, scene);
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
