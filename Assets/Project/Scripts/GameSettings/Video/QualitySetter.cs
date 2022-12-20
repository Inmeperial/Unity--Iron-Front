﻿using System.Collections.Generic;
using UnityEngine;

namespace GameSettings.Video
{
    public class QualitySetter : SettingSelector
    {
        protected override void Configure()
        {
            base.Configure();

            int qualityIndex = Settings.Instance.SettingsData.qualityIndex;

            _selector.SetValue(qualityIndex);
        }

        protected override void InitializeSelector()
        {
            string[] qualityNames = QualitySettings.names;

            List<SelectorOption> selectorOptions = new List<SelectorOption>();

            foreach (var qualityName in qualityNames)
            {

                selectorOptions.Add(new SelectorOption(qualityName));
            }

            _selector.SetOptions(selectorOptions.ToArray());
        }

        protected override void OnValueChanged(int value)
        {
            
        }

        protected override void ApplySetting()
        {
            Settings.Instance.SetQualityLevel(_selector.CurrentValue);
            Settings.Instance.SettingsData.qualityIndex = _selector.CurrentValue;
        }
    }
}

