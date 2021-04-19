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
    private int _bulletsForBody;
    public Button buttonLArm;
    public bool _buttonLArmSelected;
    private int _bulletsForLArm;
    public Button buttonRArm;
    public bool _buttonRArmSelected;
    private int _bulletsForRArm;
    public Button buttonLegs;
    public bool _buttonLegsSelected;
    private int _bulletsForLegs;


    [SerializeField] private Character _selectedEnemy;
    [SerializeField] private CharacterSelection _charSelection;
    [SerializeField] private Character _selectedChar;
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
        _selectedEnemy = null;
    }

    public void BodySelection()
    {
        if (CharacterHasBullets(_selectedChar))
        {
            Debug.Log("entro al body");
            _bulletsForBody++;
            _selectedChar.ReduceAvailableBullets(1);
        }

    }

    public void LeftArmSelection()
    {
        if (CharacterHasBullets(_selectedChar))
        {
            _bulletsForLArm++;
            _selectedChar.ReduceAvailableBullets(1);
        }
    }

    public void RightArmSelection()
    {
        if (CharacterHasBullets(_selectedChar))
        {
            _bulletsForRArm++;
            _selectedChar.ReduceAvailableBullets(1);
        }
    }

    public void LegsSelection()
    {
        if (CharacterHasBullets(_selectedChar))
        {
            _bulletsForLegs++;
            _selectedChar.ReduceAvailableBullets(1);
        }
    }

    bool CharacterHasBullets(Character c)
    {
        if (c.GetAvailableBullets() > 0)
            return true;
        else return false;
    }
    public void ExecuteAttack()
    {
        if (_selectedEnemy != null)
        {
            if (_bulletsForBody > 0)
            {
                _selectedEnemy.AttackBody(_bulletsForBody, _selectedChar.damage);
            }
                

            if (_bulletsForLArm > 0)
            {
                _selectedEnemy.AttackLeftArm(_bulletsForLArm, _selectedChar.damage);

            }

            if (_bulletsForRArm > 0)
            {
                _selectedEnemy.AttackRightArm(_bulletsForRArm, _selectedChar.damage);

            }

            if (_bulletsForLegs > 0)
            {
                _selectedEnemy.AttackLegs(_bulletsForLegs, _selectedChar.damage);

            }

            if (_selectedEnemy.CanAttack() == false)
            {
                _selectedEnemy = null;
                bodyPartsButtons.SetActive(false);
                buttonExecuteAttack.interactable = false;
            }
        }
    }

    public void SetEnemy(Character enemy)
    {
        _selectedEnemy = enemy;
    }

    public void SetCharacter(Character character)
    {
        _selectedChar = character;
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
