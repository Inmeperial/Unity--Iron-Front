using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterSelection : MonoBehaviour
{
    public LayerMask charMask;
    public LayerMask gridBlockMask;
    private Character _selection;
    TileHighlight _highlight;
    TurnManager _turnManager;
    ButtonsUIManager _buttonsManager;
    public bool _canSelectUnit;
    private Character _enemySelection;

    public TextMeshProUGUI stepsCounter;
    public event Action OnCharacterSelect = delegate { };
    public event Action OnCharacterDeselect = delegate { };

    //private bool _selectingEnemy;


    private void Start()
    {
        _canSelectUnit = true;
        _highlight = GetComponent<TileHighlight>();
        _turnManager = FindObjectOfType<TurnManager>();
        _buttonsManager = FindObjectOfType<ButtonsUIManager>();

        if (_buttonsManager == null)
            Debug.Log("buttons nul");
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (Input.GetMouseButtonDown(0) && _canSelectUnit && MouseRay.CheckIfType(charMask))
            {
                SelectCharacterFromObject(charMask);

            }
            if (Input.GetMouseButtonDown(0) && _canSelectUnit && MouseRay.CheckIfType(gridBlockMask))
            {
                SelectCharacterFromTile(gridBlockMask);
            }
                
        }
        

        if (_selection)
        {
            stepsCounter.text = _selection.GetSteps().ToString();
        }
    }

    private void SelectCharacterFromTile(LayerMask layerMask)
    {
        var tile = MouseRay.GetTargetGameObject(layerMask);
        Debug.Log(tile.name);
        if (tile != null && tile.CompareTag("GridBlock"))
        {
            var c = tile.GetComponent<Tile>().GetCharacterAbove();
            if (c != null)
                SelectionOf(c);
        }
            
    }

    //Selection of the character that will move.
    public void SelectCharacterFromObject(LayerMask charMask)
    {
        var character = MouseRay.GetTargetTransform(charMask);
        if (character != null && character.CompareTag("Character"))
        {
            var c = character.GetComponent<Character>();
            SelectionOf(c);
        }
    }

    void SelectionOf(Character c)
    {
        if (c.CanBeSelected())
        {
                
            if (c.GetUnitTeam() == _turnManager.GetActiveTeam())
            {
                _buttonsManager.DeselectActions();
                if (_selection != null)
                    _selection.DeselectThisUnit();
                if (_enemySelection != null)
                {
                    _enemySelection.DeselectThisUnit();
                    _enemySelection = null;
                }
                _selection = c;
                _selection.SelectThisUnit();
                _highlight.ChangeActiveCharacter(_selection);
                _buttonsManager.DeactivateUndo();
                _buttonsManager.SetPlayerCharacter(_selection);
                _buttonsManager.SetPlayerUI();
                stepsCounter.text = _selection.GetSteps().ToString();
                if (_enemySelection != null)
                {
                    _selection.RotateTowardsEnemy(_enemySelection.transform.position);
                }
            }
            else
            {
                if (_enemySelection != null)
                {
                    _enemySelection.DeselectThisUnit();
                }
                _enemySelection = c;
                _enemySelection.SelectedAsEnemy();
                _buttonsManager.SetEnemy(_enemySelection);
                _buttonsManager.SetEnemyUI();
                if (_selection != null)
                {
                    _selection.RotateTowardsEnemy(_enemySelection.transform.position);
                }
            }
        }
    }


    //Returns the character that is currently selected.
    public Character GetActualChar()
    {
        return _selection;
    }

    //Returns the enemy that is currently selected.
    public Character GetSelectedEnemy()
    {
        return _enemySelection;
    }

    public void ActivateCharacterSelection(bool state)
    {
        _canSelectUnit = state;
    }

    public void ResetSelector()
    {
        DeselectUnit();
        //_selectingEnemy = false;
        _buttonsManager.DeactivateCharacterButtons();
    }

    public void DeselectUnit()
    {
        Debug.Log("entro al deselect unit del char selection");
        if (_selection)
        {
            _selection.DeselectThisUnit();
            _selection = null;
        }
        if (_enemySelection)
        {
            Debug.Log("deselect enemy");
            _enemySelection.DeselectThisUnit();
            _enemySelection = null;
        }
    }

    //public void CanSelectEnemy()
    //{
    //    _selectingEnemy = true;
    //}

    //public void CantSelectEnemy()
    //{
    //    _selectingEnemy = false;
    //}
}