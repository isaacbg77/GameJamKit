using UnityEngine;

namespace GameJamKit.Sound
{
    public interface ISoundService
    {
        void PlaySound(AudioSource source, float pitchDelta = 0);
        void PlaySingleSound(AudioClip? clip);
    }
}
