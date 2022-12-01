using UnityEngine;

namespace GameSettings
{
    public class LoadDefaultSettings : MonoBehaviour
    {
        public void Load()
        {
            Settings.Instance.LoadDefaultSettings();
        }
    }
}

