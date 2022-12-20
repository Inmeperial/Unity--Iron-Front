namespace GameSettings.Audio
{
    public class GeneralVolume : VolumeSlider
    {
        protected override void Configure()
        {
            base.Configure();
            float volume = Settings.Instance.SettingsData.generalVolume;

            _slider.value = (int)volume;
        }

        protected override void ApplySetting()
        {
            base.ApplySetting();
            Settings.Instance.SettingsData.generalVolume = _slider.value;
        }
    }
}

