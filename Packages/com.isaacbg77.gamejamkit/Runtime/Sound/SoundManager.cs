using UnityEngine;

namespace GameJamKit.Sound
{
    public class SoundManager : MonoBehaviour, ISoundService
    {
        public void PlaySound(AudioSource source, float pitchDelta = 0)
        {
            source.pitch = Random.Range(1 - pitchDelta, 1 + pitchDelta);
            AudioSource.PlayClipAtPoint(source.clip, transform.position);
        }

        public void PlaySingleSound(AudioClip? clip)
        {
            if (clip == null) return;
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }
}
