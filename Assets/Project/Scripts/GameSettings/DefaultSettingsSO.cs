using UnityEngine;

namespace GameSettings
{
    [CreateAssetMenu(fileName = "Default Settings", menuName = "Scriptable Objects/DefaultSettings")]
    public class DefaultSettingsSO : ScriptableObject
    {
        [Header("Video")]
        [Tooltip("1: lowest - 4: highest")]
        public int qualityIndex;
        [Tooltip("-1 equals display resolution")]
        public int resolutionIndex;
        [Tooltip("0: Fullscreen - 1: Borderless Full Screen - 2: Maximized Window - 3: Window")]
        public int windowModeIndex;

        [Header("Sound")]
        public float generalVolume;
        public float musicVolume;
        public float fxVolume;
        public bool mute;
    }
}

