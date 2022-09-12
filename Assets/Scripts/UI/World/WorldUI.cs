using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class WorldUI : Initializable
{
    [Header("Mecha")]
    [SerializeField] private Character _owner;
    public Character Owner => _owner;

    #region Fields
    [Header("Status")]
    [SerializeField] private GameObject _statusContainer;
    [SerializeField] private float _showDuration;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private GameObject _overweightIcon;
    
    [Header("Body")]
    [SerializeField] private Slider _bodyHpSlider;
    [SerializeField] private Slider _bodyDamageSlider;

    [Header("Left Arm")]
    [SerializeField] private Slider _leftGunHpSlider;
    [SerializeField] private Slider _leftGunDamageSlider;

    [Header("Right Arm")]
    [SerializeField] private Slider _rightGunHpSlider;
    [SerializeField] private Slider _rightGunDamageSlider;

    [Header("Legs")]
    [SerializeField] private Slider _legsHpSlider;
    [SerializeField] private Slider _legsDamageSlider;

    [Header("Actions")]
    [SerializeField] private GameObject _moveActionIcon;
    [SerializeField] private GameObject _attackActionIcon;
    #endregion

    private int _partsBeingUpdated;

    public Action<Character> OnUpdateFinished;

    public override void Initialize()
    {
        SetSlidersLimits();
    }
    public void Show()
    {
        SetName(_owner.GetCharacterName());
        float bodyHP = _owner.GetBody().CurrentHP;
        SetBodyHPBar(bodyHP);

        float leftGunHP = 0;

        if (_owner.GetLeftGun())
            leftGunHP = _owner.GetLeftGun().CurrentHP;

        SetLeftGunHPBar(leftGunHP);

        float rightGunHP = 0;

        if (_owner.GetRightGun())
            rightGunHP = _owner.GetRightGun().CurrentHP;

        SetRightGunHPBar(rightGunHP);

        float legsHP = _owner.GetLegs().CurrentHP;

        SetLegsHPBar(legsHP);

        MoveActionIconStatus(_owner.CanMove());

        AttackActionIconStatus(_owner.CanAttack());

        OverweightIconStatus(_owner.IsOverweight());

        _statusContainer.SetActive(true);
    }
    public void Hide() => _statusContainer.SetActive(false);

    public void HideWithTimer() => StartCoroutine(DeactivateUI(_showDuration));

    private IEnumerator DeactivateUI(float timer)
    {
        yield return new WaitForSeconds(timer);
        _statusContainer.SetActive(false);
    }
    
    
    private void SetSlidersLimits()
    {
        float bodyMax = _owner.GetBody().MaxHp;
        _bodyHpSlider.maxValue = bodyMax;
        _bodyHpSlider.minValue = 0;
        _bodyDamageSlider.maxValue = bodyMax;
        _bodyDamageSlider.minValue = 0;

        float rightGunMax = _owner.GetRightGun().MaxHP;
        _rightGunHpSlider.maxValue = rightGunMax;
        _rightGunHpSlider.minValue = 0;
        _rightGunDamageSlider.maxValue = rightGunMax;
        _rightGunDamageSlider.minValue = 0;

        float leftGunMax = _owner.GetLeftGun().MaxHP;
        _leftGunHpSlider.maxValue = leftGunMax;
        _leftGunHpSlider.minValue = 0;
        _leftGunDamageSlider.maxValue = leftGunMax;
        _leftGunDamageSlider.minValue = 0;

        float legsMax = _owner.GetLegs().MaxHP;
        _legsHpSlider.maxValue = legsMax;
        _legsHpSlider.minValue = 0;
        _legsDamageSlider.maxValue = legsMax;
        _legsDamageSlider.minValue = 0;
    }

    //public void Toggle(bool state) => _isToggledOn = state;

    #region WorldCanvas
    private void SetBodyHPBar(float quantity)
    {
        if (quantity < 0)
        {
            _bodyHpSlider.value = 0;
            _bodyDamageSlider.value = 0;
        }
        else
        {
            _bodyHpSlider.value = quantity;
            _bodyDamageSlider.value = quantity;
        }
    }
   
    public void UpdateBodyHPBar(float receivedDamage)
    {
        _bodyHpSlider.value = _owner.GetBody().CurrentHP;

        _partsBeingUpdated++;

        Show();
        StartCoroutine(UpdateBodySlider(receivedDamage));
    }

    private IEnumerator UpdateBodySlider(float receivedDamage)
    {
        for (int i = 1; i <= receivedDamage; i++)
        {
            _bodyDamageSlider.value -= 1;
            yield return new WaitForEndOfFrame();
        }
       
        if (_bodyDamageSlider.value < 0)
            _bodyDamageSlider.value = 0;

        PartFinishedUpdating();
    }
   
    private void SetLeftGunHPBar(float quantity)
    {
        if (quantity < 0)
        {
            _leftGunHpSlider.value = 0;
            _leftGunDamageSlider.value = 0;
        }
        else
        {
            _leftGunHpSlider.value = quantity;
            _leftGunDamageSlider.value = quantity;
        }
    }
   
    public void UpdateLeftGunHPBar(float receivedDamage)
    {
        _leftGunHpSlider.value = _owner.GetLeftGun().CurrentHP;

        _partsBeingUpdated++;

        Show();

        StartCoroutine(UpdateLeftGunSlider(receivedDamage));
    }

    private IEnumerator UpdateLeftGunSlider(float receivedDamage)
    {
        for (int i = 1; i <= receivedDamage; i++)
        {
            _leftGunDamageSlider.value -= 1;
            yield return new WaitForEndOfFrame();
        }
       
        if (_leftGunDamageSlider.value < 0)
            _leftGunDamageSlider.value = 0;

        PartFinishedUpdating();
    }

    private void SetRightGunHPBar(float quantity)
    {
        if (quantity < 0)
        {
            _rightGunHpSlider.value = 0;
            _rightGunDamageSlider.value = 0;
        }
        else
        {
            _rightGunHpSlider.value = quantity;
            _rightGunDamageSlider.value = quantity;
        }
    }
    
    public void UpdateRightGunHPBar(float receivedDamage)
    {
        _rightGunHpSlider.value = _owner.GetRightGun().CurrentHP;

        _partsBeingUpdated++;

        Show();

        StartCoroutine(UpdateRightGunSlider(receivedDamage));
    }

    private IEnumerator UpdateRightGunSlider(float receivedDamage)
    {
        for (int i = 1; i <= receivedDamage; i++)
        {
            _rightGunDamageSlider.value -= 1;
            yield return new WaitForEndOfFrame();
        }
       
        if (_rightGunDamageSlider.value < 0)
            _rightGunDamageSlider.value = 0;

        PartFinishedUpdating();
    }

    private void SetLegsHPBar(float quantity)
    {
        if (quantity < 0)
        {
            _legsHpSlider.value = 0;
            _legsDamageSlider.value = 0;
        }
        else
        {
            _legsHpSlider.value = quantity;
            _legsDamageSlider.value = quantity;
        }
    }
    
    public void UpdateLegsHPBar(float receivedDamage)
    {
        _legsHpSlider.value = _owner.GetLegs().CurrentHP;

        _partsBeingUpdated++;

        Show();

        StartCoroutine(UpdateLegsSlider(receivedDamage));
    }

    private IEnumerator UpdateLegsSlider(float receivedDamage)
    {
        for (int i = 1; i <= receivedDamage; i++)
        {
            _legsDamageSlider.value -= 1;
            yield return new WaitForEndOfFrame();
        }
       
        if (_legsDamageSlider.value < 0)
            _legsDamageSlider.value = 0;

        PartFinishedUpdating();
    }

    private void PartFinishedUpdating()
    {
        _partsBeingUpdated--;

        if (_partsBeingUpdated > 0)
            return;

        _partsBeingUpdated = 0;

        OnUpdateFinished?.Invoke(_owner);
    }

    public void MoveActionIconStatus(bool status) => _moveActionIcon.SetActive(status);
    public void AttackActionIconStatus(bool status) => _attackActionIcon.SetActive(status);
    public void OverweightIconStatus(bool status) => _overweightIcon.SetActive(status);
    private void SetName(string name) => _nameText.text = name;    
    #endregion

}
