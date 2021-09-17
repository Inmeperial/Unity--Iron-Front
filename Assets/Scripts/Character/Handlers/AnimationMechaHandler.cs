using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMechaHandler : MonoBehaviour
{
    private Animator _animator;
    private ParticleMechaHandler _particleMechaHandler;
    private AudioMechaHandler _audioMechaHandler;
    private Character _character;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _particleMechaHandler = this.GetComponent<ParticleMechaHandler>();
        _audioMechaHandler = this.GetComponent<AudioMechaHandler>();
        _character = this.GetComponent<Character>();
    }

    #region Set Dead, ReviceDamange and Walking ON/OFF

    public void SetIsReciveDamageAnimatorFalse()
    {
        if (_character.body.GetCurrentHp() <= 0)
        {
            SetIsDeadAnimatorTrue();
        }
        else
        {
            _animator.SetBool("isReciveDamageAnimator", false);
        }
    }

    public void SetIsDeadAnimatorTrue()
    {
        _animator.SetBool("isDead", true);
    }

    public void SetPauseDeadAnimation()
    {
        _animator.speed = 0;
        _particleMechaHandler.SetMachineOff();
    }

    public void SetIsReciveDamageAnimatorTrue()
    {
        _animator.SetBool("isReciveDamageAnimator", true);
    }

    public void SetIsWalkingAnimatorFalse()
    {
        _animator.SetBool("isWalkingAnimator", false);
    }

    public void SetIsWalkingAnimatorTrue()
    {
        _animator.SetBool("isWalkingAnimator", true);
    }

    #endregion

    #region Set Animation Weapons ON/OFF

    public void SetIsSniperAttackRightAnimatorFalse() //call in Animaton
    {
        _animator.SetBool("isSniperAttackRight", false);
    }

    public void SetIsSniperAttackLeftAnimatorFalse() //call in Animaton
    {
        _animator.SetBool("isSniperAttackLeft", false);
    }

    public void SetIsHammerAttackRightAnimatorFalse() //call in Animaton
    {
        _animator.SetBool("isHammerAttackRight", false);
    }

    public void SetIsHammerAttackLeftAnimatorFalse() //call in Animaton
    {
        _animator.SetBool("isHammerAttackLeft", false);
    }

    public void SetIsShotgunAttackRightAnimatorFalse() //call in Animaton
    {
        _animator.SetBool("isShotgunAttackRight", false);
    }

    public void SetIsShotgunAttackLeftAnimatorFalse() //call in Animaton
    {
        _animator.SetBool("isShotgunAttackLeft", false);
    }

    public void SetIsMachineGunAttackLeftAnimatorFalse() //call in Animaton
    {
        _animator.SetBool("isMachinegunAttackLeft", false);
    }

    public void SetIsMachineGunAttackRightAnimatorFalse() //call in Animaton
    {
        _animator.SetBool("isMachinegunAttackRight", false);
    }

    public void SetIsSniperAttackRightAnimatorTrue() //call in Animaton
    {
        _animator.SetBool("isSniperAttackRight", true);
    }

    public void SetIsSniperAttackLeftAnimatorTrue() //call in Animaton
    {
        _animator.SetBool("isSniperAttackLeft", true);
    }

    public void SetIsHammerAttackRightAnimatorTrue() //call in Animaton
    {
        _animator.SetBool("isHammerAttackRight", true);
    }

    public void SetIsHammerAttackLeftAnimatorTrue() //call in Animaton
    {
        _animator.SetBool("isHammerAttackLeft", true);
    }

    public void SetIsShotgunAttackRightAnimatorTrue() //call in Animaton
    {
        _animator.SetBool("isShotgunAttackRight", true);
    }

    public void SetIsShotgunAttackLeftAnimatorTrue() //call in Animaton
    {
        _animator.SetBool("isShotgunAttackLeft", true);
    }

    public void SetIsMachineGunAttackLeftAnimatorTrue() //call in Animaton
    {
        _animator.SetBool("isMachinegunAttackLeft", true);
    }

    public void SetIsMachineGunAttackRightAnimatorTrue() //call in Animaton
    {
        _animator.SetBool("isMachinegunAttackRight", true);
    }

    #endregion

    #region Set Particles Weapons ON/OFF

    //Tengo que usar int para R/L porque solo hay Float/Int/String de parametros.

    public void SetParticleShootGun(int num) //call in Animaton
    {
        Gun gun;
        if (num == 0)
        {
            gun = _character.GetRightGun();
        }
        else
        {
            gun = _character.GetLeftGun();
        }

        _particleMechaHandler.SetParticleWeapon(gun.GetParticleSpawn(), EnumsClass.ParticleActionType.ShootGun);
    }

    public void SetParticleAssaultRifle(int num) //call in Animaton
    {
        //Anim keyFrame = 26.1 - 45.3 - 63.9 - 81.6 - 

        Gun gun;
        if (num == 0)
        {
            gun = _character.GetRightGun();
        }
        else
        {
            gun = _character.GetLeftGun();
        }

        _particleMechaHandler.SetParticleWeapon(gun.GetParticleSpawn(), EnumsClass.ParticleActionType.AssaultRifle);
    }

    public void SetParticletRifle(int num) //call in Animaton
    {
        //Anim keyFrame = 45.8

        Gun gun;
        if (num == 0)
        {
            gun = _character.GetRightGun();
        }
        else
        {
            gun = _character.GetLeftGun();
        }

        _particleMechaHandler.SetParticleWeapon(gun.GetParticleSpawn(), EnumsClass.ParticleActionType.Rifle);
    }

    #endregion

}
