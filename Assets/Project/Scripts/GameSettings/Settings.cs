using GameSettings.Audio;
using System.Collections;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Audio;

namespace GameSettings
{
    public class Settings : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioMixer _audioMixer;

        [Header("Data")]
        [SerializeField] private DefaultSettingsSO _defaultSettings;

        private SettingsData _settingsData;

        public SettingsData SettingsData => _settingsData;

        private static Settings _instance;
        public static Settings Instance => _instance;

        public void Initialize()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            DontDestroyOnLoad(gameObject);

            _settingsData = new SettingsData();

            LoadSettings();
        }

        public void ApplySettingsOnGameOpen()
        {
            UpdateVolume("MasterVolume", _settingsData.generalVolume);
            UpdateVolume("MusicVolume", _settingsData.musicVolume);
            UpdateVolume("FXVolume", _settingsData.fxVolume);
            UpdateVolume("EnvironmentVolume", _settingsData.environmentVolume);
            SetMute(_settingsData.mute);

            SetQualityLevel(_settingsData.qualityIndex);
        }

        public void SaveSettings()
        {
            string settings = JsonUtility.ToJson(_settingsData);

            PlayerPrefs.SetString("Settings", settings);

            PlayerPrefs.Save();
        }

        public void LoadSettings()
        {
            if (!PlayerPrefs.HasKey("Settings"))
                LoadDefaultSettings();
            else
                LoadPlayerPrefsSettings();
        }

        public void LoadDefaultSettings()
        {
            _settingsData.LoadDefaultSettings(_defaultSettings);
        }

        private void LoadPlayerPrefsSettings()
        {
            string settings = PlayerPrefs.GetString("Settings");

            JsonUtility.FromJsonOverwrite(settings, _settingsData);
        }

#region Audio
        public void UpdateVolume(string name, float value)
        {
            if (_settingsData.mute)
                value = 0.0001f;

            _audioMixer.SetFloat(name, SoundUtilities.FloatToDB(value / 100));
        }

        public void SetMute(bool mute)
        {
            if (mute)
                UpdateVolume("MasterVolume", 0.0001f);
            else
                UpdateVolume("MasterVolume", _settingsData.generalVolume);
        }
        #endregion

        #region Video

        public void SetResolution(Resolution resolution)
        {
            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
                return;

            bool isFullScreen = Screen.fullScreen;

            Screen.SetResolution(resolution.width, resolution.height, isFullScreen);
        }

        public void SetWindowMode(FullScreenMode screenMode)
        {
            if (screenMode == Screen.fullScreenMode)
                return;

            Screen.fullScreenMode = screenMode;
        }

        public void SetQualityLevel(int index)
        {
            if (index == QualitySettings.GetQualityLevel())
                return;

            QualitySettings.SetQualityLevel(index, true);
        }

        #endregion
    }


}

