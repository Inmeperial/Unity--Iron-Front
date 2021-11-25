using System;
using UnityEngine;

public abstract class Equipable : MonoBehaviour
{
    public enum Location
    {
        Body,
        LeftGun,
        RightGun,
        Legs
    }
    
    //TODO: remover despues
    protected EquipableSO.EquipableType _equipableType;
    protected int _availableUses;
    protected string _equipableName;
    protected Character _character;
    protected EquipmentButton _button;
    protected Sprite _icon;
    protected Location _location;
    public abstract void Initialize(Character character, EquipableSO data, Location location);

    public abstract void Select();

    public abstract void Deselect();

    public abstract void Use(Action callback = null);

    public EquipableSO.EquipableType GetEquipableType()
    {
        return _equipableType;
    }

    public virtual string GetEquipableName()
    {
        return _equipableName;
    }

    public void SetButton(EquipmentButton button)
    {
        _button = button;
    }

    public Sprite GetIcon()
    {
        return _icon;
    }
    
    public int GetAvailableUses()
    {
        return _availableUses;
    }
    
    protected virtual void UpdateButtonText(string text, EquipableSO data)
    {
        _button.SetButtonText(text, data.buttonTextFontSize);
    }

    public abstract void UpdateEquipableState();

    public abstract bool CanBeUsed();
}
