namespace GameSettings.Audio
{
    public class EnvironmentVolume : VolumeSlider
    {
        protected override void Configure()
        {
            base.Configure();
            float volume = Settings.Instance.SettingsData.environmentVolume;

            _slider.value = (int)volume;
        }

        protected override void OnSettingChange()
        {
            base.OnSettingChange();
            Settings.Instance.SettingsData.environmentVolume = _slider.value;
        }
    }
}

