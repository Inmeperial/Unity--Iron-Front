using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MechaPartButton : CustomButton
{
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private Slider _currentHPSlider;
    [SerializeField] private Slider _damagePreviewSlider;


    [SerializeField] private TextMeshProUGUI _bulletsCountText;
    
    private int _bulletsCount;
    private Character _characterSelected;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        //TODO: Resaltar partes
        //_materialHandler.SetSelectedPartMaterialToBody(_part, true);
        //_characterSelected.SetShaderForPart(SwitchTextureEnum.TextureHighLight, PartsMechaEnum.armL)
        UpdateDamagePreviewSlider();
        
        _damagePreviewSlider.gameObject.SetActive(true);
    }
    
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (_bulletsCount <= 0)
        {
            _damagePreviewSlider.gameObject.SetActive(false);
            //TODO: Resaltar partes off
            //_materialHandler.SetSelectedPartMaterialToBody(_part, false);
        }
            
    }

    protected override void PressRight()
    {
        base.PressRight();
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

    public void SetPart(MaterialMechaHandler handler, MechaParts part)
    {
        //_materialHandler = handler;
        //_part = part;
    }
}
