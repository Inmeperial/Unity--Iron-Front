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

    public bool playerSelected;
    private void Start()
    {
        _canSelectUnit = true;
        _highlight = GetComponent<TileHighlight>();
        _turnManager = FindObjectOfType<TurnManager>();
        _buttonsManager = FindObjectOfType<ButtonsUIManager>();
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
    }

    private void SelectCharacterFromTile(LayerMask layerMask)
    {
        var tile = MouseRay.GetTargetGameObject(layerMask);
        if (tile && tile.CompareTag("GridBlock"))
        {
            var c = tile.GetComponent<Tile>().GetCharacterAbove();
            if (c) SelectionOf(c);
        }
    }

    //Selection of the character that will move.
    public void SelectCharacterFromObject(LayerMask charMask)
    {
        var character = MouseRay.GetTargetTransform(charMask);
        if (character && character.CompareTag("Character"))
        {
            var c = character.GetComponent<Character>();
            SelectionOf(c);
        }
    }

    public void SelectionOf(Character c)
    {
        Debug.Log("llamo selection");
        if (c.IsMyTurn())
        {
            Debug.Log("selecciono");
            _buttonsManager.DeselectActions();
            if (_selection)
                _selection.DeselectThisUnit();
            if (_enemySelection)
            {
                _enemySelection.DeselectThisUnit();
                _enemySelection = null;
            }
        
            playerSelected = true;
            _selection = c;
            _selection.SelectThisUnit();
            _highlight.ChangeActiveCharacter(_selection);
            _buttonsManager.SetPlayerCharacter(_selection);
            _buttonsManager.SetPlayerUI();
            if (_enemySelection)
            {
                _selection.RotateTowardsEnemy(_enemySelection.transform.position);
            }
        }
        // if (c.GetUnitTeam() == _turnManager.GetActiveTeam())
        // {
        //     _buttonsManager.DeselectActions();
        //     if (_selection)
        //         _selection.DeselectThisUnit();
        //     if (_enemySelection)
        //     {
        //         _enemySelection.DeselectThisUnit();
        //         _enemySelection = null;
        //     }
        //
        //     playerSelected = true;
        //     _selection = c;
        //     _selection.SelectThisUnit();
        //     _highlight.ChangeActiveCharacter(_selection);
        //     _buttonsManager.SetPlayerCharacter(_selection);
        //     _buttonsManager.SetPlayerUI();
        //     if (_enemySelection)
        //     {
        //         _selection.RotateTowardsEnemy(_enemySelection.transform.position);
        //     }
        // }
        //else
        else if (c.GetUnitTeam() != _turnManager.GetActiveTeam())
        {
            Debug.Log("clickeo enemigo");
            if (_enemySelection)
            {
                _enemySelection.DeselectThisUnit();
            }
            _enemySelection = c;
            _enemySelection.SelectedAsEnemy();
            _buttonsManager.SetEnemy(_enemySelection);
            _buttonsManager.SetEnemyUI();
            if (_selection && _selection.CanAttack())
            {
                _selection.RotateTowardsEnemy(_enemySelection.transform.position);
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
        _buttonsManager.DeactivateCharacterButtons();
    }

    public void DeselectUnit()
    {
        if (_selection)
        {
            playerSelected = false;
            _selection.DeselectThisUnit();
            _selection = null;
        }
        if (_enemySelection)
        {
            _enemySelection.DeselectThisUnit();
            _enemySelection = null;
        }
    }

    public bool PlayerIsSelected(Character character)
    {
        if (_selection) return character == _selection;
        return false;
    }
}