using GameJamKit.SceneManagement;
using UnityEngine;

namespace GameJamKit.UI.Menus
{
    public class WinMenu : MonoBehaviour
    {
        [SerializeField] private SceneLoadData? nextScene;
        [SerializeField] private float delay = 5f;

        private void Awake()
        {
            if (nextScene == null) Debug.LogError($"{nameof(WinMenu)}: {nameof(nextScene)} is null");
        }

        private void Start()
        {
            _ = RestartGameWithDelay();
        }

        private async Awaitable RestartGameWithDelay()
        {
            if (nextScene == null) return;

            await Awaitable.WaitForSecondsAsync(delay);
            _ = SceneLoader.Instance.LoadSceneAsync(nextScene);
        }
    }
}
