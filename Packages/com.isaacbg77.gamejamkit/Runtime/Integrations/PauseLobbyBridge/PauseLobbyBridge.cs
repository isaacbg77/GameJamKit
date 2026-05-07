using GameJamKit.Lobby;
using GameJamKit.Pause;
using GameJamKit.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameJamKit.Integrations
{
    public class PauseLobbyBridge : MonoBehaviour
    {
        [SerializeField] private InputActionReference? perPlayerPauseAction;
        [SerializeField] private bool pauseOnDeviceLost = true;
        [SerializeField] private bool resumeOnDeviceRegained = true;

        private IPauseService? pauseService;
        private IPlayerLobbyService? lobbyService;

        private void Awake()
        {
            pauseService = InterfaceFinder.FindInterfaceByType<IPauseService>();
            if (pauseService == null) Debug.LogError($"{nameof(PauseLobbyBridge)}: {nameof(pauseService)} is null");

            lobbyService = InterfaceFinder.FindInterfaceByType<IPlayerLobbyService>();
            if (lobbyService == null) Debug.LogError($"{nameof(PauseLobbyBridge)}: {nameof(lobbyService)} is null");
        }

        private void OnEnable()
        {
            if (lobbyService == null) return;

            lobbyService.PlayerJoined += OnPlayerJoined;
            lobbyService.PlayerLeft += OnPlayerLeft;
            lobbyService.PlayerDisconnected += OnPlayerDisconnected;
            lobbyService.PlayerReconnected += OnPlayerReconnected;

            foreach (var playerInput in lobbyService.JoinedPlayers)
            {
                SubscribePauseAction(playerInput);
            }
        }

        private void OnDisable()
        {
            if (lobbyService == null) return;

            lobbyService.PlayerJoined -= OnPlayerJoined;
            lobbyService.PlayerLeft -= OnPlayerLeft;
            lobbyService.PlayerDisconnected -= OnPlayerDisconnected;
            lobbyService.PlayerReconnected -= OnPlayerReconnected;

            foreach (var playerInput in lobbyService.JoinedPlayers)
            {
                UnsubscribePauseAction(playerInput);
            }
        }

        private void OnPlayerJoined(PlayerInput playerInput) => SubscribePauseAction(playerInput);
        private void OnPlayerLeft(PlayerInput playerInput) => UnsubscribePauseAction(playerInput);

        private void OnPlayerDisconnected(PlayerInput playerInput)
        {
            if (pauseOnDeviceLost) pauseService?.Pause();
        }

        private void OnPlayerReconnected(PlayerInput playerInput)
        {
            if (resumeOnDeviceRegained) pauseService?.Unpause();
        }

        private void SubscribePauseAction(PlayerInput playerInput)
        {
            if (perPlayerPauseAction == null) return;
            playerInput.actions[perPlayerPauseAction.action.name].performed += OnPausePerformed;
        }

        private void UnsubscribePauseAction(PlayerInput playerInput)
        {
            if (perPlayerPauseAction == null) return;
            playerInput.actions[perPlayerPauseAction.action.name].performed -= OnPausePerformed;
        }

        private void OnPausePerformed(InputAction.CallbackContext context) => pauseService?.Toggle();
    }
}
