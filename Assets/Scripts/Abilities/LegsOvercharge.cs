using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsOvercharge : Ability
{
    public override void Select()
    {
        _character.DeselectThisUnit();
        _character.legsOvercharged = true;
        _character.IncreaseAvailableSteps(_character.legs.GetMaxSteps());
        _button.OnRightClick?.Invoke();
    }

    public override void Deselect()
    {
        Debug.Log("deselect legs overcharge");
        _button.interactable = false;
        StartCoroutine(SelectCharacterDelay());
    }

    public override void Use(Action callback = null)
    {
        Debug.Log("use legs overcharge");
    }

    IEnumerator SelectCharacterDelay()
    {
        yield return new WaitForEndOfFrame();
        _character.SelectThisUnit();
    }
}
