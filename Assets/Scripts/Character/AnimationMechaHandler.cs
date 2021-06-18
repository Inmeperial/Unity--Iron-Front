using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMechaHandler : MonoBehaviour
{
    public void SetPauseDeadAnimation()
    {
        this.GetComponent<Animator>().speed = 0;
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

    public void SetIsHammerkAttackRightAnimatorFalse()
    {
        this.GetComponent<Animator>().SetBool("isHammerkAttackRight", false);
    }

    public void SetIsHammerkAttackLeftAnimatorFalse()
    {
        this.GetComponent<Animator>().SetBool("isHammerkAttackLeft", false);
    }
    public void SetIsShotgunAttackRightAnimatorFalse()
    {
        this.GetComponent<Animator>().SetBool("isShotgunAttackRight", false);
    }

    public void SetIsShotgunAttackLeftAnimatorFalse()
    {
        this.GetComponent<Animator>().SetBool("isShotgunAttackLeft", false);
    }

    public void SetIsSniperAttackRightAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isSniperAttackRight", true);
    }

    public void SetIsSniperAttackLeftAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isSniperAttackLeft", true);
    }

    public void SetIsHammerkAttackRightAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isHammerkAttackRight", true);
    }

    public void SetIsHammerkAttackLeftAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isHammerkAttackLeft", true);
    }

    public void SetIsShotgunAttackRightAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isShotgunAttackRight", true);
    }

    public void SetIsShotgunAttackLeftAnimatorTrue()
    {
        this.GetComponent<Animator>().SetBool("isShotgunAttackLeft", true);
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
