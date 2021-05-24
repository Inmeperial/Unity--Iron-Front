using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonsUIManager : MonoBehaviour
{
    public LayerMask gridBlock;
    public GameObject moveContainer;
    public Button buttonMove;
    public Button buttonUndo;
    public Button buttonExecuteAttack;
    public Button buttonEndTurn;

    public KeyCode deselectKey;
    public KeyCode selectLGunKey;
    public KeyCode selectRGunKey;
    public KeyCode showWorldUIKey;
    public KeyCode toggleWorldUIKey;


    #region Buttons
    public GameObject bodyPartsButtonsContainer;
    public CustomButton buttonBody;
    private bool _buttonBodySelected;
    private int _bulletsForBody;
    public CustomButton buttonLArm;
    private bool _buttonLArmSelected;
    private int _bulletsForLArm;
    public CustomButton buttonRArm;
    private bool _buttonRArmSelected;
    private int _bulletsForRArm;
    public CustomButton buttonLegs;
    private bool _buttonLegsSelected;
    private int _bulletsForLegs;
    #endregion

    #region HUD
    //Player
    public GameObject playerHudContainer;
    public TextMeshProUGUI leftGunTypeText;
    public TextMeshProUGUI leftGunDamageText;
    public TextMeshProUGUI leftGunHitsText;
    public TextMeshProUGUI leftGunHitChanceText;
    public TextMeshProUGUI rightGunTypeText;
    public TextMeshProUGUI rightGunDamageText;
    public TextMeshProUGUI rightGunHitsText;
    public TextMeshProUGUI rightGunHitChanceText;
    public TextMeshProUGUI playerBodyCurrHp;
    public TextMeshProUGUI playerLeftArmCurrHp;
    public TextMeshProUGUI playerRightArmCurrHp;
    public TextMeshProUGUI playerLegsCurrHp;
    public GameObject leftWeaponCircle;
    public GameObject rightWeaponCircle;

    //Enemy
    public GameObject enemyHudContainer;
    public TextMeshProUGUI enemyBodyCurrHp;
    public TextMeshProUGUI bulletsForBodyText;
    public TextMeshProUGUI enemyLeftArmCurrHp;
    public TextMeshProUGUI bulletsForLArmText;
    public TextMeshProUGUI enemyRightArmCurrHp;
    public TextMeshProUGUI bulletsForRArmText;
    public TextMeshProUGUI enemyLegsCurrHp;
    public TextMeshProUGUI bulletsForLegsText;
    #endregion

    //OTHERS
    [SerializeField] private CharacterSelection _charSelection;
    [SerializeField] private Character _selectedChar;
    [SerializeField] private Character _selectedEnemy;
    private TurnManager _turnManager;
    private int _partsSelected;


    private bool bodyInsight;
    private bool legsInsight;
    private bool rArmInsigh;
    private bool lArmInsight;

    private bool _worldUIActive = false;
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
        gridBlock = _charSelection.gridBlockMask;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (((_selectedChar && _selectedChar.IsMoving() == false) || _selectedEnemy) &&
                (Input.GetKeyDown(deselectKey) || (Input.GetMouseButtonDown(0) && CheckIfTileNotSelected())))
                DeselectUnit();
        }

        if (Input.GetKeyDown(selectLGunKey))
            UnitSwapToLeftGun();

        if (Input.GetKeyDown(selectRGunKey))
            UnitSwapToRightGun();
        
        if (Input.GetKeyDown(showWorldUIKey))
            ShowAllWorldUI();
        
        if (Input.GetKeyUp(showWorldUIKey))
            HideAllWorldUI();
            
        if (Input.GetKeyDown(toggleWorldUIKey))
        {
            if(_worldUIActive)
                HideAllWorldUI();
            else ShowAllWorldUI();
        }
    }

    /// <summary>
    /// Returns false if tile is selected for movement or attack.
    /// Returns true if tile is not selected.
    /// </summary>
    bool CheckIfTileNotSelected()
    {
        var tileObj = MouseRay.GetTargetGameObject(gridBlock).GetComponent<Tile>();
        if (tileObj)
        {
            if (tileObj.inMoveRange || tileObj.inAttackRange)
                return false;
            return true;
        }

        return true;
    }
    #region ButtonsActions

   

    void SetCharacterMovementButtons()
    {
        if (_selectedChar.ThisUnitCanMove())
        {
            buttonMove.onClick.RemoveAllListeners();
            buttonMove.onClick.AddListener(_selectedChar.Move);
            buttonUndo.onClick.RemoveAllListeners();
            buttonUndo.onClick.AddListener(_selectedChar.pathCreator.UndoLastWaypoint);
            ActivateMoveContainer();
            DeactivateMoveButton();
        }
        else DeactivateMoveContainer();
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
                bulletsForBodyText.text = "x " + _bulletsForBody;
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
            bulletsForBodyText.text = "x " + _bulletsForBody;
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
            bulletsForBodyText.text = "x " + _bulletsForBody;
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
            var gun = _selectedChar.GetSelectedGun();
            if (_partsSelected <= gun.GetAvailableSelections())
            {
                if (_bulletsForLArm == 0)
                {
                    _partsSelected++;
                }
                _bulletsForLArm += gun.BulletsPerClick();
                bulletsForLArmText.text = "x " + _bulletsForLArm;

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
            bulletsForLArmText.text = "x " + _bulletsForLArm;
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
            bulletsForLArmText.text = "x " + _bulletsForLArm;
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
            var gun = _selectedChar.GetSelectedGun();
            if (_partsSelected <= gun.GetAvailableSelections())
            {
                if (_bulletsForRArm == 0)
                {
                    _partsSelected++;
                }
                _bulletsForRArm += gun.BulletsPerClick();
                bulletsForRArmText.text = "x " + _bulletsForRArm;
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
            bulletsForRArmText.text = "x " + _bulletsForRArm;
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
            bulletsForRArmText.text = "x " + _bulletsForRArm;
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
            var gun = _selectedChar.GetSelectedGun();
            if (_partsSelected <= gun.GetAvailableSelections())
            {
                if (_bulletsForLegs == 0)
                {
                    _partsSelected++;
                }
                _bulletsForLegs += gun.BulletsPerClick();
                bulletsForLegsText.text = "x " + _bulletsForLegs;
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
            bulletsForLegsText.text = "x " + _bulletsForLegs;
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
            bulletsForLegsText.text = "x " + _bulletsForLegs;
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
            var gun = _selectedChar.GetSelectedGun();
            _selectedChar.Shoot();
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
            //_charSelection.CantSelectEnemy();
            DeactivateMoveContainer();
            DeactivateBodyPartsContainer();
            buttonExecuteAttack.interactable = false;
        }
    }

    public void EndTurn()
    {
        if (_selectedChar == null || _selectedChar.IsMoving() == false)
        {
            _turnManager.EndTurn();
            DeactivateEnemyHUD();
            DeactivatePlayerHUD();
            DeactivateMoveContainer();
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
        if (_selectedChar != null && _selectedChar.LeftArmAlive())
        {
            BodyClear();
            LeftArmClear();
            RightArmClear();
            LegsClear();
            _selectedChar.SelectLeftGun();
            leftWeaponCircle.SetActive(true);
            rightWeaponCircle.SetActive(false);
            ShowPlayerHudText(playerBodyCurrHp, _selectedChar.GetBodyHP(), playerLeftArmCurrHp, _selectedChar.GetLeftArmHP(), playerRightArmCurrHp, _selectedChar.GetRightArmHP(), playerLegsCurrHp, _selectedChar.legs.GetLegsHP());

            if (_selectedChar.HasEnemiesInRange())
                ActivateBodyPartsContainer();
            else DeactivateBodyPartsContainer();
        }
        else Debug.Log("left arm broken");
    }

    public void UnitSwapToRightGun()
    {
        if (_selectedChar != null && _selectedChar.RightArmAlive())
        {
            BodyClear();
            LeftArmClear();
            RightArmClear();
            LegsClear();
            _selectedChar.SelectRightGun();
            rightWeaponCircle.SetActive(true);
            leftWeaponCircle.SetActive(false);
            ShowPlayerHudText(playerBodyCurrHp, _selectedChar.GetBodyHP(), playerLeftArmCurrHp, _selectedChar.GetLeftArmHP(), playerRightArmCurrHp, _selectedChar.GetRightArmHP(), playerLegsCurrHp, _selectedChar.legs.GetLegsHP());

            if (_selectedChar.HasEnemiesInRange())
                ActivateBodyPartsContainer();
            else DeactivateBodyPartsContainer();
        }
        else Debug.Log("right arm broken");
    }

    public void ShowAllWorldUI()
    {
        _worldUIActive = true;
        var units = _turnManager.GetEnemies(Teams.Team.Box).Concat(_turnManager.GetEnemies(Teams.Team.Capsule));
        foreach (var unit in units)
        {
            unit.ShowWorldUI();
        }
    }

    public void HideAllWorldUI()
    {
        _worldUIActive = false;
        var units = _turnManager.GetEnemies(Teams.Team.Box).Concat(_turnManager.GetEnemies(Teams.Team.Capsule));
        foreach (var unit in units)
        {
            unit.HideWorldUI();
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
                }

                if (_buttonLArmSelected == false)
                {
                    buttonLArm.interactable = false;
                }

                if (_buttonRArmSelected == false)
                {
                    buttonRArm.interactable = false;
                }

                if (_buttonLegsSelected == false)
                {
                    buttonLegs.interactable = false;
                }

                buttonExecuteAttack.interactable = true;
            }
            else if (_partsSelected < _selectedChar.GetSelectedGun().GetAvailableSelections())
            {
                if (!_buttonBodySelected && bodyInsight)
                {
                    buttonBody.interactable = true;
                }

                if (!_buttonLArmSelected && lArmInsight)
                {
                    buttonLArm.interactable = true;
                }

                if (!_buttonRArmSelected && rArmInsigh)
                {
                    buttonRArm.interactable = true;
                }

                if (!_buttonLegsSelected && legsInsight)
                {
                    buttonLegs.interactable = true;
                }
            }
        }
    }

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

        DeterminateButtonsActivation();
    }

    
    #endregion

    #region HUD Text
    public void SetPlayerUI()
    {
        SetCharacterMovementButtons();

        ShowPlayerHudText(playerBodyCurrHp, _selectedChar.GetBodyHP(), playerLeftArmCurrHp, _selectedChar.GetLeftArmHP(), playerRightArmCurrHp, _selectedChar.GetRightArmHP(), playerLegsCurrHp, _selectedChar.legs.GetLegsHP());

        if (_selectedChar.RightArmAlive())
        {
            rightWeaponCircle.SetActive(true);
            leftWeaponCircle.SetActive(false);
        }
        else if (_selectedChar.LeftArmAlive())
        {
            rightWeaponCircle.SetActive(false);
            leftWeaponCircle.SetActive(true);
        }
        else
        {
            rightWeaponCircle.SetActive(false);
            leftWeaponCircle.SetActive(false);
        }

        if (_selectedChar.CanAttack())
        {
            if (_selectedChar.HasEnemiesInRange() && _selectedEnemy != null)
            {
                ActivateBodyPartsContainer();
            }
                
        }

        playerHudContainer.SetActive(true);
    }

    public void DeactivatePlayerHUD()
    {
        playerHudContainer.SetActive(false);
        DeactivateMoveContainer();
    }

    public void SetEnemyUI()
    {
        if (_selectedChar != null)
        {
            if (_selectedEnemy.CanBeAttacked())
            {
                bulletsForBodyText.text = "x " + 0;
                bulletsForLegsText.text = "x " + 0;
                bulletsForLArmText.text = "x " + 0;
                bulletsForRArmText.text = "x " + 0;
                ActivateBodyPartsContainer();
            }
                
            else DeactivateBodyPartsContainer();
        }
        ShowUnitHudText(enemyBodyCurrHp, _selectedEnemy.GetBodyHP(), enemyLeftArmCurrHp, _selectedEnemy.GetLeftArmHP(), enemyRightArmCurrHp, _selectedEnemy.GetRightArmHP(), enemyLegsCurrHp, _selectedEnemy.legs.GetLegsHP());
        enemyHudContainer.SetActive(true);
    }



    public void DeactivateEnemyHUD()
    {
        DeactivateBodyPartsContainer();
        enemyHudContainer.SetActive(false);
    }
    void ShowPlayerHudText(TextMeshProUGUI bodyHpText, float bodyValue, TextMeshProUGUI lArmHpText, float lArmValue, TextMeshProUGUI rArmHpText, float rArmValue, TextMeshProUGUI legsHpText, float legsValue)
    {
        ShowUnitHudText(bodyHpText, bodyValue, lArmHpText, lArmValue, rArmHpText, rArmValue, legsHpText, legsValue);

        if (_selectedChar.LeftArmAlive())
        {
            var left = _selectedChar.GetLeftGun();
            leftGunTypeText.text = left.GetGunTypeString();

            var b = left.GetAvailableBullets().ToString();
            leftGunHitsText.text = b;

            var dmg = left.GetBulletDamage().ToString();
            leftGunDamageText.text = dmg;

            var h = left.GetHitChance().ToString();
            leftGunHitChanceText.text = h + "%";
        }
        else
        {
            
            leftGunTypeText.text = "No gun - Arm destroyed";
            leftGunDamageText.text = "";
            leftGunHitChanceText.text = "";
        }

        if (_selectedChar.RightArmAlive())
        {
            var right = _selectedChar.GetRightGun();

            rightGunTypeText.text = right.GetGunTypeString();

            var b = right.GetAvailableBullets().ToString();
            rightGunHitsText.text = b;

            var dmg = right.GetBulletDamage().ToString();
            rightGunDamageText.text = dmg;

            var h = right.GetHitChance().ToString();
            rightGunHitChanceText.text = h + "%";

        }
        else
        {
            
            rightGunTypeText.text = "No gun - Arm destroyed";
			rightGunDamageText.text = "";
			rightGunHitChanceText.text = "";
        }


    }

    void ShowUnitHudText(TextMeshProUGUI bodyHpText, float bodyValue, TextMeshProUGUI lArmHpText, float lArmValue, TextMeshProUGUI rArmHpText, float rArmValue, TextMeshProUGUI legsHpText, float legsValue)
    {
        bodyHpText.text = bodyValue.ToString() + " HP";
        lArmHpText.text = lArmValue.ToString() + " HP";
        rArmHpText.text = rArmValue.ToString() + " HP";
        legsHpText.text = legsValue.ToString() + " HP";
    }

    public void UpdateBodyHUD(int value, bool isPlayer)
    {
        if (isPlayer)
        {
            playerBodyCurrHp.text = value.ToString();
        }
        else
        {
            enemyBodyCurrHp.text = value.ToString();
        }
        
    }

    public void UpdateLeftArmHUD(int value, bool isPlayer)
    {
        if (isPlayer)
        {
            playerLeftArmCurrHp.text = value.ToString();
        }
        else
        {
            enemyLeftArmCurrHp.text = value.ToString();
        }
        
    }

    public void UpdateRightArmHUD(int value, bool isPlayer)
    {
        if (isPlayer)
        {
            playerRightArmCurrHp.text = value.ToString();
        }
        else
        {
            enemyRightArmCurrHp.text = value.ToString();
        }
    }
    public void UpdateLegsHUD(int value, bool isPlayer)
    {
        if (isPlayer)
        {
            playerLegsCurrHp.text = value.ToString();
        }
        else
        {
            enemyLegsCurrHp.text = value.ToString();
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
        if (_selectedEnemy != null)
        {
            if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetBodyPosition(), "Body") && _selectedEnemy.GetBodyHP() > 0)
            {
                bodyInsight = true;
                buttonBody.interactable = true;
            }
            else
            {
                bodyInsight = false;
                buttonBody.interactable = false;
            }

            if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetLArmPosition(), "LArm") && _selectedEnemy.GetLeftArmHP() > 0)
            {
                lArmInsight = true;
                buttonLArm.interactable = true;
            }
            else
            {
                lArmInsight = false;
                buttonLArm.interactable = false;
            }

            if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetRArmPosition(), "RArm") && _selectedEnemy.GetRightArmHP() > 0)
            {
                rArmInsigh = true;
                buttonRArm.interactable = true;
            }
            else
            {
                rArmInsigh = false;
                buttonRArm.interactable = false;
            }


            if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetLegsPosition(), "Legs") &&_selectedEnemy.legs.GetLegsHP() > 0)
            {
                legsInsight = true;
                buttonLegs.interactable = true;
            }
            else
            {
                legsInsight = false;
                buttonLegs.interactable = false;
            }
            bodyPartsButtonsContainer.SetActive(true);
        }
    }

    public void DeactivateBodyPartsContainer()
    {
        bodyPartsButtonsContainer.SetActive(false);
    }

    public void ActivateUndo()
    {
        buttonUndo.interactable = true;
    }

    public void DeactivateUndo()
    {
        buttonUndo.interactable = false;
    }

    public void ActivateMoveContainer()
    {
        moveContainer.SetActive(true);
    }

    public void DeactivateMoveContainer()
    {
        moveContainer.SetActive(false);
    }
    public void ActivateMoveButton()
    {
        buttonMove.interactable = true;
    }

    public void DeactivateMoveButton()
    {
        buttonMove.interactable = false;
    }

    public void ActivateExecuteAttackButton()
    {
        buttonExecuteAttack.interactable = true;
    }

    public void DeactivateExecuteAttackButton()
    {
        buttonExecuteAttack.interactable = false;
    }
    #endregion
}
