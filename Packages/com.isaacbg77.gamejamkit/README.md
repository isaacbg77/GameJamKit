# Game Jam Kit

Reusable Unity systems for game jams, extracted from shipped jam projects. Targets Unity 6.

## Modules

| Module | Asmdef | What's in it |
| --- | --- | --- |
| Utilities | `GameJamKit.Utilities` | `Singleton<T>` / `PersistentSingleton<T>`, `InterfaceFinder`, scene-scoped find/instantiate extensions, list shuffle |
| Scene Management | `GameJamKit.SceneManagement` | Async `SceneLoader` with load/unload/music events + `SceneLoadData` ScriptableObject |
| Sound | `GameJamKit.Sound` | `ISoundService` + `SoundManager`, `IMusicService` + `MusicManager` with cross-fade, `AudioMixerSlider` |
| UI | `GameJamKit.UI` | `IInitializableUI`, `GameVersionUI`, prebuilt menu scripts (`MainMenu`, `BackToMainMenuScript`, `InstructionsMenu`, `WinMenu`) |
| Pause | `GameJamKit.Pause` | `IPauseService` + standalone `PauseService` (Time.timeScale + panel) |
| Lobby | `GameJamKit.Lobby` | `IPlayerLobbyService` + local-multiplayer lobby driven by Input System `PlayerInputManager` |
| Pause/Lobby bridge | `GameJamKit.Integrations.PauseLobbyBridge` | Optional: auto-pause on device-lost, auto-resume on device-regained, per-player pause input |

## Installation

In your project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.isaacbg77.gamejamkit": "file:../../game-jam-kit",
    "com.eflatun.scenereference": "git+https://github.com/starikcetin/Eflatun.SceneReference.git#4.1.1"
  }
}
```

Or use **Window → Package Manager → Add package from disk** and pick `package.json`.

### Required external packages

The Scene Management module depends on **Eflatun.SceneReference**. Add it to your project manifest as shown above — it can't be declared in this package's `dependencies` because it's distributed via git URL, not a registry.

Input System is declared as a hard dependency. UGUI ships with Unity.

## Conventions

- Services expose interfaces (`ISoundService`, `IPauseService`, `IPlayerLobbyService`); consumers depend on the interface, not the concrete MonoBehaviour.
- Same-scene service lookup: `MonoBehaviourExtensions.FindObjectInMyScene<T>()`.
- Cross-scene/persistent lookup: `InterfaceFinder.FindInterfaceByType<T>()`.
- Resolve dependencies in `Awake()`. Log via `Debug.LogError` with `nameof()` if a lookup returns null.
- Events use named delegate types declared inside the relevant interface.
- Nullable reference types are enabled — `?` annotations are accurate.
