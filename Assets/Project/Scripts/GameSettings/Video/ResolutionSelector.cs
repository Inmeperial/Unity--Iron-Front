using System.Collections.Generic;
using UnityEngine;

namespace GameSettings.Video
{
    public class ResolutionSelector : SettingSelector
    {
        private List<Resolution> _resolutions = new List<Resolution>();

        protected override void Configure()
        {
            base.Configure();
            Debug.Log("CONFIGURE RESOLUTION SELECTOR");

            int resolutionIndex = Settings.Instance.SettingsData.resolutionIndex;

            //if (resolutionIndex < 0)
            //{
            //    int count = 0;


            //    foreach (Resolution resolution in _resolutions)
            //    {
            //        if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
            //        {
            //            resolutionIndex = count;
            //            break;
            //        }
            //        count++;
            //    }
            //}
            _selector.SetValue(resolutionIndex);
        }

        protected override void InitializeSelector()
        {
            Debug.Log("INITIALIZE RESOLUTION SELECTOR");
            List<SelectorOption> selectorOptions = new List<SelectorOption>();

            Resolution[] screenRes = Screen.resolutions;

            for (int i = screenRes.Length-1; i >= 0; i--)
            {
                Resolution resolution = screenRes[i];

                bool isValid = true;

                foreach (Resolution res in _resolutions)
                {
                    if (res.width == resolution.width && res.height == resolution.height)
                    {
                        isValid = false;
                        break;
                    }
                }

                if (!isValid)
                    continue;

                _resolutions.Insert(0, resolution);
            }

            foreach (Resolution res in _resolutions)
            {
                selectorOptions.Add(new SelectorOption(res.width + "x" + res.height));
            }
            _selector.SetOptions(selectorOptions.ToArray());
        }

        protected override void OnValueChanged(int value)
        {

        }

        protected override void ApplySetting()
        {
            Resolution selectedResolution = _resolutions[_selector.CurrentValue];

            Settings.Instance.SetResolution(selectedResolution);
            Settings.Instance.SettingsData.resolutionIndex = _selector.CurrentValue;
        }    
    }
}

