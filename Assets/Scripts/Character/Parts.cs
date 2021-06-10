﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    protected const int _missHit = 0;
    protected const int _normalHit = 1;
    protected const int _criticalHit = 2;
    protected Character _myChar;

    private void Start()
    {
        _myChar = transform.parent.GetComponent<Character>();
    }
}
