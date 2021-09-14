using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMechaHandler : MonoBehaviour
{
    private Animator _animator;
    private SmokeMechaHandler _smokeMechaHandler;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _smokeMechaHandler = this.GetComponent<SmokeMechaHandler>();
    }

    public void SetPauseDeadAnimation()
    {
        _animator.speed = 0;
        EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Dead);
        _smokeMechaHandler.SetDiseableParticle();
    }

    public void SetIsDeadAnimatorTrue()
    {
        _animator.SetBool("isDead", true);
    }

    public void SetIsWalkingAnimatorFalse()
    {
        _animator.SetBool("isWalkingAnimator", false);
    }

    public void SetIsReciveDamageAnimatorFalse()
    {
        if (GetComponent<Character>().body.GetCurrentHp() <= 0)
        {
            SetIsDeadAnimatorTrue();
        }
        else
        {
            _animator.SetBool("isReciveDamageAnimator", false);
        }
    }

    public void SetIsSniperAttackRightAnimatorFalse()
    {
        _animator.SetBool("isSniperAttackRight", false);
    }

    public void SetIsSniperAttackLeftAnimatorFalse()
    {
        _animator.SetBool("isSniperAttackLeft", false);
    }

    public void SetIsHammerAttackRightAnimatorFalse()
    {
        _animator.SetBool("isHammerAttackRight", false);
    }

    public void SetIsHammerAttackLeftAnimatorFalse()
    {
        _animator.SetBool("isHammerAttackLeft", false);
    }
    public void SetIsShotgunAttackRightAnimatorFalse()
    {
        _animator.SetBool("isShotgunAttackRight", false);
    }

    public void SetIsShotgunAttackLeftAnimatorFalse()
    {
        _animator.SetBool("isShotgunAttackLeft", false);
    }

    public void SetIsMachineGunAttackLeftAnimatorFalse()
    {
        _animator.SetBool("isMachinegunAttackLeft", false);
    }

    public void SetIsMachineGunAttackRightAnimatorFalse()
    {
        _animator.SetBool("isMachinegunAttackRight", false);
    }

    public void SetIsSniperAttackRightAnimatorTrue()
    {
        _animator.SetBool("isSniperAttackRight", true);
    }

    public void SetIsSniperAttackLeftAnimatorTrue()
    {
        _animator.SetBool("isSniperAttackLeft", true);
    }

    public void SetIsHammerAttackRightAnimatorTrue()
    {
        _animator.SetBool("isHammerAttackRight", true);
    }

    public void SetIsHammerAttackLeftAnimatorTrue()
    {
        _animator.SetBool("isHammerAttackLeft", true);
    }

    public void SetIsShotgunAttackRightAnimatorTrue()
    {
        _animator.SetBool("isShotgunAttackRight", true);
    }

    public void SetIsShotgunAttackLeftAnimatorTrue()
    {
        _animator.SetBool("isShotgunAttackLeft", true);
    }

    public void SetIsMachineGunAttackLeftAnimatorTrue()
    {
        _animator.SetBool("isMachinegunAttackLeft", true);
    }

    public void SetIsMachineGunAttackRightAnimatorTrue()
    {
        _animator.SetBool("isMachinegunAttackRight", true);
    }

    public void SetIsWalkingAnimatorTrue()
    {
        _animator.SetBool("isWalkingAnimator", true);
    }

    public void SetIsReciveDamageAnimatorTrue()
    {
        _animator.SetBool("isReciveDamageAnimator", true);
    }

}
