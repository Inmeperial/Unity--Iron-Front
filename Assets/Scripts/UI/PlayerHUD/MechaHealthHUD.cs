using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MechaHealthHUD : MonoBehaviour
{
    [SerializeField] private GameObject _container;

    [Header("Body")]
    [SerializeField] private Slider _bodyHealthSlider;
    [SerializeField] private TextMeshProUGUI _bodyHealthText;

    [Header("Guns")]
    [SerializeField] private Slider _leftGunHealthSlider;
    [SerializeField] private TextMeshProUGUI _leftGunHealthText;
    [SerializeField] private Slider _rightGunHealthSlider;
    [SerializeField] private TextMeshProUGUI _rightGunHealthText;

    [Header("Legs")]
    [SerializeField] private Slider _legsHealthSlider;
    [SerializeField] private TextMeshProUGUI _legsHealthText;

    private void Awake()
    {
        Character[] mechas = FindObjectsOfType<Character>();

        foreach (var mecha in mechas)
        {
            mecha.OnMechaSelected += OnMechaSelected;

            mecha.GetBody().OnHealthChanged += OnBodyHPChange;

            Gun leftGun = mecha.GetLeftGun();

            if (leftGun)
                leftGun.OnHealthChanged += OnLeftGunHPChange;

            Gun rightGun = mecha.GetRightGun();

            if (rightGun)
                rightGun.OnHealthChanged += OnRightGunHPChange;

            mecha.GetLegs().OnHealthChanged += OnLegsHPChange;
        }
        _container.SetActive(false);
    }

    public void ShowHealthContainer() => _container.SetActive(true);
    public void HideHealthContainer() => _container.SetActive(false);

    private void OnMechaSelected(Character mecha)
    {
        Body body = mecha.GetBody();
        _bodyHealthSlider.maxValue = body.MaxHP;
        _bodyHealthSlider.value = body.CurrentHP;
        _bodyHealthText.text = body.CurrentHP.ToString();

        Gun leftGun = mecha.GetLeftGun();
        if (leftGun)
        {
            _leftGunHealthSlider.maxValue = leftGun.MaxHP;
            _leftGunHealthSlider.value = leftGun.CurrentHP;
            _leftGunHealthText.text = leftGun.CurrentHP.ToString();
        }
        else
        {
            _leftGunHealthSlider.maxValue = 1;
            _leftGunHealthSlider.value = 0;
            _leftGunHealthText.text = "0";
        }

        Gun rightGun = mecha.GetRightGun();
        if (rightGun)
        {
            _rightGunHealthSlider.maxValue = rightGun.MaxHP;
            _rightGunHealthSlider.value = rightGun.CurrentHP;
            _rightGunHealthText.text = rightGun.CurrentHP.ToString();
        }
        else
        {
            _rightGunHealthSlider.maxValue = 1;
            _rightGunHealthSlider.value = 0;
            _rightGunHealthText.text = "0";
        }

        Legs legs = mecha.GetLegs();
        _legsHealthSlider.maxValue = legs.MaxHP;
        _legsHealthSlider.value = legs.CurrentHP;
        _legsHealthText.text = legs.CurrentHP.ToString();

        ShowHealthContainer();
    }

    private void OnBodyHPChange(float newValue)
    {
        _bodyHealthSlider.value = newValue;
        _bodyHealthText.text = newValue.ToString();
    }

    private void OnLeftGunHPChange(float newValue)
    {
        _leftGunHealthSlider.value = newValue;
        _leftGunHealthText.text = newValue.ToString();
    }

    private void OnRightGunHPChange(float newValue)
    {
        _rightGunHealthSlider.value = newValue;
        _rightGunHealthText.text = newValue.ToString();
    }

    private void OnLegsHPChange(float newValue)
    {
        _legsHealthSlider.value = newValue;
        _legsHealthText.text = newValue.ToString();
    }

    private void OnDestroy()
    {
        Character[] mechas = FindObjectsOfType<Character>();

        foreach (var mecha in mechas)
        {
            mecha.OnMechaSelected -= OnMechaSelected;

            mecha.GetBody().OnHealthChanged -= OnBodyHPChange;

            Gun leftGun = mecha.GetLeftGun();

            if (leftGun)
                leftGun.OnHealthChanged -= OnLeftGunHPChange;

            Gun rightGun = mecha.GetRightGun();

            if (rightGun)
                rightGun.OnHealthChanged -= OnRightGunHPChange;

            mecha.GetLegs().OnHealthChanged -= OnLegsHPChange;
        }
    }
}
