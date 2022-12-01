using UnityEngine;

namespace GameSettings
{
    public class Settings : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SettingItem[] _settingItems;

        [Header("Data")]
        [SerializeField] private DefaultSettingsSO _defaultSettings;

        private SettingsData _settingsData;

        public SettingsData SettingsData => _settingsData;

        private static Settings _instance;
        public static Settings Instance => _instance;

        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            DontDestroyOnLoad(gameObject);

            Initialize();
        }

        public void Initialize()
        {
            _settingsData = new SettingsData();

            LoadSettings();            
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

            NotifyLoad();
        }

        private void LoadPlayerPrefsSettings()
        {
            string settings = PlayerPrefs.GetString("Settings");

            JsonUtility.FromJsonOverwrite(settings, _settingsData);

            NotifyLoad();
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

