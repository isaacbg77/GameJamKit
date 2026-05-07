using UnityEngine;
using UnityEngine.InputSystem;

namespace GameJamKit.Pause
{
    public class PauseService : MonoBehaviour, IPauseService
    {
        [SerializeField] private GameObject? pausePanelPrefab;
        [SerializeField] private InputActionReference? pauseAction;

        private GameObject? pausePanelInstance;
        private bool isPaused;

        public bool IsPaused => isPaused;

        public event IPauseService.PauseStateChangedEvent? Paused;
        public event IPauseService.PauseStateChangedEvent? Resumed;

        private void Awake()
        {
            if (pausePanelPrefab == null) Debug.LogError($"{nameof(PauseService)}: {nameof(pausePanelPrefab)} is null");
        }

        private void OnEnable()
        {
            if (pauseAction == null) return;
            pauseAction.action.Enable();
            pauseAction.action.performed += OnPausePerformed;
        }

        private void OnDisable()
        {
            if (pauseAction == null) return;
            pauseAction.action.performed -= OnPausePerformed;
        }

        public void Pause()
        {
            if (isPaused) return;
            isPaused = true;

            if (pausePanelInstance == null && pausePanelPrefab != null)
            {
                pausePanelInstance = Instantiate(pausePanelPrefab, transform);
            }
            pausePanelInstance?.SetActive(true);

            Time.timeScale = 0f;
            Paused?.Invoke();
        }

        public void Unpause()
        {
            if (!isPaused) return;
            isPaused = false;

            pausePanelInstance?.SetActive(false);
            Time.timeScale = 1f;
            Resumed?.Invoke();
        }

        public void Toggle()
        {
            if (isPaused) Unpause();
            else Pause();
        }

        private void OnPausePerformed(InputAction.CallbackContext context) => Toggle();
    }
}
