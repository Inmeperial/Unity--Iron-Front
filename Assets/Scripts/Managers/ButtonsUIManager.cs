using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonsUIManager : MonoBehaviour
{
    public GameObject moveContainer;
    public GameObject actionMenu;
    public Button buttonMove;
    public Button buttonUndo;
    public Button buttonSelectEnemy;
    public Button buttonExecuteAttack;



    #region Buttons
    public GameObject bodyPartsButtonsContainer;
    public Button buttonBody;
    public Button buttonBodyMinus;
    public Button buttonBodyX;
    public bool _buttonBodySelected;
    private int _bulletsForBody;
    public Button buttonLArm;
    public Button buttonLArmMinus;
    public Button buttonLArmX;
    public bool _buttonLArmSelected;
    private int _bulletsForLArm;
    public Button buttonRArm;
    public Button buttonRArmMinus;
    public Button buttonRArmX;
    public bool _buttonRArmSelected;
    private int _bulletsForRArm;
    public Button buttonLegs;
    public Button buttonLegsMinus;
    public Button buttonLegsX;
    public bool _buttonLegsSelected;
    private int _bulletsForLegs;
    #endregion

    #region HUD
    //Player
    public GameObject playerHudContainer;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI rangeText;
    public Slider playerBodySlider;
    public TextMeshProUGUI playerBodyCurrHP;
    public Slider playerLeftArmSlider;
    public TextMeshProUGUI playerLeftArmCurrHP;
    public Slider playerRightArmSlider;
    public TextMeshProUGUI playerRightArmCurrHP;
    public Slider playerLegsSlider;
    public TextMeshProUGUI playerLegsCurrHP;

    //Enemy
    public GameObject enemyHudContainer;
    public Slider enemyBodySlider;
    public TextMeshProUGUI enemyBodyCurrHP;
    public Slider enemyLeftArmSlider;
    public TextMeshProUGUI enemyLeftArmCurrHP;
    public Slider enemyRightArmSlider;
    public TextMeshProUGUI enemyRightArmCurrHP;
    public Slider enemyLegsSlider;
    public TextMeshProUGUI enemyLegsCurrHP;
    #endregion
    //OTHERS
    [SerializeField] private CharacterSelection _charSelection;
    [SerializeField] private Character _selectedChar;
    [SerializeField] private Character _selectedEnemy;
    private TurnManager _turnManager;
    private int _partsSelected;
    private void Start()
    {
        enemyHudContainer.SetActive(false);
        playerHudContainer.SetActive(false);
        moveContainer.SetActive(false);
        actionMenu.SetActive(false);
        bodyPartsButtonsContainer.SetActive(false);
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
        moveContainer.SetActive(true);
        buttonMove.interactable = true;
        buttonUndo.interactable = true;
    }

    public void DeactivateMoveButton()
    {
        moveContainer.SetActive(false);
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

    void SetCharacterMovementButtons()
    {
        buttonMove.onClick.RemoveAllListeners();
        buttonMove.onClick.AddListener(_selectedChar.Move);
        buttonUndo.onClick.RemoveAllListeners();
        buttonUndo.onClick.AddListener(_selectedChar.pathCreator.UndoLastWaypoint);
        buttonSelectEnemy.interactable = true;
    }
    public void SetPlayerUI()
    {
        SetCharacterMovementButtons();
        ShowHUDSliders(_selectedChar, playerBodySlider, playerLeftArmSlider, playerRightArmSlider, playerLegsSlider);
        ShowPlayerHudText(playerBodyCurrHP, playerBodySlider, playerLeftArmCurrHP, playerLeftArmSlider, playerRightArmCurrHP, playerRightArmSlider, playerLegsCurrHP, playerLegsSlider);
        ActivateMoveButton();
        playerHudContainer.SetActive(true);
        actionMenu.SetActive(true);
    }
    public void DeactivatePlayerHUD()
    {
        playerHudContainer.SetActive(false);
        actionMenu.SetActive(true);
    }


    public void SetEnemyUI()
    {
        bodyPartsButtonsContainer.SetActive(true);
        ShowHUDSliders(_selectedEnemy, enemyBodySlider, enemyLeftArmSlider, enemyRightArmSlider, enemyLegsSlider);
        ShowUnitHudText(enemyBodyCurrHP, enemyBodySlider, enemyLeftArmCurrHP, enemyLeftArmSlider, enemyRightArmCurrHP, enemyRightArmSlider, enemyLegsCurrHP, enemyLegsSlider);
        enemyHudContainer.SetActive(true);
    }

    public void DeactivateEnemyHUD()
    {
        bodyPartsButtonsContainer.SetActive(false);
        enemyHudContainer.SetActive(false);
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
            var gun = _selectedChar.selectedGun;
            if (_partsSelected <= gun.GetAvailableSelections())
            {
                if (_bulletsForBody == 0)
                {
                    _partsSelected++;
                }
                _bulletsForBody += gun.BulletsPerClick();

                CreateBulletInUI(buttonBody, _bulletsForBody);

                gun.ReduceAvailableBullets();

                buttonExecuteAttack.interactable = true;
                _buttonBodySelected = true;
                DeterminateButtonsActivation();
            }
        }

    }

    

    public void BodyMinus()
    {
        if (_bulletsForBody > 0)
        {
            var gun = _selectedChar.selectedGun;
            gun.IncreaseAvailableBullets();
            _bulletsForBody = _bulletsForBody > 0 ? (_bulletsForBody - gun.BulletsPerClick()) : 0;
            DeleteBulletInUI(buttonBody, gun.BulletsPerClick());
            CheckIfCanExecuteAttack();
            if (_bulletsForBody == 0)
            {
                if (_partsSelected > 0)
                    _partsSelected--;
                _buttonBodySelected = false;
                DeterminateButtonsActivation();
            }
        }
    }

    public void BodyClear()
    {
        if (_bulletsForBody > 0)
        {
            var gun = _selectedChar.selectedGun;
            gun.IncreaseAvailableBullets(_bulletsForBody);
            _bulletsForBody = 0;
            _buttonBodySelected = false;
            if (_partsSelected > 0)
                _partsSelected--;
            CheckIfCanExecuteAttack();
            DeterminateButtonsActivation();
        }
    }

    public void LeftArmSelection()
    {
        if (CharacterHasBullets(_selectedChar))
        {
            var gun = _selectedChar.selectedGun;
            if (_partsSelected <= _selectedChar.selectedGun.GetAvailableSelections())
            {
                if (_bulletsForLArm == 0)
                {
                    _partsSelected++;
                }
                _bulletsForLArm += gun.BulletsPerClick();

                CreateBulletInUI(buttonLArm, _bulletsForLArm);

                gun.ReduceAvailableBullets();

                buttonExecuteAttack.interactable = true;
                _buttonLArmSelected = true;
                DeterminateButtonsActivation();
            }
        }
    }

    public void LeftArmMinus()
    {
        if (_bulletsForLArm > 0)
        {
            var gun = _selectedChar.selectedGun;
            gun.IncreaseAvailableBullets();
            _bulletsForLArm = _bulletsForLArm > 0 ? (_bulletsForLArm - gun.BulletsPerClick()) : 0;
            DeleteBulletInUI(buttonLArm, gun.BulletsPerClick());
            CheckIfCanExecuteAttack();
            if (_bulletsForLArm == 0)
            {
                if (_partsSelected > 0)
                    _partsSelected--;
                _buttonLArmSelected = false;
                DeterminateButtonsActivation();
            }
        }
    }

    public void LeftArmClear()
    {
        if (_bulletsForLArm > 0)
        {
            var gun = _selectedChar.selectedGun;
            gun.IncreaseAvailableBullets(_bulletsForLArm);
            _bulletsForLArm = 0;
            _buttonLArmSelected = false;
            if (_partsSelected > 0)
                _partsSelected--;
            CheckIfCanExecuteAttack();
            DeterminateButtonsActivation();
        }
    }

    public void RightArmSelection()
    {
        if (CharacterHasBullets(_selectedChar))
        {
            var gun = _selectedChar.selectedGun;
            if (_partsSelected <= _selectedChar.selectedGun.GetAvailableSelections())
            {
                if (_bulletsForRArm == 0)
                {
                    _partsSelected++;
                }
                _bulletsForRArm += gun.BulletsPerClick();
                CreateBulletInUI(buttonRArm, _bulletsForRArm);
                gun.ReduceAvailableBullets();
                buttonExecuteAttack.interactable = true;
                _buttonRArmSelected = true;
                DeterminateButtonsActivation();
            }
        }
    }

    public void RightArmMinus()
    {
        if (_bulletsForRArm > 0)
        {
            var gun = _selectedChar.selectedGun;
            gun.IncreaseAvailableBullets();
            _bulletsForRArm = _bulletsForRArm > 0 ? (_bulletsForRArm - gun.BulletsPerClick()) : 0;
            DeleteBulletInUI(buttonRArm, gun.BulletsPerClick());
            CheckIfCanExecuteAttack();
            if (_bulletsForRArm == 0)
            {
                if (_partsSelected > 0)
                    _partsSelected--;
                _buttonRArmSelected = false;
                DeterminateButtonsActivation();
            }
        }
    }

    public void RightArmClear()
    {
        if (_bulletsForRArm > 0)
        {
            var gun = _selectedChar.selectedGun;
            gun.IncreaseAvailableBullets(_bulletsForRArm);
            _bulletsForRArm = 0;
            _buttonRArmSelected = false;
            if (_partsSelected > 0)
                _partsSelected--;
            CheckIfCanExecuteAttack();
            DeterminateButtonsActivation();
        }
    }

    public void LegsSelection()
    {
        if (CharacterHasBullets(_selectedChar))
        {
            var gun = _selectedChar.selectedGun;
            if (_partsSelected <= _selectedChar.selectedGun.GetAvailableSelections())
            {
                if (_bulletsForLegs == 0)
                {
                    _partsSelected++;
                }
                _bulletsForLegs += gun.BulletsPerClick();
                CreateBulletInUI(buttonLegs, _bulletsForLegs);
                gun.ReduceAvailableBullets();
                buttonExecuteAttack.interactable = true;
                _buttonLegsSelected = true;
                DeterminateButtonsActivation();
            }
        }
    }

    public void LegsMinus()
    {
        if (_bulletsForLegs > 0)
        {
            var gun = _selectedChar.selectedGun;
            gun.IncreaseAvailableBullets();
            _bulletsForLegs = _bulletsForLegs > 0 ? (_bulletsForLegs - gun.BulletsPerClick()) : 0;
            DeleteBulletInUI(buttonLegs, gun.BulletsPerClick());
            CheckIfCanExecuteAttack();
            if (_bulletsForLegs == 0)
            {
                if (_partsSelected > 0)
                    _partsSelected--;
                _buttonLegsSelected = false;
                DeterminateButtonsActivation();
            }
        }
    }

    public void LegsClear()
    {
        if (_bulletsForLegs > 0)
        {
            var gun = _selectedChar.selectedGun;
            gun.IncreaseAvailableBullets(_bulletsForLegs);
            _bulletsForLegs = 0;
            _buttonLegsSelected = false;
            if (_partsSelected > 0)
                _partsSelected--;
            CheckIfCanExecuteAttack();
            DeterminateButtonsActivation();
        }
    }

    

    public void ExecuteAttack()
    {
        if (_selectedEnemy != null)
        {
            var gun = _selectedChar.selectedGun;
            if (_bulletsForBody > 0)
            {
                var d = gun.DamageCalculation(_bulletsForBody);
                _selectedEnemy.AttackBody(d);
                enemyBodySlider.value = _selectedEnemy.GetBodyHP();
                enemyBodyCurrHP.text = enemyBodySlider.value.ToString();
                if (gun.AbilityUsed() == false)
                {
                    gun.Ability();
                }
                _selectedChar.DeactivateAttack();
            }


            if (_bulletsForLArm > 0)
            {
                var d = gun.DamageCalculation(_bulletsForLArm);
                _selectedEnemy.AttackLeftArm(d);
                enemyLeftArmSlider.value = _selectedEnemy.GetLeftArmHP();
                enemyLeftArmCurrHP.text = enemyLeftArmSlider.value.ToString();
                _selectedChar.DeactivateAttack();
            }

            if (_bulletsForRArm > 0)
            {
                var d = gun.DamageCalculation(_bulletsForRArm);
                _selectedEnemy.AttackRightArm(d);
                enemyRightArmSlider.value = _selectedEnemy.GetRightArmHP();
                enemyRightArmCurrHP.text = enemyRightArmSlider.value.ToString();
                _selectedChar.DeactivateAttack();
            }

            if (_bulletsForLegs > 0)
            {
                var d = gun.DamageCalculation(_bulletsForLegs);
                _selectedEnemy.AttackLegs(d);
                enemyLegsSlider.value = _selectedEnemy.GetLegsHP();
                enemyLegsCurrHP.text = enemyLegsSlider.value.ToString();
                _selectedChar.DeactivateAttack();
            }
        }

        if (_selectedChar.CanAttack() == false)
        {
            bodyPartsButtonsContainer.SetActive(false);
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
        DeactivateEnemyHUD();
        DeactivatePlayerHUD();
        DeactivateMoveButton();
        ResetBodyParts();
    }

    #endregion

    #region Utilities

    public void AddBulletsToBody(int quantity)
    {
        _bulletsForBody = quantity;
        _bulletsForLArm = 0;
        _bulletsForRArm = 0;
        _bulletsForLegs = 0;
        ExecuteAttack();
    }

    public void AddBulletsToLArm(int quantity)
    {
        _bulletsForBody = 0;
        _bulletsForLArm = quantity;
        _bulletsForRArm = 0;
        _bulletsForLegs = 0;
        ExecuteAttack();
    }

    public void AddBulletsToRArm(int quantity)
    {
        _bulletsForBody = 0;
        _bulletsForLArm = 0;
        _bulletsForRArm = quantity;
        _bulletsForLegs = 0;
        ExecuteAttack();
    }

    public void AddBulletsToLegs(int quantity)
    {
        _bulletsForBody = 0;
        _bulletsForLArm = 0;
        _bulletsForRArm = 0;
        _bulletsForLegs = quantity;
        ExecuteAttack();
    }

    void CreateBulletInUI(Button button, int quantity)
    {
        for (int i = button.GetComponent<BodyPartSelection>().count; i < quantity; i++)
        {
            button.GetComponent<BodyPartSelection>().GenerateCounter();
        }
    }

    void DeleteBulletInUI(Button button, int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            button.GetComponent<BodyPartSelection>().RemoveCounter();
        }
    }

    bool CharacterHasBullets(Character c)
    {
        if (c.selectedGun.GetAvailableBullets() > 0)
            return true;
        else return false;
    }

    //Checks if player can attack en enemy.
    void CheckIfCanExecuteAttack()
    {
        if (_bulletsForBody == 0 && _bulletsForLArm == 0 && _bulletsForLegs == 0 && _bulletsForRArm == 0)
            buttonExecuteAttack.interactable = false;
    }

    //Determines which buttons will be interactable.
    void DeterminateButtonsActivation()
    {
        if (_selectedChar.selectedGun.GetAvailableBullets() <= 0 || _partsSelected == _selectedChar.selectedGun.GetAvailableSelections())
        {
            if (_buttonBodySelected == false)
            {
                buttonBody.interactable = false;
                buttonBodyMinus.interactable = false;
                buttonBodyX.interactable = false;
            }

            if (_buttonLArmSelected == false)
            {
                buttonLArm.interactable = false;
                buttonLArmMinus.interactable = false;
                buttonLArmX.interactable = false;
            }

            if (_buttonRArmSelected == false)
            {
                buttonRArm.interactable = false;
                buttonRArmMinus.interactable = false;
                buttonRArmX.interactable = false;
            }

            if (_buttonLegsSelected == false)
            {
                buttonLegs.interactable = false;
                buttonLegsMinus.interactable = false;
                buttonLegsX.interactable = false;
            }

            buttonExecuteAttack.interactable = true;
        }
        else if (_partsSelected < _selectedChar.selectedGun.GetAvailableSelections())
        {
            if (!_buttonBodySelected)
            {
                buttonBody.interactable = true;
                buttonBodyMinus.interactable = true;
                buttonBodyX.interactable = true;
            }

            if (!_buttonLArmSelected)
            {
                buttonLArm.interactable = true;
                buttonLArmMinus.interactable = true;
                buttonLArmX.interactable = true;
            }

            if (!_buttonRArmSelected)
            {
                buttonRArm.interactable = true;
                buttonRArmMinus.interactable = true;
                buttonRArmX.interactable = true;
            }

            if (!_buttonLegsSelected)
            {
                buttonLegs.interactable = true;
                buttonLegsMinus.interactable = true;
                buttonLegsX.interactable = true;
            }
        }
    }

    void ResetBodyParts()
    {
        _buttonBodySelected = false;
        _bulletsForBody = 0;
        buttonBody.GetComponent<BodyPartSelection>().ClearCounters();

        _buttonLArmSelected = false;
        _bulletsForLArm = 0;
        buttonLArm.GetComponent<BodyPartSelection>().ClearCounters();

        _buttonRArmSelected = false;
        _bulletsForRArm = 0;
        buttonRArm.GetComponent<BodyPartSelection>().ClearCounters();

        _buttonLegsSelected = false;
        _bulletsForLegs = 0;
        buttonLegs.GetComponent<BodyPartSelection>().ClearCounters();

        _partsSelected = 0;

        DeterminateButtonsActivation();
    }
    #endregion

    #region HUD Text

    void ShowPlayerHudText(TextMeshProUGUI bodyHpText, Slider bodySlider, TextMeshProUGUI lArmHpText, Slider lArmSlider, TextMeshProUGUI rArmHpText, Slider rArmSlider, TextMeshProUGUI legsHpText, Slider legsSlider)
    {
        ShowUnitHudText(bodyHpText, bodySlider, lArmHpText, lArmSlider, rArmHpText, rArmSlider, legsHpText, legsSlider);

        var dmg = _selectedChar.selectedGun.GetBulletDamage().ToString();
        var b = _selectedChar.selectedGun.GetAvailableBullets();
        damageText.text = "DMG " + dmg + " x " + b + " hits";

        var r = _selectedChar.selectedGun.GetAttackRange().ToString();
        rangeText.text = "Range " + r;
    }

    void ShowHUDSliders(Character unit, Slider body, Slider lArm, Slider rArm, Slider legs)
    {
        body.maxValue = unit.GetBodyMaxHP();
        body.value = unit.GetBodyHP() > 0 ? unit.GetBodyHP() : 0;

        lArm.maxValue = unit.GetLeftArmMaxHP();
        lArm.value = unit.GetLeftArmHP() > 0 ? unit.GetLeftArmHP() : 0;

        rArm.maxValue = unit.GetRightArmMaxHP();
        rArm.value = unit.GetRightArmHP() > 0 ? unit.GetRightArmHP() : 0;

        legs.maxValue = unit.GetLegsMaxHP();
        legs.value = unit.GetLegsHP() > 0 ? unit.GetLegsHP() : 0;
    }

    void ShowUnitHudText(TextMeshProUGUI bodyHpText, Slider bodySlider, TextMeshProUGUI lArmHpText, Slider lArmSlider,TextMeshProUGUI rArmHpText, Slider rArmSlider, TextMeshProUGUI legsHpText, Slider legsSlider)
    {
        bodyHpText.text = bodySlider.value.ToString();
        lArmHpText.text = lArmSlider.value.ToString();
        rArmHpText.text = rArmSlider.value.ToString();
        legsHpText.text = legsSlider.value.ToString();
    }


    #endregion
    public void SetEnemy(Character enemy)
    {
        _selectedEnemy = enemy;
    }

    public void SetPlayerCharacter(Character character)
    {
        _selectedChar = character;
    }
}
