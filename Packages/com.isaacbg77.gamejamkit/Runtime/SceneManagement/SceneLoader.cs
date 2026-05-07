using Eflatun.SceneReference;
using GameJamKit.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJamKit.SceneManagement
{
    public class SceneLoader : PersistentSingleton<SceneLoader>
    {
        public delegate void SceneLoadEvent(Scene scene);
        public event SceneLoadEvent? SceneLoaded;

        public delegate void SceneUnloadEvent(string sceneName);
        public event SceneUnloadEvent? SceneUnloaded;

        public delegate void SceneMusicRequestedEvent(AudioClip clip);
        public event SceneMusicRequestedEvent? SceneMusicRequested;

        public async Awaitable<Scene?> LoadSceneAsync(SceneLoadData sceneData, bool setAsActiveScene = true)
        {
            var loadedScene = await LoadSceneAsync(sceneData.Scene, setAsActiveScene);

            if (sceneData.SceneMusic != null) SceneMusicRequested?.Invoke(sceneData.SceneMusic);

            return loadedScene;
        }

        private async Awaitable<Scene?> LoadSceneAsync(SceneReference scene, bool setAsActiveScene = true)
        {
            Debug.Log($"{nameof(SceneLoader)}: Loading scene {scene.Name}...");

            var loadOp = SceneManager.LoadSceneAsync(scene.BuildIndex, LoadSceneMode.Additive);
            if (loadOp == null)
            {
                Debug.LogError($"{nameof(SceneLoader)}: Failed to load scene {scene.Name}");
                return null;
            }

            Scene? capturedScene = null;
            SceneManager.sceneLoaded += OnSceneLoaded;

            while (!loadOp.isDone) await Awaitable.EndOfFrameAsync();

            SceneManager.sceneLoaded -= OnSceneLoaded;

            if (capturedScene == null)
            {
                Debug.LogError($"{nameof(SceneLoader)}: Failed to capture loaded scene {scene.Name}");
                return null;
            }

            var loadedScene = capturedScene.Value;

            if (setAsActiveScene)
            {
                _ = UnloadSceneAsync(SceneManager.GetActiveScene());
                SceneManager.SetActiveScene(loadedScene);
            }

            Debug.Log($"{nameof(SceneLoader)}: Loaded scene {scene.Name}!");
            SceneLoaded?.Invoke(loadedScene);
            return loadedScene;

            void OnSceneLoaded(Scene s, LoadSceneMode _) => capturedScene = s;
        }

        public async Awaitable UnloadSceneAsync(Scene scene)
        {
            var sceneName = scene.name;
            Debug.Log($"{nameof(SceneLoader)}: Unloading scene {sceneName}...");

            var unloadOp = SceneManager.UnloadSceneAsync(scene);
            if (unloadOp == null)
            {
                Debug.LogError($"{nameof(SceneLoader)}: Failed to unload scene {sceneName}");
                return;
            }

            while (!unloadOp.isDone) await Awaitable.EndOfFrameAsync();

            SceneUnloaded?.Invoke(sceneName);
            Debug.Log($"{nameof(SceneLoader)}: Unloaded scene {sceneName}!");
        }
    }
}
