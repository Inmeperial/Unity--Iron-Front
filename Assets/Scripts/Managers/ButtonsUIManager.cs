using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonsUIManager : MonoBehaviour
{
    public Button buttonMove;
    public Button buttonUndo;
    public Button buttonSelectEnemy;
    public Button buttonExecuteAttack;

    public GameObject hud;
    //BUTTONS
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

    //UI
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI rangeText;
    public Slider bodySlider;
    public TextMeshProUGUI bodyCurrHP;
    public Slider leftArmSlider;
    public TextMeshProUGUI leftArmCurrHP;
    public Slider rightArmSlider;
    public TextMeshProUGUI rightArmCurrHP;
    public Slider legsSlider;
    public TextMeshProUGUI legsCurrHP;

    //OTHERS
    [SerializeField] private Character _selectedEnemy;
    [SerializeField] private CharacterSelection _charSelection;
    [SerializeField] private Character _selectedChar;
    private TurnManager _turnManager;
    private void Start()
    {
        hud.SetActive(false);

        _buttonBodySelected = false;
        _buttonLArmSelected = false;
        _buttonRArmSelected = false;
        _buttonLegsSelected = false;

        _charSelection = FindObjectOfType<CharacterSelection>();
        _turnManager = FindObjectOfType<TurnManager>();
    }
    #region Buttons
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

    public void SetEnemyUI()
    {
        hud.SetActive(true);
        ShowHUDSliders();
        ShowHUDText();
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
                _selectedEnemy.AttackBody(_bulletsForBody, _selectedChar.GetBulletDamage());
                bodySlider.value = _selectedEnemy.GetBodyHP();
                bodyCurrHP.text = _selectedEnemy.GetBodyHP().ToString();
            }
                

            if (_bulletsForLArm > 0)
            {
                _selectedEnemy.AttackLeftArm(_bulletsForLArm, _selectedChar.GetBulletDamage());
                leftArmSlider.value = _selectedEnemy.GetLeftArmHP();
                leftArmCurrHP.text = _selectedEnemy.GetLeftArmHP().ToString();
            }

            if (_bulletsForRArm > 0)
            {
                _selectedEnemy.AttackRightArm(_bulletsForRArm, _selectedChar.GetBulletDamage());
                rightArmSlider.value = _selectedEnemy.GetRightArmHP();
                rightArmCurrHP.text = _selectedEnemy.GetRightArmHP().ToString();
            }
        }

            if (_bulletsForLegs > 0)
            {
                _selectedEnemy.AttackLegs(_bulletsForLegs, _selectedChar.GetBulletDamage());
                legsSlider.value = _selectedEnemy.GetLegsHP();
                legsCurrHP.text = _selectedEnemy.GetLegsHP().ToString();
            }

            if (_selectedEnemy.CanAttack() == false)
            {
                _selectedEnemy = null;
                buttonExecuteAttack.interactable = false;
            }
    }
    public void SelectEnemy()
    {
        _charSelection.CanSelectEnemy();
    }
    public void EndTurn()
    {
        _turnManager.EndTurn();
        hud.SetActive(false);
    }
    #endregion

    #region UI Text

    void ShowHUDSliders()
    {
        bodySlider.maxValue = _selectedEnemy.GetBodyMaxHP();
        bodySlider.value = _selectedEnemy.GetBodyHP();

        leftArmSlider.maxValue = _selectedEnemy.GetLeftArmMaxHP();
        leftArmSlider.value = _selectedEnemy.GetLeftArmHP();

        rightArmSlider.maxValue = _selectedEnemy.GetRightArmMaxHP();
        rightArmSlider.value = _selectedEnemy.GetRightArmHP();

        legsSlider.maxValue = _selectedEnemy.GetLegsMaxHP();
        legsSlider.value = _selectedEnemy.GetLegsHP();
    }

    void ShowHUDText()
    {
        var dmg = _selectedChar.GetBulletDamage();
        var bullets = _selectedChar.GetAvailableBullets();
        damageText.text = "DMG " + dmg + " x " + bullets + " hit";

        var range = _selectedChar.GetRange();
        rangeText.text = "Range " + range;

        bodyCurrHP.text = _selectedEnemy.GetBodyHP().ToString();
        leftArmCurrHP.text = _selectedEnemy.GetLeftArmHP().ToString();
        rightArmCurrHP.text = _selectedEnemy.GetRightArmHP().ToString();
        legsCurrHP.text = _selectedEnemy.GetLegsHP().ToString();
    }
    #endregion
    public void SetEnemy(Character enemy)
    {
        _selectedEnemy = enemy;
    }

    public void SetCharacter(Character character)
    {
        _selectedChar = character;
    }


}
