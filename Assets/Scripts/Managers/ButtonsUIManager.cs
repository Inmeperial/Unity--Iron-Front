using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonsUIManager : MonoBehaviour
{
    public GameObject moveContainer;
    public Button buttonMove;
    public Button buttonUndo;
    public Button buttonExecuteAttack;
    public Button buttonEndTurn;

    public KeyCode deselectKey;



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
    public TextMeshProUGUI gunTypeText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI critText;
    public TextMeshProUGUI hitChanceText;
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
        DeactivateBodyPartsContainer();
        _buttonBodySelected = false;
        _buttonLArmSelected = false;
        _buttonRArmSelected = false;
        _buttonLegsSelected = false;

        _charSelection = FindObjectOfType<CharacterSelection>();
        _turnManager = FindObjectOfType<TurnManager>();
    }

    private void Update()
    {
        if (_selectedChar != null && _selectedChar.IsMoving() == false && Input.GetKeyDown(deselectKey))
            DeselectUnit();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            UnitSwapToLeftGun();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            UnitSwapToRightGun();
    }

    #region ButtonsActions
    public void ActivateMoveButton()
    {
        moveContainer.SetActive(true);
        buttonMove.interactable = true;
        buttonUndo.interactable = true;
        DeactivateEndTurnButton();
    }

    public void DeactivateMoveButton()
    {
        moveContainer.SetActive(false);
        buttonMove.interactable = false;
        buttonUndo.interactable = false;
        //buttonEndTurn.gameObject.SetActive(true);
    }

    public void ActivateExecuteAttackButton()
    {
        buttonExecuteAttack.interactable = true;
    }

    public void DeactivateExecuteAttackButton()
    {
        buttonExecuteAttack.interactable = false;
    }

    void SetCharacterMovementButtons()
    {
        if (_selectedChar.ThisUnitCanMove())
        {
            buttonMove.onClick.RemoveAllListeners();
            buttonMove.onClick.AddListener(_selectedChar.Move);
            buttonUndo.onClick.RemoveAllListeners();
            buttonUndo.onClick.AddListener(_selectedChar.pathCreator.UndoLastWaypoint);
            ActivateMoveButton();
        }
        else DeactivateMoveButton();
    }
    

    public void DeactivateCharacterButtons()
    {
        buttonExecuteAttack.interactable = false;
        buttonMove.interactable = false;
        buttonUndo.interactable = false;
        _selectedEnemy = null;
    }

    public void BodySelection()
    {
        if (CharacterHasBullets(_selectedChar))
        {
            var gun = _selectedChar.GetSelectedGun();
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
            var gun = _selectedChar.GetSelectedGun();
            gun.IncreaseAvailableBullets();
            _bulletsForBody = _bulletsForBody > 0 ? (_bulletsForBody - gun.BulletsPerClick()) : 0;
            DeleteBulletInUI(buttonBody, gun.BulletsPerClick());
            CheckIfCanExecuteAttack();
            if (_bulletsForBody == 0)
            {
                if (_partsSelected > 0)
                    _partsSelected--;
                _buttonBodySelected = false;
            }
            DeterminateButtonsActivation();
        }
    }

    public void BodyClear()
    {
        if (_bulletsForBody > 0)
        {
            var gun = _selectedChar.GetSelectedGun();
            gun.IncreaseAvailableBullets(_bulletsForBody);
            _bulletsForBody = 0;
            _buttonBodySelected = false;
            ClearBulletsInUI(buttonBody);
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
            var gun = _selectedChar.GetSelectedGun();
            if (_partsSelected <= gun.GetAvailableSelections())
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
            var gun = _selectedChar.GetSelectedGun();
            gun.IncreaseAvailableBullets();
            _bulletsForLArm = _bulletsForLArm > 0 ? (_bulletsForLArm - gun.BulletsPerClick()) : 0;
            DeleteBulletInUI(buttonLArm, gun.BulletsPerClick());
            CheckIfCanExecuteAttack();
            if (_bulletsForLArm == 0)
            {
                if (_partsSelected > 0)
                    _partsSelected--;
                _buttonLArmSelected = false;
            }
            DeterminateButtonsActivation();
        }
    }

    public void LeftArmClear()
    {
        if (_bulletsForLArm > 0)
        {
            var gun = _selectedChar.GetSelectedGun();
            gun.IncreaseAvailableBullets(_bulletsForLArm);
            _bulletsForLArm = 0;
            _buttonLArmSelected = false;
            ClearBulletsInUI(buttonLArm);
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
            var gun = _selectedChar.GetSelectedGun();
            if (_partsSelected <= gun.GetAvailableSelections())
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
            var gun = _selectedChar.GetSelectedGun();
            gun.IncreaseAvailableBullets();
            _bulletsForRArm = _bulletsForRArm > 0 ? (_bulletsForRArm - gun.BulletsPerClick()) : 0;
            DeleteBulletInUI(buttonRArm, gun.BulletsPerClick());
            CheckIfCanExecuteAttack();
            if (_bulletsForRArm == 0)
            {
                if (_partsSelected > 0)
                    _partsSelected--;
                _buttonRArmSelected = false;
            }
            DeterminateButtonsActivation();
        }
    }

    public void RightArmClear()
    {
        if (_bulletsForRArm > 0)
        {
            var gun = _selectedChar.GetSelectedGun();
            gun.IncreaseAvailableBullets(_bulletsForRArm);
            _bulletsForRArm = 0;
            _buttonRArmSelected = false;
            ClearBulletsInUI(buttonRArm);
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
            var gun = _selectedChar.GetSelectedGun();
            if (_partsSelected <= gun.GetAvailableSelections())
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
            var gun = _selectedChar.GetSelectedGun();
            gun.IncreaseAvailableBullets();
            _bulletsForLegs = _bulletsForLegs > 0 ? (_bulletsForLegs - gun.BulletsPerClick()) : 0;
            DeleteBulletInUI(buttonLegs, gun.BulletsPerClick());
            CheckIfCanExecuteAttack();
            if (_bulletsForLegs == 0)
            {
                if (_partsSelected > 0)
                    _partsSelected--;
                _buttonLegsSelected = false;
            }
            DeterminateButtonsActivation();
        }
    }

    public void LegsClear()
    {
        if (_bulletsForLegs > 0)
        {
            var gun = _selectedChar.GetSelectedGun();
            gun.IncreaseAvailableBullets(_bulletsForLegs);
            _bulletsForLegs = 0;
            _buttonLegsSelected = false;
            ClearBulletsInUI(buttonLegs);
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
            var gun = _selectedChar.GetSelectedGun();
            if (_bulletsForBody > 0)
            {
                var d = gun.DamageCalculation(_bulletsForBody);
                _selectedEnemy.TakeDamageBody(d);
                _bulletsForBody = 0;
                if (gun.AbilityUsed() == false)
                {
                    gun.Ability();
                }
                _selectedChar.DeactivateAttack();
            }


            if (_bulletsForLArm > 0)
            {
                var d = gun.DamageCalculation(_bulletsForLArm);
                _selectedEnemy.TakeDamageLeftArm(d);
                _bulletsForLArm = 0;
                
                if (gun.AbilityUsed() == false)
                {
                    gun.Ability();
                }
                _selectedChar.DeactivateAttack();
            }

            if (_bulletsForRArm > 0)
            {
                var d = gun.DamageCalculation(_bulletsForRArm);
                _selectedEnemy.TakeDamageRightArm(d);
                _bulletsForRArm = 0;
                if (gun.AbilityUsed() == false)
                {
                    gun.Ability();
                }
                _selectedChar.DeactivateAttack();
            }

            if (_bulletsForLegs > 0)
            {
                var d = gun.DamageCalculation(_bulletsForLegs);
                _selectedEnemy.TakeDamageLegs(d);
                _bulletsForLegs = 0;
                if (gun.AbilityUsed() == false)
                {
                    gun.Ability();
                }
                _selectedChar.DeactivateAttack();
            }
        }

        if (_selectedChar.CanAttack() == false)
        {
            _selectedChar.ResetInRangeLists();
            _charSelection.CantSelectEnemy();
            DeactivateMoveButton();
            DeactivateBodyPartsContainer();
            buttonExecuteAttack.interactable = false;
        }
    }



    public void SelectEnemy()
    {
        if (_selectedChar != null && _selectedChar.CanAttack())
            _charSelection.CanSelectEnemy();
    }

    public void EndTurn()
    {
        if (_selectedChar == null || _selectedChar.IsMoving() == false)
        {
            _turnManager.EndTurn();
            DeactivateEnemyHUD();
            DeactivatePlayerHUD();
            DeactivateMoveButton();
            ResetBodyParts();
        }
    }

    public void DeselectActions()
    {
        if (_selectedChar != null)
        {
            var gun = _selectedChar.GetSelectedGun();

            if (_bulletsForBody > 0)
                gun.IncreaseAvailableBullets(_bulletsForBody);

            if (_bulletsForLArm > 0)
                gun.IncreaseAvailableBullets(_bulletsForLArm);

            if (_bulletsForRArm > 0)
                gun.IncreaseAvailableBullets(_bulletsForRArm);

            if (_bulletsForLegs > 0)
                gun.IncreaseAvailableBullets(_bulletsForLegs);
            
            ResetBodyParts();
        }

        DeactivatePlayerHUD();

        DeactivateEnemyHUD();

        ActivateEndTurnButton();

        _selectedChar = null;
        _selectedEnemy = null;
    }

    public void DeselectUnit()
    {
        DeselectActions();
        _charSelection.DeselectUnit();
    }

    public void UnitSwapToLeftGun()
    {
        if (_selectedChar != null)
        {
            BodyClear();
            LeftArmClear();
            RightArmClear();
            LegsClear();
            _selectedChar.SelectLeftGun();
            ShowPlayerHudText(playerBodyCurrHP, playerBodySlider, playerLeftArmCurrHP, playerLeftArmSlider, playerRightArmCurrHP, playerRightArmSlider, playerLegsCurrHP, playerLegsSlider);
        }
    }

    public void UnitSwapToRightGun()
    {
        if (_selectedChar != null)
        {
            BodyClear();
            LeftArmClear();
            RightArmClear();
            LegsClear();
            _selectedChar.SelectRightGun();
            ShowPlayerHudText(playerBodyCurrHP, playerBodySlider, playerLeftArmCurrHP, playerLeftArmSlider, playerRightArmCurrHP, playerRightArmSlider, playerLegsCurrHP, playerLegsSlider);
        }
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

    void ClearBulletsInUI(Button button)
    {
        button.GetComponent<BodyPartSelection>().ClearCounters();
    }

    bool CharacterHasBullets(Character c)
    {
        if (c.GetSelectedGun().GetAvailableBullets() > 0)
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
        if (_selectedChar != null)
        {
            if (_selectedChar.GetSelectedGun().GetAvailableBullets() <= 0 || _partsSelected == _selectedChar.GetSelectedGun().GetAvailableSelections())
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
            else if (_partsSelected < _selectedChar.GetSelectedGun().GetAvailableSelections())
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
    public void SetPlayerUI()
    {
        if (_selectedEnemy != null)
            _selectedChar.SetEnemy(_selectedEnemy);
        SetCharacterMovementButtons();

        ShowHUDSliders(_selectedChar, playerBodySlider, playerLeftArmSlider, playerRightArmSlider, playerLegsSlider);

        ShowPlayerHudText(playerBodyCurrHP, playerBodySlider, playerLeftArmCurrHP, playerLeftArmSlider, playerRightArmCurrHP, playerRightArmSlider, playerLegsCurrHP, playerLegsSlider);

        if (_selectedChar.CanAttack() && _selectedChar.HasEnemiesInRange() && _selectedEnemy != null)
            ActivateBodyPartsContainer();
        //else DeactivateBodyPartsContainer();

        playerHudContainer.SetActive(true);
        DeactivateEndTurnButton();
    }

    public void DeactivatePlayerHUD()
    {
        playerHudContainer.SetActive(false);
        DeactivateMoveButton();
    }

    public void SetEnemyUI()
    {
        if (_selectedChar != null)
        {
            if (_selectedEnemy.CanBeAttacked())
                ActivateBodyPartsContainer();
            else DeactivateBodyPartsContainer();
        }
        ShowHUDSliders(_selectedEnemy, enemyBodySlider, enemyLeftArmSlider, enemyRightArmSlider, enemyLegsSlider);
        ShowUnitHudText(enemyBodyCurrHP, enemyBodySlider, enemyLeftArmCurrHP, enemyLeftArmSlider, enemyRightArmCurrHP, enemyRightArmSlider, enemyLegsCurrHP, enemyLegsSlider);
        enemyHudContainer.SetActive(true);
    }



    public void DeactivateEnemyHUD()
    {
        DeactivateBodyPartsContainer();
        enemyHudContainer.SetActive(false);
    }
    void ShowPlayerHudText(TextMeshProUGUI bodyHpText, Slider bodySlider, TextMeshProUGUI lArmHpText, Slider lArmSlider, TextMeshProUGUI rArmHpText, Slider rArmSlider, TextMeshProUGUI legsHpText, Slider legsSlider)
    {
        ShowUnitHudText(bodyHpText, bodySlider, lArmHpText, lArmSlider, rArmHpText, rArmSlider, legsHpText, legsSlider);

        var gun = _selectedChar.GetSelectedGun();

        gunTypeText.text = gun.GetGunTypeString();

        var dmg = gun.GetBulletDamage().ToString();
        var b = gun.GetAvailableBullets();
        damageText.text = "DMG " + dmg + " x " + b + " hits";

        var r = gun.GetAttackRange().ToString();
        rangeText.text = "Range " + r;

        var cc = gun.GetCritChance().ToString();
        var cd = gun.GetCritMultiplier().ToString();

        critText.text = "Crit % " + cc + " | " + "Crit Dmg x" + cd;

        var h = gun.GetHitChance().ToString();
        hitChanceText.text = "Hit % " + h;
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

    public void UpdateBodyHUD(int value, bool isPlayer)
    {
        if (isPlayer)
        {
            playerBodySlider.value = value;
            playerBodyCurrHP.text = playerBodySlider.value.ToString();
        }
        else
        {
            enemyBodySlider.value = value;
            enemyBodyCurrHP.text = enemyBodySlider.value.ToString();
        }
        
    }

    public void UpdateLeftArmHUD(int value, bool isPlayer)
    {
        if (isPlayer)
        {
            playerLeftArmSlider.value = value;
            playerLeftArmCurrHP.text = playerLeftArmSlider.value.ToString();
        }
        else
        {
            enemyLeftArmSlider.value = value;
            enemyLeftArmCurrHP.text = enemyLeftArmSlider.value.ToString();
        }
        
    }

    public void UpdateRightArmHUD(int value, bool isPlayer)
    {
        if (isPlayer)
        {
            playerRightArmSlider.value = value;
            playerRightArmCurrHP.text = playerRightArmSlider.value.ToString();
        }
        else
        {
            enemyRightArmSlider.value = value;
            enemyRightArmCurrHP.text = enemyRightArmSlider.value.ToString();
        }
    }
    public void UpdateLegsHUD(int value, bool isPlayer)
    {
        if (isPlayer)
        {
            playerLegsSlider.value = value;
            playerLegsCurrHP.text = playerLegsSlider.value.ToString();
        }
        else
        {
            enemyLegsSlider.value = value;
            enemyLegsCurrHP.text = enemyLegsSlider.value.ToString();
        }
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

    #region Activators
    public void ActivateEndTurnButton()
    {
        buttonEndTurn.gameObject.SetActive(true);
    }

    public void DeactivateEndTurnButton()
    {
        buttonEndTurn.gameObject.SetActive(false);
    }

    public void ActivateBodyPartsContainer()
    {
        buttonBody.interactable = true;
        buttonBodyMinus.interactable = true;
        buttonBodyX.interactable = true;

        buttonLArm.interactable = true;
        buttonLArmMinus.interactable = true;
        buttonLArmX.interactable = true;

        buttonRArm.interactable = true;
        buttonRArmMinus.interactable = true;
        buttonRArmX.interactable = true;

        buttonLegs.interactable = true;
        buttonLegsMinus.interactable = true;
        buttonLegsX.interactable = true;
        bodyPartsButtonsContainer.SetActive(true);
    }

    public void DeactivateBodyPartsContainer()
    {
        bodyPartsButtonsContainer.SetActive(false);
    }
    #endregion
}
