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
    
    //[Header("Keys")]
    //public KeyCode deselectKey;
    //public KeyCode selectLGunKey;
    //public KeyCode selectRGunKey;
    //public KeyCode showWorldUIKey;
    //public KeyCode toggleWorldUIKey;

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

    //[Header("Player HUD")]
    #region HUD
    //Player
    //public GameObject playerHudContainer;
    [Space]
    
    //[Header("Attack HUD")]
    
    //Weapon Attack
    //public GameObject attackHudContainer;
    //public TextMeshProUGUI attackWeaponNameText;
    //public TextMeshProUGUI attackWeaponHitsText;
    //public TextMeshProUGUI attackWeaponDamageText;
    //public TextMeshProUGUI attackWeaponHitChanceText;
    //public GameObject hitContainer;
    //public GameObject hitImagePrefab;
    //private List<GameObject> _hitImagesCreated = new List<GameObject>();
    
    #endregion

    //OTHERS
    private Character _selectedChar;
    private Character _selectedEnemy;
    //private int _partsSelected;
    private SoundsMenu _soundsMenuManager;

    //private bool _bodyInsight;
    //private bool _legsInsight;
    //private bool _rArmInsight;
    //private bool _lArmInsight;

    //private bool _worldUIActive = false;
    //private bool _worldUIToggled;
    //public static ButtonsUIManager Instance;
    

    //private void Awake()
    //{
    //    if (Instance != null)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        Instance = this;
    //        //DontDestroyOnLoad(gameObject);
    //    }
    //}
    
    private void Start()
    {
     //   playerHudContainer.SetActive(false);
        //_buttonBodySelected = false;
        //_buttonLArmSelected = false;
        //_buttonRArmSelected = false;
        //_buttonLegsSelected = false;
        
        //_soundsMenuManager = FindObjectOfType<SoundsMenu>();
        
        //bodyButton.ShowButton();
        //leftGunButton.ShowButton();
        //rightGunButton.ShowButton();
        //legsButton.ShowButton();
    }

    private void Update()
    {
        //if (EventSystem.current.IsPointerOverGameObject() == false)
        //{
        //    //if (_selectedChar && _selectedChar.IsMoving() == false && _selectedEnemy &&
        //    //    Input.GetKeyDown(deselectKey))
        //    //{
        //    //    _selectedChar.ResetRotationOnDeselect();
        //    //    DeselectActions();
        //    //    ClearSelectedEnemy();
        //    //}
                

        //    //if ((_selectedChar && _selectedChar.IsMoving() == false) && _selectedChar.GetPath().Count > 0 &&
        //    //    Input.GetMouseButtonDown(1))
        //    //{
        //    //    _selectedChar.GetWaypointsPathfinding().UndoLastWaypoint();
        //    //}
                
        //}

        //if (_selectedChar || (_selectedChar && _selectedEnemy && _selectedEnemy.IsSelectedForAttack() == false))
        //{
        //    if (Input.GetKeyDown(selectLGunKey))
        //        UnitSwapToLeftGun();

        //    if (Input.GetKeyDown(selectRGunKey))
        //        UnitSwapToRightGun();
        //}
        
        
        //if (_selectedChar && _selectedEnemy) return;

        //if (!_worldUIToggled)
        //{
        //    if (Input.GetKeyDown(showWorldUIKey))
        //        ShowAllWorldUI();
        
        //    if (Input.GetKeyUp(showWorldUIKey))
        //        HideAllWorldUI(); 
        //}
        
            
        //if (Input.GetKeyDown(toggleWorldUIKey))
        //{
        //    if (_worldUIActive)
        //    {
        //        ToggleWorldUI(false);
        //    }
        //    else ToggleWorldUI(true);
        //}
    }
    #region ButtonsActions

    
    //Se ejecuta cuando se hace click izquierdo en el boton de body
    //public void BodySelection()
    //{
    //    if (!CharacterHasBullets(_selectedChar)) return;
        
    //    Gun gun = _selectedChar.GetSelectedGun();
        
    //    if (_partsSelected > gun.GetAvailableSelections()) return;
        
    //    if (_bulletsForBody == 0)
    //    {
    //        _partsSelected++;
    //    }
    //    _bulletsForBody += gun.GetBulletsPerClick();
    //    DestroyImage(gun.GetBulletsPerClick());
    //    gun.ReduceAvailableBullets();
    //    //_selectedEnemy.GetMyUI().SetBodyCount(_bulletsForBody);
    //    bodyButton.SetBulletsCount(_bulletsForBody);
    //    buttonExecuteAttack.interactable = true;
    //    buttonExecuteAttack.gameObject.SetActive(true);
    //    _buttonBodySelected = true;
    //    DeterminateButtonsActivation();

    //}

    //Se ejecuta cuando se hace click derecho en el boton de body
    //public void BodyMinus()
    //{
    //    if (_bulletsForBody <= 0) return;
        
    //    Gun gun = _selectedChar.GetSelectedGun();
    //    gun.IncreaseAvailableBullets();
    //    _bulletsForBody = _bulletsForBody > 0 ? (_bulletsForBody - gun.GetBulletsPerClick()) : 0;
    //    //_selectedEnemy.GetMyUI().SetBodyCount(_bulletsForBody);
    //    bodyButton.SetBulletsCount(_bulletsForBody);
    //    CheckIfCanExecuteAttack();
    //    CreateImage(gun.GetBulletsPerClick());
    //    if (_bulletsForBody == 0)
    //    {
    //        if (_partsSelected > 0)
    //            _partsSelected--;
    //        _buttonBodySelected = false;
    //    }
    //    DeterminateButtonsActivation();
    //}
    
    
    //Se ejecuta al cambiar de arma
    //private void BodyClear()
    //{
    //    if (_bulletsForBody <= 0) return;
        
    //    Gun gun = _selectedChar.GetSelectedGun();
    //    gun.IncreaseAvailableBullets(_bulletsForBody);
    //    DestroyImage(gun.GetMaxBullets());
    //    _hitImagesCreated.Clear();
    //    _bulletsForBody = 0;
    //    _buttonBodySelected = false;
    //    if (_selectedEnemy)
    //        //_selectedEnemy.GetMyUI().SetBodyCount(_bulletsForBody);
    //        bodyButton.SetBulletsCount(_bulletsForBody);
    //    if (_partsSelected > 0)
    //        _partsSelected--;
    //    CheckIfCanExecuteAttack();
    //    DeterminateButtonsActivation();
    //}

    //Se ejecuta cuando se hace click izquierdo en el boton de left arm
    //public void LeftArmSelection()
    //{
    //    if (!CharacterHasBullets(_selectedChar)) return;
        
    //    Gun gun = _selectedChar.GetSelectedGun();
        
    //    if (!gun) return;
        
    //    if (_partsSelected > gun.GetAvailableSelections()) return;
        
    //    if (_bulletsForLGun == 0)
    //    {
    //        _partsSelected++;
    //    }
    //    _bulletsForLGun += gun.GetBulletsPerClick();
    //    DestroyImage(gun.GetBulletsPerClick());
    //    //_selectedEnemy.GetMyUI().SetLeftArmCount(_bulletsForLGun);
    //    leftGunButton.SetBulletsCount(_bulletsForLGun);
    //    gun.ReduceAvailableBullets();

    //    buttonExecuteAttack.interactable = true;
    //    buttonExecuteAttack.gameObject.SetActive(true);
    //    _buttonLArmSelected = true;
    //    DeterminateButtonsActivation();
    //}
    
    //Se ejecuta cuando se hace click derecho en el boton de left arm
    //public void LeftArmMinus()
    //{
    //    if (_bulletsForLGun <= 0) return;
        
    //    Gun gun = _selectedChar.GetSelectedGun();
    //    gun.IncreaseAvailableBullets();
    //    _bulletsForLGun = _bulletsForLGun > 0 ? (_bulletsForLGun - gun.GetBulletsPerClick()) : 0;
    //    //_selectedEnemy.GetMyUI().SetLeftArmCount(_bulletsForLGun);
    //    leftGunButton.SetBulletsCount(_bulletsForLGun);
    //    CheckIfCanExecuteAttack();
    //    CreateImage(gun.GetBulletsPerClick());
    //    if (_bulletsForLGun == 0)
    //    {
    //        if (_partsSelected > 0)
    //            _partsSelected--;
    //        _buttonLArmSelected = false;
    //    }
    //    DeterminateButtonsActivation();
    //}
    
    //Se ejecuta al cambiar de arma
    //private void LeftArmClear()
    //{
    //    if (_bulletsForLGun <= 0) return;
        
    //    Gun gun = _selectedChar.GetSelectedGun();
    //    gun.IncreaseAvailableBullets(_bulletsForLGun);
    //    DestroyImage(gun.GetMaxBullets());
    //    _bulletsForLGun = 0;
    //    if (_selectedEnemy)
    //        //_selectedEnemy.GetMyUI().SetLeftArmCount(_bulletsForLGun);
    //        leftGunButton.SetBulletsCount(_bulletsForLGun);
    //    _buttonLArmSelected = false;
    //    if (_partsSelected > 0)
    //        _partsSelected--;
    //    CheckIfCanExecuteAttack();
    //    DeterminateButtonsActivation();
    //}
    
    
    //Se ejecuta cuando se hace click izquierdo en el boton de right arm
    //public void RightArmSelection()
    //{
    //    if (!CharacterHasBullets(_selectedChar)) return;
        
    //    Gun gun = _selectedChar.GetSelectedGun();

    //    if (!gun) return;
        
    //    if (_partsSelected > gun.GetAvailableSelections()) return;
        
    //    if (_bulletsForRGun == 0)
    //    {
    //        _partsSelected++;
    //    }
    //    _bulletsForRGun += gun.GetBulletsPerClick();
    //    DestroyImage(gun.GetBulletsPerClick());
    //    //_selectedEnemy.GetMyUI().SetRightArmCount(_bulletsForRGun);
    //    rightGunButton.SetBulletsCount(_bulletsForRGun);
    //    gun.ReduceAvailableBullets();
    //    buttonExecuteAttack.interactable = true;
    //    buttonExecuteAttack.gameObject.SetActive(true);
    //    _buttonRArmSelected = true;
    //    DeterminateButtonsActivation();
    //}

    //Se ejecuta cuando se hace click derecho en el boton de right arm
    //public void RightArmMinus()
    //{
    //    if (_bulletsForRGun <= 0) return;
        
    //    Gun gun = _selectedChar.GetSelectedGun();
    //    gun.IncreaseAvailableBullets();
    //    _bulletsForRGun = _bulletsForRGun > 0 ? (_bulletsForRGun - gun.GetBulletsPerClick()) : 0;
    //    //_selectedEnemy.GetMyUI().SetRightArmCount(_bulletsForRGun);
    //    rightGunButton.SetBulletsCount(_bulletsForRGun);
    //    CreateImage(gun.GetBulletsPerClick());
    //    CheckIfCanExecuteAttack();
    //    if (_bulletsForRGun == 0)
    //    {
    //        if (_partsSelected > 0)
    //            _partsSelected--;
    //        _buttonRArmSelected = false;
    //    }
    //    DeterminateButtonsActivation();
    //}

    //Se ejecuta al cambiar de arma
    //private void RightArmClear()
    //{
    //    if (_bulletsForRGun <= 0) return;
        
    //    Gun gun = _selectedChar.GetSelectedGun();
    //    gun.IncreaseAvailableBullets(_bulletsForRGun);
    //    DestroyImage(gun.GetMaxBullets());
    //    _bulletsForRGun = 0;
    //    if (_selectedEnemy)
    //        //_selectedEnemy.GetMyUI().SetRightArmCount(_bulletsForRGun);
    //        rightGunButton.SetBulletsCount(_bulletsForRGun);

    //    _buttonRArmSelected = false;
    //    if (_partsSelected > 0)
    //        _partsSelected--;
    //    CheckIfCanExecuteAttack();
    //    DeterminateButtonsActivation();
    //}

    //Se ejecuta cuando se hace click izquierdo en el boton de legs
    //public void LegsSelection()
    //{
    //    if (!CharacterHasBullets(_selectedChar)) return;
        
    //    Gun gun = _selectedChar.GetSelectedGun();

    //    if (_partsSelected > gun.GetAvailableSelections()) return;
        
    //    if (_bulletsForLegs == 0)
    //    {
    //        _partsSelected++;
    //    }
    //    _bulletsForLegs += gun.GetBulletsPerClick();
    //    DestroyImage(gun.GetBulletsPerClick());
    //    //_selectedEnemy.GetMyUI().SetLegsCount(_bulletsForLegs);
    //    legsButton.SetBulletsCount(_bulletsForLegs);

    //    gun.ReduceAvailableBullets();
    //    buttonExecuteAttack.interactable = true;
    //    buttonExecuteAttack.gameObject.SetActive(true);
    //    _buttonLegsSelected = true;
    //    DeterminateButtonsActivation();
    //}
    
    //Se ejecuta cuando se hace click derecho en el boton de legs
    //public void LegsMinus()
    //{
    //    if (_bulletsForLegs <= 0) return;
        
    //    Gun gun = _selectedChar.GetSelectedGun();
    //    gun.IncreaseAvailableBullets();
    //    _bulletsForLegs = _bulletsForLegs > 0 ? (_bulletsForLegs - gun.GetBulletsPerClick()) : 0;
    //    //_selectedEnemy.GetMyUI().SetLegsCount(_bulletsForLegs);
    //    legsButton.SetBulletsCount(_bulletsForLegs);
    //    CreateImage(gun.GetBulletsPerClick());
    //    CheckIfCanExecuteAttack();
    //    if (_bulletsForLegs == 0)
    //    {
    //        if (_partsSelected > 0)
    //            _partsSelected--;
    //        _buttonLegsSelected = false;
    //    }
    //    DeterminateButtonsActivation();
    //}

    //Se ejecuta al cambiar de arma
    //private void LegsClear()
    //{
    //    if (_bulletsForLegs <= 0) return;
        
    //    Gun gun = _selectedChar.GetSelectedGun();
    //    gun.IncreaseAvailableBullets(_bulletsForLegs);
    //    DestroyImage(gun.GetMaxBullets());
    //    _bulletsForLegs = 0;
    //    if (_selectedEnemy)
    //        //_selectedEnemy.GetMyUI().SetLegsCount(_bulletsForLegs);
    //        legsButton.SetBulletsCount(_bulletsForLegs);
    //    _buttonLegsSelected = false;
    //    if (_partsSelected > 0)
    //        _partsSelected--;
    //    CheckIfCanExecuteAttack();
    //    DeterminateButtonsActivation();
    //}
    
    
    /// <summary>
    /// Executed when left-clicking Fire button.
    /// </summary>
    //public void ExecuteAttack()
    //{
        //if (!_selectedEnemy)
        //    return;
        
        //DeactivateEndTurnButton();
        //CloseUpCamera cam = FindObjectOfType<CloseUpCamera>();
        //WorldUI ui = _selectedEnemy.GetMyUI();

        //ui.ResetButtons();
        //bodyButton.ResetButton();
        //rightGunButton.ResetButton();
        //leftGunButton.ResetButton();
        //legsButton.ResetButton();
        
        //float rightGunHP = 0;

        //if (_selectedEnemy.GetRightGun())
        //    rightGunHP = _selectedEnemy.GetRightGun().CurrentHP;
        
        //float leftGunHP = 0;

        //if (_selectedEnemy.GetLeftGun())
        //    leftGunHP = _selectedEnemy.GetLeftGun().CurrentHP;
        
        //ui.SetLimits(_selectedEnemy.GetBody().MaxHp, rightGunHP, leftGunHP, _selectedEnemy.GetLegs().MaxHp);

        //buttonExecuteAttack.interactable = false;

        //buttonExecuteAttack.gameObject.SetActive(false);

        //_selectedChar.ResetInRangeLists();

        //DeactivateBodyPartsContainer();

        //attackHudContainer.SetActive(false);

        //if (_selectedChar.GunsOffOnCloseup())
        //{
        //    if (_selectedChar.GetLeftGun())
        //        _selectedChar.GetLeftGun().ChangeMeshRenderStatus(true);
            
        //    if (_selectedChar.GetRightGun())
        //        _selectedChar.GetRightGun().ChangeMeshRenderStatus(true);
        //}
        
        //Character[] units = TurnManager.Instance.GetAllUnits();
        //foreach (Character unit in units)
        //{
        //    unit.ShowMechaMesh();
        //    unit.ShowGunsMesh();
        //}
        //cam.MoveCameraToParent(cam.transform.parent.position, _selectedEnemy.transform.position, Attack, attackDelay);
    //}

    /// <summary>
    /// Executed when CloseUp Camera returns to it's original position.
    /// </summary>
    //private void Attack()
    //{
    //    //_selectedChar.RotateTowardsEnemy(_selectedEnemy.transform);
    //    //_selectedChar.SetInitialRotation(_selectedChar.transform.rotation);

    //    //Gun gun = _selectedChar.GetSelectedGun();
    //    ////_selectedChar.Shoot();
    //    //_selectedEnemy.SetHurtAnimation();
    //    //if (_bulletsForBody > 0)
    //    //{
    //    //    List<Tuple<int,int>> d = gun.GetCalculatedDamage(_bulletsForBody);
    //    //    _selectedEnemy.GetBody().ReceiveDamage(d);
    //    //    _bulletsForBody = 0;
            
                
    //    //    if (gun.IsGunSkillAvailable())
    //    //    {
    //    //        gun.GunSkill();
    //    //    }
    //    //    _selectedChar.DeactivateAttack();
            
    //    //    if (_selectedEnemy.GetBody().CurrentHP <= 0 && !_selectedEnemy.IsDead())
    //    //    {
    //    //        _selectedEnemy.Dead();
    //    //    }
            
    //    //}


    //    //if (_bulletsForLGun > 0)
    //    //{
    //    //    if (_selectedEnemy.GetLeftGun())
    //    //    {
    //    //        List<Tuple<int,int>> d = gun.GetCalculatedDamage(_bulletsForLGun);
    //    //        _selectedEnemy.GetLeftGun().ReceiveDamage(d);
    //    //        _bulletsForLGun = 0;
                
    //    //        if (gun.IsGunSkillAvailable() == false)
    //    //        {
    //    //            gun.GunSkill();
    //    //        }
    //    //        _selectedChar.DeactivateAttack(); 
    //    //    }
    //    //}

    //    //if (_bulletsForRGun > 0)
    //    //{
    //    //    if (_selectedEnemy.GetRightGun())
    //    //    {
    //    //        List<Tuple<int,int>> d = gun.GetCalculatedDamage(_bulletsForRGun);
    //    //        _selectedEnemy.GetRightGun().ReceiveDamage(d);
    //    //        _bulletsForRGun = 0;
    //    //        if (gun.IsGunSkillAvailable() == false)
    //    //        {
    //    //            gun.GunSkill();
    //    //        }
    //    //        _selectedChar.DeactivateAttack(); 
    //    //    }
            
    //    //}

    //    //if (_bulletsForLegs > 0)
    //    //{
    //    //    List<Tuple<int,int>> d = gun.GetCalculatedDamage(_bulletsForLegs);
    //    //    _selectedEnemy.GetLegs().ReceiveDamage(d);
    //    //    _bulletsForLegs = 0;
    //    //    if (gun.IsGunSkillAvailable() == false)
    //    //    {
    //    //        gun.GunSkill();
    //    //    }
    //    //    _selectedChar.DeactivateAttack();
    //    //}
        
    //    //_selectedEnemy.NotSelectedForAttack();
    //    PortraitsController.Instance.PortraitsActiveState(true);
        
    //    //if (_selectedChar.GetUnitTeam() == EnumsClass.Team.Green)
    //    //    ActivateEndTurnButton();
    //    //DeselectActions();
    //    //CharacterSelection.Instance.Selection(_selectedChar);
    //}

    //public void EndTurn()
    //{
    //    //if (_selectedChar != null && _selectedChar.IsMoving()) return;
        
    //    //_selectedChar = null;
    //    //_selectedEnemy = null;
    //    TurnManager.Instance.EndTurn();
    //    //DeactivateBodyPartsContainer();
    //    //DeactivatePlayerHUD();
    //    //ResetBodyParts();
    //}

    //public void DeselectActions()
    //{
    //    if (_selectedChar)
    //    {
    //        if (_selectedEnemy)
    //        {
    //            //attackHudContainer.SetActive(false);
    //            //Character[] units = TurnManager.Instance.GetAllUnits();
    //            //foreach (Character u in units)
    //            //{
    //            //    u.ChangePartsMeshActiveStatus(true);
    //            //    u.ChangeGunsMeshActiveStatus(true);
    //            //}
    //            //var cam = FindObjectOfType<CloseUpCamera>();
    //            //_selectedChar.CancelEnemySelection();
    //            //_selectedChar.ResetRotationAndRays();

    //            //cam.MoveCameraToParent(cam.transform.parent.position, _selectedEnemy.transform.position, cam.ResetCamera);
    //            //cam.MoveCameraToParent(_selectedEnemy.transform.position, cam.ResetCamera);
    //        }
    //        //Gun gun = _selectedChar.GetSelectedGun();

    //        //if (gun)
    //        //{
    //        //    if (_bulletsForBody > 0)
    //        //        gun.IncreaseAvailableBullets(_bulletsForBody);

    //        //    if (_bulletsForLGun > 0)
    //        //        gun.IncreaseAvailableBullets(_bulletsForLGun);

    //        //    if (_bulletsForRGun > 0)
    //        //        gun.IncreaseAvailableBullets(_bulletsForRGun);

    //        //    if (_bulletsForLegs > 0)
    //        //        gun.IncreaseAvailableBullets(_bulletsForLegs);
            
    //        //    //DestroyImage(gun.GetMaxBullets());
    //        //}
            
            
    //        //ResetBodyParts();
    //    }

    //    //if (_selectedEnemy)
    //    //{
    //    //    _selectedEnemy.NotSelectedForAttack();
    //    //}
        
    //    //CharacterSelection.Instance.ActivateCharacterSelection();

    //    //if (_selectedChar)
    //    //    CharacterSelector.Instance.SelectionWithDelay(_selectedChar);
        
    //    //DeactivateBodyPartsContainer();

    //    //if (TurnManager.Instance.GetActiveTeam() == EnumsClass.Team.Green)
    //    //    ActivateEndTurnButton();
    //    //buttonExecuteAttack.gameObject.SetActive(false);
    //    //PortraitsController.Instance.PortraitsActiveState(true);
        
    //}

    //private void ClearSelectedEnemy()
    //{
    //    _selectedEnemy = null;
    //}

    /// <summary>
    /// Change to Left Gun of selected Character when pressig on UI or key.
    /// </summary>
    //public void UnitSwapToLeftGun()
    //{
    //    //if (!_selectedChar || !_selectedChar.GetLeftGun())
    //    //    return;
        
    //    //if (_selectedChar.IsOnElevator() && _selectedChar.GetLeftGun().GetGunType() == EnumsClass.GunsType.Melee) return;
        
    //    //AudioManager.audioManagerInstance.PlaySound(_soundsMenuManager.GetClickSound(), _soundsMenuManager.GetObjectToAddAudioSource());
    //    //BodyClear();
    //    //LeftArmClear();
    //    //RightArmClear();
    //    //LegsClear();
    //    //_selectedChar.SelectLeftGun();

    //    if (_selectedChar.HasEnemiesInRange())
    //        ActivateBodyPartsContainer();
    //    else DeactivateBodyPartsContainer();
    //}

    /// <summary>
    /// Change to Right Gun of selected Character when pressig on UI or key.
    /// </summary>
    //public void UnitSwapToRightGun()
    //{
    //    //if (!_selectedChar || !_selectedChar.GetRightGun())
    //    //    return;
        
    //    //AudioManager.audioManagerInstance.PlaySound(_soundsMenuManager.GetClickSound(), _soundsMenuManager.GetObjectToAddAudioSource());
    //    //BodyClear();
    //    //LeftArmClear();
    //    //RightArmClear();
    //    //LegsClear();
    //    //_selectedChar.SelectRightGun();

    //    if (_selectedChar.HasEnemiesInRange())
    //        ActivateBodyPartsContainer();
    //    else DeactivateBodyPartsContainer();
    //}

    //Muestro el WorldCanvas de todas las unidades
    //private void ShowAllWorldUI()
    //{
    //    _worldUIActive = true;
    //    Character[] units = TurnManager.Instance.GetAllUnits();
    //    foreach (Character unit in units)
    //    {
    //        if (!unit.IsSelectedForAttack() && unit.CanBeSelected())
    //        {
    //            unit.SetWorldUIValues();
    //            unit.GetWorldUI().Show();
    //        }
    //    }
    //}

    //Oculto el WorldCanvas de todas las unidades
    //private void HideAllWorldUI()
    //{
    //    _worldUIActive = false;
    //    Character[] units = TurnManager.Instance.GetAllUnits();
    //    foreach (Character unit in units)
    //    {
    //        unit.GetWorldUI().Hide();
    //    }
    //}

    //private void ToggleWorldUI(bool state)
    //{
    //    if (state)
    //    {
    //        _worldUIActive = true;
    //        _worldUIToggled = true;
    //        Character[] units = TurnManager.Instance.GetAllUnits();

    //        foreach (Character unit in units)
    //        {
    //            if (unit.IsDead()) continue;
                
    //            if (!unit.IsSelectedForAttack() && unit.CanBeSelected())
    //            {
    //                unit.SetWorldUIValues();
    //                WorldUI worldUI = unit.GetWorldUI();
    //                worldUI.Show();
    //                worldUI.Toggle(true);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        _worldUIActive = false;
    //        _worldUIToggled = false;
    //        Character[] units = TurnManager.Instance.GetAllUnits();
    //        foreach (Character unit in units)
    //        {
    //            WorldUI worldUI = unit.GetWorldUI();
    //            worldUI.Hide();
    //            worldUI.Toggle(true);
    //        }
    //    }
    //}

    #endregion

    #region Utilities

    //public void AddBulletsToBody(int quantity)
    //{
    //    _bulletsForBody = quantity;
    //    _bulletsForLGun = 0;
    //    _bulletsForRGun = 0;
    //    _bulletsForLegs = 0;
    //    Attack();
    //}

    //public void AddBulletsToLArm(int quantity)
    //{
    //    _bulletsForBody = 0;
    //    _bulletsForLGun = quantity;
    //    _bulletsForRGun = 0;
    //    _bulletsForLegs = 0;
    //    Attack();
    //}

    //public void AddBulletsToRArm(int quantity)
    //{
    //    _bulletsForBody = 0;
    //    _bulletsForLGun = 0;
    //    _bulletsForRGun = quantity;
    //    _bulletsForLegs = 0;
    //    Attack();
    //}

    //public void AddBulletsToLegs(int quantity)
    //{
    //    _bulletsForBody = 0;
    //    _bulletsForLGun = 0;
    //    _bulletsForRGun = 0;
    //    _bulletsForLegs = quantity;
    //    Attack();
    //}

    //private bool CharacterHasBullets(Character c)
    //{
    //    return c.GetSelectedGun().GetAvailableBullets() > 0;
    //}

    //Checks if player can attack en enemy.
    //private void CheckIfCanExecuteAttack()
    //{
    //    if (_bulletsForBody == 0 && _bulletsForLGun == 0 && _bulletsForLegs == 0 && _bulletsForRGun == 0)
    //        buttonExecuteAttack.interactable = false;
    //}

    //Determines which buttons will be interactable.
    //private void DeterminateButtonsActivation()
    //{
    //    if (!_selectedChar)
    //        return;
        
    //    //WorldUI ui = _selectedEnemy.GetMyUI();
    //    if (_selectedChar.GetSelectedGun().GetAvailableBullets() <= 0 || _partsSelected == _selectedChar.GetSelectedGun().GetAvailableSelections())
    //    {
    //        if (_buttonBodySelected == false)
    //        {
    //            //ui.BodyEnabling(false);
    //            bodyButton.HideButton();
    //        }

    //        if (_buttonLArmSelected == false)
    //        {
    //            //ui.LeftArmEnabling(false);
    //            leftGunButton.HideButton();
    //        }

    //        if (_buttonRArmSelected == false)
    //        {
    //            //ui.RightArmEnabling(false);
    //            rightGunButton.HideButton();
    //        }

    //        if (_buttonLegsSelected == false)
    //        {
    //            //ui.LegsEnabling(false);
    //            legsButton.HideButton();
    //        }
    //        buttonExecuteAttack.interactable = true;
    //        buttonExecuteAttack.gameObject.SetActive(true);
    //    }
    //    else if (_partsSelected < _selectedChar.GetSelectedGun().GetAvailableSelections())
    //    {
    //        //if (!_buttonBodySelected && _bodyInsight)
    //        //{
    //        //    //ui.BodyEnabling(true, this);
    //        //    bodyButton.ShowButton();
    //        //}

    //        //if (!_buttonLArmSelected && _lArmInsight)
    //        //{
    //        //    //ui.LeftArmEnabling(true, this);
    //        //    leftGunButton.ShowButton();
    //        //}

    //        //if (!_buttonRArmSelected && _rArmInsight)
    //        //{
    //        //    //ui.RightArmEnabling(true, this);
    //        //    rightGunButton.ShowButton();
    //        //}

    //        //if (!_buttonLegsSelected && _legsInsight)
    //        //{
    //        //    //ui.LegsEnabling(true, this);
    //        //    legsButton.ShowButton();
    //        //}

    //        //if (_partsSelected <= 0)
    //        //{
    //        //    buttonExecuteAttack.gameObject.SetActive(false);
    //        //}
    //    }
    //}

    //private void ResetBodyParts()
    //{
    //    _buttonBodySelected = false;
    //    _bulletsForBody = 0;

    //    _buttonLArmSelected = false;
    //    _bulletsForLGun = 0;

    //    _buttonRArmSelected = false;
    //    _bulletsForRGun = 0;

    //    _buttonLegsSelected = false;
    //    _bulletsForLegs = 0;

    //    //_partsSelected = 0;
    //}

    //void CreateImage(int quantity)
    //{
    //    int c = 0;
    //    for (int i = 0; i < quantity; i++)
    //    {
    //        c++;
    //        GameObject image = Instantiate(hitImagePrefab, hitContainer.transform, true);
    //        _hitImagesCreated.Add(image);
    //    }
    //}

    //void DestroyImage(int quantity)
    //{
    //    for (int i = 0; i < quantity; i++)
    //    {
    //        if (_hitImagesCreated.Count <= 0) break;
    //        GameObject image = _hitImagesCreated[_hitImagesCreated.Count - 1];
    //        _hitImagesCreated.RemoveAt(_hitImagesCreated.Count - 1);
    //        Destroy(image);
    //    }
    //}

    #endregion

    #region HUD Text
    ///// <summary>
    ///// Set actual Player Character UI.
    ///// </summary>
    //public void SetPlayerUI()
    //{
    //    if (_selectedChar.CanAttack())
    //    {
    //        if (_selectedChar.HasEnemiesInRange() && _selectedEnemy)
    //        {
    //            ActivateBodyPartsContainer();
    //        }
    //    }
    //    playerHudContainer.SetActive(true);
    //}

    //private void DeactivatePlayerHUD()
    //{
    //    playerHudContainer.SetActive(false);
    //}

    /// <summary>
    /// Set actual Enemy Character UI.
    /// </summary>
    //public void SetEnemyUI()
    //{
    //    if (_selectedChar)
    //    {
    //        if (_selectedChar.CanAttack() && _selectedEnemy.CanBeAttacked())
    //        {
    //            //_selectedEnemy.SelectedForAttack();
    //            //DeactivateEndTurnButton();
    //            //PortraitsController.Instance.PortraitsActiveState(false);

    //            //Action toDo = () =>
    //            //{
    //                //ActivateBodyPartsContainer();
    //                //playerHudContainer.SetActive(false);
    //                //_selectedChar.ResetTilesInAttackRange();
    //                //_selectedChar.ResetTilesInMoveRange();
    //                //_selectedChar.SelectingEnemy();
    //            //};

    //            //FindObjectOfType<CloseUpCamera>().MoveCameraWithLerp(_selectedEnemy.transform.position, _selectedChar.transform.position, toDo);
                
    //        }
                
    //        //else DeactivateBodyPartsContainer();
    //    }
    //    //else
    //    //{
    //    //    buttonExecuteAttack.interactable = false;
    //    //}
    //}

    //private void SetAttackHUD()
    //{
    //    Gun gun = _selectedChar.GetSelectedGun();
    //    attackWeaponNameText.text = gun.GetGunName();
    //    attackWeaponHitsText.text = gun.GetMaxBullets().ToString();
    //    attackWeaponDamageText.text = gun.GetBulletDamage().ToString();
    //    attackWeaponHitChanceText.text = gun.GetHitChance().ToString();

    //    CreateImage(gun.GetMaxBullets());
    //    attackHudContainer.SetActive(true);
    //}
    #endregion
    
    //public void SetEnemy(Character enemy)
    //{
    //    _selectedEnemy = enemy;
    //}

    //public void SetPlayerCharacter(Character character)
    //{
    //    _selectedChar = character;
    //}

    #region Activators
    
    //public void ActivateEndTurnButton()
    //{
    //    buttonEndTurn.gameObject.SetActive(true);
    //}

    //public void DeactivateEndTurnButton()
    //{
    //    buttonEndTurn.gameObject.SetActive(false);
    //}

    //private void ActivateBodyPartsContainer()
    //{
    //    if (_selectedEnemy)
    //    {
    //        if (_selectedChar.GunsOffOnCloseup())
    //        {
    //            if (_selectedChar.GetLeftGun()) 
    //                _selectedChar.GetLeftGun().ChangeMeshRenderStatus(false);
                
    //            if (_selectedChar.GetRightGun()) 
    //                _selectedChar.GetRightGun().ChangeMeshRenderStatus(false);
    //        }
    //        _selectedChar.RotateTowardsEnemy(_selectedEnemy.transform);
    //        //ActivateParts();
    //    }
    //}

    //private void ActivateParts()
    //{
    //    //if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetBodyPosition(), "Body", false) && _selectedEnemy.GetBody().CurrentHP > 0)
    //    //{
    //    //    _bodyInsight = true;
    //    //}
    //    //else
    //    //{
    //    //    _bodyInsight = false;
    //    //}

    //    //if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetLArmPosition(), "LGun", false) && _selectedEnemy.GetLeftGun())
    //    //{
    //    //    _lArmInsight = true;
    //    //}
    //    //else
    //    //{
    //    //    _lArmInsight = false;
    //    //}

    //    //if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetRArmPosition(), "RGun", false) && _selectedEnemy.GetRightGun())
    //    //{
    //    //    _rArmInsight = true;
    //    //}
    //    //else
    //    //{
    //    //    _rArmInsight = false;
    //    //}


    //    //if (_selectedChar.RayToPartsForAttack(_selectedEnemy.GetLegsPosition(), "Legs", false) &&_selectedEnemy.GetLegs().CurrentHP > 0)
    //    //{
    //    //    _legsInsight = true;
    //    //}
    //    //else
    //    //{
    //    //    _legsInsight = false;
    //    //}
        
    //    //Character[] units = TurnManager.Instance.GetAllUnits();

    //    //foreach (Character unit in units)
    //    //{
    //    //    //if (unit == _selectedChar || unit == _selectedEnemy)
    //    //    if (unit == _selectedEnemy)
    //    //        continue;

    //    //    unit.HideMechaMesh();

    //    //    if (unit == _selectedChar)
    //    //        continue;

    //    //    unit.HideGunsMesh();
    //    //}
    //    //_selectedChar.RaysOffDelay();

    //    //WorldUI ui = _selectedEnemy.GetMyUI();
    //    //ui.ButtonsEnabling(_bodyInsight, _lArmInsight, _rArmInsight, _legsInsight, this);
        
        
    //    //ui.SetBodyHpText(_selectedEnemy.GetBody().GetCurrentHp());

    //    //if (_bodyInsight)
    //    //{
    //    //    Body body = _selectedEnemy.GetBody();
    //    //    bodyButton.SetCharacter(_selectedEnemy, body);
    //    //    //bodyButton.SetHpText(body.CurrentHP.ToString());
    //    //    bodyButton.SetSlider(0, body.MaxHp);
    //    //    bodyButton.UpdateHP(body.CurrentHP);
    //    //    bodyButton.UpdateDamagePreviewSlider();
    //    //    bodyButton.ShowButton();
    //    //}
    //    //else
    //    //{
    //    //    bodyButton.SetCharacter(null,null);
    //    //    //bodyButton.SetHpText("0");
    //    //    bodyButton.SetSlider(0, 0);
    //    //    bodyButton.UpdateHP(0);
    //    //    bodyButton.HideButton();
    //    //}

    //    //if (_lArmInsight)
    //    //{
    //    //    Gun leftGun = _selectedEnemy.GetLeftGun();
    //    //    if (leftGun)
    //    //    {
    //    //        //ui.SetLeftArmHpText(_selectedEnemy.GetLeftGun().GetCurrentHp());
    //    //        leftGunButton.SetCharacter(_selectedEnemy, leftGun);
    //    //        //leftGunButton.SetHpText(leftGun.CurrentHP.ToString());
    //    //        leftGunButton.SetSlider(0, leftGun.MaxHP);
    //    //        leftGunButton.UpdateHP(leftGun.CurrentHP);
    //    //        leftGunButton.UpdateDamagePreviewSlider();
    //    //        leftGunButton.ShowButton();
    //    //    }
    //    //}
    //    //else
    //    //{
    //    //    //ui.SetLeftArmHpText(0);
    //    //    leftGunButton.SetCharacter(null, null);
    //    //    //leftGunButton.SetHpText("0");
    //    //    leftGunButton.SetSlider(0, 0);
    //    //    leftGunButton.UpdateHP(0);
    //    //    leftGunButton.HideButton();
    //    //}

    //    //if (_rArmInsight)
    //    //{
    //    //    Gun rightGun = _selectedEnemy.GetRightGun();
    //    //    if (rightGun)
    //    //    {
    //    //        //ui.SetRightArmHpText(_selectedEnemy.GetRightGun().GetCurrentHp());
    //    //        rightGunButton.SetCharacter(_selectedEnemy, rightGun);
    //    //        //rightGunButton.SetHpText(rightGun.CurrentHP.ToString());
    //    //        rightGunButton.SetSlider(0, rightGun.MaxHP);
    //    //        rightGunButton.UpdateHP(rightGun.CurrentHP);
    //    //        rightGunButton.UpdateDamagePreviewSlider();
    //    //        rightGunButton.ShowButton();
    //    //    }
    //    //}
    //    //else
    //    //{
    //    //    //ui.SetRightArmHpText(0);
    //    //    rightGunButton.SetCharacter(null, null);
    //    //    //rightGunButton.SetHpText("0");
    //    //    rightGunButton.SetSlider(0, 0);
    //    //    rightGunButton.UpdateHP(0);
    //    //    rightGunButton.HideButton();
    //    //}

    //    ////ui.SetLegsHpText(_selectedEnemy.GetLegs().GetCurrentHp());

    //    //if (_legsInsight)
    //    //{
    //    //    Legs legs = _selectedEnemy.GetLegs();
    //    //    legsButton.SetCharacter(_selectedEnemy, legs);
    //    //    //legsButton.SetHpText(legs.CurrentHP.ToString());
    //    //    legsButton.SetSlider(0, legs.MaxHp);
    //    //    legsButton.UpdateHP(legs.CurrentHP);
    //    //    legsButton.UpdateDamagePreviewSlider();
    //    //    legsButton.ShowButton();
    //    //}
    //    //else
    //    //{
    //    //    legsButton.SetCharacter(null, null);
    //    //    //legsButton.SetHpText("0");
    //    //    legsButton.SetSlider(0, 0);
    //    //    legsButton.UpdateHP(0);
    //    //    legsButton.HideButton();
    //    //}
       

    //    //ui.ButtonsContainerSetActive(true);
    //    //SetAttackHUD();
    //}

    //public void DeactivateBodyPartsContainer()
    //{
    //    if (_selectedEnemy)
    //    {
    //        //_selectedEnemy.GetMyUI().ButtonsContainerSetActive(false);
    //        bodyButton.SetMechas(null, null, default);
    //        //bodyButton.SetHpText("0");
    //        bodyButton.HideButton();

    //        leftGunButton.SetMechas(null, null, default);
    //        //leftGunButton.SetHpText("0");
    //        leftGunButton.HideButton();
            
    //        rightGunButton.SetMechas(null, null, default);
    //        //rightGunButton.SetHpText("0");
    //        rightGunButton.HideButton();

    //        legsButton.SetMechas(null, null, default);
    //        //legsButton.SetHpText("0");
    //        legsButton.HideButton(); ;
    //    }
            
    //}

    //public void ActivateExecuteAttackButton()
    //{
    //    buttonExecuteAttack.interactable = true;
    //}

    //public void DeactivateExecuteAttackButton()
    //{
    //    buttonExecuteAttack.interactable = false;
    //}
    #endregion
}
