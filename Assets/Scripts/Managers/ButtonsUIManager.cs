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
    public Button buttonExecuteAttack;
    public float attackDelay;
    public Button buttonEndTurn;

    public KeyCode deselectKey;
    public KeyCode selectLGunKey;
    public KeyCode selectRGunKey;
    public KeyCode showWorldUIKey;
    public KeyCode toggleWorldUIKey;

    #region Buttons
    private bool _buttonBodySelected;
    private int _bulletsForBody;
    private bool _buttonLArmSelected;
    private int _bulletsForLArm;
    private bool _buttonRArmSelected;
    private int _bulletsForRArm;
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
    public Slider playerBodySlider;
    public TextMeshProUGUI playerLeftArmCurrHp;
    public Slider playerLeftArmSlider;
    public TextMeshProUGUI playerRightArmCurrHp;
    public Slider playerRightArmSlider;
    public TextMeshProUGUI playerLegsCurrHp;
    public Slider playerLegsSlider;
    public GameObject leftWeaponCircle;
    public GameObject rightWeaponCircle;

    //Weapon Attack
    public GameObject attackHudContainer;
    public TextMeshProUGUI attackWeaponNameText;
    public TextMeshProUGUI attackWeaponHitsText;
    public TextMeshProUGUI attackWeaponDamageText;
    public TextMeshProUGUI attackWeaponHitChanceText;
    public GameObject hitContainer;
    public GameObject hitImagePrefab;
    private List<GameObject> _hitImagesCreated = new List<GameObject>();
    
    //Enemy
    // public TextMeshProUGUI enemyBodyCurrHp;
    // public TextMeshProUGUI bulletsForBodyText;
    // public TextMeshProUGUI enemyLeftArmCurrHp;
    // public TextMeshProUGUI bulletsForLArmText;
    // public TextMeshProUGUI enemyRightArmCurrHp;
    // public TextMeshProUGUI bulletsForRArmText;
    // public TextMeshProUGUI bulletsForLegsText;
    #endregion

    //OTHERS
    [SerializeField] private CharacterSelection _charSelection;
    [SerializeField] private Character _selectedChar;
    [SerializeField] private Character _selectedEnemy;
    private TurnManager _turnManager;
    private int _partsSelected;
    private SoundsMenu _soundsMenuManager;
    private EffectsController _effectsController;

    private bool _bodyInsight;
    private bool _legsInsight;
    private bool _rArmInsight;
    private bool _lArmInsight;

    private bool _worldUIActive = false;
    private void Start()
    {
        _effectsController = FindObjectOfType<EffectsController>();
        playerHudContainer.SetActive(false);
        _buttonBodySelected = false;
        _buttonLArmSelected = false;
        _buttonRArmSelected = false;
        _buttonLegsSelected = false;

        _charSelection = FindObjectOfType<CharacterSelection>();
        _soundsMenuManager = FindObjectOfType<SoundsMenu>();
        _turnManager = FindObjectOfType<TurnManager>();
        gridBlock = _charSelection.gridBlockMask;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (((_selectedChar && _selectedChar.IsMoving() == false) || _selectedEnemy) &&
                (Input.GetKeyDown(deselectKey)))
                DeselectActions();

            if ((_selectedChar && _selectedChar.IsMoving() == false) && _selectedChar.GetPath().Count > 0 &&
                Input.GetMouseButtonDown(1))
            {
                _selectedChar.pathCreator.UndoLastWaypoint();
            }
                
        }

        if (_selectedChar || (_selectedChar && _selectedEnemy && _selectedEnemy.IsSelectedForAttack() == false))
        {
            if (Input.GetKeyDown(selectLGunKey))
                UnitSwapToLeftGun();

            if (Input.GetKeyDown(selectRGunKey))
                UnitSwapToRightGun();
        }
        
        
        if (_selectedChar && _selectedEnemy) return;
        
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
    #region ButtonsActions

    
    //Se ejecuta cuando se hace click izquierdo en el boton de body
    public void BodySelection()
    {
        if (!CharacterHasBullets(_selectedChar)) return;
        
        var gun = _selectedChar.GetSelectedGun();
        
        if (_partsSelected > gun.GetAvailableSelections()) return;
        
        if (_bulletsForBody == 0)
        {
            _partsSelected++;
        }
        _bulletsForBody += gun.GetBulletsPerClick();
        DestroyImage(gun.GetBulletsPerClick());
        gun.ReduceAvailableBullets();
        _selectedEnemy.GetMyUI().SetBodyCount(_bulletsForBody);
        buttonExecuteAttack.interactable = true;
        buttonExecuteAttack.gameObject.SetActive(true);
        _buttonBodySelected = true;
        DeterminateButtonsActivation();

    }

    //Se ejecuta cuando se hace click derecho en el boton de body
    public void BodyMinus()
    {
        if (_bulletsForBody <= 0) return;
        
        var gun = _selectedChar.GetSelectedGun();
        gun.IncreaseAvailableBullets();
        _bulletsForBody = _bulletsForBody > 0 ? (_bulletsForBody - gun.GetBulletsPerClick()) : 0;
        _selectedEnemy.GetMyUI().SetBodyCount(_bulletsForBody);
        CheckIfCanExecuteAttack();
        CreateImage(gun.GetBulletsPerClick());
        if (_bulletsForBody == 0)
        {
            if (_partsSelected > 0)
                _partsSelected--;
            _buttonBodySelected = false;
        }
        DeterminateButtonsActivation();
    }
    
    
    //Se ejecuta al cambiar de arma
    private void BodyClear()
    {
        if (_bulletsForBody <= 0) return;
        
        var gun = _selectedChar.GetSelectedGun();
        gun.IncreaseAvailableBullets(_bulletsForBody);
        DestroyImage(gun.GetMaxBullets());
        _hitImagesCreated.Clear();
        _bulletsForBody = 0;
        _buttonBodySelected = false;
        if (_selectedEnemy)
            _selectedEnemy.GetMyUI().SetBodyCount(_bulletsForBody);
        if (_partsSelected > 0)
            _partsSelected--;
        CheckIfCanExecuteAttack();
        DeterminateButtonsActivation();
    }

    //Se ejecuta cuando se hace click izquierdo en el boton de left arm
    public void LeftArmSelection()
    {
        if (!CharacterHasBullets(_selectedChar)) return;
        
        var gun = _selectedChar.GetSelectedGun();
        
        if (_partsSelected > gun.GetAvailableSelections()) return;
        
        if (_bulletsForLArm == 0)
        {
            _partsSelected++;
        }
        _bulletsForLArm += gun.GetBulletsPerClick();
        DestroyImage(gun.GetBulletsPerClick());
        _selectedEnemy.GetMyUI().SetLeftArmCount(_bulletsForLArm);
        gun.ReduceAvailableBullets();

        buttonExecuteAttack.interactable = true;
        buttonExecuteAttack.gameObject.SetActive(true);
        _buttonLArmSelected = true;
        DeterminateButtonsActivation();
    }
    
    //Se ejecuta cuando se hace click derecho en el boton de left arm
    public void LeftArmMinus()
    {
        if (_bulletsForLArm <= 0) return;
        
        var gun = _selectedChar.GetSelectedGun();
        gun.IncreaseAvailableBullets();
        _bulletsForLArm = _bulletsForLArm > 0 ? (_bulletsForLArm - gun.GetBulletsPerClick()) : 0;
        _selectedEnemy.GetMyUI().SetLeftArmCount(_bulletsForLArm);
        CheckIfCanExecuteAttack();
        CreateImage(gun.GetBulletsPerClick());
        if (_bulletsForLArm == 0)
        {
            if (_partsSelected > 0)
                _partsSelected--;
            _buttonLArmSelected = false;
        }
        DeterminateButtonsActivation();
    }
    
    //Se ejecuta al cambiar de arma
    private void LeftArmClear()
    {
        if (_bulletsForLArm <= 0) return;
        
        var gun = _selectedChar.GetSelectedGun();
        gun.IncreaseAvailableBullets(_bulletsForLArm);
        DestroyImage(gun.GetMaxBullets());
        _bulletsForLArm = 0;
        if (_selectedEnemy)
            _selectedEnemy.GetMyUI().SetLeftArmCount(_bulletsForLArm);
        _buttonLArmSelected = false;
        if (_partsSelected > 0)
            _partsSelected--;
        CheckIfCanExecuteAttack();
        DeterminateButtonsActivation();
    }
    
    
    //Se ejecuta cuando se hace click izquierdo en el boton de right arm
    public void RightArmSelection()
    {
        if (!CharacterHasBullets(_selectedChar)) return;
        
        var gun = _selectedChar.GetSelectedGun();

        if (_partsSelected > gun.GetAvailableSelections()) return;
        
        if (_bulletsForRArm == 0)
        {
            _partsSelected++;
        }
        _bulletsForRArm += gun.GetBulletsPerClick();
        DestroyImage(gun.GetBulletsPerClick());
        _selectedEnemy.GetMyUI().SetRightArmCount(_bulletsForRArm);
        gun.ReduceAvailableBullets();
        buttonExecuteAttack.interactable = true;
        buttonExecuteAttack.gameObject.SetActive(true);
        _buttonRArmSelected = true;
        DeterminateButtonsActivation();
    }

    //Se ejecuta cuando se hace click derecho en el boton de right arm
    public void RightArmMinus()
    {
        if (_bulletsForRArm <= 0) return;
        
        var gun = _selectedChar.GetSelectedGun();
        gun.IncreaseAvailableBullets();
        _bulletsForRArm = _bulletsForRArm > 0 ? (_bulletsForRArm - gun.GetBulletsPerClick()) : 0;
        _selectedEnemy.GetMyUI().SetRightArmCount(_bulletsForRArm);
        CreateImage(gun.GetBulletsPerClick());
        CheckIfCanExecuteAttack();
        if (_bulletsForRArm == 0)
        {
            if (_partsSelected > 0)
                _partsSelected--;
            _buttonRArmSelected = false;
        }
        DeterminateButtonsActivation();
    }

    //Se ejecuta al cambiar de arma
    private void RightArmClear()
    {
        if (_bulletsForRArm <= 0) return;
        
        var gun = _selectedChar.GetSelectedGun();
        gun.IncreaseAvailableBullets(_bulletsForRArm);
        DestroyImage(gun.GetMaxBullets());
        _bulletsForRArm = 0;
        if (_selectedEnemy)
            _selectedEnemy.GetMyUI().SetRightArmCount(_bulletsForRArm);

        _buttonRArmSelected = false;
        if (_partsSelected > 0)
            _partsSelected--;
        CheckIfCanExecuteAttack();
        DeterminateButtonsActivation();
    }

    //Se ejecuta cuando se hace click izquierdo en el boton de legs
    public void LegsSelection()
    {
        if (!CharacterHasBullets(_selectedChar)) return;
        
        var gun = _selectedChar.GetSelectedGun();

        if (_partsSelected > gun.GetAvailableSelections()) return;
        
        if (_bulletsForLegs == 0)
        {
            _partsSelected++;
        }
        _bulletsForLegs += gun.GetBulletsPerClick();
        DestroyImage(gun.GetBulletsPerClick());
        _selectedEnemy.GetMyUI().SetLegsCount(_bulletsForLegs);

        gun.ReduceAvailableBullets();
        buttonExecuteAttack.interactable = true;
        buttonExecuteAttack.gameObject.SetActive(true);
        _buttonLegsSelected = true;
        DeterminateButtonsActivation();
    }
    
    //Se ejecuta cuando se hace click derecho en el boton de legs
    public void LegsMinus()
    {
        if (_bulletsForLegs <= 0) return;
        
        var gun = _selectedChar.GetSelectedGun();
        gun.IncreaseAvailableBullets();
        _bulletsForLegs = _bulletsForLegs > 0 ? (_bulletsForLegs - gun.GetBulletsPerClick()) : 0;
        _selectedEnemy.GetMyUI().SetLegsCount(_bulletsForLegs);
        CreateImage(gun.GetBulletsPerClick());
        CheckIfCanExecuteAttack();
        if (_bulletsForLegs == 0)
        {
            if (_partsSelected > 0)
                _partsSelected--;
            _buttonLegsSelected = false;
        }
        DeterminateButtonsActivation();
    }

    //Se ejecuta al cambiar de arma
    private void LegsClear()
    {
        if (_bulletsForLegs <= 0) return;
        
        var gun = _selectedChar.GetSelectedGun();
        gun.IncreaseAvailableBullets(_bulletsForLegs);
        DestroyImage(gun.GetMaxBullets());
        _bulletsForLegs = 0;
        if (_selectedEnemy)
            _selectedEnemy.GetMyUI().SetLegsCount(_bulletsForLegs);
        _buttonLegsSelected = false;
        if (_partsSelected > 0)
            _partsSelected--;
        CheckIfCanExecuteAttack();
        DeterminateButtonsActivation();
    }
    
    
    /// <summary>
    /// Executed when left-clicking Fire button.
    /// </summary>
    public void ExecuteAttack()
    {
        if (!_selectedEnemy) return;
        
        buttonEndTurn.gameObject.SetActive(false);
        var cam = FindObjectOfType<CloseUpCamera>();
        var ui = _selectedEnemy.GetMyUI();
        ui.SetLimits(_selectedEnemy.body.GetMaxHp(), _selectedEnemy.rightArm.GetCurrentHp(), _selectedEnemy.leftArm.GetCurrentHp(), _selectedEnemy.legs.GetMaxHp());
        buttonExecuteAttack.interactable = false;
        buttonExecuteAttack.gameObject.SetActive(false);
        _selectedChar.ResetInRangeLists();
        DeactivateBodyPartsContainer();
        attackHudContainer.SetActive(false);
        _selectedChar.bodyRenderContainer.SetActive(true);
        if (_selectedChar.gunsOffOnCloseUp)
        {
            _selectedChar.GetLeftGun().ModelsOn();
            _selectedChar.GetRightGun().ModelsOn();
        }
        
        var units = _turnManager.GetAllUnits();
        foreach (var u in units)
        {
            u.gameObject.SetActive(true);
        }
        cam.MoveCameraToParent(cam.transform.parent.position, _selectedEnemy.transform.position, Attack, attackDelay);
    }

    /// <summary>
    /// Executed when CloseUp Camera returns to it's original position.
    /// </summary>
    private void Attack()
    {
        var gun = _selectedChar.GetSelectedGun();
        _selectedChar.Shoot();
        _selectedEnemy.SetHurtAnimation();
        if (_bulletsForBody > 0)
        {
            var d = gun.DamageCalculation(_bulletsForBody);
            _selectedEnemy.body.TakeDamageBody(d);
            _bulletsForBody = 0;
            
                
            if (gun.AbilityUsed() == false)
            {
                gun.Ability();
            }
            _selectedChar.DeactivateAttack();
            
            if (_selectedEnemy.body.GetCurrentHp() <= 0 && !_selectedEnemy.IsDead())
            {
                _selectedEnemy.Dead();
            }
            
        }


        if (_bulletsForLArm > 0)
        {
            var d = gun.DamageCalculation(_bulletsForLArm);
            _selectedEnemy.leftArm.TakeDamageArm(d);
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
            _selectedEnemy.rightArm.TakeDamageArm(d);
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
            _selectedEnemy.legs.TakeDamageLegs(d);
            _bulletsForLegs = 0;
            if (gun.AbilityUsed() == false)
            {
                gun.Ability();
            }
            _selectedChar.DeactivateAttack();
        }
        //_turnManager.OrderTurns();
        
        
        _selectedEnemy.SetSelectedForAttack(false);
        _turnManager.PortraitsActiveState(true);
        buttonEndTurn.gameObject.SetActive(true);
        DeselectActions();
        _charSelection.Selection(_selectedChar);
    }

    public void EndTurn()
    {
        if (_selectedChar != null && _selectedChar.IsMoving()) return;
        
        _turnManager.EndTurn();
        DeactivateEnemyHUD();
        DeactivatePlayerHUD();
        ResetBodyParts();
    }

    // private void DeselectUnit()
    // {
    //     DeselectActions();
    //     _charSelection.DeselectUnit();
    // }
    
    public void DeselectActions()
    {
        if (_selectedChar)
        {
            if (_selectedEnemy)
            {
                _selectedChar.bodyRenderContainer.SetActive(true);
                if (_selectedChar.gunsOffOnCloseUp)
                {
                    _selectedChar.GetLeftGun().ModelsOn();
                    _selectedChar.GetRightGun().ModelsOn();  
                }
                
                attackHudContainer.SetActive(false);
                var units = _turnManager.GetAllUnits();
                foreach (var u in units)
                {
                    u.gameObject.SetActive(true);
                }
                var cam = FindObjectOfType<CloseUpCamera>();
                cam.MoveCameraToParent(cam.transform.parent.position, _selectedEnemy.transform.position, cam.ResetCamera);

            }
            var gun = _selectedChar.GetSelectedGun();

            if (_bulletsForBody > 0)
                gun.IncreaseAvailableBullets(_bulletsForBody);

            if (_bulletsForLArm > 0)
                gun.IncreaseAvailableBullets(_bulletsForLArm);

            if (_bulletsForRArm > 0)
                gun.IncreaseAvailableBullets(_bulletsForRArm);

            if (_bulletsForLegs > 0)
                gun.IncreaseAvailableBullets(_bulletsForLegs);
            
            DestroyImage(gun.GetMaxBullets());
            
            ResetBodyParts();
        }

        if (_selectedEnemy)
        {
            _selectedEnemy.SetSelectedForAttack(false);
        }
        
        _charSelection.ActivateCharacterSelection(true);
        // DeactivatePlayerHUD();
        //
        DeactivateEnemyHUD();
        buttonEndTurn.gameObject.SetActive(true);
        buttonExecuteAttack.gameObject.SetActive(false);
        _turnManager.PortraitsActiveState(true);
        //_selectedChar = null;
        _selectedEnemy = null;
    }

    /// <summary>
    /// Change to Left Gun of selected Character when pressig on UI or key.
    /// </summary>
    public void UnitSwapToLeftGun()
    {
        if (!_selectedChar || !_selectedChar.LeftArmAlive()) return;
        
        AudioManager.audioManagerInstance.PlaySound(_soundsMenuManager.GetClickSound(), _soundsMenuManager.GetObjectToAddAudioSource());
        BodyClear();
        LeftArmClear();
        RightArmClear();
        LegsClear();
        _selectedChar.SelectLeftGun();
        leftWeaponCircle.SetActive(true);
        rightWeaponCircle.SetActive(false);
        //ShowPlayerHudText(playerBodyCurrHp, _selectedChar.body.GetCurrentHp(), playerLeftArmCurrHp, _selectedChar.leftArm.GetCurrentHp(), playerRightArmCurrHp, _selectedChar.rightArm.GetCurrentHp(), playerLegsCurrHp, _selectedChar.legs.GetCurrentHp());
        
        ShowPlayerHudText();

        if (_selectedChar.HasEnemiesInRange())
            ActivateBodyPartsContainer();
        else DeactivateBodyPartsContainer();
    }

    /// <summary>
    /// Change to Right Gun of selected Character when pressig on UI or key.
    /// </summary>
    public void UnitSwapToRightGun()
    {
        if (!_selectedChar || !_selectedChar.RightArmAlive()) return;
        
        AudioManager.audioManagerInstance.PlaySound(_soundsMenuManager.GetClickSound(), _soundsMenuManager.GetObjectToAddAudioSource());
        BodyClear();
        LeftArmClear();
        RightArmClear();
        LegsClear();
        _selectedChar.SelectRightGun();
        rightWeaponCircle.SetActive(true);
        leftWeaponCircle.SetActive(false);
        //ShowPlayerHudText(playerBodyCurrHp, _selectedChar.body.GetCurrentHp(), playerLeftArmCurrHp, _selectedChar.leftArm.GetCurrentHp(), playerRightArmCurrHp, _selectedChar.rightArm.GetCurrentHp(), playerLegsCurrHp, _selectedChar.legs.GetCurrentHp());
        
        ShowPlayerHudText();

        if (_selectedChar.HasEnemiesInRange())
            ActivateBodyPartsContainer();
        else DeactivateBodyPartsContainer();
    }

    //Muestro el WorldCanvas de todas las unidades
    private void ShowAllWorldUI()
    {
        _worldUIActive = true;
        var units = _turnManager.GetEnemies(EnumsClass.Team.Red).Concat(_turnManager.GetEnemies(EnumsClass.Team.Green));
        foreach (var unit in units)
        {
            if (!unit.IsSelectedForAttack() && unit.CanBeSelected())
                unit.ShowWorldUI();
        }
    }

    //Oculto el WorldCanvas de todas las unidades
    private void HideAllWorldUI()
    {
        _worldUIActive = false;
        var units = _turnManager.GetEnemies(EnumsClass.Team.Red).Concat(_turnManager.GetEnemies(EnumsClass.Team.Green));
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
        Attack();
    }

    public void AddBulletsToLArm(int quantity)
    {
        _bulletsForBody = 0;
        _bulletsForLArm = quantity;
        _bulletsForRArm = 0;
        _bulletsForLegs = 0;
        Attack();
    }

    public void AddBulletsToRArm(int quantity)
    {
        _bulletsForBody = 0;
        _bulletsForLArm = 0;
        _bulletsForRArm = quantity;
        _bulletsForLegs = 0;
        Attack();
    }

    public void AddBulletsToLegs(int quantity)
    {
        _bulletsForBody = 0;
        _bulletsForLArm = 0;
        _bulletsForRArm = 0;
        _bulletsForLegs = quantity;
        Attack();
    }

    private bool CharacterHasBullets(Character c)
    {
        return c.GetSelectedGun().GetAvailableBullets() > 0;
    }

    //Checks if player can attack en enemy.
    private void CheckIfCanExecuteAttack()
    {
        if (_bulletsForBody == 0 && _bulletsForLArm == 0 && _bulletsForLegs == 0 && _bulletsForRArm == 0)
            buttonExecuteAttack.interactable = false;
    }

    //Determines which buttons will be interactable.
    private void DeterminateButtonsActivation()
    {
        if (!_selectedChar) return;
        
        var ui = _selectedEnemy.GetMyUI();
        if (_selectedChar.GetSelectedGun().GetAvailableBullets() <= 0 || _partsSelected == _selectedChar.GetSelectedGun().GetAvailableSelections())
        {
            if (_buttonBodySelected == false)
            {
                ui.BodyEnabling(false);
            }

            if (_buttonLArmSelected == false)
            {
                ui.LeftArmEnabling(false);
            }

            if (_buttonRArmSelected == false)
            {
                ui.RightArmEnabling(false);
            }

            if (_buttonLegsSelected == false)
            {
                ui.LegsEnabling(false);
            }
            buttonExecuteAttack.interactable = true;
            buttonExecuteAttack.gameObject.SetActive(true);
        }
        else if (_partsSelected < _selectedChar.GetSelectedGun().GetAvailableSelections())
        {
            if (!_buttonBodySelected && _bodyInsight)
            {
                ui.BodyEnabling(true, this);
            }

            if (!_buttonLArmSelected && _lArmInsight)
            {
                ui.LeftArmEnabling(true, this);
            }

            if (!_buttonRArmSelected && _rArmInsight)
            {
                ui.RightArmEnabling(true, this);
            }

            if (!_buttonLegsSelected && _legsInsight)
            {
                ui.LegsEnabling(true, this);
            }

            if (_partsSelected <= 0)
            {
                buttonExecuteAttack.gameObject.SetActive(false);
            }
        }
    }

    private void ResetBodyParts()
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

    void CreateImage(int quantity)
    {
        var c = 0;
        for (int i = 0; i < quantity; i++)
        {
            c++;
            var image = Instantiate(hitImagePrefab, hitContainer.transform, true);
            _hitImagesCreated.Add(image);
        }
        Debug.Log("creo: " + c);
    }

    void DestroyImage(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            if (_hitImagesCreated.Count <= 0) break;
            var image = _hitImagesCreated[_hitImagesCreated.Count - 1];
            _hitImagesCreated.RemoveAt(_hitImagesCreated.Count - 1);
            Destroy(image);

        }
    }

    #endregion

    #region HUD Text
    /// <summary>
    /// Set actual Player Character UI.
    /// </summary>
    public void SetPlayerUI()
    {
        //ShowPlayerHudText(playerBodyCurrHp, _selectedChar.body.GetCurrentHp(), playerLeftArmCurrHp, _selectedChar.leftArm.GetCurrentHp(), playerRightArmCurrHp, _selectedChar.rightArm.GetCurrentHp(), playerLegsCurrHp, _selectedChar.legs.GetCurrentHp());

        ShowPlayerHudText();
        
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
            if (_selectedChar.HasEnemiesInRange() && _selectedEnemy)
            {
                ActivateBodyPartsContainer();
            }
        }
        playerHudContainer.SetActive(true);
    }

    private void DeactivatePlayerHUD()
    {
        playerHudContainer.SetActive(false);
    }

    /// <summary>
    /// Set actual Enemy Character UI.
    /// </summary>
    public void SetEnemyUI()
    {
        if (_selectedChar)
        {
            if (_selectedChar.CanAttack() && _selectedEnemy.CanBeAttacked())
            {
                _selectedEnemy.SetSelectedForAttack(true);
                buttonEndTurn.gameObject.SetActive(false);
                _turnManager.PortraitsActiveState(false);
                
                
                FindObjectOfType<CloseUpCamera>().MoveCameraWithLerp(_selectedEnemy.transform.position, _selectedChar.transform.position, ActivateBodyPartsContainer);
                playerHudContainer.SetActive(false);
                _selectedChar.ResetTilesInAttackRange();
                _selectedChar.ResetTilesInMoveRange();
                _selectedChar.SetSelectingEnemy(true);
            }
                
            else DeactivateBodyPartsContainer();
        }
        else
        {
            buttonExecuteAttack.interactable = false;
        }
    }

    private void DeactivateEnemyHUD()
    {
        DeactivateBodyPartsContainer();
    }

    private void SetAttackHUD()
    {
        var gun = _selectedChar.GetSelectedGun();
        attackWeaponNameText.text = gun.GetGunTypeString();
        attackWeaponHitsText.text = gun.GetMaxBullets().ToString();
        attackWeaponDamageText.text = gun.GetBulletDamage().ToString();
        attackWeaponHitChanceText.text = gun.GetHitChance().ToString();

        CreateImage(gun.GetMaxBullets());
        attackHudContainer.SetActive(true);
    }

    //private void ShowPlayerHudText(TextMeshProUGUI bodyHpText, float bodyValue, TextMeshProUGUI lArmHpText, float lArmValue, TextMeshProUGUI rArmHpText, float rArmValue, TextMeshProUGUI legsHpText, float legsValue)
    private void ShowPlayerHudText()
    {
        //ShowUnitHudText(bodyHpText, bodyValue, lArmHpText, lArmValue, rArmHpText, rArmValue, legsHpText, legsValue);
        ShowUnitHudText();

        playerBodySlider.minValue = 0;
        playerBodySlider.maxValue = _selectedChar.body.GetMaxHp();
        playerBodySlider.value = _selectedChar.body.GetCurrentHp();
        
        playerLeftArmSlider.minValue = 0;
        playerLeftArmSlider.maxValue = _selectedChar.leftArm.GetMaxHp();
        playerLeftArmSlider.value = _selectedChar.leftArm.GetCurrentHp();
        
        playerRightArmSlider.minValue = 0;
        playerRightArmSlider.maxValue = _selectedChar.rightArm.GetMaxHp();
        playerRightArmSlider.value = _selectedChar.rightArm.GetCurrentHp();
        
        playerLegsSlider.minValue = 0;
        playerLegsSlider.maxValue = _selectedChar.legs.GetMaxHp();
        playerLegsSlider.value = _selectedChar.legs.GetCurrentHp();

        if (_selectedChar.LeftArmAlive())
        {
            var left = _selectedChar.GetLeftGun();
            if (left)
            {
                leftGunTypeText.text = left.GetGunTypeString();
                var b = left.GetAvailableBullets().ToString();
                leftGunHitsText.text = b;

                var dmg = left.GetBulletDamage().ToString();
                leftGunDamageText.text = dmg;

                var h = left.GetHitChance().ToString();
                leftGunHitChanceText.text = h + "%";
            }
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
            if (right)
            {
                rightGunTypeText.text = right.GetGunTypeString();

                var b = right.GetAvailableBullets().ToString();
                rightGunHitsText.text = b;

                var dmg = right.GetBulletDamage().ToString();
                rightGunDamageText.text = dmg;

                var h = right.GetHitChance().ToString();
                rightGunHitChanceText.text = h + "%"; 
            }
        }
        else
        {
            rightGunTypeText.text = "No gun - Arm destroyed";
			rightGunDamageText.text = "";
			rightGunHitChanceText.text = "";
        }
    }

    void ShowUnitHudText()
    //void ShowUnitHudText(TextMeshProUGUI bodyHpText, float bodyValue, TextMeshProUGUI lArmHpText, float lArmValue, TextMeshProUGUI rArmHpText, float rArmValue, TextMeshProUGUI legsHpText, float legsValue)
    {
        // bodyHpText.text = bodyValue.ToString();
        // lArmHpText.text = lArmValue.ToString();
        // rArmHpText.text = rArmValue.ToString();
        // legsHpText.text = legsValue.ToString();

        playerBodyCurrHp.text = _selectedChar.body.GetCurrentHp().ToString();
        playerLeftArmCurrHp.text = _selectedChar.leftArm.GetCurrentHp().ToString();
        playerRightArmCurrHp.text = _selectedChar.rightArm.GetCurrentHp().ToString();
        playerLegsCurrHp.text = _selectedChar.legs.GetCurrentHp().ToString();
    }
    
    public void UpdateLegsHUD(float value, bool isPlayer)
    {
        if (!isPlayer) return;
        
        playerLegsCurrHp.text = value.ToString();
        playerLegsSlider.value = value;
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

    private void ActivateBodyPartsContainer()
    {
        if (_selectedEnemy)
        {
            _selectedChar.bodyRenderContainer.SetActive(false);
            if (_selectedChar.gunsOffOnCloseUp)
            {
                _selectedChar.GetLeftGun().ModelsOff();
                _selectedChar.GetRightGun().ModelsOff();
            }
            _selectedChar.RotateTowardsEnemy(_selectedEnemy.transform.position, ActivateParts);
        }
    }

    private void ActivateParts()
    {
        if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetBodyPosition(), "Body") && _selectedEnemy.body.GetCurrentHp() > 0)
        {
            _bodyInsight = true;
        }
        else
        {
            _bodyInsight = false;
        }

        if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetLArmPosition(), "LArm") && _selectedEnemy.leftArm.GetCurrentHp() > 0)
        {
            _lArmInsight = true;
        }
        else
        {
            _lArmInsight = false;
        }

        if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetRArmPosition(), "RArm") && _selectedEnemy.rightArm.GetCurrentHp() > 0)
        {
            _rArmInsight = true;
        }
        else
        {
            _rArmInsight = false;
        }


        if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetLegsPosition(), "Legs") &&_selectedEnemy.legs.GetCurrentHp() > 0)
        {
            _legsInsight = true;
        }
        else
        {
            _legsInsight = false;
        }
        
        var units = _turnManager.GetAllUnits();

        foreach (var u in units)
        {
            if (u == _selectedChar || u == _selectedEnemy) continue;
                    
            u.gameObject.SetActive(false);
        }
        _selectedChar.RaysOffDelay();
        //_selectedChar.RaysOff();
        
        var ui = _selectedEnemy.GetMyUI();
        ui.ButtonsEnabling(_bodyInsight, _lArmInsight, _rArmInsight, _legsInsight, this);
        
        
        ui.SetBodyHpText(_selectedEnemy.body.GetCurrentHp());

        ui.SetLeftArmHpText(_selectedEnemy.leftArm.GetCurrentHp());

        ui.SetRightArmHpText(_selectedEnemy.rightArm.GetCurrentHp());

        ui.SetLegsHpText(_selectedEnemy.legs.GetCurrentHp());

        ui.ButtonsContainerSetActive(true);
        SetAttackHUD();
    }

    public void DeactivateBodyPartsContainer()
    {
        if (_selectedEnemy)
            _selectedEnemy.GetMyUI().ButtonsContainerSetActive(false);
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
