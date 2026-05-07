using GameJamKit.SceneManagement;
using GameJamKit.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GameJamKit.Lobby
{
    public class ToSceneAfterLobby : MonoBehaviour
    {
        [SerializeField] private InputActionReference? startGameAction;
        [SerializeField] private Button? backButton;

        [Header("Scene References")]
        [SerializeField] private SceneLoadData? nextScene;
        [SerializeField] private SceneLoadData? backScene;

        private IPlayerLobbyService? lobbyService;

        private void Awake()
        {
            if (startGameAction == null) Debug.LogError($"{nameof(ToSceneAfterLobby)}: {nameof(startGameAction)} is null");
            if (backButton == null) Debug.LogError($"{nameof(ToSceneAfterLobby)}: {nameof(backButton)} is null");

            if (nextScene == null) Debug.LogError($"{nameof(ToSceneAfterLobby)}: {nameof(nextScene)} is null");
            if (backScene == null) Debug.LogError($"{nameof(ToSceneAfterLobby)}: {nameof(backScene)} is null");

            lobbyService = InterfaceFinder.FindInterfaceByType<IPlayerLobbyService>();
            if (lobbyService == null) Debug.LogError($"{nameof(ToSceneAfterLobby)}: {nameof(lobbyService)} is null");
        }

        private void OnEnable()
        {
            if (lobbyService != null)
            {
                lobbyService.UnlockLobby();

                lobbyService.PlayerJoined += OnPlayerJoined;
                lobbyService.PlayerLeft += OnPlayerLeft;

                foreach (var playerInput in lobbyService.JoinedPlayers)
                {
                    SubscribeStartAction(playerInput);
                }
            }

            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackButtonClicked);
            }
        }

        private void OnDisable()
        {
            if (lobbyService != null)
            {
                lobbyService.PlayerJoined -= OnPlayerJoined;
                lobbyService.PlayerLeft -= OnPlayerLeft;

                foreach (var playerInput in lobbyService.JoinedPlayers)
                {
                    UnsubscribeStartAction(playerInput);
                }
            }

            if (backButton != null)
            {
                backButton.onClick.RemoveAllListeners();
            }
        }

        private void OnPlayerJoined(PlayerInput playerInput) => SubscribeStartAction(playerInput);
        private void OnPlayerLeft(PlayerInput playerInput) => UnsubscribeStartAction(playerInput);

        private void SubscribeStartAction(PlayerInput playerInput)
        {
            if (startGameAction == null) return;
            playerInput.actions[startGameAction.action.name].performed += OnStartGamePerformed;
        }

        private void UnsubscribeStartAction(PlayerInput playerInput)
        {
            if (startGameAction == null) return;
            playerInput.actions[startGameAction.action.name].performed -= OnStartGamePerformed;
        }

        private void OnStartGamePerformed(InputAction.CallbackContext context)
        {
            _ = StartGameAsync();
        }

        private void OnBackButtonClicked()
        {
            if (backScene == null) return;
            _ = SceneLoader.Instance.LoadSceneAsync(backScene);
        }

        private async Awaitable StartGameAsync()
        {
            if (lobbyService == null || nextScene == null) return;

            if (!lobbyService.LobbyReady)
            {
                Debug.LogError($"{nameof(ToSceneAfterLobby)}: Not enough players to start the game!");
                return;
            }

            lobbyService.LockLobby();
            await SceneLoader.Instance.LoadSceneAsync(nextScene);
        }
    }
}
