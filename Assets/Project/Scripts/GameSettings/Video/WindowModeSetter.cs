using UnityEngine;

namespace GameSettings.Video
{
    public class WindowModeSetter : SettingItem
    {
        [Header("References")]
        [SerializeField] private Selector _selector;

        public override void OnSettingsLoaded()
        {
            Initialize();

            _selector.SetValue(Settings.SettingsData.windowModeIndex);
        }

        private void Initialize()
        {
            SelectorOption[] selectorOptions = new SelectorOption[4];

            selectorOptions[0] = new SelectorOption("Full Screen");
            selectorOptions[1] = new SelectorOption("Borderless Full Screen");
            selectorOptions[2] = new SelectorOption("Maximized Window");
            selectorOptions[3] = new SelectorOption("Window");

            _selector.SetOptions(selectorOptions);

            _selector.OnValueChanged += SetWindowMode;
        }

        public void SetWindowMode(int screenModeIndex)
        {
            FullScreenMode selectedMode = (FullScreenMode)screenModeIndex;
            Screen.fullScreenMode = selectedMode;

            OnSettingsChange();
        }

        private void OnDestroy()
        {
            _selector.OnValueChanged -= SetWindowMode;
        }

        protected override void OnSettingsChange()
        {
            Settings.SettingsData.windowModeIndex = _selector.CurrentValue;
        }
    }
}

