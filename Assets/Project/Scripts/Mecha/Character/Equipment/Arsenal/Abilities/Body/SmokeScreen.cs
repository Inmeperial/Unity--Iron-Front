using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScreen : Ability
{
    private GameObject _smokeObject;

    private SmokeScreenSO _abilityData;

    public override void Initialize(Character character, EquipableSO data)
    {
        base.Initialize(character, data);
	    
        _abilityData = data as SmokeScreenSO;

        _character.GetBody().ConfigureSmokeScreen(_abilityData.hpPercentageForSmokeActivation);
    }
    
    public override void Select()
    {
        Debug.Log("select ability");
    }

    public override void Deselect()
    {
        Destroy(_smokeObject);
    }

    public override void Use(Action callback = null)
    {
        Debug.Log("use ability");
        _smokeObject = Instantiate(_abilityData.smokeScreenObject, _character.transform);

        StartCoroutine(DestroySmoke());
    }

    private IEnumerator DestroySmoke()
    {
        yield return new WaitUntil(() => _character.IsMyTurn());
        
        if (_smokeObject)
            Destroy(_smokeObject);
    }
}
