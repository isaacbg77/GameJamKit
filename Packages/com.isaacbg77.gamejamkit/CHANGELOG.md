# Changelog

## [0.1.0] - 2026-05-06

Initial extraction from TOJam2025 (Too Many Spells) and GMUWinterJam2025 (Looters).

### Added
- `GameJamKit.Utilities` — Singleton base classes, InterfaceFinder, scene-scoped find/instantiate extensions, list shuffle
- `GameJamKit.SceneManagement` — async SceneLoader with load/unload/music-request events; SceneLoadData ScriptableObject
- `GameJamKit.Sound` — ISoundService/SoundManager, MusicManager with vanilla Awaitable cross-fade, AudioMixerSlider
- `GameJamKit.UI` — IInitializableUI, GameVersionUI, MainMenu/CreditsMenu/InstructionsMenu/WinMenu scripts
- `GameJamKit.Pause` — IPauseService and standalone PauseService (Time.timeScale + panel)
- `GameJamKit.Lobby` — IPlayerLobbyService and PlayerLobbyService for local multiplayer; PlayerLobbyUI; ToSceneAfterLobby
- `GameJamKit.Integrations.PauseLobbyBridge` — optional component pairing Pause and Lobby (auto-pause on device-lost)

### Changed from source projects
- MusicManager: replaced DOTween fade with Awaitable-driven volume lerp using `Time.unscaledDeltaTime`
- PauseService: removed direct dependency on lobby; per-player input subscription moved to PauseLobbyBridge
- PlayerLobbyService: replaced `PlayerSession.ResetPlayer()` call in `UnlockLobby` with a `LobbyUnlocked` event
