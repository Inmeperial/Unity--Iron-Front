using UnityEngine;

namespace GameSettings.Video
{
    public class WindowModeSelector : SettingSelector
    {
        protected override void Configure()
        {
            base.Configure();

            int windowModeIndex = Settings.Instance.SettingsData.windowModeIndex;

            _selector.SetValue(windowModeIndex);
        }

        protected override void InitializeSelector()
        {
            SelectorOption[] selectorOptions = new SelectorOption[4];

            selectorOptions[0] = new SelectorOption("Full Screen");
            selectorOptions[1] = new SelectorOption("Borderless Full Screen");
            selectorOptions[2] = new SelectorOption("Maximized Window");
            selectorOptions[3] = new SelectorOption("Window");

            _selector.SetOptions(selectorOptions);
        }

        protected override void OnValueChanged(int value)
        {
            
        }

        protected override void ApplySetting()
        {
            FullScreenMode selectedMode = (FullScreenMode)_selector.CurrentValue;

            Settings.Instance.SetWindowMode(selectedMode);
            Settings.Instance.SettingsData.windowModeIndex = _selector.CurrentValue;
        }        
    }
}

