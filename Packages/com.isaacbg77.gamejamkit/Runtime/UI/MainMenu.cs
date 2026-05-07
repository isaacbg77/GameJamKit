using GameJamKit.SceneManagement;
using UnityEngine;

namespace GameJamKit.UI.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private SceneLoadData? nextScene;
        [SerializeField] private SceneLoadData? instructionsScene;
        [SerializeField] private SceneLoadData? creditsScene;

        private void Awake()
        {
            if (nextScene == null) Debug.LogError($"{nameof(MainMenu)}: {nameof(nextScene)} is null");
            if (instructionsScene == null) Debug.LogError($"{nameof(MainMenu)}: {nameof(instructionsScene)} is null");
            if (creditsScene == null) Debug.LogError($"{nameof(MainMenu)}: {nameof(creditsScene)} is null");
        }

        public void StartGame()
        {
            if (nextScene == null) return;
            _ = SceneLoader.Instance.LoadSceneAsync(nextScene);
        }

        public void ShowInstructions()
        {
            if (instructionsScene == null) return;
            _ = SceneLoader.Instance.LoadSceneAsync(instructionsScene);
        }

        public void ShowCredits()
        {
            if (creditsScene == null) return;
            _ = SceneLoader.Instance.LoadSceneAsync(creditsScene);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
