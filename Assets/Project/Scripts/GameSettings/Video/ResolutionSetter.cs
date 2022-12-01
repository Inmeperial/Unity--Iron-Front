using System.Collections.Generic;
using UnityEngine;

namespace GameSettings.Video
{
    public class ResolutionSetter : SettingItem
    {
        [Header("References")]
        [SerializeField] private Selector _selector;

        private Resolution[] _resolutions;

        public override void OnSettingsLoaded()
        {
            Initialize();

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

        private void Initialize()
        {
            List<SelectorOption> selectorOptions = new List<SelectorOption>();

            _resolutions = Screen.resolutions;

            foreach (Resolution resolution in _resolutions)
            {
                selectorOptions.Add(new SelectorOption(resolution.width + "x" + resolution.height));
            }
            _selector.SetOptions(selectorOptions.ToArray());

            _selector.OnValueChanged += SetResolution;
        }

        private void SetResolution(int resolutionIndex)
        {
            Resolution selectedResolution = _resolutions[resolutionIndex];

            bool isFullScreen = Screen.fullScreen;

            Screen.SetResolution(selectedResolution.width, selectedResolution.height, isFullScreen);

            OnSettingsChange();
        }

        private void OnDestroy()
        {
            _selector.OnValueChanged -= SetResolution;
        }

        protected override void OnSettingsChange()
        {
            Settings.Instance.SettingsData.resolutionIndex = _selector.CurrentValue;
        }
    }
}

