using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    public LayerMask charMask;
    private Character _selection;
    TileHighlight _highlight;
    TurnManager _turnManager;
    public bool _canSelect;
    
    public Button buttonMove;
    public Button buttonUndo;
    public TextMeshProUGUI stepsCounter;
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

        if (_selection)
            stepsCounter.text = _selection.GetSteps().ToString();
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
                buttonMove.onClick.RemoveAllListeners();
                buttonMove.onClick.AddListener(_selection.Move);
                buttonUndo.onClick.RemoveAllListeners();
                buttonUndo.onClick.AddListener(_selection.pathCreator.UndoLastWaypoint);
                stepsCounter.text = _selection.GetSteps().ToString();
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
