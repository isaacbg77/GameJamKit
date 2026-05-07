using UnityEngine;

namespace GameJamKit.Sound
{
    public interface IMusicService
    {
        Awaitable PlayMusic(AudioClip clip);
        Awaitable StopMusic();
    }
}
