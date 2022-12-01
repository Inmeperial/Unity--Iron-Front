using TMPro;
using UnityEngine;
using UnityEngine.Audio;

namespace GameSettings.Audio
{
    public abstract class VolumeSlider : SettingsSlider
    {
        [Header("References")]
        [SerializeField] protected AudioMixer _audioMixer;

        public override void OnSettingsLoaded()
        {
            _slider.onValueChanged.AddListener((float value) => OnValueChange(value));
        }

        protected override void OnValueChange(float value)
        {
            _statusText.text = (int)value + "%";

            _audioMixer.SetFloat("Volume", SoundUtilities.FloatToDB(value / 100));

            OnSettingsChange();
        }
    }
}

