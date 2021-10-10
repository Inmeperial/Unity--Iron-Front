using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Framework;
using UnityEngine;

[Condition("Basic/CheckPosition")]
public class BTCheckPosition : ConditionBase
{
    [InParam("maxPosition")] 
    [Help("Value of X position where unit will change its movement")]
    public float maxValue;

    [InParam("minPosition")] 
    [Help("Value of X position where unit will change its movement")]
    public float minValue;
    
    [InParam("myXPosition")] 
    [Help("X position of object")]
    public float myXPosition;

    [InParam("moveleft")] 
    [Help("move state")]
    public bool moveleft = true;
    
    
    public override bool Check()
    {
        if (myXPosition >= maxValue)
        {
            moveleft = true;
        }
        else if (myXPosition <= minValue)
        {
            moveleft = false;
        }

        return moveleft;
    }
}
