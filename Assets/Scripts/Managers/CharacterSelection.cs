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
    public bool _canSelectUnit;
    private Character _enemySelection;
    public Button buttonMove;
    public Button buttonUndo;
    public Button buttonSelectEnemy;
    public Button buttonExecuteAttack;
    public TextMeshProUGUI stepsCounter;
    public TextMeshProUGUI enemyHpCounter;
    public GameObject enemyHPContainer;
    public event Action OnCharacterSelect = delegate { };
    public event Action OnCharacterDeselect = delegate { };

    private bool _selectingEnemy;

    public GameObject bodyPartsButtons;
    public Button buttonBody;
    public bool _buttonBodySelected;
    public Button buttonLArm;
    public bool _buttonLArmSelected;
    public Button buttonRArm;
    public bool _buttonRArmSelected;
    public Button buttonLegs;
    public bool _buttonLegsSelected;
    private void Start()
    {
        _canSelectUnit = true;
        _highlight = GetComponent<TileHighlight>();
        _turnManager = FindObjectOfType<TurnManager>();
        bodyPartsButtons.SetActive(false);

        _buttonBodySelected = false;
        _buttonLArmSelected = false;
        _buttonRArmSelected = false;
        _buttonLegsSelected = false;

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
                buttonMove.onClick.RemoveAllListeners();
                buttonMove.onClick.AddListener(_selection.Move);
                buttonUndo.onClick.RemoveAllListeners();
                buttonUndo.onClick.AddListener(_selection.pathCreator.UndoLastWaypoint);
                buttonSelectEnemy.interactable = true;
                stepsCounter.text = _selection.GetSteps().ToString();
            }
            else if (_selectingEnemy && c.CanBeAttacked())
            {
                _enemySelection = c;
                _enemySelection.SelectedAsEnemy();
                _selection.SetEnemy(_enemySelection);
                enemyHPContainer.SetActive(true);
                enemyHpCounter.text = _enemySelection.GetHP() + " / " + _enemySelection.maxHp;
                if (_selection.CanAttack())
                {
                    bodyPartsButtons.SetActive(true);
                    buttonExecuteAttack.interactable = true;
                    //SetBodyPartsButtons(_enemySelection);
                }
            }
        }
    }

    public void SelectEnemy()
    {
        _selectingEnemy = true;
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
        buttonExecuteAttack.interactable = false;
        buttonSelectEnemy.interactable = false;
        buttonMove.interactable = false;
        buttonUndo.interactable = false;
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

    public void ActivateMoveButton()
    {
        buttonMove.interactable = true;
        buttonUndo.interactable = true;
    }

    public void DeactivateMoveButton()
    {
        buttonMove.interactable = false;
        buttonUndo.interactable = false;
    }

    public void ActivateAttackButton()
    {
        buttonExecuteAttack.interactable = true;
    }

    public void DeactivateAttackButton()
    {
        buttonExecuteAttack.interactable = false;
    }

    public void UpdateHP(int currentHP, int maxHP)
    {
        enemyHpCounter.text = currentHP + " / " + maxHP;
    }

    public void BodySelection()
    {
        _buttonBodySelected = !_buttonBodySelected;
    }

    public void LeftArmSelection()
    {
        _buttonLArmSelected = !_buttonLArmSelected;
    }

    public void RightArmSelection()
    {
        _buttonRArmSelected = !_buttonRArmSelected;
    }

    public void LegsSelection()
    {
        _buttonLegsSelected = !_buttonLegsSelected;
    }

    public void ExecuteAttack()
    {
        if (_enemySelection != null)
        {
            if (_buttonBodySelected)
                _enemySelection.AttackBody();

            if (_buttonLArmSelected)
                _enemySelection.AttackLeftArm();
            
            if (_buttonRArmSelected)
                _enemySelection.AttackRightArm();

            if (_buttonLegsSelected)
                _enemySelection.AttackLegs();

            if (_enemySelection.CanAttack() == false)
            {
                bodyPartsButtons.SetActive(false);
                buttonExecuteAttack.interactable = false;
            }
        }
    }
}
