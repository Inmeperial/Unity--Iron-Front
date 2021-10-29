using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializationManager : MonoBehaviour
{
    private void Awake()
    {
        var characters = FindObjectsOfType<Character>();

        foreach (var c in characters)
        {
            c.ManualAwake();
        }
        
        FindObjectOfType<EquipmentManager>().ManualAwake();

        var turnManager = FindObjectOfType<TurnManager>();
        turnManager.ManualAwake();
        
        
    }

    private void Start()
    {
        var characters = FindObjectsOfType<Character>();
        
        foreach (var c in characters)
        {
            c.ManualStart();
        }

        var getParts = FindObjectsOfType<GetPartsOfMecha>();

        foreach (var g in getParts)
        {
            g.ManualStart();
        }
        
        var turnManager = FindObjectOfType<TurnManager>();
        turnManager.ManualStart();
    }
}
