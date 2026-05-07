using GameJamKit.SceneManagement;
using GameJamKit.Utilities;
using UnityEngine;

namespace GameJamKit.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : Singleton<MusicManager>
    {
        [SerializeField] private float fadeTime = 1f;

        private AudioSource? musicSource;
        private float startVolume;
        private int fadeToken;

        protected override void Awake()
        {
            base.Awake();

            musicSource = GetComponent<AudioSource>();
            if (musicSource == null) Debug.LogError($"{nameof(MusicManager)}: {nameof(musicSource)} not found");

            SceneLoader.Instance.SceneMusicRequested += OnSceneMusicRequested;
        }

        private void Start()
        {
            if (musicSource != null) startVolume = musicSource.volume;
        }

        private void OnDestroy()
        {
            fadeToken++;

            if (SceneLoader.Instance != null)
                SceneLoader.Instance.SceneMusicRequested -= OnSceneMusicRequested;
        }

        private void OnSceneMusicRequested(AudioClip clip)
        {
            _ = PlayMusic(clip);
        }

        public async Awaitable PlayMusic(AudioClip clip)
        {
            if (musicSource == null) return;

            var token = ++fadeToken;

            if (musicSource.isPlaying)
            {
                await FadeVolumeAsync(musicSource, 0f, fadeTime, token);
                if (token != fadeToken || this == null || musicSource == null) return;
                musicSource.Stop();
            }

            musicSource.clip = clip;
            musicSource.volume = 0f;
            musicSource.Play();

            await FadeVolumeAsync(musicSource, startVolume, fadeTime, token);
        }

        public async Awaitable StopMusic()
        {
            if (musicSource == null) return;

            var token = ++fadeToken;
            await FadeVolumeAsync(musicSource, 0f, fadeTime, token);
            if (token != fadeToken || this == null || musicSource == null) return;
            musicSource.Stop();
        }

        private async Awaitable FadeVolumeAsync(AudioSource source, float targetVolume, float duration, int token)
        {
            if (duration <= 0f)
            {
                source.volume = targetVolume;
                return;
            }

            var fromVolume = source.volume;
            var elapsed = 0f;

            while (elapsed < duration)
            {
                if (token != fadeToken || this == null || source == null) return;
                await Awaitable.NextFrameAsync();
                if (token != fadeToken || this == null || source == null) return;

                elapsed += Time.unscaledDeltaTime;
                source.volume = Mathf.Lerp(fromVolume, targetVolume, Mathf.Clamp01(elapsed / duration));
            }

            if (source != null) source.volume = targetVolume;
        }
    }
}
