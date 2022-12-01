namespace GameSettings.Audio
{
    public class GeneralVolume : VolumeSlider
    {
        public override void OnSettingsLoaded()
        {
            base.OnSettingsLoaded();

            float volume = Settings.SettingsData.generalVolume;

            _slider.value = (int)volume;
        }

        protected override void OnSettingsChange()
        {
            Settings.SettingsData.generalVolume = _slider.value;
        }
    }
}

