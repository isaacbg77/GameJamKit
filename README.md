# Game Jam Kit

Reusable Unity 6 systems for game jams — scene loading, sound/music, pause, local multiplayer lobby, and menu scaffolding. Extracted from past jam projects so the same boilerplate doesn't get rebuilt every weekend.

## Installation

Add to your jam project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.isaacbg77.gamejamkit": "https://github.com/isaacbg77/GameJamKit.git?path=/Packages/com.isaacbg77.gamejamkit",
    "com.eflatun.scenereference": "git+https://github.com/starikcetin/Eflatun.SceneReference.git#4.1.1"
  }
}
```

Both packages are git-distributed; Eflatun.SceneReference is required for the Scene Management module.
