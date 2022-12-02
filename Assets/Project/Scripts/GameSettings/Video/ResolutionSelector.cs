using System.Collections.Generic;
using UnityEngine;

namespace GameSettings.Video
{
    public class ResolutionSelector : SettingSelector
    {
        private Resolution[] _resolutions;   

        protected override void Configure()
        {
            base.Configure();

            int resolutionIndex = Settings.Instance.SettingsData.resolutionIndex;

            if (resolutionIndex < 0)
            {
                int count = 0;

                _resolutions = Screen.resolutions;

                foreach (Resolution resolution in _resolutions)
                {
                    if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
                    {
                        resolutionIndex = count;
                        break;
                    }
                    count++;
                }
            }
            _selector.SetValue(resolutionIndex);
        }

        protected override void InitializeSelector()
        {
            List<SelectorOption> selectorOptions = new List<SelectorOption>();

            _resolutions = Screen.resolutions;

            foreach (Resolution resolution in _resolutions)
            {
                selectorOptions.Add(new SelectorOption(resolution.width + "x" + resolution.height));
            }
            _selector.SetOptions(selectorOptions.ToArray());
        }

        protected override void OnValueChanged(int value)
        {
            Resolution selectedResolution = _resolutions[value];

            Settings.Instance.SetResolution(selectedResolution);

            OnSettingChange();
        }

        protected override void OnSettingChange()
        {
            Settings.Instance.SettingsData.resolutionIndex = _selector.CurrentValue;
        }    
    }
}

