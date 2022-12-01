using System.Collections.Generic;
using UnityEngine;

namespace GameSettings.Video
{
    public class QualitySetter : SettingItem
    {
        [Header("References")]
        [SerializeField] private Selector _selector;

        public override void OnSettingsLoaded()
        {
            InitializeDropdown();

            _selector.SetValue(Settings.SettingsData.qualityIndex);
        }

        private void InitializeDropdown()
        {
            string[] qualityNames = QualitySettings.names;

            List<SelectorOption> selectorOptions = new List<SelectorOption>();

            foreach (var qualityName in qualityNames)
            {

                selectorOptions.Add(new SelectorOption(qualityName));
            }

            _selector.SetOptions(selectorOptions.ToArray());

            _selector.OnValueChanged += SetQuality;

        }

        private void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex, true);
            OnSettingsChange();
        }

        private void OnDestroy()
        {
            _selector.OnValueChanged -= SetQuality;
        }

        protected override void OnSettingsChange()
        {
            Settings.SettingsData.qualityIndex = _selector.CurrentValue;
        }
    }
}

