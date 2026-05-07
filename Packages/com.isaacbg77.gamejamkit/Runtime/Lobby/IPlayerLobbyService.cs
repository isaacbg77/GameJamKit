using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace GameJamKit.Lobby
{
    public interface IPlayerLobbyService
    {
        IReadOnlyList<PlayerInput> JoinedPlayers { get; }
        bool LobbyReady { get; }

        delegate void PlayerEvent(PlayerInput playerInput);
        event PlayerEvent? PlayerJoined;
        event PlayerEvent? PlayerLeft;
        event PlayerEvent? PlayerDisconnected;
        event PlayerEvent? PlayerReconnected;

        delegate void LobbyReadyEvent(bool isReady);
        event LobbyReadyEvent? LobbyReadyChanged;

        delegate void LobbyStateEvent();
        event LobbyStateEvent? LobbyUnlocked;
        event LobbyStateEvent? LobbyLocked;

        void UnlockLobby();
        void LockLobby();
    }
}
