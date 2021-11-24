using System;

public class Item : Equipable
{
    public override void Initialize(Character character, EquipableSO data, Location location)
    {
        _character = character;
        _availableUses = data.maxUses;
        _location = location;
    }

    public override void Select()
    {
        
    }

    public override void Deselect()
    {
        
    }

    public override void Use(Action callback = null)
    {
        
    }

    public override string GetEquipableName()
    {
        return "";
    }

    public int GetItemUses()
    {
        return _availableUses;
    }
    
    protected virtual void UpdateUses()
    {
        _availableUses--;
    }

}
