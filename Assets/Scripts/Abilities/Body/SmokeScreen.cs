using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScreen : BodyAbility
{
    private GameObject _smokeObject;

    private SmokeScreenSO _abilityData;

    public override void Initialize(Character character, EquipableSO data, Location location)
    {
        base.Initialize(character, data, location);
	    
        _abilityData = data as SmokeScreenSO;
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

    IEnumerator DestroySmoke()
    {
        yield return new WaitUntil(() => _character.IsMyTurn());
        
        if (_smokeObject) Destroy(_smokeObject);
    }
}
