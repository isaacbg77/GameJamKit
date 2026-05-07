using GameJamKit.SceneManagement;
using UnityEngine;

namespace GameJamKit.UI.Menus
{
    public class ToMainMenu : MonoBehaviour
    {
        [SerializeField] private SceneLoadData? mainMenuScene;

        private void Awake()
        {
            if (mainMenuScene == null) Debug.LogError($"{nameof(ToMainMenu)}: {nameof(mainMenuScene)} is null");
        }

        public void GoToMainMenu()
        {
            if (mainMenuScene == null) return;
            _ = SceneLoader.Instance.LoadSceneAsync(mainMenuScene);
        }
    }
}
