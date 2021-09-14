using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : Parts
{
    public BodySO bodyData;
    private float _bodyMaxHP;
    private float _bodyHP;

    private void Awake()
    {
        _bodyMaxHP = bodyData.maxHP;
        _bodyHP = _bodyMaxHP;
    }
    public override void UpdateHp(float newValue)
    {
        _bodyHP = newValue;
    }
    
    public override float GetMaxHp()
    {
        return _bodyMaxHP;
    }

    public override float GetCurrentHp()
    {
        return _bodyHP;
    }
    
    //Lo ejecuta el ButtonsUIManager, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public override void TakeDamage(List<Tuple<int,int>> damages)
    {
        WorldUI ui = _myChar.GetMyUI();
        ui.SetBodySlider(_bodyHP);
        int total = 0;
        Vector3 bodyPos = transform.position;
        for (int i = 0; i < damages.Count; i++)
        {
            total += damages[i].Item1;
            float hp = _bodyHP - damages[i].Item1;
            _bodyHP = hp > 0 ? hp : 0;

            EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Damage);
            EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Hit);
            int item = damages[i].Item2;
            switch (item)
            {
                case MissHit:
                    EffectsController.Instance.CreateDamageText("Miss", 0, bodyPos, i == damages.Count - 1 ? true : false);
                    break;
                   
                case NormalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 1, bodyPos, i == damages.Count - 1 ? true : false);
                    _myChar.HitSoundMecha();
                    break;
               
                case CriticalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 2, bodyPos, i == damages.Count - 1 ? true : false);
                    _myChar.HitSoundMecha();
                    break;
            }
        }
        
        ui.ContainerActivation(true);
        ui.UpdateBodySlider(total, (int)_bodyHP);
        _myChar.MakeNotAttackable();
    }
    
    //Lo ejecuta el mortero, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public override void TakeDamage(int damage)
    {
        WorldUI ui = _myChar.GetMyUI();
        ui.SetBodySlider(_bodyHP);
        float hp = _bodyHP - damage;
        _bodyHP = hp > 0 ? hp : 0;

        Vector3 bodyPos = transform.position;
        EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Damage);
        EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Hit);
        _myChar.HitSoundMecha();
        EffectsController.Instance.CreateDamageText(damage.ToString(), 1, bodyPos, true);
        ui.ContainerActivation(true);
        ui.UpdateBodySlider(damage, (int)_bodyHP);
        _myChar.MakeNotAttackable();
    }

    public void MechaDeath()
    {
        _myChar.NotSelectable();
        //_myChar.effectsController.PlayParticlesEffect(this.gameObject, "Dead");
    }

}
