using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public LayerMask charMask;
    private Character _selection;
    TileHighlight _highlight;
    TurnManager _turnManager;
    public bool _canSelect;
    
    public Button moveButton;

    public event Action OnCharacterSelect = delegate { };
    public event Action OnCharacterDeselect = delegate { };
    private void Start()
    {
        _canSelect = true;
        _highlight = GetComponent<TileHighlight>();
        _turnManager = FindObjectOfType<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canSelect)
            SelectCharacter(charMask);
    }

    //Selection of the character that will move.
    void SelectCharacter(LayerMask charMask)
    {
        var character = MouseRay.GetTargetTransform(charMask);
        if (character != null && character.CompareTag("Character"))
        {
            var c = character.GetComponent<Character>();
            if (c.GetUnitTeam() == _turnManager.GetActiveTeam())
            {
                //Check if i have a previous unit and deselect it.
                if (_selection != null)
                {
                    _selection.DeselectThisUnit();
                }
                OnCharacterDeselect();
                _selection = c;
                _selection.SelectThisUnit();
                _highlight.ChangeActiveCharacter(_selection);
                moveButton.onClick.RemoveAllListeners();
                moveButton.onClick.AddListener(_selection.Move);
                OnCharacterSelect();
            }
        }
    }

    //Returns the character that is currently selected.
    public Character GetActualChar()
    {
        return _selection;
    }

    public void ActivateCharacterSelection(bool state)
    {
        _canSelect = state;
    }

    public void DeselectUnit()
    {
        if (_selection)
            _selection.DeselectThisUnit();
    }
}
