using System;

public class Item : Equipable
{
    protected ItemSO _itemData;
    //TODO: remover despues
    //protected ItemSO.ItemType _itemType;
    public override void Initialize(Character character, EquipableSO data, Location location)
    {
        _character = character;
        _itemData = data as ItemSO;
        _availableUses = _itemData.maxUses;
        //_itemType = _itemData.itemType;
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
        return _itemData.equipableName;
    }


    public int GetItemDamage()
    {
        return _itemData.damage;
    }
    
    public int GetItemAoE()
    {
        return _itemData.areaOfEffect;
    }

    public int GetItemDuration()
    {
        return _itemData.duration;;
    }

    public int GetItemRange()
    {
        return _itemData.useRange;
    }

    public int GetMaxUses()
    {
        return _itemData.maxUses;
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
