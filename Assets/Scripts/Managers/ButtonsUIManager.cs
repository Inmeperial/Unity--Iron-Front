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
            if (_partsSelected <= _selectedChar.selectedGun.AvailableSelections())
            {
                Debug.Log("entro al body");
                if (_bulletsForBody == 0)
                {
                    _partsSelected++;
                }
                _bulletsForBody++;
                buttonBody.GetComponent<BodyPartSelection>().GenerateCounter();
                _selectedChar.selectedGun.ReduceAvailableBullets(1);
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
            _bulletsForBody--;
            _selectedChar.selectedGun.IncreaseAvailableBullets(1);
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
            _selectedChar.selectedGun.IncreaseAvailableBullets(_bulletsForBody);
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
            if (_partsSelected <= _selectedChar.selectedGun.AvailableSelections())
            {
                if (_bulletsForLArm == 0)
                {
                    _partsSelected++;
                }
                _bulletsForLArm++;
                buttonLArm.GetComponent<BodyPartSelection>().GenerateCounter();
                _selectedChar.selectedGun.ReduceAvailableBullets(1);
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
            _bulletsForLArm--;
            _selectedChar.selectedGun.IncreaseAvailableBullets(1);
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
            _selectedChar.selectedGun.IncreaseAvailableBullets(_bulletsForLArm);
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
            if (_partsSelected <= _selectedChar.selectedGun.AvailableSelections())
            {
                if (_bulletsForRArm == 0)
                {
                    _partsSelected++;
                }
                _bulletsForRArm++;
                buttonRArm.GetComponent<BodyPartSelection>().GenerateCounter();
                _selectedChar.selectedGun.ReduceAvailableBullets(1);
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
            _bulletsForRArm--;
            _selectedChar.selectedGun.IncreaseAvailableBullets(1);
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
            _selectedChar.selectedGun.IncreaseAvailableBullets(_bulletsForRArm);
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
            if (_partsSelected <= _selectedChar.selectedGun.AvailableSelections())
            {
                if (_bulletsForLegs == 0)
                {
                    _partsSelected++;
                }
                _bulletsForLegs++;
                buttonLegs.GetComponent<BodyPartSelection>().GenerateCounter();
                _selectedChar.selectedGun.ReduceAvailableBullets(1);
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
            _bulletsForLegs--;
            _selectedChar.selectedGun.IncreaseAvailableBullets(1);
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
            _selectedChar.selectedGun.IncreaseAvailableBullets(_bulletsForLegs);
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
        var b = _bulletsForBody + _bulletsForLArm + _bulletsForLegs + _bulletsForRArm;
        if (_partsSelected == _selectedChar.selectedGun.AvailableSelections() /*|| b >= _selectedChar.selectedGun.GetAvailableBullets()*/)
        {
            if (!_buttonBodySelected)
                buttonBody.interactable = false;

            if (!_buttonLArmSelected)
                buttonLArm.interactable = false;

            if (!_buttonRArmSelected)
                buttonRArm.interactable = false;

            if (!_buttonLegsSelected)
                buttonLegs.interactable = false;
        }

        if (_partsSelected < _selectedChar.selectedGun.AvailableSelections() /*|| b < _selectedChar.selectedGun.GetAvailableBullets()*/)
        {
            if (!_buttonBodySelected)
                buttonBody.interactable = true;

            if (!_buttonLArmSelected)
                buttonLArm.interactable = true;

            if (!_buttonRArmSelected)
                buttonRArm.interactable = true;

            if (!_buttonLegsSelected)
                buttonLegs.interactable = true;
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
                enemyBodyCurrHP.text = _selectedEnemy.GetBodyHP().ToString();
            }


            if (_bulletsForLArm > 0)
            {
                _selectedEnemy.AttackLeftArm(_bulletsForLArm, _selectedChar.selectedGun.GetBulletDamage());
                enemyLeftArmSlider.value = _selectedEnemy.GetLeftArmHP();
                enemyLeftArmCurrHP.text = _selectedEnemy.GetLeftArmHP().ToString();
            }

            if (_bulletsForRArm > 0)
            {
                _selectedEnemy.AttackRightArm(_bulletsForRArm, _selectedChar.selectedGun.GetBulletDamage());
                enemyRightArmSlider.value = _selectedEnemy.GetRightArmHP();
                enemyRightArmCurrHP.text = _selectedEnemy.GetRightArmHP().ToString();
            }
        }

        if (_bulletsForLegs > 0)
        {
            _selectedEnemy.AttackLegs(_bulletsForLegs, _selectedChar.selectedGun.GetBulletDamage());
            enemyLegsSlider.value = _selectedEnemy.GetLegsHP();
            enemyLegsCurrHP.text = _selectedEnemy.GetLegsHP().ToString();
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
        DeactivateEnemyHUD();
        DeactivatePlayerHUD();
        DeactivateMoveButton();
    }


    #endregion

    #region HUD Text
    void ShowPlayerHUDSliders()
    {
        playerBodySlider.maxValue = _selectedChar.GetBodyMaxHP();
        playerBodySlider.value = _selectedChar.GetBodyHP();

        playerLeftArmSlider.maxValue = _selectedChar.GetLeftArmMaxHP();
        playerLeftArmSlider.value = _selectedChar.GetLeftArmHP();

        playerRightArmSlider.maxValue = _selectedChar.GetRightArmMaxHP();
        playerRightArmSlider.value = _selectedChar.GetRightArmHP();

        playerLegsSlider.maxValue = _selectedChar.GetLegsMaxHP();
        playerLegsSlider.value = _selectedChar.GetLegsHP();
    }

    void ShowPlayerUnitHudText()
    {
        playerBodyCurrHP.text = _selectedChar.GetBodyHP().ToString();
        playerLeftArmCurrHP.text = _selectedChar.GetLeftArmHP().ToString();
        playerRightArmCurrHP.text = _selectedChar.GetRightArmHP().ToString();
        playerLegsCurrHP.text = _selectedChar.GetLegsHP().ToString();

        var dmg = _selectedChar.selectedGun.GetBulletDamage().ToString();
        var b = _selectedChar.selectedGun.GetAvailableBullets();
        damageText.text = "DMG " + dmg + " x " + b + " hits";

        var r = _selectedChar.selectedGun.GetAttackRange().ToString();
        rangeText.text = "Range " + r;
    }

    void ShowEnemyHUDSliders()
    {
        enemyBodySlider.maxValue = _selectedEnemy.GetBodyMaxHP();
        enemyBodySlider.value = _selectedEnemy.GetBodyHP();

        enemyLeftArmSlider.maxValue = _selectedEnemy.GetLeftArmMaxHP();
        enemyLeftArmSlider.value = _selectedEnemy.GetLeftArmHP();

        enemyRightArmSlider.maxValue = _selectedEnemy.GetRightArmMaxHP();
        enemyRightArmSlider.value = _selectedEnemy.GetRightArmHP();

        enemyLegsSlider.maxValue = _selectedEnemy.GetLegsMaxHP();
        enemyLegsSlider.value = _selectedEnemy.GetLegsHP();
    }

    void ShowEnemyHUDText()
    {
        enemyBodyCurrHP.text = _selectedEnemy.GetBodyHP().ToString();
        enemyLeftArmCurrHP.text = _selectedEnemy.GetLeftArmHP().ToString();
        enemyRightArmCurrHP.text = _selectedEnemy.GetRightArmHP().ToString();
        enemyLegsCurrHP.text = _selectedEnemy.GetLegsHP().ToString();
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
