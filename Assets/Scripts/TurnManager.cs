using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    Character[] _units;
    CharacterSelection _charSelect;
    void Start()
    {
        _units = FindObjectsOfType<Character>();
        _charSelect = FindObjectOfType<CharacterSelection>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnitIsMoving()
    {
        _charSelect.ActivateCharacterSelection(false);
    }

    public void UnitStoppedMoving()
    {
        _charSelect.ActivateCharacterSelection(true);
    }
}
