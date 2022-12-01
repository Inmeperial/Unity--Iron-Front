using UnityEngine;

namespace GameSettings
{
    public class Settings : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SettingItem[] _settingItems;

        [Header("Data")]
        [SerializeField] private DefaultSettingsSO _defaultSettings;

        [Header("Debug")]
        [SerializeField] private static SettingsData _settingsData;

        public static SettingsData SettingsData => _settingsData;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Initialize()
        {
            _settingsData = new SettingsData();

            LoadSettings();

            NotifyLoad();
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
            {
                _settingsData.LoadDefaultSettings(_defaultSettings);
                return;
            }

            string settings = PlayerPrefs.GetString("Settings");

            JsonUtility.FromJsonOverwrite(settings, _settingsData);
        }

        private void NotifyLoad()
        {
            foreach (SettingItem item in _settingItems)
            {
                item.OnSettingsLoaded();
            }
        }
    }
}

