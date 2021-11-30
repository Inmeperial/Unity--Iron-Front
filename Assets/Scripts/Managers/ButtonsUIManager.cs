using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonsUIManager : MonoBehaviour
{
    public MechaPartButton bodyButton;
    public MechaPartButton leftGunButton;
    public MechaPartButton rightGunButton;
    public MechaPartButton legsButton;
    [Header("Important Buttons")]
    public Button buttonExecuteAttack;
    public float attackDelay;
    public Button buttonEndTurn;

    [Space]
    
    [Header("Keys")]
    public KeyCode deselectKey;
    public KeyCode selectLGunKey;
    public KeyCode selectRGunKey;
    public KeyCode showWorldUIKey;
    public KeyCode toggleWorldUIKey;

    [Space]
    
    #region Buttons
    private bool _buttonBodySelected;
    private int _bulletsForBody;
    private bool _buttonLArmSelected;
    private int _bulletsForLGun;
    private bool _buttonRArmSelected;
    private int _bulletsForRGun;
    private bool _buttonLegsSelected;
    private int _bulletsForLegs;
    #endregion

    [Header("Player HUD")]
    #region HUD
    //Player
    public GameObject playerHudContainer;
    [Header("Weapons")]
    public TextMeshProUGUI leftGunTypeText;
    public TextMeshProUGUI leftGunDamageText;
    public TextMeshProUGUI leftGunHitsText;
    public TextMeshProUGUI leftGunHitChanceText;
    public TextMeshProUGUI rightGunTypeText;
    public TextMeshProUGUI rightGunDamageText;
    public TextMeshProUGUI rightGunHitsText;
    public TextMeshProUGUI rightGunHitChanceText;
    public GameObject leftWeaponCircle;
    public GameObject rightWeaponCircle;
    public Button leftWeaponBar;
    public Button rightWeaponBar;
    //public EquipmentButton equipmentButton;
    public EquipmentButton bodyEquipmentButton;
    public EquipmentButton leftGunEquipmentButton;
    public EquipmentButton rightGunEquipmentButton;
    public EquipmentButton legsEquipmentButton;
    public EquipmentButton itemEquipmentButton;
    private Dictionary<EquipmentButton, bool> _equipmentButtonsPreviousState = new Dictionary<EquipmentButton, bool>();
    public Sprite noneIcon;

    [Header("Player Stats")]
    public TextMeshProUGUI playerBodyCurrHp;
    public Slider playerBodySlider;
    public TextMeshProUGUI playerLeftArmCurrHp;
    public Slider playerLeftArmSlider;
    public TextMeshProUGUI playerRightArmCurrHp;
    public Slider playerRightArmSlider;
    public TextMeshProUGUI playerLegsCurrHp;
    public Slider playerLegsSlider;

    [Space]
    
    [Header("Attack HUD")]
    
    //Weapon Attack
    public GameObject attackHudContainer;
    public TextMeshProUGUI attackWeaponNameText;
    public TextMeshProUGUI attackWeaponHitsText;
    public TextMeshProUGUI attackWeaponDamageText;
    public TextMeshProUGUI attackWeaponHitChanceText;
    public GameObject hitContainer;
    public GameObject hitImagePrefab;
    private List<GameObject> _hitImagesCreated = new List<GameObject>();
    
    #endregion

    //OTHERS
    private Character _selectedChar;
    private Character _selectedEnemy;
    private int _partsSelected;
    private SoundsMenu _soundsMenuManager;

    private bool _bodyInsight;
    private bool _legsInsight;
    private bool _rArmInsight;
    private bool _lArmInsight;

    private bool _worldUIActive = false;
    private bool _worldUIToggled;
    public static ButtonsUIManager Instance;
    

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }
    
    private void Start()
    {
        bodyEquipmentButton.interactable = false;
        leftGunEquipmentButton.interactable = false;
        rightGunEquipmentButton.interactable = false;
        legsEquipmentButton.interactable = false;
        itemEquipmentButton.interactable = false;

        playerHudContainer.SetActive(false);
        _buttonBodySelected = false;
        _buttonLArmSelected = false;
        _buttonRArmSelected = false;
        _buttonLegsSelected = false;
        
        _soundsMenuManager = FindObjectOfType<SoundsMenu>();
        
        bodyButton.ButtonEnabling(false, () => { },() => { });
        leftGunButton.ButtonEnabling(false, () => { },() => { });
        rightGunButton.ButtonEnabling(false, () => { },() => { });
        legsButton.ButtonEnabling(false, () => { },() => { });
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (_selectedChar && _selectedChar.IsMoving() == false && _selectedEnemy &&
                Input.GetKeyDown(deselectKey))
            {
                _selectedChar.ResetRotationOnDeselect();
                DeselectActions();
                ClearSelectedEnemy();
            }
                

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

        if (!_worldUIToggled)
        {
            if (Input.GetKeyDown(showWorldUIKey))
                ShowAllWorldUI();
        
            if (Input.GetKeyUp(showWorldUIKey))
                HideAllWorldUI(); 
        }
        
            
        if (Input.GetKeyDown(toggleWorldUIKey))
        {
            if (_worldUIActive)
            {
                ToggleWorldUI(false);
            }
            else ToggleWorldUI(true);
        }
    }
    #region ButtonsActions

    
    //Se ejecuta cuando se hace click izquierdo en el boton de body
    public void BodySelection()
    {
        if (!CharacterHasBullets(_selectedChar)) return;
        
        Gun gun = _selectedChar.GetSelectedGun();
        
        if (_partsSelected > gun.GetAvailableSelections()) return;
        
        if (_bulletsForBody == 0)
        {
            _partsSelected++;
        }
        _bulletsForBody += gun.GetBulletsPerClick();
        DestroyImage(gun.GetBulletsPerClick());
        gun.ReduceAvailableBullets();
        //_selectedEnemy.GetMyUI().SetBodyCount(_bulletsForBody);
        bodyButton.SetBulletsCount(_bulletsForBody);
        buttonExecuteAttack.interactable = true;
        buttonExecuteAttack.gameObject.SetActive(true);
        _buttonBodySelected = true;
        DeterminateButtonsActivation();

    }

    //Se ejecuta cuando se hace click derecho en el boton de body
    public void BodyMinus()
    {
        if (_bulletsForBody <= 0) return;
        
        Gun gun = _selectedChar.GetSelectedGun();
        gun.IncreaseAvailableBullets();
        _bulletsForBody = _bulletsForBody > 0 ? (_bulletsForBody - gun.GetBulletsPerClick()) : 0;
        //_selectedEnemy.GetMyUI().SetBodyCount(_bulletsForBody);
        bodyButton.SetBulletsCount(_bulletsForBody);
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
        
        Gun gun = _selectedChar.GetSelectedGun();
        gun.IncreaseAvailableBullets(_bulletsForBody);
        DestroyImage(gun.GetMaxBullets());
        _hitImagesCreated.Clear();
        _bulletsForBody = 0;
        _buttonBodySelected = false;
        if (_selectedEnemy)
            //_selectedEnemy.GetMyUI().SetBodyCount(_bulletsForBody);
            bodyButton.SetBulletsCount(_bulletsForBody);
        if (_partsSelected > 0)
            _partsSelected--;
        CheckIfCanExecuteAttack();
        DeterminateButtonsActivation();
    }

    //Se ejecuta cuando se hace click izquierdo en el boton de left arm
    public void LeftArmSelection()
    {
        if (!CharacterHasBullets(_selectedChar)) return;
        
        Gun gun = _selectedChar.GetSelectedGun();
        
        if (!gun) return;
        
        if (_partsSelected > gun.GetAvailableSelections()) return;
        
        if (_bulletsForLGun == 0)
        {
            _partsSelected++;
        }
        _bulletsForLGun += gun.GetBulletsPerClick();
        DestroyImage(gun.GetBulletsPerClick());
        //_selectedEnemy.GetMyUI().SetLeftArmCount(_bulletsForLGun);
        leftGunButton.SetBulletsCount(_bulletsForLGun);
        gun.ReduceAvailableBullets();

        buttonExecuteAttack.interactable = true;
        buttonExecuteAttack.gameObject.SetActive(true);
        _buttonLArmSelected = true;
        DeterminateButtonsActivation();
    }
    
    //Se ejecuta cuando se hace click derecho en el boton de left arm
    public void LeftArmMinus()
    {
        if (_bulletsForLGun <= 0) return;
        
        Gun gun = _selectedChar.GetSelectedGun();
        gun.IncreaseAvailableBullets();
        _bulletsForLGun = _bulletsForLGun > 0 ? (_bulletsForLGun - gun.GetBulletsPerClick()) : 0;
        //_selectedEnemy.GetMyUI().SetLeftArmCount(_bulletsForLGun);
        leftGunButton.SetBulletsCount(_bulletsForLGun);
        CheckIfCanExecuteAttack();
        CreateImage(gun.GetBulletsPerClick());
        if (_bulletsForLGun == 0)
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
        if (_bulletsForLGun <= 0) return;
        
        Gun gun = _selectedChar.GetSelectedGun();
        gun.IncreaseAvailableBullets(_bulletsForLGun);
        DestroyImage(gun.GetMaxBullets());
        _bulletsForLGun = 0;
        if (_selectedEnemy)
            //_selectedEnemy.GetMyUI().SetLeftArmCount(_bulletsForLGun);
            leftGunButton.SetBulletsCount(_bulletsForLGun);
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
        
        Gun gun = _selectedChar.GetSelectedGun();

        if (!gun) return;
        
        if (_partsSelected > gun.GetAvailableSelections()) return;
        
        if (_bulletsForRGun == 0)
        {
            _partsSelected++;
        }
        _bulletsForRGun += gun.GetBulletsPerClick();
        DestroyImage(gun.GetBulletsPerClick());
        //_selectedEnemy.GetMyUI().SetRightArmCount(_bulletsForRGun);
        rightGunButton.SetBulletsCount(_bulletsForRGun);
        gun.ReduceAvailableBullets();
        buttonExecuteAttack.interactable = true;
        buttonExecuteAttack.gameObject.SetActive(true);
        _buttonRArmSelected = true;
        DeterminateButtonsActivation();
    }

    //Se ejecuta cuando se hace click derecho en el boton de right arm
    public void RightArmMinus()
    {
        if (_bulletsForRGun <= 0) return;
        
        Gun gun = _selectedChar.GetSelectedGun();
        gun.IncreaseAvailableBullets();
        _bulletsForRGun = _bulletsForRGun > 0 ? (_bulletsForRGun - gun.GetBulletsPerClick()) : 0;
        //_selectedEnemy.GetMyUI().SetRightArmCount(_bulletsForRGun);
        rightGunButton.SetBulletsCount(_bulletsForRGun);
        CreateImage(gun.GetBulletsPerClick());
        CheckIfCanExecuteAttack();
        if (_bulletsForRGun == 0)
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
        if (_bulletsForRGun <= 0) return;
        
        Gun gun = _selectedChar.GetSelectedGun();
        gun.IncreaseAvailableBullets(_bulletsForRGun);
        DestroyImage(gun.GetMaxBullets());
        _bulletsForRGun = 0;
        if (_selectedEnemy)
            //_selectedEnemy.GetMyUI().SetRightArmCount(_bulletsForRGun);
            rightGunButton.SetBulletsCount(_bulletsForRGun);

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
        
        Gun gun = _selectedChar.GetSelectedGun();

        if (_partsSelected > gun.GetAvailableSelections()) return;
        
        if (_bulletsForLegs == 0)
        {
            _partsSelected++;
        }
        _bulletsForLegs += gun.GetBulletsPerClick();
        DestroyImage(gun.GetBulletsPerClick());
        //_selectedEnemy.GetMyUI().SetLegsCount(_bulletsForLegs);
        legsButton.SetBulletsCount(_bulletsForLegs);

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
        
        Gun gun = _selectedChar.GetSelectedGun();
        gun.IncreaseAvailableBullets();
        _bulletsForLegs = _bulletsForLegs > 0 ? (_bulletsForLegs - gun.GetBulletsPerClick()) : 0;
        //_selectedEnemy.GetMyUI().SetLegsCount(_bulletsForLegs);
        legsButton.SetBulletsCount(_bulletsForLegs);
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
        
        Gun gun = _selectedChar.GetSelectedGun();
        gun.IncreaseAvailableBullets(_bulletsForLegs);
        DestroyImage(gun.GetMaxBullets());
        _bulletsForLegs = 0;
        if (_selectedEnemy)
            //_selectedEnemy.GetMyUI().SetLegsCount(_bulletsForLegs);
            legsButton.SetBulletsCount(_bulletsForLegs);
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
        
        DeactivateEndTurnButton();
        var cam = FindObjectOfType<CloseUpCamera>();
        WorldUI ui = _selectedEnemy.GetMyUI();

        //ui.ResetButtons();
        bodyButton.ResetButton();
        rightGunButton.ResetButton();
        leftGunButton.ResetButton();
        legsButton.ResetButton();
        
        float rightGunHP = 0;
        if (_selectedEnemy.GetRightGun())
            rightGunHP = _selectedEnemy.GetRightGun().GetCurrentHp();
        
        float leftGunHP = 0;
        if (_selectedEnemy.GetLeftGun())
            leftGunHP = _selectedEnemy.GetLeftGun().GetCurrentHp();
        
        ui.SetLimits(_selectedEnemy.GetBody().GetMaxHp(), rightGunHP, leftGunHP, _selectedEnemy.GetLegs().GetMaxHp());
        buttonExecuteAttack.interactable = false;
        buttonExecuteAttack.gameObject.SetActive(false);
        _selectedChar.ResetInRangeLists();
        DeactivateBodyPartsContainer();
        attackHudContainer.SetActive(false);
        foreach (var go in _selectedChar.bodyRenderContainer)
        {
            go.SetActive(true);
        }
        if (_selectedChar.gunsOffOnCloseUp)
        {
            if (_selectedChar.GetLeftGun()) _selectedChar.GetLeftGun().ModelsOn();
            
            if (_selectedChar.GetRightGun()) _selectedChar.GetRightGun().ModelsOn();
        }
        
        Character[] units = TurnManager.Instance.GetAllUnits();
        foreach (Character u in units)
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
        _selectedChar.RotateTowardsEnemy(_selectedEnemy.transform);
        _selectedChar.SetInitialRotation(_selectedChar.transform.rotation);

        Gun gun = _selectedChar.GetSelectedGun();
        _selectedChar.Shoot();
        _selectedEnemy.SetHurtAnimation();
        if (_bulletsForBody > 0)
        {
            List<Tuple<int,int>> d = gun.DamageCalculation(_bulletsForBody);
            _selectedEnemy.GetBody().TakeDamage(d);
            _bulletsForBody = 0;
            
                
            if (gun.SkillUsed() == false)
            {
                gun.Ability();
            }
            _selectedChar.DeactivateAttack();
            
            if (_selectedEnemy.GetBody().GetCurrentHp() <= 0 && !_selectedEnemy.IsDead())
            {
                _selectedEnemy.Dead();
            }
            
        }


        if (_bulletsForLGun > 0)
        {
            if (_selectedEnemy.GetLeftGun())
            {
                List<Tuple<int,int>> d = gun.DamageCalculation(_bulletsForLGun);
                _selectedEnemy.GetLeftGun().TakeDamage(d);
                _bulletsForLGun = 0;
                
                if (gun.SkillUsed() == false)
                {
                    gun.Ability();
                }
                _selectedChar.DeactivateAttack(); 
            }
        }

        if (_bulletsForRGun > 0)
        {
            if (_selectedEnemy.GetRightGun())
            {
                List<Tuple<int,int>> d = gun.DamageCalculation(_bulletsForRGun);
                _selectedEnemy.GetRightGun().TakeDamage(d);
                _bulletsForRGun = 0;
                if (gun.SkillUsed() == false)
                {
                    gun.Ability();
                }
                _selectedChar.DeactivateAttack(); 
            }
            
        }

        if (_bulletsForLegs > 0)
        {
            List<Tuple<int,int>> d = gun.DamageCalculation(_bulletsForLegs);
            _selectedEnemy.GetLegs().TakeDamage(d);
            _bulletsForLegs = 0;
            if (gun.SkillUsed() == false)
            {
                gun.Ability();
            }
            _selectedChar.DeactivateAttack();
        }
        
        _selectedEnemy.SetSelectedForAttack(false);
        PortraitsController.Instance.PortraitsActiveState(true);
        
        if (_selectedChar.GetUnitTeam() == EnumsClass.Team.Green)
            ActivateEndTurnButton();
        DeselectActions();
        //CharacterSelection.Instance.Selection(_selectedChar);
    }

    public void EndTurn()
    {
        if (_selectedChar != null && _selectedChar.IsMoving()) return;
        
        _selectedChar = null;
        _selectedEnemy = null;
        TurnManager.Instance.EndTurn();
        DeactivateBodyPartsContainer();
        DeactivatePlayerHUD();
        ResetBodyParts();
    }

    public void DeselectActions()
    {
        if (_selectedChar)
        {
            if (_selectedEnemy)
            {
                foreach (var go in _selectedChar.bodyRenderContainer)
                {
                    go.SetActive(true);
                }
                if (_selectedChar.gunsOffOnCloseUp)
                {
                    if (_selectedChar.GetLeftGun()) _selectedChar.GetLeftGun().ModelsOn();
                    
                    if (_selectedChar.GetRightGun()) _selectedChar.GetRightGun().ModelsOn();  
                }
                
                attackHudContainer.SetActive(false);
                Character[] units = TurnManager.Instance.GetAllUnits();
                foreach (Character u in units)
                {
                    u.gameObject.SetActive(true);
                }
                var cam = FindObjectOfType<CloseUpCamera>();
                _selectedChar.SetSelectingEnemy(false);
                _selectedChar.ResetRotationAndRays();
                
                cam.MoveCameraToParent(cam.transform.parent.position, _selectedEnemy.transform.position, cam.ResetCamera);

            }
            Gun gun = _selectedChar.GetSelectedGun();

            if (gun)
            {
                if (_bulletsForBody > 0)
                    gun.IncreaseAvailableBullets(_bulletsForBody);

                if (_bulletsForLGun > 0)
                    gun.IncreaseAvailableBullets(_bulletsForLGun);

                if (_bulletsForRGun > 0)
                    gun.IncreaseAvailableBullets(_bulletsForRGun);

                if (_bulletsForLegs > 0)
                    gun.IncreaseAvailableBullets(_bulletsForLegs);
            
                DestroyImage(gun.GetMaxBullets());
            }
            
            
            ResetBodyParts();
        }

        if (_selectedEnemy)
        {
            _selectedEnemy.SetSelectedForAttack(false);
        }
        
        CharacterSelection.Instance.ActivateCharacterSelection(true);

        if (_selectedChar)
            CharacterSelection.Instance.SelectionWithDelay(_selectedChar);
        
        DeactivateBodyPartsContainer();

        if (TurnManager.Instance.GetActiveTeam() == EnumsClass.Team.Green)
            ActivateEndTurnButton();
        buttonExecuteAttack.gameObject.SetActive(false);
        PortraitsController.Instance.PortraitsActiveState(true);
        
    }

    private void ClearSelectedEnemy()
    {
        _selectedEnemy = null;
    }

    /// <summary>
    /// Change to Left Gun of selected Character when pressig on UI or key.
    /// </summary>
    public void UnitSwapToLeftGun()
    {
        if (!_selectedChar || !_selectedChar.LeftGunAlive() || !_selectedChar.GetLeftGun()) return;
        
        if (_selectedChar.IsOnElevator() && _selectedChar.GetLeftGun().GetGunType() == EnumsClass.GunsType.Melee) return;
        
        AudioManager.audioManagerInstance.PlaySound(_soundsMenuManager.GetClickSound(), _soundsMenuManager.GetObjectToAddAudioSource());
        BodyClear();
        LeftArmClear();
        RightArmClear();
        LegsClear();
        _selectedChar.SelectLeftGun();
        leftWeaponCircle.SetActive(true);
        rightWeaponCircle.SetActive(false);

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
        if (!_selectedChar || !_selectedChar.RightGunAlive() || !_selectedChar.GetRightGun()) return;
        
        AudioManager.audioManagerInstance.PlaySound(_soundsMenuManager.GetClickSound(), _soundsMenuManager.GetObjectToAddAudioSource());
        BodyClear();
        LeftArmClear();
        RightArmClear();
        LegsClear();
        _selectedChar.SelectRightGun();
        rightWeaponCircle.SetActive(true);
        leftWeaponCircle.SetActive(false);

        ShowPlayerHudText();

        if (_selectedChar.HasEnemiesInRange())
            ActivateBodyPartsContainer();
        else DeactivateBodyPartsContainer();
    }

    //Muestro el WorldCanvas de todas las unidades
    private void ShowAllWorldUI()
    {
        _worldUIActive = true;
        Character[] units = TurnManager.Instance.GetAllUnits();
        foreach (Character unit in units)
        {
            if (!unit.IsSelectedForAttack() && unit.CanBeSelected())
            {
                unit.ShowWorldUI();
            }
        }
    }

    //Oculto el WorldCanvas de todas las unidades
    private void HideAllWorldUI()
    {
        _worldUIActive = false;
        Character[] units = TurnManager.Instance.GetAllUnits();
        foreach (Character unit in units)
        {
            unit.HideWorldUI();
        }
    }

    private void ToggleWorldUI(bool state)
    {
        if (state)
        {
            _worldUIActive = true;
            _worldUIToggled = true;
            Character[] units = TurnManager.Instance.GetAllUnits();
            foreach (Character unit in units)
            {
                if (unit.IsDead()) continue;
                
                if (!unit.IsSelectedForAttack() && unit.CanBeSelected())
                {
                    unit.ShowWorldUI();
                    unit.WorldUIToggled(true);
                }
            }
        }
        else
        {
            _worldUIActive = false;
            _worldUIToggled = false;
            Character[] units = TurnManager.Instance.GetAllUnits();
            foreach (Character unit in units)
            {
                unit.WorldUIToggled(false);
                unit.HideWorldUI();
            }
        }
    }

    #endregion

    #region Utilities

    public void AddBulletsToBody(int quantity)
    {
        _bulletsForBody = quantity;
        _bulletsForLGun = 0;
        _bulletsForRGun = 0;
        _bulletsForLegs = 0;
        Attack();
    }

    public void AddBulletsToLArm(int quantity)
    {
        _bulletsForBody = 0;
        _bulletsForLGun = quantity;
        _bulletsForRGun = 0;
        _bulletsForLegs = 0;
        Attack();
    }

    public void AddBulletsToRArm(int quantity)
    {
        _bulletsForBody = 0;
        _bulletsForLGun = 0;
        _bulletsForRGun = quantity;
        _bulletsForLegs = 0;
        Attack();
    }

    public void AddBulletsToLegs(int quantity)
    {
        _bulletsForBody = 0;
        _bulletsForLGun = 0;
        _bulletsForRGun = 0;
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
        if (_bulletsForBody == 0 && _bulletsForLGun == 0 && _bulletsForLegs == 0 && _bulletsForRGun == 0)
            buttonExecuteAttack.interactable = false;
    }

    //Determines which buttons will be interactable.
    private void DeterminateButtonsActivation()
    {
        if (!_selectedChar) return;
        
        WorldUI ui = _selectedEnemy.GetMyUI();
        if (_selectedChar.GetSelectedGun().GetAvailableBullets() <= 0 || _partsSelected == _selectedChar.GetSelectedGun().GetAvailableSelections())
        {
            if (_buttonBodySelected == false)
            {
                //ui.BodyEnabling(false);
                bodyButton.ButtonEnabling(false, () => { }, () => {});
            }

            if (_buttonLArmSelected == false)
            {
                //ui.LeftArmEnabling(false);
                leftGunButton.ButtonEnabling(false, () => { }, () => {});
            }

            if (_buttonRArmSelected == false)
            {
                //ui.RightArmEnabling(false);
                rightGunButton.ButtonEnabling(false, () => { }, () => {});
            }

            if (_buttonLegsSelected == false)
            {
                //ui.LegsEnabling(false);
                legsButton.ButtonEnabling(false, () => { }, () => {});
            }
            buttonExecuteAttack.interactable = true;
            buttonExecuteAttack.gameObject.SetActive(true);
        }
        else if (_partsSelected < _selectedChar.GetSelectedGun().GetAvailableSelections())
        {
            if (!_buttonBodySelected && _bodyInsight)
            {
                //ui.BodyEnabling(true, this);
                bodyButton.ButtonEnabling(true, BodyMinus, BodySelection);
            }

            if (!_buttonLArmSelected && _lArmInsight)
            {
                //ui.LeftArmEnabling(true, this);
                leftGunButton.ButtonEnabling(true, LeftArmMinus, LeftArmSelection);
            }

            if (!_buttonRArmSelected && _rArmInsight)
            {
                //ui.RightArmEnabling(true, this);
                rightGunButton.ButtonEnabling(true, RightArmMinus, RightArmSelection);
            }

            if (!_buttonLegsSelected && _legsInsight)
            {
                //ui.LegsEnabling(true, this);
                legsButton.ButtonEnabling(true, LegsMinus, LegsSelection);
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
        _bulletsForLGun = 0;

        _buttonRArmSelected = false;
        _bulletsForRGun = 0;

        _buttonLegsSelected = false;
        _bulletsForLegs = 0;

        _partsSelected = 0;
    }

    void CreateImage(int quantity)
    {
        int c = 0;
        for (int i = 0; i < quantity; i++)
        {
            c++;
            GameObject image = Instantiate(hitImagePrefab, hitContainer.transform, true);
            _hitImagesCreated.Add(image);
        }
    }

    void DestroyImage(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            if (_hitImagesCreated.Count <= 0) break;
            GameObject image = _hitImagesCreated[_hitImagesCreated.Count - 1];
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
        ShowPlayerHudText();
        
        if (_selectedChar.RightGunAlive() && _selectedChar.GetRightGun())
        {
            rightWeaponCircle.SetActive(true);
            leftWeaponCircle.SetActive(false);
        }
        else if (_selectedChar.LeftGunAlive() && _selectedChar.GetLeftGun())
        {
            rightWeaponCircle.SetActive(false);
            leftWeaponCircle.SetActive(true);
        }
        else
        {
            rightWeaponCircle.SetActive(false);
            rightWeaponBar.enabled = false;
            leftWeaponCircle.SetActive(false);
            leftWeaponBar.enabled = false;
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
                DeactivateEndTurnButton();
                PortraitsController.Instance.PortraitsActiveState(false);

                Action toDo = () =>
                {
                    ActivateBodyPartsContainer();
                    playerHudContainer.SetActive(false);
                    _selectedChar.ResetTilesInAttackRange();
                    _selectedChar.ResetTilesInMoveRange();
                    _selectedChar.SetSelectingEnemy(true);
                };

                FindObjectOfType<CloseUpCamera>().MoveCameraWithLerp(_selectedEnemy.transform.position, _selectedChar.transform.position, toDo);
                
            }
                
            else DeactivateBodyPartsContainer();
        }
        else
        {
            buttonExecuteAttack.interactable = false;
        }
    }

    private void SetAttackHUD()
    {
        Gun gun = _selectedChar.GetSelectedGun();
        attackWeaponNameText.text = gun.GetGunTypeString();
        attackWeaponHitsText.text = gun.GetMaxBullets().ToString();
        attackWeaponDamageText.text = gun.GetBulletDamage().ToString();
        attackWeaponHitChanceText.text = gun.GetHitChance().ToString();

        CreateImage(gun.GetMaxBullets());
        attackHudContainer.SetActive(true);
    }
    
    private void ShowPlayerHudText()
    {
        ShowUnitHudText();

        playerBodySlider.minValue = 0;
        playerBodySlider.maxValue = _selectedChar.GetBody().GetMaxHp();
        playerBodySlider.value = _selectedChar.GetBody().GetCurrentHp();
        
        playerLeftArmSlider.minValue = 0;
        if (_selectedChar.GetLeftGun())
        {
            playerLeftArmSlider.maxValue = _selectedChar.GetLeftGun().GetMaxHp();
            playerLeftArmSlider.value = _selectedChar.GetLeftGun().GetCurrentHp();
        }
        else
        {
            playerLeftArmSlider.maxValue = 0;
            playerLeftArmSlider.value = 0;
        }
        
        
        playerRightArmSlider.minValue = 0;

        if (_selectedChar.GetRightGun())
        {
            playerRightArmSlider.maxValue = _selectedChar.GetRightGun().GetMaxHp();
            playerRightArmSlider.value = _selectedChar.GetRightGun().GetCurrentHp();
        }
        else
        {
            playerRightArmSlider.maxValue = 0;
            playerRightArmSlider.value = 0;
        }
        
        
        playerLegsSlider.minValue = 0;
        playerLegsSlider.maxValue = _selectedChar.GetLegs().GetMaxHp();
        playerLegsSlider.value = _selectedChar.GetLegs().GetCurrentHp();

        Gun left = _selectedChar.GetLeftGun();
        if (_selectedChar.LeftGunAlive() && left)
        {
            leftGunTypeText.text = left.GetGunTypeString();
            string b = left.GetAvailableBullets().ToString();
            leftGunHitsText.text = b;

            string dmg = left.GetBulletDamage().ToString();
            leftGunDamageText.text = dmg;

            string h = left.GetHitChance().ToString();
            leftGunHitChanceText.text = h + "%";
            
            leftWeaponBar.enabled = true;
            leftWeaponCircle.transform.parent.GetComponent<Button>().enabled = true;
        }
        else
        {
            if (_selectedChar.LeftGunAlive()) leftGunTypeText.text = "No gun";
            else leftGunTypeText.text = "Arm Destroyed";
            leftGunHitsText.text = "";
            leftGunDamageText.text = "";
            leftGunHitChanceText.text = "";
            leftWeaponBar.enabled = false;
            leftWeaponCircle.transform.parent.GetComponent<Button>().enabled = false;
        }
        
        Gun right = _selectedChar.GetRightGun();
        if (_selectedChar.RightGunAlive() && right)
        {
            rightGunTypeText.text = right.GetGunTypeString();

            string b = right.GetAvailableBullets().ToString();
            rightGunHitsText.text = b;

            string dmg = right.GetBulletDamage().ToString();
            rightGunDamageText.text = dmg;

            string h = right.GetHitChance().ToString();
            rightGunHitChanceText.text = h + "%";
            
            rightWeaponBar.enabled = true;
            rightWeaponCircle.transform.parent.GetComponent<Button>().enabled = true;
        }
        else
        {
            if (_selectedChar.RightGunAlive()) rightGunTypeText.text = "No gun";

            else
            {
                rightGunTypeText.text = "Arm Destroyed";
                rightGunHitsText.text = "";
                rightGunDamageText.text = "";
                rightGunHitChanceText.text = "";
                rightWeaponBar.enabled = false;
                rightWeaponCircle.transform.parent.GetComponent<Button>().enabled = false;
            }
            
        }

        var body = _selectedChar.GetBody();
        ConfigureEquipmentButton(bodyEquipmentButton, body.GetAbility());

        var leftGun = _selectedChar.GetLeftGun();

        if (leftGun)
        {
            ConfigureEquipmentButton(leftGunEquipmentButton, leftGun.GetAbility());
        }
        else ConfigureEquipmentButton(leftGunEquipmentButton, null);
        
        var rightGun = _selectedChar.GetRightGun();
        
        if (rightGun)
        {
            ConfigureEquipmentButton(rightGunEquipmentButton, rightGun.GetAbility());
        }
        else ConfigureEquipmentButton(rightGunEquipmentButton, null);

        var legs = _selectedChar.GetLegs();
        
        ConfigureEquipmentButton(legsEquipmentButton, legs.GetAbility());

        if (body.GetItem())
        {
            ConfigureEquipmentButton(itemEquipmentButton, body.GetItem());
        }
        else ConfigureEquipmentButton(itemEquipmentButton, null);
    }

    void ConfigureEquipmentButton(EquipmentButton button, Equipable equipable)
    {
        button.Reset();

        if (equipable == null)
        {
            button.interactable = false;
            button.SetButtonIcon(noneIcon);
            return;
        }
        
        equipable.SetButton(button);

        button.SetButtonIcon(equipable.GetIcon());
        
        button.SetCharacter(_selectedChar);
            
        button.AddLeftClick(equipable.Select);
        
        //This deactivates all buttons except the clicked one
        button.AddLeftClick(() =>
        {
            //TODO: descomentar si hace falta
            //_selectedChar.GetSelectedEquipable().Deselect();
            _equipmentButtonsPreviousState.Clear();
            
            if (button != bodyEquipmentButton)
            {
                _equipmentButtonsPreviousState.Add(bodyEquipmentButton, bodyEquipmentButton.IsInteractable());
                bodyEquipmentButton.interactable = false;
            }
            
            if (button != leftGunEquipmentButton)
            {
                _equipmentButtonsPreviousState.Add(leftGunEquipmentButton, leftGunEquipmentButton.IsInteractable());
                leftGunEquipmentButton.interactable = false;
            }
            
            if (button != rightGunEquipmentButton)
            {
                _equipmentButtonsPreviousState.Add(rightGunEquipmentButton, rightGunEquipmentButton.IsInteractable());
                rightGunEquipmentButton.interactable = false;
            }
            
            if (button != legsEquipmentButton)
            {
                _equipmentButtonsPreviousState.Add(legsEquipmentButton, legsEquipmentButton.IsInteractable());
                legsEquipmentButton.interactable = false;
            }
            
            if (button != itemEquipmentButton)
            {
                _equipmentButtonsPreviousState.Add(itemEquipmentButton, itemEquipmentButton.IsInteractable());
                itemEquipmentButton.interactable = false;
            }
        });
            
        button.AddRightClick(equipable.Deselect);
        
        button.AddRightClick(ActivateEquipablesButtons);
        
        if (equipable.GetEquipableType() == EquipableSO.EquipableType.Passive)
        {
            button.interactable = false;
        }

        else if (equipable.GetEquipableType() == EquipableSO.EquipableType.Item &&
                 equipable.GetAvailableUses() <= 0)
        {
            button.interactable = false;
        }
            
        else if (equipable.CanBeUsed() == false)
        {
            button.interactable = false;
        }
            
        else
        {
            button.interactable = true;
        }
    }

    void ShowUnitHudText()
    {
        playerBodyCurrHp.text = _selectedChar.GetBody().GetCurrentHp().ToString();

        if (_selectedChar.GetLeftGun())
            playerLeftArmCurrHp.text = _selectedChar.GetLeftGun().GetCurrentHp().ToString();
        else playerLeftArmCurrHp.text = "0";

        if (_selectedChar.GetRightGun())
        {
            playerRightArmCurrHp.text = _selectedChar.GetRightGun().GetCurrentHp().ToString(); 
        }
        else playerRightArmCurrHp.text = "0";

        playerLegsCurrHp.text = _selectedChar.GetLegs().GetCurrentHp().ToString();
    }

    public void UpdateBodyHUD(float value)
    {
        playerBodyCurrHp.text = value.ToString();
        playerBodySlider.value = value;
    }
    
    public void UpdateRightArmHUD(float value)
    {
        playerRightArmCurrHp.text = value.ToString();
        playerRightArmSlider.value = value;
    }
    
    public void UpdateLeftArmHUD(float value)
    {
        playerLeftArmCurrHp.text = value.ToString();
        playerLeftArmSlider.value = value;
    }

    public void UpdateLegsHUD(float value)
    {
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
            foreach (var go in _selectedChar.bodyRenderContainer)
            {
                go.SetActive(false);
            }
            if (_selectedChar.gunsOffOnCloseUp)
            {
                if (_selectedChar.GetLeftGun()) _selectedChar.GetLeftGun().ModelsOff();
                
                if (_selectedChar.GetRightGun()) _selectedChar.GetRightGun().ModelsOff();
            }
            //_selectedChar.RotateTowardsEnemy(_selectedEnemy.transform.position, ActivateParts);
            _selectedChar.RotateTowardsEnemy(_selectedEnemy.transform);
            ActivateParts();
        }
    }

    private void ActivateParts()
    {
        if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetBodyPosition(), "Body", false) && _selectedEnemy.GetBody().GetCurrentHp() > 0)
        {
            _bodyInsight = true;
        }
        else
        {
            _bodyInsight = false;
        }

        if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetLArmPosition(), "LGun", false) && _selectedEnemy.GetLeftGun())
        {
            _lArmInsight = true;
        }
        else
        {
            _lArmInsight = false;
        }

        if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetRArmPosition(), "RGun", false) && _selectedEnemy.GetRightGun())
        {
            _rArmInsight = true;
        }
        else
        {
            _rArmInsight = false;
        }


        if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetLegsPosition(), "Legs", false) &&_selectedEnemy.GetLegs().GetCurrentHp() > 0)
        {
            _legsInsight = true;
        }
        else
        {
            _legsInsight = false;
        }
        
        Character[] units = TurnManager.Instance.GetAllUnits();

        foreach (Character u in units)
        {
            if (u == _selectedChar || u == _selectedEnemy) continue;
            
            u.gameObject.SetActive(false);
        }
        _selectedChar.RaysOffDelay();

        //WorldUI ui = _selectedEnemy.GetMyUI();
        //ui.ButtonsEnabling(_bodyInsight, _lArmInsight, _rArmInsight, _legsInsight, this);
        
        
        //ui.SetBodyHpText(_selectedEnemy.GetBody().GetCurrentHp());

        if (_bodyInsight)
        {
            bodyButton.SetCharacter(_selectedEnemy, PartsMechaEnum.body);
            bodyButton.SetHpText(_selectedEnemy.GetBody().GetCurrentHp().ToString());
            bodyButton.UpdateHpSlider(_selectedEnemy.GetBody().GetCurrentHp());
            bodyButton.ButtonEnabling(true, BodyMinus, BodySelection);
        }
        else
        {
            bodyButton.SetCharacter(null, PartsMechaEnum.body);
            bodyButton.SetHpText("0");
            bodyButton.UpdateHpSlider(0);
            bodyButton.ButtonEnabling(false, () => { },() => { });
        }

        if (_lArmInsight)
        {
            if (_selectedEnemy.GetLeftGun())
            {
                //ui.SetLeftArmHpText(_selectedEnemy.GetLeftGun().GetCurrentHp());
                leftGunButton.SetCharacter(_selectedEnemy, PartsMechaEnum.weaponL);
                leftGunButton.SetHpText(_selectedEnemy.GetLeftGun().GetCurrentHp().ToString());
                leftGunButton.UpdateHpSlider(_selectedEnemy.GetLeftGun().GetCurrentHp());
                leftGunButton.ButtonEnabling(true, LeftArmMinus, LeftArmSelection);
            }
        }
        else
        {
            //ui.SetLeftArmHpText(0);
            leftGunButton.SetCharacter(null, PartsMechaEnum.weaponL);
            leftGunButton.SetHpText("0");
            leftGunButton.UpdateHpSlider(0);
            leftGunButton.ButtonEnabling(false, () => { },() => { });
        }

        if (_rArmInsight)
        {
            if (_selectedEnemy.GetRightGun())
            {
                //ui.SetRightArmHpText(_selectedEnemy.GetRightGun().GetCurrentHp());
                rightGunButton.SetCharacter(_selectedEnemy, PartsMechaEnum.weaponR);
                rightGunButton.SetHpText(_selectedEnemy.GetRightGun().GetCurrentHp().ToString());
                rightGunButton.UpdateHpSlider(_selectedEnemy.GetRightGun().GetCurrentHp());
                rightGunButton.ButtonEnabling(true, RightArmMinus, RightArmSelection);
            }
        }
        else
        {
            //ui.SetRightArmHpText(0);
            rightGunButton.SetCharacter(null, PartsMechaEnum.weaponR);
            rightGunButton.SetHpText("0");
            rightGunButton.UpdateHpSlider(0);
            rightGunButton.ButtonEnabling(false, () => { },() => { });
        }

        //ui.SetLegsHpText(_selectedEnemy.GetLegs().GetCurrentHp());

        if (_legsInsight)
        {
            legsButton.SetCharacter(_selectedEnemy, PartsMechaEnum.legL);
            legsButton.SetHpText(_selectedEnemy.GetLegs().GetCurrentHp().ToString());
            legsButton.UpdateHpSlider(_selectedEnemy.GetLegs().GetCurrentHp());
            legsButton.ButtonEnabling(true, LegsMinus, LegsSelection);
        }
        else
        {
            legsButton.SetCharacter(null, default);
            legsButton.SetHpText("0");
            legsButton.UpdateHpSlider(0);
            legsButton.ButtonEnabling(false, () => { },() => { });
        }
       

        //ui.ButtonsContainerSetActive(true);
        SetAttackHUD();
    }

    public void DeactivateBodyPartsContainer()
    {
        if (_selectedEnemy)
        {
            //_selectedEnemy.GetMyUI().ButtonsContainerSetActive(false);
            bodyButton.SetCharacter(null, default);
            bodyButton.SetHpText("0");
            bodyButton.ButtonEnabling(false, () => { },() => { });
            
            leftGunButton.SetCharacter(null, default);
            leftGunButton.SetHpText("0");
            leftGunButton.ButtonEnabling(false, () => { },() => { });
            
            rightGunButton.SetCharacter(null, default);
            rightGunButton.SetHpText("0");
            rightGunButton.ButtonEnabling(false, () => { },() => { });
            
            legsButton.SetCharacter(null, default);
            legsButton.SetHpText("0");
            legsButton.ButtonEnabling(false, () => { },() => { });
        }
            
    }

    public void ActivateExecuteAttackButton()
    {
        buttonExecuteAttack.interactable = true;
    }

    public void DeactivateExecuteAttackButton()
    {
        buttonExecuteAttack.interactable = false;
    }

    public void RightWeaponCircleState(bool state)
    {
        rightWeaponCircle.transform.parent.gameObject.SetActive(state);
    }
    public void LeftWeaponCircleState(bool state)
    {
        leftWeaponCircle.transform.parent.gameObject.SetActive(state);
    }
    public void RightWeaponBarButtonState(bool state)
    {
        rightWeaponBar.enabled = state;
    }
    public void LeftWeaponBarButtonState(bool state)
    {
        leftWeaponBar.enabled = state;
    }
    #endregion

    public void DeactivateEquipablesButtons()
    {
        _equipmentButtonsPreviousState.Clear();
        
        _equipmentButtonsPreviousState.Add(bodyEquipmentButton, bodyEquipmentButton.IsInteractable());
        bodyEquipmentButton.interactable = false;
        
        _equipmentButtonsPreviousState.Add(leftGunEquipmentButton, leftGunEquipmentButton.IsInteractable());
        leftGunEquipmentButton.interactable = false;
        
        _equipmentButtonsPreviousState.Add(rightGunEquipmentButton, rightGunEquipmentButton.IsInteractable());
        rightGunEquipmentButton.interactable = false;
        
        _equipmentButtonsPreviousState.Add(legsEquipmentButton, legsEquipmentButton.IsInteractable());
        legsEquipmentButton.interactable = false;
        
        _equipmentButtonsPreviousState.Add(itemEquipmentButton, itemEquipmentButton.IsInteractable());
        itemEquipmentButton.interactable = false;
    }

    public void ActivateEquipablesButtons()
    {
        foreach (var kv in _equipmentButtonsPreviousState)
        {
            kv.Key.interactable = kv.Value;
        }
        
        _equipmentButtonsPreviousState.Clear();
    }
}
