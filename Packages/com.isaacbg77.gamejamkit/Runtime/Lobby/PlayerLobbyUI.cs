using System.Collections.Generic;
using GameJamKit.Sound;
using GameJamKit.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameJamKit.Lobby
{
    public class PlayerLobbyUI : MonoBehaviour
    {
        [SerializeField] private List<GameObject>? playerSlots;
        [SerializeField] private GameObject? startText;
        [SerializeField] private AudioClip? playerJoinSound;

        private IPlayerLobbyService? lobbyService;
        private ISoundService? soundService;

        private void Awake()
        {
            if (playerSlots == null) Debug.LogError($"{nameof(PlayerLobbyUI)}: {nameof(playerSlots)} is null");
            if (startText == null) Debug.LogError($"{nameof(PlayerLobbyUI)}: {nameof(startText)} is null");
            if (playerJoinSound == null) Debug.LogError($"{nameof(PlayerLobbyUI)}: {nameof(playerJoinSound)} is null");

            lobbyService = InterfaceFinder.FindInterfaceByType<IPlayerLobbyService>();
            if (lobbyService == null) Debug.LogError($"{nameof(PlayerLobbyUI)}: {nameof(lobbyService)} is null");

            soundService = InterfaceFinder.FindInterfaceByType<ISoundService>();
            if (soundService == null) Debug.LogError($"{nameof(PlayerLobbyUI)}: {nameof(soundService)} is null");
        }

        private void OnEnable()
        {
            if (lobbyService == null) return;

            lobbyService.PlayerJoined += OnPlayerJoined;
            lobbyService.PlayerLeft += OnPlayerLeft;
            lobbyService.LobbyReadyChanged += OnLobbyReadyChanged;

            foreach (var playerInput in lobbyService.JoinedPlayers)
            {
                SetPlayerSlotActive(playerInput, true);
            }

            OnLobbyReadyChanged(lobbyService.LobbyReady);
        }

        private void OnDisable()
        {
            if (lobbyService == null) return;

            lobbyService.PlayerJoined -= OnPlayerJoined;
            lobbyService.PlayerLeft -= OnPlayerLeft;
            lobbyService.LobbyReadyChanged -= OnLobbyReadyChanged;
        }

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            SetPlayerSlotActive(playerInput, true);
            soundService?.PlaySingleSound(playerJoinSound);
        }

        private void OnPlayerLeft(PlayerInput playerInput)
        {
            SetPlayerSlotActive(playerInput, false);
        }

        private void SetPlayerSlotActive(PlayerInput playerInput, bool active)
        {
            if (playerSlots == null) return;
            if (playerInput.playerIndex < 0 || playerInput.playerIndex >= playerSlots.Count) return;
            playerSlots[playerInput.playerIndex].SetActive(active);
        }

        private void OnLobbyReadyChanged(bool canStart)
        {
            if (startText == null) return;
            startText.SetActive(canStart);
        }
    }
}
