using System;
using UnityEngine;

namespace GameSettings
{
    public abstract class SettingItem : MonoBehaviour
    {
        public abstract void OnSettingsLoaded();
        protected abstract void OnSettingsChange();
    }


}

