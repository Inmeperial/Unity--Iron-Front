using System;
using UnityEngine;

public class SmokeScreen : Ability
{
    private GameObject _smokeObject;

    private SmokeScreenSO _abilityData;

    public override void Initialize(Character character, EquipableSO data)
    {
        base.Initialize(character, data);
	    
        _abilityData = data as SmokeScreenSO;

        _character.GetBody().OnDamageTaken += CheckSmokescreen;
    }

    private void CheckSmokescreen(Character arg1 = null, float arg2 = 0)
    {
        Body body = _character.GetBody();
        if (body.CurrentHP <= 0)
            return;

        float hpPercentage = body.CurrentHP * 100 / body.MaxHp;

        if (hpPercentage > _abilityData.hpPercentageForSmokeActivation)
            return;
        
        Use();
    }

    public override void Select()
    {
        Debug.Log("select ability");
    }

    public override void Deselect()
    {
        Destroy(_smokeObject);
    }

    public override void Use()
    {
        Debug.Log("use smokescreen");

        _character.GetBody().OnDamageTaken -= CheckSmokescreen;

        _character.OnMechaTurnStart += DestroySmokeScreen;

        _smokeObject = Instantiate(_abilityData.smokeScreenObject, _character.transform);
    }

    private void DestroySmokeScreen()
    {
        if (!_smokeObject)
            return;

        _character.OnMechaTurnStart -= DestroySmokeScreen;
        Destroy(_smokeObject);
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
