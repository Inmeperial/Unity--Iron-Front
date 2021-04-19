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
    ButtonsManager _buttonsManager;
    public bool _canSelectUnit;
    private Character _enemySelection;
    
    public TextMeshProUGUI stepsCounter;
    public TextMeshProUGUI enemyHpCounter;
    public GameObject enemyHPContainer;
    public event Action OnCharacterSelect = delegate { };
    public event Action OnCharacterDeselect = delegate { };

    private bool _selectingEnemy;

   
    private void Start()
    {
        _canSelectUnit = true;
        _highlight = GetComponent<TileHighlight>();
        _turnManager = FindObjectOfType<TurnManager>();
        _buttonsManager = FindObjectOfType<ButtonsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canSelectUnit)
            SelectCharacter(charMask);

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
                    _selection.DeselectThisUnit();
                }
                _selection = c;
                _selection.SelectThisUnit();
                _selection.AddTilesInMoveRange();
                _highlight.ChangeActiveCharacter(_selection);
                _buttonsManager.SetCharacter(_selection);
                _buttonsManager.SetCharacterMovementButtons(_selection);
                stepsCounter.text = _selection.GetSteps().ToString();
            }
            else if (_selectingEnemy && c.CanBeAttacked())
            {
                _enemySelection = c;
                _enemySelection.SelectedAsEnemy();
                _selection.SetEnemy(_enemySelection);
                _buttonsManager.SetEnemy(_enemySelection);
                enemyHPContainer.SetActive(true);
                enemyHpCounter.text = _enemySelection.GetHP() + " / " + _enemySelection.maxHp;
                if (_selection.CanAttack())
                {
                    _buttonsManager.SetCharacterAttackButtons();
                    //SetBodyPartsButtons(_enemySelection);
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
            //ClearBodyPartsButtons();
            _enemySelection.DeselectThisUnit();
            enemyHPContainer.SetActive(false);
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

    

    public void UpdateHP(int currentHP, int maxHP)
    {
        enemyHpCounter.text = currentHP + " / " + maxHP;
    }

    public void CanSelectEnemy()
    {
        _selectingEnemy = true;
    }
}
