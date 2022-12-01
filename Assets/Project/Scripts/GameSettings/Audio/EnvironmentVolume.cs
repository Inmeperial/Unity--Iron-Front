namespace GameSettings.Audio
{
    public class EnvironmentVolume : VolumeSlider
    {
        protected override void InitializeSliderValue()
        {
            float volume = Settings.Instance.SettingsData.environmentVolume;

            _slider.value = (int)volume;
        }

        protected override void OnSettingsChange()
        {
            Settings.Instance.SettingsData.environmentVolume = _slider.value;
        }
    }
}

