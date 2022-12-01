using GameSettings.Audio;
using System;
using System.Threading;

namespace GameSettings
{
    [Serializable]
    public class SettingsData
    {
        #region Video
        public int resolutionIndex;
        public int qualityIndex;
        public int windowModeIndex;
        #endregion

        #region Audio
        public float generalVolume;
        public float musicVolume;
        public float fxVolume;
        public bool mute;
        #endregion        

        public void LoadDefaultSettings(DefaultSettingsSO defaultData)
        {
            qualityIndex = defaultData.qualityIndex;
            resolutionIndex = defaultData.resolutionIndex;
            windowModeIndex = defaultData.windowModeIndex;
            generalVolume = defaultData.generalVolume;
            fxVolume = defaultData.fxVolume;
            mute = defaultData.mute;
        }
    }    
}

