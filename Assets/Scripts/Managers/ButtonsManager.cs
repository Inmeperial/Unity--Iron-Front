using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    public Button buttonMove;
    public Button buttonUndo;
    public Button buttonSelectEnemy;
    public Button buttonExecuteAttack;

    public GameObject bodyPartsButtons;
    public Button buttonBody;
    public bool _buttonBodySelected;
    public Button buttonLArm;
    public bool _buttonLArmSelected;
    public Button buttonRArm;
    public bool _buttonRArmSelected;
    public Button buttonLegs;
    public bool _buttonLegsSelected;


    private Character _selectedEnemy;
    private CharacterSelection _charSelection;
    private TurnManager _turnManager;
    private void Start()
    {
        bodyPartsButtons.SetActive(false);

        _buttonBodySelected = false;
        _buttonLArmSelected = false;
        _buttonRArmSelected = false;
        _buttonLegsSelected = false;

        _charSelection = FindObjectOfType<CharacterSelection>();
        _turnManager = FindObjectOfType<TurnManager>();
    }
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

    public void SetCharacterMovementButtons(Character selection)
    {
        buttonMove.onClick.RemoveAllListeners();
        buttonMove.onClick.AddListener(selection.Move);
        buttonUndo.onClick.RemoveAllListeners();
        buttonUndo.onClick.AddListener(selection.pathCreator.UndoLastWaypoint);
        buttonSelectEnemy.interactable = true;
    }

    public void SetCharacterAttackButtons()
    {
        bodyPartsButtons.SetActive(true);
        buttonExecuteAttack.interactable = true;
    }

    public void DeactivateCharacterButtons()
    {
        buttonExecuteAttack.interactable = false;
        buttonSelectEnemy.interactable = false;
        buttonMove.interactable = false;
        buttonUndo.interactable = false;
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
        if (_selectedEnemy != null)
        {
            if (_buttonBodySelected)
                _selectedEnemy.AttackBody();

            if (_buttonLArmSelected)
                _selectedEnemy.AttackLeftArm();

            if (_buttonRArmSelected)
                _selectedEnemy.AttackRightArm();

            if (_buttonLegsSelected)
                _selectedEnemy.AttackLegs();

            if (_selectedEnemy.CanAttack() == false)
            {
                bodyPartsButtons.SetActive(false);
                buttonExecuteAttack.interactable = false;
            }
        }
    }

    public void SetEnemy(Character enemy)
    {
        _selectedEnemy = enemy;
    }

    public void SelectEnemy()
    {
        _charSelection.CanSelectEnemy();
    }

    public void EndTurn()
    {
        _turnManager.EndTurn();
    }
}
