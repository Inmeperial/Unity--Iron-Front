using System.Collections.Generic;
using UnityEngine;

namespace GameSettings
{
    [CreateAssetMenu(fileName = "Default Settings", menuName = "Scriptable Objects/DefaultSettings")]
    public class DefaultSettingsSO : ScriptableObject
    {
        [Header("Video")]
        [Tooltip("1: lowest - 4: highest")]
        public int qualityIndex;
        [Tooltip("0: Fullscreen - 1: Borderless Full Screen - 2: Maximized Window - 3: Window")]
        public FullScreenMode windowModeIndex;

        [Header("Sound")]
        public float generalVolume;
        public float musicVolume;
        public float fxVolume;
        public float environmentVolume;
        public bool mute;

        public int GetDefaultResolutionIndex()
        {
            Resolution[] screenRes = Screen.resolutions;

            List<Resolution> resolutions = new List<Resolution>();
            for (int i = screenRes.Length - 1; i >= 0; i--)
            {
                Resolution resolution = screenRes[i];

                bool isValid = true;

                foreach (Resolution res in resolutions)
                {
                    if (res.width == resolution.width && res.height == resolution.height)
                    {
                        isValid = false;
                        break;
                    }
                }

                if (!isValid)
                    continue;

                resolutions.Insert(0, resolution);
            }

            return resolutions.Count-1;
        }
    }
}

