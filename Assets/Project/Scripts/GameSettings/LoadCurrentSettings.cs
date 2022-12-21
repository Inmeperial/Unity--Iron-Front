using UnityEngine;

namespace GameSettings
{
    public class LoadCurrentSettings : MonoBehaviour
    {
        public void LoadSettings()
        {
            Settings.Instance.ConfigureCurrentSettings();
        }
    }
}

