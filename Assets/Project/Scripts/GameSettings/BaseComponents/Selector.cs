using System;
using TMPro;
using UnityEngine;

namespace GameSettings
{
    public class Selector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _text;

        private SelectorOption[] _options;

        private int _currentValue;

        public Action<int> OnValueChanged { get; set; }
        public int CurrentValue { get => _currentValue; }

        public void SetOptions(SelectorOption[] options)
        {
            _options = options;
        }

        public void ChangeToDirection(int direction)
        {
            int desiredValue = CurrentValue + direction;

            if (desiredValue < 0 || desiredValue >= _options.Length)
                return;

            SetValue(desiredValue);
        }

        public void SetValue(int value)
        {
            _text.text = _options[value].TextValue;

            _currentValue = value;

            OnValueChanged?.Invoke(value);
        }
    }
}

