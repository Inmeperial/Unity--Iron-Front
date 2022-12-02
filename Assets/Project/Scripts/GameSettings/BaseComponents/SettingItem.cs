using System.Threading.Tasks;
using UnityEngine;

namespace GameSettings
{
    public abstract class SettingItem : MonoBehaviour
    {
        public virtual void Initialize()
        {
            Configure();
        }
        protected abstract void Configure();
        protected abstract void OnSettingChange();
    }
}

