namespace GameSettings.Audio
{
    public class MusicVolume : VolumeSlider
    {
        protected override void InitializeSliderValue()
        {
            float volume = Settings.Instance.SettingsData.musicVolume;

            _slider.value = (int)volume;
        }

        protected override void OnSettingsChange()
        {
            Settings.Instance.SettingsData.musicVolume = _slider.value;
        }
    }
}

