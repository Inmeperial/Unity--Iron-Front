using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

namespace GameSettings.Audio
{
    public abstract class VolumeSlider : SettingsSlider
    {
        [Header("References")]
        [SerializeField] protected AudioMixer _audioMixer;

        [Header("Configs")]
        [SerializeField] protected string _volumeParameterName;
        public override void OnSettingsLoaded()
        {
            _slider.onValueChanged.AddListener((float value) => OnValueChange(value));

            InitializeValueDelay();
        }

        protected override void OnValueChange(float value)
        {
            _statusText.text = (int)value + "%";

            _audioMixer.SetFloat(_volumeParameterName, SoundUtilities.FloatToDB(value / 100));

            OnSettingsChange();
        }

        private async void InitializeValueDelay()
        {
            await Task.Delay(100);

            InitializeSliderValue();
        }

        protected abstract void InitializeSliderValue();
    }
}

