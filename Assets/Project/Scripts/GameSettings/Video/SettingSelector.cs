using UnityEngine;

namespace GameSettings.Video
{
    public abstract class SettingSelector : SettingItem
    {
        [Header("References")]
        [SerializeField] protected Selector _selector;

        public override void Initialize()
        {
            InitializeSelector();

            base.Initialize();
        }

        protected abstract void InitializeSelector();

        protected abstract void OnValueChanged(int value);

        protected override void Configure()
        {
            _selector.OnValueChanged += OnValueChanged;
        }

        protected virtual void OnDestroy()
        {
            _selector.OnValueChanged -= OnValueChanged;
        }
    }
}

