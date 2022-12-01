namespace GameSettings.Audio
{
    public class MusicVolume : VolumeSlider
    {
        public override void OnSettingsLoaded()
        {
            base.OnSettingsLoaded();

            float volume = Settings.SettingsData.musicVolume;

            _slider.value = (int)volume;
        }

        protected override void OnSettingsChange()
        {
            Settings.SettingsData.musicVolume = _slider.value;
        }
    }
}

