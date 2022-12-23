using System;
using UnityEngine;

public class SmokeScreen : Ability
{
    private GameObject _smokeObject;

    private SmokeScreenSO _abilityData;

    ParticleSystem _smokeParticles;

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
        _character.DeselectCurrentEquipable();
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
        _smokeObject.transform.position = _character.GetPositionTile().transform.position + Vector3.up * 1.5f;

        AudioManager.Instance.PlaySound(_abilityData.sound, _smokeObject);

        _smokeParticles = Instantiate(_abilityData.particleEffect, _character.transform.position, Quaternion.identity);
        _smokeParticles.transform.forward = _smokeObject.transform.up;
        _smokeParticles.time = 0f;
        _smokeParticles.Play();
    }

    private void DestroySmokeScreen()
    {
        if (!_smokeObject)
            return;

        _character.OnMechaTurnStart -= DestroySmokeScreen;
        Destroy(_smokeObject);
        _smokeParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        Destroy(_smokeParticles.gameObject, 3f);
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
