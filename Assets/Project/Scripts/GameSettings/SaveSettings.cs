using UnityEngine;

namespace GameSettings
{
    public class SaveSettings : MonoBehaviour
    {
        public void Save()
        {
            Settings.Instance.SaveSettings();
        }
    }
}

