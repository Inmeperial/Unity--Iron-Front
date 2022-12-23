using System;
using System.Collections;
using UnityEngine;

public class LegsOvercharge : Ability
{
    private LegsOverChargeSO _abilityData;

    public override void Initialize(Character character, EquipableSO data)
    {
        base.Initialize(character, data);
	    
        if (data == null)
        {
            throw new Exception("data is null");

        }
        var dataAsLegsOverchargeSO = (LegsOverChargeSO)data;

        if (dataAsLegsOverchargeSO == null)
        {
            Debug.Log("data type: " + data.GetType().ToString());
            throw new Exception("dataAsLegsOverchargeSO is null");
        }
        _abilityData = data as LegsOverChargeSO;
    }
    public override void Select()
    {
        OnEquipableSelected?.Invoke();

        _character.DeselectCurrentEquipable();

        Use();
    }

    public override void Deselect()
    {
        _button.interactable = false;

        StartCoroutine(SelectCharacterDelay());
    }

    public override void Use()
    {
        PlayVFX(_abilityData.particleEffect, _character.GetLegs().transform.position, _character.transform.forward);

        PlaySound(_abilityData.sound, gameObject);

        _character.DeselectThisUnit();

        _character.LegsOverchargeActivate();

        _character.IncreaseAvailableSteps(_character.GetLegs().GetMaxSteps());

        _button.OnRightClick?.Invoke();

        AbilityUsed(_abilityData);

        UpdateButtonText(_availableUses.ToString(), _abilityData);

        _button.interactable = false;

        OnEquipableUsed?.Invoke();
    }

    private IEnumerator SelectCharacterDelay()
    {
        yield return new WaitForEndOfFrame();
        _character.SelectThisUnit();
    }

    public override string GetEquipableName()
    {
        return _abilityData.objectName;
    }

    public override string GetEquipableDescription()
    {
        return _abilityData.objectDescription;
    }
}
