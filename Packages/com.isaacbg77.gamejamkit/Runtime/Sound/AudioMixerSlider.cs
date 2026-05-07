using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace GameJamKit.Sound
{
    [RequireComponent(typeof(Slider))]
    public class AudioMixerSlider : MonoBehaviour
    {
        [SerializeField] private AudioMixer? mixer;
        [SerializeField] private string parameterName = "Volume";

        private Slider? slider;

        private void Awake()
        {
            if (mixer == null) Debug.LogError($"{nameof(AudioMixerSlider)}: {nameof(mixer)} is null");

            slider = GetComponent<Slider>();
            if (slider == null) Debug.LogError($"{nameof(AudioMixerSlider)}: {nameof(slider)} is null");
        }

        private void OnEnable()
        {
            if (slider != null)
            {
                slider.onValueChanged.AddListener(OnSliderChanged);
                OnSliderChanged(slider.value);
            }
        }
        
        private void OnDisable()
        {
            if (slider != null)
            {
                slider.onValueChanged.RemoveListener(OnSliderChanged);
            }
        }

        private void Start()
        {
            if (mixer == null || slider == null) return;
            if (mixer.GetFloat(parameterName, out var dB))
            {
                slider.value = Mathf.Pow(10f, dB / 20f);
            }
        }

        private void OnSliderChanged(float value)
        {
            if (mixer == null) return;
            var safe = Mathf.Max(value, 0.0001f);
            mixer.SetFloat(parameterName, Mathf.Log10(safe) * 20f);
        }
    }
}
