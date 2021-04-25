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
    private Character _selection;
    TileHighlight _highlight;
    TurnManager _turnManager;
    ButtonsUIManager _buttonsManager;
    public bool _canSelectUnit;
    private Character _enemySelection;

    public TextMeshProUGUI stepsCounter;
    public event Action OnCharacterSelect = delegate { };
    public event Action OnCharacterDeselect = delegate { };

    private bool _selectingEnemy;


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
            if (Input.GetMouseButtonDown(0) && _canSelectUnit)
                SelectCharacter(charMask);
        }
        

        if (_selection)
        {
            stepsCounter.text = _selection.GetSteps().ToString();
        }
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
                if (_selection != null && _selection != c)
                {
                    _buttonsManager.DeselectActions();
                }
                _selection = c;
                _selection.SelectThisUnit();
                _highlight.ChangeActiveCharacter(_selection);
                _buttonsManager.SetPlayerCharacter(_selection);
                _buttonsManager.SetPlayerUI();
                stepsCounter.text = _selection.GetSteps().ToString();
            }
            else
            {
                if (_enemySelection != null && c != _enemySelection)
                {
                    _enemySelection.DeselectThisUnit();
                }
                _enemySelection = c;
                _enemySelection.SelectedAsEnemy();
                if (_selection != null)
                    _selection.SetEnemy(_enemySelection);
                _buttonsManager.SetEnemy(_enemySelection);
                _buttonsManager.SetEnemyUI();
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
        _selectingEnemy = false;
        _buttonsManager.DeactivateCharacterButtons();
    }

    public void DeselectUnit()
    {
        if (_selection)
        {
            _selection.DeselectThisUnit();
            _selection = null;
        }
        if (_enemySelection)
        {
            _enemySelection.DeselectThisUnit();
            _selection = null;
        }
    }

    //void ClearBodyPartsButtons()
    //{
    //    buttonBody.onClick.RemoveListener(_enemySelection.AttackBody);
    //    buttonLArm.onClick.RemoveListener(_enemySelection.AttackLeftArm);
    //    buttonRArm.onClick.RemoveListener(_enemySelection.AttackRightArm);
    //    buttonLegs.onClick.RemoveListener(_enemySelection.AttackLegs);
    //}

    //void SetBodyPartsButtons(Character unit)
    //{
    //    buttonBody.onClick.AddListener(unit.AttackBody);
    //    buttonLArm.onClick.AddListener(unit.AttackLeftArm);
    //    buttonRArm.onClick.AddListener(unit.AttackRightArm);
    //    buttonLegs.onClick.AddListener(unit.AttackLegs);
    //}

    public void CanSelectEnemy()
    {
        _selectingEnemy = true;
    }

    public void CantSelectEnemy()
    {
        _selectingEnemy = false;
    }
}