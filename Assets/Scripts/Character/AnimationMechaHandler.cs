using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMechaHandler : MonoBehaviour
{
    public void SetPauseDeadAnimation()
    {
        this.GetComponent<Animator>().speed = 0;
        this.GetComponent<Character>().effectsController.PlayParticlesEffect(this.gameObject, "Dead");
    }

    public void SetIsDeadAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isDead", true);
    }

    public void SetIsWalkingAnimatorFalse()
    {
        this.GetComponent<Animator>().SetBool("isWalkingAnimator", false);
    }

    public void SetIsReciveDamageAnimatorFalse()
    {
        if (this.GetComponent<Character>().body.GetCurrentHp() <= 0)
        {
            SetIsDeadAnimatorTrue();
        }
        else
        {
            this.GetComponent<Animator>().SetBool("isReciveDamageAnimator", false);
        }
    }

    public void SetIsSniperAttackRightAnimatorFalse()
    {
        this.GetComponent<Animator>().SetBool("isSniperAttackRight", false);
    }

    public void SetIsSniperAttackLeftAnimatorFalse()
    {
        this.GetComponent<Animator>().SetBool("isSniperAttackLeft", false);
    }

    public void SetIsHammerAttackRightAnimatorFalse()
    {
        this.GetComponent<Animator>().SetBool("isHammerAttackRight", false);
    }

    public void SetIsHammerAttackLeftAnimatorFalse()
    {
        this.GetComponent<Animator>().SetBool("isHammerAttackLeft", false);
    }
    public void SetIsShotgunAttackRightAnimatorFalse()
    {
        this.GetComponent<Animator>().SetBool("isShotgunAttackRight", false);
    }

    public void SetIsShotgunAttackLeftAnimatorFalse()
    {
        this.GetComponent<Animator>().SetBool("isShotgunAttackLeft", false);
    }

    public void SetIsMachineGunAttackLeftAnimatorFalse()
    {
        this.GetComponent<Animator>().SetBool("isMachinegunAttackLeft", false);
    }

    public void SetIsMachineGunAttackRightAnimatorFalse()
    {
        this.GetComponent<Animator>().SetBool("isMachinegunAttackRight", false);
    }

    public void SetIsSniperAttackRightAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isSniperAttackRight", true);
    }

    public void SetIsSniperAttackLeftAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isSniperAttackLeft", true);
    }

    public void SetIsHammerAttackRightAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isHammerAttackRight", true);
    }

    public void SetIsHammerAttackLeftAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isHammerAttackLeft", true);
    }

    public void SetIsShotgunAttackRightAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isShotgunAttackRight", true);
    }

    public void SetIsShotgunAttackLeftAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isShotgunAttackLeft", true);
    }

    public void SetIsMachineGunAttackLeftAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isMachinegunAttackLeft", true);
    }

    public void SetIsMachineGunAttackRightAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isMachinegunAttackRight", true);
    }

    public void SetIsWalkingAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isWalkingAnimator", true);
    }

    public void SetIsReciveDamageAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isReciveDamageAnimator", true);
    }

}
