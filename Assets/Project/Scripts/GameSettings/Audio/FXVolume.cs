namespace GameSettings.Audio
{
    public class FXVolume : VolumeSlider
    {
        public override void OnSettingsLoaded()
        {
            base.OnSettingsLoaded();

            float volume = Settings.SettingsData.fxVolume;

            _slider.value = (int)volume;
        }

        protected override void OnSettingsChange()
        {
            Settings.SettingsData.fxVolume = _slider.value;
        }
    }
}

