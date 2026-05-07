using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameJamKit.Lobby
{
    public class PlayerLobbyService : MonoBehaviour, IPlayerLobbyService
    {
        [SerializeField] private int minPlayers = 2;

        private PlayerInputManager? playerInputManager;

        private readonly List<PlayerInput> joinedPlayers = new();
        public IReadOnlyList<PlayerInput> JoinedPlayers => joinedPlayers;

        public bool LobbyReady => joinedPlayers.Count >= minPlayers;

        public event IPlayerLobbyService.PlayerEvent? PlayerJoined;
        public event IPlayerLobbyService.PlayerEvent? PlayerLeft;
        public event IPlayerLobbyService.PlayerEvent? PlayerDisconnected;
        public event IPlayerLobbyService.PlayerEvent? PlayerReconnected;

        public event IPlayerLobbyService.LobbyReadyEvent? LobbyReadyChanged;
        public event IPlayerLobbyService.LobbyStateEvent? LobbyUnlocked;
        public event IPlayerLobbyService.LobbyStateEvent? LobbyLocked;

        private bool joiningEnabled;

        private void Awake()
        {
            playerInputManager = FindAnyObjectByType<PlayerInputManager>();
            if (playerInputManager == null) Debug.LogError($"{nameof(PlayerLobbyService)}: {nameof(playerInputManager)} is null");
        }

        private void OnEnable()
        {
            if (playerInputManager == null) return;

            playerInputManager.onPlayerJoined += OnPlayerJoined;
            playerInputManager.onPlayerLeft += OnPlayerLeft;
        }

        private void OnDisable()
        {
            if (playerInputManager == null) return;

            playerInputManager.onPlayerJoined -= OnPlayerJoined;
            playerInputManager.onPlayerLeft -= OnPlayerLeft;
        }

        public void UnlockLobby()
        {
            joiningEnabled = true;
            playerInputManager?.EnableJoining();

            LobbyUnlocked?.Invoke();
        }

        public void LockLobby()
        {
            joiningEnabled = false;
            playerInputManager?.DisableJoining();

            LobbyLocked?.Invoke();
        }

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            joinedPlayers.Add(playerInput);

            playerInput.onDeviceLost += OnDeviceLost;
            playerInput.onDeviceRegained += OnDeviceRegained;

            PlayerJoined?.Invoke(playerInput);
            LobbyReadyChanged?.Invoke(LobbyReady);
        }

        private void OnPlayerLeft(PlayerInput playerInput)
        {
            joinedPlayers.Remove(playerInput);

            playerInput.onDeviceLost -= OnDeviceLost;
            playerInput.onDeviceRegained -= OnDeviceRegained;

            PlayerLeft?.Invoke(playerInput);
            LobbyReadyChanged?.Invoke(LobbyReady);

            if (joiningEnabled) playerInputManager?.EnableJoining();
        }

        private void OnDeviceLost(PlayerInput playerInput) => PlayerDisconnected?.Invoke(playerInput);
        private void OnDeviceRegained(PlayerInput playerInput) => PlayerReconnected?.Invoke(playerInput);
    }
}
