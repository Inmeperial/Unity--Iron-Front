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
    private void Update()
    {
        Debug.Log("Bullets for left arm: " + _bulletsForLArm);
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
        ShowPlayerHUDSliders();
        ShowPlayerUnitHudText();
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
        ShowEnemyHUDSliders();
        ShowEnemyHUDText();
        
        enemyHudContainer.SetActive(true);
    }

    public void DeactivateEnemyHUD()
    {
        bodyPartsButtonsContainer.SetActive(false);
        enemyHudContainer.SetActive(false);
        buttonExecuteAttack.enabled = false;
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

                
                CreateBulletInUI(buttonBody, gun, _bulletsForBody);

                gun.ReduceAvailableBullets();

                buttonExecuteAttack.interactable = true;
                _buttonBodySelected = true;
                DeterminateButtonsActivation();
            }
        }

    }

    void BulletsReduction(Gun gun)
    {
        //switch (gun.GetGunType())
        //{
        //    case Gun.GunType.AssaultRifle:
        //        gun.ReduceAvailableBullets(1);
        //        break;

        //    case Gun.GunType.Rifle:
        //        gun.ReduceAvailableBullets(gun.BulletsPerClick());
        //        break;

        //    case Gun.GunType.Shotgun:
        //        gun.ReduceAvailableBullets(0);
        //        break;

        //    case Gun.GunType.Melee:
        //        gun.ReduceAvailableBullets(0);
        //        break;
        //}
    }

    void CreateBulletInUI(Button button, Gun gun, int quantity)
    {
        switch (gun.GetGunType())
        {
            case Gun.GunType.AssaultRifle:
                button.GetComponent<BodyPartSelection>().GenerateCounter();
                break;

            case Gun.GunType.Rifle:
                for (int i = 0; i < quantity; i++)
                {
                    button.GetComponent<BodyPartSelection>().GenerateCounter();
                }
                break;

            case Gun.GunType.Shotgun:
                button.GetComponent<BodyPartSelection>().GenerateCounter();
                break;

            case Gun.GunType.Melee:
                button.GetComponent<BodyPartSelection>().GenerateCounter();
                break;
        }
        
    }

    void DeleteBulletInUI(Button button, Gun gun, int quantity)
    {
        switch (gun.GetGunType())
        {
            case Gun.GunType.AssaultRifle:
                button.GetComponent<BodyPartSelection>().GenerateCounter();
                break;

            case Gun.GunType.Rifle:
                for (int i = 0; i < quantity; i++)
                {
                    button.GetComponent<BodyPartSelection>().RemoveCounter();
                }
                break;

            case Gun.GunType.Shotgun:
                button.GetComponent<BodyPartSelection>().GenerateCounter();
                break;

            case Gun.GunType.Melee:
                button.GetComponent<BodyPartSelection>().GenerateCounter();
                break;
        }

    }

    public void BodyMinus()
    {
        if (_bulletsForBody > 0)
        {
            var gun = _selectedChar.selectedGun;
            gun.IncreaseAvailableBullets();
            _bulletsForBody = _bulletsForBody > 0 ? (_bulletsForBody - gun.BulletsPerClick()) : 0;
            DeleteBulletInUI(buttonBody, gun, gun.BulletsPerClick());
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

                CreateBulletInUI(buttonLArm, gun, _bulletsForLArm);

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
            DeleteBulletInUI(buttonLArm, gun, gun.BulletsPerClick());
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
                CreateBulletInUI(buttonRArm, gun, _bulletsForRArm);
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
            DeleteBulletInUI(buttonRArm, gun, gun.BulletsPerClick());
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
                CreateBulletInUI(buttonLegs, gun, _bulletsForLegs);
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
            DeleteBulletInUI(buttonLegs, gun, gun.BulletsPerClick());
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

    bool CharacterHasBullets(Character c)
    {
        if (c.selectedGun.GetAvailableBullets() > 0)
            return true;
        else return false;
    }

    void CheckIfCanExecuteAttack()
    {
        if (_bulletsForBody == 0 && _bulletsForLArm == 0 && _bulletsForLegs == 0 && _bulletsForRArm == 0)
            buttonExecuteAttack.interactable = false;
    }

    void DeterminateButtonsActivation()
    {
        Debug.Log("bullets: " + _selectedChar.selectedGun.GetAvailableBullets());
        if (_selectedChar.selectedGun.GetAvailableBullets() <= 0 || _partsSelected == _selectedChar.selectedGun.GetAvailableSelections())
        {
            Debug.Log("entro al if");

            Debug.Log(_buttonBodySelected);
            Debug.Log(_buttonLArmSelected);
            Debug.Log(_buttonRArmSelected);
            Debug.Log(_buttonLegsSelected);
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

    public void ExecuteAttack()
    {
        if (_selectedEnemy != null)
        {
            if (_bulletsForBody > 0)
            {
                _selectedEnemy.AttackBody(_bulletsForBody, _selectedChar.selectedGun.GetBulletDamage());
                enemyBodySlider.value = _selectedEnemy.GetBodyHP();
                enemyBodyCurrHP.text = enemyBodySlider.value.ToString();
                _selectedChar.DeactivateAttack();
            }


            if (_bulletsForLArm > 0)
            {
                _selectedEnemy.AttackLeftArm(_bulletsForLArm, _selectedChar.selectedGun.GetBulletDamage());
                enemyLeftArmSlider.value = _selectedEnemy.GetLeftArmHP();
                enemyLeftArmCurrHP.text = enemyLeftArmSlider.value.ToString();
                _selectedChar.DeactivateAttack();
            }

            if (_bulletsForRArm > 0)
            {
                _selectedEnemy.AttackRightArm(_bulletsForRArm, _selectedChar.selectedGun.GetBulletDamage());
                enemyRightArmSlider.value = _selectedEnemy.GetRightArmHP();
                enemyRightArmCurrHP.text = enemyRightArmSlider.value.ToString();
                _selectedChar.DeactivateAttack();
            }

            if (_bulletsForLegs > 0)
            {
                _selectedEnemy.AttackLegs(_bulletsForLegs, _selectedChar.selectedGun.GetBulletDamage());
                enemyLegsSlider.value = _selectedEnemy.GetLegsHP();
                enemyLegsCurrHP.text = enemyLegsSlider.value.ToString();
                _selectedChar.DeactivateAttack();
            }
        }

        

        if (_selectedChar.CanAttack() == false)
        {
            bodyPartsButtonsContainer.SetActive(false);
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
        DeactivateEnemyHUD();
        DeactivatePlayerHUD();
        DeactivateMoveButton();
        ResetBodyParts();
    }

    #endregion

    void ResetBodyParts()
    {
        _buttonBodySelected = false;
        _bulletsForBody = 0;

        _buttonLArmSelected = false;
        _bulletsForLArm = 0;

        _buttonRArmSelected = false;
        _bulletsForRArm = 0;

        _buttonLegsSelected = false;
        _bulletsForLegs = 0;

        _partsSelected = 0;
    }

    #region HUD Text
    void ShowPlayerHUDSliders()
    {
        playerBodySlider.maxValue = _selectedChar.GetBodyMaxHP();
        playerBodySlider.value = _selectedChar.GetBodyHP() > 0 ? _selectedChar.GetBodyHP() : 0;

        playerLeftArmSlider.maxValue = _selectedChar.GetLeftArmMaxHP();
        playerLeftArmSlider.value = _selectedChar.GetLeftArmHP() > 0 ? _selectedChar.GetLeftArmHP() : 0;

        playerRightArmSlider.maxValue = _selectedChar.GetRightArmMaxHP();
        playerRightArmSlider.value = _selectedChar.GetRightArmHP() > 0 ? _selectedChar.GetRightArmHP() : 0;

        playerLegsSlider.maxValue = _selectedChar.GetLegsMaxHP();
        playerLegsSlider.value = _selectedChar.GetLegsHP() > 0 ? _selectedChar.GetLegsHP() : 0;
    }

    void ShowPlayerUnitHudText()
    {
        playerBodyCurrHP.text = playerBodySlider.value.ToString();
        playerLeftArmCurrHP.text = playerLeftArmSlider.value.ToString();
        playerRightArmCurrHP.text = playerRightArmSlider.value.ToString();
        playerLegsCurrHP.text = playerLegsSlider.value.ToString();

        var dmg = _selectedChar.selectedGun.GetBulletDamage().ToString();
        var b = _selectedChar.selectedGun.GetAvailableBullets();
        damageText.text = "DMG " + dmg + " x " + b + " hits";

        var r = _selectedChar.selectedGun.GetAttackRange().ToString();
        rangeText.text = "Range " + r;
    }

    void ShowEnemyHUDSliders()
    {
        enemyBodySlider.maxValue = _selectedEnemy.GetBodyMaxHP();
        enemyBodySlider.value = _selectedEnemy.GetBodyHP() > 0 ? _selectedEnemy.GetBodyHP() : 0;

        enemyLeftArmSlider.maxValue = _selectedEnemy.GetLeftArmMaxHP();
        enemyLeftArmSlider.value = _selectedEnemy.GetLeftArmHP() > 0 ? _selectedEnemy.GetLeftArmHP() : 0;

        enemyRightArmSlider.maxValue = _selectedEnemy.GetRightArmMaxHP();
        enemyRightArmSlider.value = _selectedEnemy.GetRightArmHP() > 0 ? _selectedEnemy.GetRightArmHP() : 0;

        enemyLegsSlider.maxValue = _selectedEnemy.GetLegsMaxHP();
        enemyLegsSlider.value = _selectedEnemy.GetLegsHP() > 0 ? _selectedEnemy.GetLegsHP() : 0;
    }

    void ShowEnemyHUDText()
    {
        enemyBodyCurrHP.text = enemyBodySlider.value.ToString();
        enemyLeftArmCurrHP.text = enemyLeftArmSlider.value.ToString();
        enemyRightArmCurrHP.text = enemyRightArmSlider.value.ToString();
        enemyLegsCurrHP.text = enemyLegsSlider.value.ToString();
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
