using System;
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
    private Character _characterSelectedToAttack;
    private PartsMechaEnum _partEnum;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        UpdateDamagePreviewSlider();
        
        _damagePreviewSlider.gameObject.SetActive(true);

        _characterSelectedToAttack.SetShaderForPart(SwitchTextureEnum.TextureFresnel, _partEnum);

        if (_partEnum == PartsMechaEnum.body)
        {
            _characterSelectedToAttack.SetShaderForPart(SwitchTextureEnum.TextureFresnel, PartsMechaEnum.armL);
            _characterSelectedToAttack.SetShaderForPart(SwitchTextureEnum.TextureFresnel, PartsMechaEnum.armR);
        }
        
        if (_partEnum == PartsMechaEnum.legL)
        {
            _characterSelectedToAttack.SetShaderForPart(SwitchTextureEnum.TextureFresnel, PartsMechaEnum.legR);
        }
    }
    
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (_bulletsCount <= 0)
        {
            _damagePreviewSlider.gameObject.SetActive(false);
            _characterSelectedToAttack.SetShaderForPart(SwitchTextureEnum.TextureClean, _partEnum);
            
            if (_partEnum == PartsMechaEnum.body)
            {
                _characterSelectedToAttack.SetShaderForPart(SwitchTextureEnum.TextureClean, PartsMechaEnum.armL);
                _characterSelectedToAttack.SetShaderForPart(SwitchTextureEnum.TextureClean, PartsMechaEnum.armR);
            }
            
            if (_partEnum == PartsMechaEnum.legL)
            {
                _characterSelectedToAttack.SetShaderForPart(SwitchTextureEnum.TextureClean, PartsMechaEnum.legR);
            }
        }
            
    }

    protected override void PressRight()
    {
        base.PressRight();

        if (_bulletsCount <= 0)
        {
            _characterSelectedToAttack.SetShaderForPart(SwitchTextureEnum.TextureClean, _partEnum);
            
            if (_partEnum == PartsMechaEnum.body)
            {
                _characterSelectedToAttack.SetShaderForPart(SwitchTextureEnum.TextureClean, PartsMechaEnum.armL);
                _characterSelectedToAttack.SetShaderForPart(SwitchTextureEnum.TextureClean, PartsMechaEnum.armR);
            }
            
            if (_partEnum == PartsMechaEnum.legL)
            {
                _characterSelectedToAttack.SetShaderForPart(SwitchTextureEnum.TextureClean, PartsMechaEnum.legR);
            }
        }
        
        UpdateDamagePreviewSlider();
    }

    protected override void PressLeft()
    {
        base.PressLeft();
        UpdateDamagePreviewSlider();
    }

    public void SetSlider(float minValue, float maxValue)
    {
        _currentHPSlider.minValue = minValue;
        _currentHPSlider.maxValue = maxValue;
        
        _damagePreviewSlider.minValue = minValue;
        _damagePreviewSlider.maxValue = maxValue;
    }

    public void UpdateHpSlider(float value)
    {
        _currentHPSlider.value = value;
    }

    public void UpdateDamagePreviewSlider()
    {
        Character selectedCharacter = CharacterSelection.Instance.GetSelectedCharacter();

        Gun gun = selectedCharacter.GetSelectedGun();

        float estimatedDamage = gun.GetBulletDamage() * _bulletsCount;

        _damagePreviewSlider.value = _currentHPSlider.value - estimatedDamage;
    }

    public void SetHpText(string text)
    {
        _hpText.text = text;
    }

    public void SetBulletsCount(int value)
    {
        _bulletsCountText.text = value.ToString();
        _bulletsCount = value;
    }

    /// <summary>
    /// Sets the character and the part that corresponds to this button.
    /// </summary>
    /// <param name="character"></param>
    /// <param name="part">Body includes arms and legL includes legR</param>
    public void SetCharacter(Character character, PartsMechaEnum part)
    {
        _characterSelectedToAttack = character;
        _partEnum = part;
    }

    public void ResetButton()
    {
        _bulletsCount = 0;
        _bulletsCountText.text = "0";
        if (!_characterSelectedToAttack) return;
        _characterSelectedToAttack.SetShaderForPart(SwitchTextureEnum.TextureClean, _partEnum);
        
        if (_partEnum == PartsMechaEnum.body)
        {
            _characterSelectedToAttack.SetShaderForPart(SwitchTextureEnum.TextureClean, PartsMechaEnum.armL);
            _characterSelectedToAttack.SetShaderForPart(SwitchTextureEnum.TextureClean, PartsMechaEnum.armR);
        }

        if (_partEnum == PartsMechaEnum.legL)
        {
            _characterSelectedToAttack.SetShaderForPart(SwitchTextureEnum.TextureClean, PartsMechaEnum.legR);
        }
    }
    
    public void ButtonEnabling(bool status, UnityAction rightAction, UnityAction leftAction)
    {
        gameObject.SetActive(status);
        interactable = status;

        OnRightClick.RemoveAllListeners();
        OnLeftClick.RemoveAllListeners();
        OnRightClick.AddListener(rightAction);
        OnLeftClick.AddListener(leftAction);

        SetBulletsCount(0);
    }
}
