using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScreen : Ability
{
    public override void Select()
    {
        Debug.Log("select ability");
    }

    public override void Deselect()
    {
        Debug.Log("deselect ability");
    }

    public override void Use(Action callback = null)
    {
        Debug.Log("use ability");
    }
}
