﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonsUIManager : MonoBehaviour
{
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
    private int _bulletsForLArm;
    private bool _buttonRArmSelected;
    private int _bulletsForRArm;
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
    public EquipmentButton[] equipmentButtons;

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
            DontDestroyOnLoad(gameObject);
        }
    }
    
    private void Start()
    {
        foreach (var button in equipmentButtons)
        {
            button.interactable = false;
            button.gameObject.SetActive(false);
        }
        
        playerHudContainer.SetActive(false);
        _buttonBodySelected = false;
        _buttonLArmSelected = false;
        _buttonRArmSelected = false;
        _buttonLegsSelected = false;
        
        _soundsMenuManager = FindObjectOfType<SoundsMenu>();
    }

    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked) return;
        
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
        
        Gun gun = _selectedChar.GetSelectedGun();
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
        
        Gun gun = _selectedChar.GetSelectedGun();
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
        
        Gun gun = _selectedChar.GetSelectedGun();
        
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
        
        Gun gun = _selectedChar.GetSelectedGun();
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
        
        Gun gun = _selectedChar.GetSelectedGun();
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
        
        Gun gun = _selectedChar.GetSelectedGun();

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
        
        Gun gun = _selectedChar.GetSelectedGun();
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
        
        Gun gun = _selectedChar.GetSelectedGun();
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
        
        Gun gun = _selectedChar.GetSelectedGun();

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
        
        Gun gun = _selectedChar.GetSelectedGun();
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
        
        Gun gun = _selectedChar.GetSelectedGun();
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
        
        DeactivateEndTurnButton();
        var cam = FindObjectOfType<CloseUpCamera>();
        WorldUI ui = _selectedEnemy.GetMyUI();
        ui.SetLimits(_selectedEnemy.GetBody().GetMaxHp(), _selectedEnemy.GetRightArm().GetCurrentHp(), _selectedEnemy.GetLeftArm().GetCurrentHp(), _selectedEnemy.GetLegs().GetMaxHp());
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
            
                
            if (gun.AbilityUsed() == false)
            {
                gun.Ability();
            }
            _selectedChar.DeactivateAttack();
            
            if (_selectedEnemy.GetBody().GetCurrentHp() <= 0 && !_selectedEnemy.IsDead())
            {
                _selectedEnemy.Dead();
            }
            
        }


        if (_bulletsForLArm > 0)
        {
            List<Tuple<int,int>> d = gun.DamageCalculation(_bulletsForLArm);
            _selectedEnemy.GetLeftArm().TakeDamage(d);
            _bulletsForLArm = 0;
                
            if (gun.AbilityUsed() == false)
            {
                gun.Ability();
            }
            _selectedChar.DeactivateAttack();
        }

        if (_bulletsForRArm > 0)
        {
            List<Tuple<int,int>> d = gun.DamageCalculation(_bulletsForRArm);
            _selectedEnemy.GetRightArm().TakeDamage(d);
            _bulletsForRArm = 0;
            if (gun.AbilityUsed() == false)
            {
                gun.Ability();
            }
            _selectedChar.DeactivateAttack();
        }

        if (_bulletsForLegs > 0)
        {
            List<Tuple<int,int>> d = gun.DamageCalculation(_bulletsForLegs);
            _selectedEnemy.GetLegs().TakeDamage(d);
            _bulletsForLegs = 0;
            if (gun.AbilityUsed() == false)
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
        CharacterSelection.Instance.Selection(_selectedChar);
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

                if (_bulletsForLArm > 0)
                    gun.IncreaseAvailableBullets(_bulletsForLArm);

                if (_bulletsForRArm > 0)
                    gun.IncreaseAvailableBullets(_bulletsForRArm);

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
        if (!_selectedChar || !_selectedChar.LeftArmAlive() || !_selectedChar.GetLeftGun()) return;
        
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
        if (!_selectedChar || !_selectedChar.RightArmAlive() || !_selectedChar.GetRightGun()) return;
        
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
        
        WorldUI ui = _selectedEnemy.GetMyUI();
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
        
        if (_selectedChar.RightArmAlive() && _selectedChar.GetRightGun())
        {
            rightWeaponCircle.SetActive(true);
            leftWeaponCircle.SetActive(false);
        }
        else if (_selectedChar.LeftArmAlive() && _selectedChar.GetLeftGun())
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
        playerLeftArmSlider.maxValue = _selectedChar.GetLeftArm().GetMaxHp();
        playerLeftArmSlider.value = _selectedChar.GetLeftArm().GetCurrentHp();
        
        playerRightArmSlider.minValue = 0;
        playerRightArmSlider.maxValue = _selectedChar.GetRightArm().GetMaxHp();
        playerRightArmSlider.value = _selectedChar.GetRightArm().GetCurrentHp();
        
        playerLegsSlider.minValue = 0;
        playerLegsSlider.maxValue = _selectedChar.GetLegs().GetMaxHp();
        playerLegsSlider.value = _selectedChar.GetLegs().GetCurrentHp();

        Gun left = _selectedChar.GetLeftGun();
        if (_selectedChar.LeftArmAlive() && left)
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
            if (_selectedChar.LeftArmAlive()) leftGunTypeText.text = "No gun";
            else leftGunTypeText.text = "Arm Destroyed";
            leftGunHitsText.text = "";
            leftGunDamageText.text = "";
            leftGunHitChanceText.text = "";
            leftWeaponBar.enabled = false;
            leftWeaponCircle.transform.parent.GetComponent<Button>().enabled = false;
        }
        
        Gun right = _selectedChar.GetRightGun();
        if (_selectedChar.RightArmAlive() && right)
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
            if (_selectedChar.RightArmAlive()) rightGunTypeText.text = "No gun";
            else rightGunTypeText.text = "Arm Destroyed";
            rightGunHitsText.text = "";
			rightGunDamageText.text = "";
			rightGunHitChanceText.text = "";
            rightWeaponBar.enabled = false;
            rightWeaponCircle.transform.parent.GetComponent<Button>().enabled = false;
        }

        //Item item = _selectedChar.GetItem();

        List<Equipable> equipables = _selectedChar.GetEquipables();

        if (equipables != null && equipables.Count > 0)
        {
            for (int i = 0; i < equipmentButtons.Length; i++)
            {
                if (i > equipables.Count-1)
                {
                    return;
                }
                
                var button = equipmentButtons[i];
                var equipment = equipables[i];
                
                equipment.SetButton(button);
                button.gameObject.SetActive(true);
                Ability ability;
                switch (equipment.GetEquipableType())
                {
                    case EquipableSO.EquipableType.Item:
                        var item = equipment as Item;
                        if (item)
                        {
                            button.SetItemButtonName(item.GetEquipableName() + " x" + item.GetItemUses());

                            if (item.GetItemUses() > 0)
                            {
                                button.ClearLeftClick();
                                button.AddLeftClick(item.Select);
                                
                                button.ClearRightClick();
                                button.AddRightClick(item.Deselect);

                                button.SetCharacter(_selectedChar);
                                button.interactable = true;
                            }
                            
                        }
                        else button.interactable = false;
                            
                        break;
                    case EquipableSO.EquipableType.PassiveAbility:
                        ability = equipment as Ability;
                        if (ability)
                        {
                            button.SetItemButtonName(ability.GetEquipableName() + " " + ability.AbilityStatus());
                            button.interactable = false;
                        }
                        break;
                    case EquipableSO.EquipableType.ActiveAbility:
                        ability = equipment as Ability;
                        if (ability)
                        {
                            button.SetItemButtonName(ability.GetEquipableName() + " " + ability.AbilityStatus());
                            button.ClearLeftClick();
                            button.AddLeftClick(ability.Select);

                            button.ClearRightClick();
                            button.AddRightClick(ability.Deselect);

                            button.SetCharacter(_selectedChar);
                            button.interactable = true;
                        }
                        break;
                }
            }
        }
    }

    void ShowUnitHudText()
    {
        playerBodyCurrHp.text = _selectedChar.GetBody().GetCurrentHp().ToString();
        playerLeftArmCurrHp.text = _selectedChar.GetLeftArm().GetCurrentHp().ToString();
        playerRightArmCurrHp.text = _selectedChar.GetRightArm().GetCurrentHp().ToString();
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

        if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetLArmPosition(), "LArm", false) && _selectedEnemy.GetLeftArm().GetCurrentHp() > 0)
        {
            _lArmInsight = true;
        }
        else
        {
            _lArmInsight = false;
        }

        if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetRArmPosition(), "RArm", false) && _selectedEnemy.GetRightArm().GetCurrentHp() > 0)
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

        WorldUI ui = _selectedEnemy.GetMyUI();
        ui.ButtonsEnabling(_bodyInsight, _lArmInsight, _rArmInsight, _legsInsight, this);
        
        
        ui.SetBodyHpText(_selectedEnemy.GetBody().GetCurrentHp());

        ui.SetLeftArmHpText(_selectedEnemy.GetLeftArm().GetCurrentHp());

        ui.SetRightArmHpText(_selectedEnemy.GetRightArm().GetCurrentHp());

        ui.SetLegsHpText(_selectedEnemy.GetLegs().GetCurrentHp());

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

    public void EquipmentButtonState(EquipmentButton button, bool state)
    {
        button.interactable = state;
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
}
