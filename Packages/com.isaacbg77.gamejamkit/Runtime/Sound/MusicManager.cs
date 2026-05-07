using System;
using System.Threading;
using GameJamKit.SceneManagement;
using UnityEngine;

namespace GameJamKit.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour, IMusicService
    {
        [SerializeField] private float fadeTime = 1f;

        private AudioSource? musicSource;
        private float startVolume;
        private CancellationTokenSource? fadeCts;

        private void Awake()
        {
            musicSource = GetComponent<AudioSource>();
            if (musicSource == null) Debug.LogError($"{nameof(MusicManager)}: {nameof(musicSource)} not found");
        }

        private void Start()
        {
            if (musicSource != null) startVolume = musicSource.volume;
        }

        private void OnEnable()
        {
            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.SceneMusicRequested += OnSceneMusicRequested;
            }
        }

        private void OnDisable()
        {
            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.SceneMusicRequested -= OnSceneMusicRequested;
            }
        }

        private void OnDestroy()
        {
            fadeCts?.Cancel();
            fadeCts?.Dispose();
        }

        private void OnSceneMusicRequested(AudioClip clip) => _ = PlayMusic(clip);

        public async Awaitable PlayMusic(AudioClip clip)
        {
            if (musicSource == null) return;

            var ct = StartNewFade();

            try
            {
                if (musicSource.isPlaying)
                {
                    await FadeVolumeAsync(musicSource, 0f, fadeTime, ct);
                    musicSource.Stop();
                }

                musicSource.clip = clip;
                musicSource.volume = 0f;
                musicSource.Play();

                await FadeVolumeAsync(musicSource, startVolume, fadeTime, ct);
            }
            catch (OperationCanceledException) { }
        }

        public async Awaitable StopMusic()
        {
            if (musicSource == null) return;

            var fadeCancelToken = StartNewFade();

            try
            {
                await FadeVolumeAsync(musicSource, 0f, fadeTime, fadeCancelToken);
                musicSource.Stop();
            }
            catch (OperationCanceledException) { }
        }

        private CancellationToken StartNewFade()
        {
            fadeCts?.Cancel();
            fadeCts?.Dispose();
            fadeCts = new CancellationTokenSource();
            return fadeCts.Token;
        }

        private async Awaitable FadeVolumeAsync(AudioSource source, float targetVolume, float duration, CancellationToken token)
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
                await Awaitable.NextFrameAsync(token);
                elapsed += Time.unscaledDeltaTime;
                source.volume = Mathf.Lerp(fromVolume, targetVolume, Mathf.Clamp01(elapsed / duration));
            }

            source.volume = targetVolume;
        }
    }
}
