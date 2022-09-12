using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MechaPartButton : CustomButton
{
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private Slider _currentHPSlider;
    [SerializeField] private Slider _damagePreviewSlider;


    [SerializeField] private TextMeshProUGUI _bulletsCountText;
    
    private int _bulletsCount;
    private Character _attackingMecha;
    private Character _mechaSelectedToAttack;
    private MechaPart _part;

    public int BulletsCount { get => _bulletsCount; set => _bulletsCount = value; }

    public Action OnButtonClicked;
    public Action<int> OnAddBullets;
    public Action<int> OnReduceBullets;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        UpdateDamagePreviewSlider();
        
        _damagePreviewSlider.gameObject.SetActive(true);

        _part.SetShader(SwitchTextureEnum.TextureFresnel);
    }
    
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (_bulletsCount <= 0)
        {
            _damagePreviewSlider.gameObject.SetActive(false);
            _part.SetShader(SwitchTextureEnum.TextureClean);
        }            
    }

    protected override void PressRight()
    {
        base.PressRight();

        if (_bulletsCount <= 0)
            _part.SetShader(SwitchTextureEnum.TextureClean);

        ReduceBullets();        
        UpdateDamagePreviewSlider();
    }

    protected override void PressLeft()
    {
        base.PressLeft();

        AddBullets();
        UpdateDamagePreviewSlider();
    }

    public void SetSlider(float minValue, float maxValue)
    {
        _currentHPSlider.minValue = minValue;
        _currentHPSlider.maxValue = maxValue;
        
        _damagePreviewSlider.minValue = minValue;
        _damagePreviewSlider.maxValue = maxValue;
    }

    public void UpdateHP(float value)
    {
        _currentHPSlider.value = value;
        _hpText.text = value.ToString();
    }

    public void UpdateDamagePreviewSlider()
    {
        Gun gun = _attackingMecha.GetSelectedGun();

        float estimatedDamage = gun.GetBulletDamage() * _bulletsCount;

        _damagePreviewSlider.value = _currentHPSlider.value - estimatedDamage;
    }

    public void SetBulletsCount(int value)
    {
        _bulletsCountText.text = value.ToString();
        _bulletsCount = value;
    }

    public void SetMechas(Character attacker, Character characterToAttack, MechaPart part)
    {
        _attackingMecha = attacker;
        _mechaSelectedToAttack = characterToAttack;
        _part = part;
    }

    public void ResetButton()
    {
        _bulletsCount = 0;
        _bulletsCountText.text = "0";

        _attackingMecha = null;
        _mechaSelectedToAttack = null;

        ClearSelectedPartShader();

        _part = null;
    }
    
    public void ShowButton()
    {
        EnableButton();
        SetBulletsCount(0);
        gameObject.SetActive(true);
    }
    public void HideButton()
    {
        DisableButton();
        ClearSelectedPartShader();
        gameObject.SetActive(false);
    }

    private void ClearSelectedPartShader()
    {
        if (_part == null)
            return;

        _part.SetShader(SwitchTextureEnum.TextureClean);
    }    
    private void EnableButton() => interactable = true;
    private void DisableButton() => interactable = false;

    private void AddBullets()
    {
        Gun gun = _attackingMecha.GetSelectedGun();

        if (gun.GetAvailableBullets() <= 0 || _bulletsCount >= gun.GetMaxBullets())
            return;

        _bulletsCount += gun.GetBulletsPerClick();

        gun.ReduceAvailableBullets();

        _bulletsCountText.text = _bulletsCount.ToString();

        OnButtonClicked?.Invoke();
        OnAddBullets?.Invoke(gun.GetBulletsPerClick());
    }

    private void ReduceBullets()
    {
        if (_bulletsCount <= 0)
            return;

        Gun gun = _attackingMecha.GetSelectedGun();

        if (gun.GetMaxBullets() <= gun.GetAvailableBullets())
            return;

        gun.IncreaseAvailableBullets();

        _bulletsCount = _bulletsCount > 0 ? (_bulletsCount - gun.GetBulletsPerClick()) : 0;

        _bulletsCountText.text = _bulletsCount.ToString();

        OnButtonClicked?.Invoke();
        OnReduceBullets?.Invoke(gun.GetBulletsPerClick());
    }
    public void Attack()
    {
        if (_bulletsCount <= 0)
            return;

        Gun selectedGun = _attackingMecha.GetSelectedGun();
        selectedGun.Attack(_part, _bulletsCount);
        selectedGun.GunSkill(_part);
    }
}
