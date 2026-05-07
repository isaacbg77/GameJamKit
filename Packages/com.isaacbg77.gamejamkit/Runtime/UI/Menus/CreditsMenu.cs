using Eflatun.SceneReference;
using GameJamKit.SceneManagement;
using UnityEngine;

namespace GameJamKit.UI.Menus
{
    public class CreditsMenu : MonoBehaviour
    {
        [SerializeField] private SceneReference? mainMenuScene;

        private void Awake()
        {
            if (mainMenuScene == null) Debug.LogError($"{nameof(CreditsMenu)}: {nameof(mainMenuScene)} is null");
        }

        public void Back()
        {
            if (mainMenuScene == null) return;
            _ = SceneLoader.Instance.LoadSceneAsync(mainMenuScene);
        }
    }
}
