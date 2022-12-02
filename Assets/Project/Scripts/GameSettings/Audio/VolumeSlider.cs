using System.Threading.Tasks;
using UnityEngine;

namespace GameSettings.Audio
{
    public abstract class VolumeSlider : SettingsSlider
    {
        [Header("Configs")]
        [SerializeField] protected string _volumeParameterName;

        protected override void OnSettingChange()
        {
            Settings.Instance.UpdateVolume(_volumeParameterName, _slider.value);
        }      
    }
}

