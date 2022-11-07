using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthHUD : Initializable
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
        HideHealthContainer();
    }

    public override void Initialize()
    {
        Character[] mechas = GameManager.Instance.GetMechas();

        foreach (Character mecha in mechas)
        {
            mecha.OnMechaSelected += OnMechaSelected;

            Body body = mecha.GetBody();
            body.OnHealthChanged += OnBodyHPChange;

            Gun leftGun = mecha.GetLeftGun();

            if (leftGun)
                leftGun.OnHealthChanged += OnLeftGunHPChange;

            Gun rightGun = mecha.GetRightGun();

            if (rightGun)
                rightGun.OnHealthChanged += OnRightGunHPChange;

            Legs legs = mecha.GetLegs();
            legs.OnHealthChanged += OnLegsHPChange;
        }

        GameManager.Instance.OnBeginTurn += ShowHealthContainer;
        GameManager.Instance.OnEndTurn += HideHealthContainer;
        GameManager.Instance.OnEnemyMechaSelected += HideHealthContainer;
        GameManager.Instance.OnEnemyMechaDeselected += ShowHealthContainer;
        GameManager.Instance.OnBeginAttackPreparations += HideHealthContainer;
        GameManager.Instance.OnMechaAttackPreparationsFinished += ShowHealthContainer;
    }

    private void ShowHealthContainer() => _container.SetActive(true);
    private void HideHealthContainer() => _container.SetActive(false);

    private void OnMechaSelected(Character mecha)
    {
        Body body = mecha.GetBody();
        _bodyHealthSlider.maxValue = body.MaxHp;
        _bodyHealthSlider.value = body.CurrentHP;
        _bodyHealthText.text = body.CurrentHP.ToString();

        Gun leftGun = mecha.GetLeftGun();
        if (leftGun)
        {
            _leftGunHealthSlider.maxValue = leftGun.MaxHp;
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
            _rightGunHealthSlider.maxValue = rightGun.MaxHp;
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
        _legsHealthSlider.maxValue = legs.MaxHp;
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
        Character[] mechas = GameManager.Instance.GetMechas();

        foreach (var mecha in mechas)
        {
            mecha.OnMechaSelected -= OnMechaSelected;

            Body body = mecha.GetBody();
            body.OnHealthChanged -= OnBodyHPChange;

            Gun leftGun = mecha.GetLeftGun();

            if (leftGun)
                leftGun.OnHealthChanged -= OnLeftGunHPChange;

            Gun rightGun = mecha.GetRightGun();

            if (rightGun)
                rightGun.OnHealthChanged -= OnRightGunHPChange;

            Legs legs = mecha.GetLegs();
            legs.OnHealthChanged -= OnLegsHPChange;
        }

        GameManager.Instance.OnBeginTurn -= ShowHealthContainer;
        GameManager.Instance.OnEndTurn -= HideHealthContainer;
    }
}
