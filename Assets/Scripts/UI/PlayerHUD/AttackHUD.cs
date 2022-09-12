using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AttackHUD : Initializable
{
    [Header("Inputs")]
    [SerializeField] private InputsReader _inputsReader;    

    [Header("Selected Gun HUD")]
    [SerializeField] private GameObject _attackHUDContainer;
    [SerializeField] private TextMeshProUGUI _attackWeaponNameText;
    [SerializeField] private TextMeshProUGUI _attackWeaponHitsText;
    [SerializeField] private TextMeshProUGUI _attackWeaponDamageText;
    [SerializeField] private TextMeshProUGUI _attackWeaponHitChanceText;
    [SerializeField] private GameObject _hitContainer;
    [SerializeField] private GameObject _hitImagePrefab;

    private List<GameObject> _hitImagesCreated = new List<GameObject>();

    [Header("Attack Buttons")]
    [SerializeField] private MechaPartButton _bodyButton;
    [SerializeField] private MechaPartButton _leftGunButton;
    [SerializeField] private MechaPartButton _rightGunButton;
    [SerializeField] private MechaPartButton _legsButton;
    [SerializeField] private Button _attackButton;

    private int _partsSelectedForAttack;

    private Character _selectedCharacter;
    private Character _selectedEnemy;

    public Action OnAttackButtonClicked;

    // Start is called before the first frame update
    public override void Initialize()
    {
        _inputsReader.OnDeselectKeyPressed += HidePartsButtons;
        _inputsReader.OnDeselectKeyPressed += HideAttackButton;

        Character[] mechas = GameManager.Instance.GetMechas();

        foreach (Character mecha in mechas)
        {
            mecha.OnSelectingEnemy += OnSelectingEnemy;
        }
        _attackButton.onClick.AddListener(ExecuteAttackButtonClick);

        GameManager.Instance.OnMechaAttackPreparationsFinished += ExecuteAttack;
        GameManager.Instance.OnBeginAttackPreparations += HideAttackHUD;

        _bodyButton.OnButtonClicked += CheckAmountOfPartsSelected;
        _bodyButton.OnButtonClicked += UpdateExecuteAttackButtonState;
        _bodyButton.OnButtonClicked += DeterminateButtonsActivation;
        _bodyButton.OnAddBullets += DestroyBulletsImage;
        _bodyButton.OnReduceBullets += CreateBulletsImage;

        _leftGunButton.OnButtonClicked += CheckAmountOfPartsSelected;
        _leftGunButton.OnButtonClicked += UpdateExecuteAttackButtonState;
        _leftGunButton.OnButtonClicked += DeterminateButtonsActivation;
        _leftGunButton.OnAddBullets += DestroyBulletsImage;
        _leftGunButton.OnReduceBullets += CreateBulletsImage;

        _rightGunButton.OnButtonClicked += CheckAmountOfPartsSelected;
        _rightGunButton.OnButtonClicked += UpdateExecuteAttackButtonState;
        _rightGunButton.OnButtonClicked += DeterminateButtonsActivation;
        _rightGunButton.OnAddBullets += DestroyBulletsImage;
        _rightGunButton.OnReduceBullets += CreateBulletsImage;

        _legsButton.OnButtonClicked += CheckAmountOfPartsSelected;
        _legsButton.OnButtonClicked += UpdateExecuteAttackButtonState;
        _legsButton.OnButtonClicked += DeterminateButtonsActivation;
        _legsButton.OnAddBullets += DestroyBulletsImage;
        _legsButton.OnReduceBullets += CreateBulletsImage;

        HideAttackHUD();
        HidePartsButtons();
    }

    private void OnSelectingEnemy()
    {
        _selectedCharacter = GameManager.Instance.CurrentTurnMecha;
        SetAttackHud(_selectedCharacter.GetSelectedGun());
        ShowAttackHUD();

        _selectedEnemy = GameManager.Instance.SelectedEnemy;

        if (_selectedCharacter.IsEnemyBodyInSight(_selectedEnemy))
            ShowBodyButton();

        if (_selectedCharacter.IsEnemyLeftGunInSight(_selectedEnemy))
            ShowLeftGunButton();

        if (_selectedCharacter.IsEnemyRightGunInSight(_selectedEnemy))
            ShowRightGunButton();

        if (_selectedCharacter.IsEnemyLegsInSight(_selectedEnemy))
            ShowLegsButton();
    }

    private void CreateBulletsImage(int quantity)
    {
        int c = 0;
        for (int i = 0; i < quantity; i++)
        {
            c++;
            GameObject image = Instantiate(_hitImagePrefab, _hitContainer.transform, true);
            _hitImagesCreated.Add(image);
        }
    }

    private void DestroyBulletsImage(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            if (_hitImagesCreated.Count <= 0) break;
            GameObject image = _hitImagesCreated[_hitImagesCreated.Count - 1];
            _hitImagesCreated.RemoveAt(_hitImagesCreated.Count - 1);
            Destroy(image);
        }
    }

    private void SetAttackHud(Gun gun)
    {
        _attackWeaponNameText.text = gun.GetGunName();
        _attackWeaponHitsText.text = gun.GetMaxBullets().ToString();
        _attackWeaponDamageText.text = gun.GetBulletDamage().ToString();
        _attackWeaponHitChanceText.text = gun.GetHitChance().ToString();

        DestroyBulletsImage(_hitImagesCreated.Count);
        _hitImagesCreated.Clear();
        CreateBulletsImage(gun.GetMaxBullets());
    }

    public void ShowAttackHUD() => _attackHUDContainer.SetActive(true);
    public void HideAttackHUD() => _attackHUDContainer.SetActive(false);

    private void ShowBodyButton()
    {
        ConfigureBodyButton();        
        _bodyButton.ShowButton();
    }

    private void ShowLeftGunButton()
    {
        ConfigureLeftGunButton();
        _leftGunButton.ShowButton();
    }

    private void ShowRightGunButton()
    {
        ConfigureRightGunButton();
        _rightGunButton.ShowButton();
    }

    private void ShowLegsButton()
    {
        ConfigureLegsButton();
        _legsButton.ShowButton();
    }

    private void HidePartsButtons()
    {
        _bodyButton.HideButton();
        _leftGunButton.HideButton();
        _rightGunButton.HideButton();
        _legsButton.HideButton();
    }
    
    private void ConfigureBodyButton()
    {
        Body body = _selectedEnemy.GetBody();
        _bodyButton.ResetButton();
        _bodyButton.SetMechas(_selectedCharacter, _selectedEnemy, body);
        _bodyButton.SetSlider(0, body.MaxHp);
        _bodyButton.UpdateHP(body.CurrentHP);
        _bodyButton.UpdateDamagePreviewSlider();
    }

    private void ConfigureLeftGunButton()
    {
        Gun gun = _selectedEnemy.GetLeftGun();
        _leftGunButton.ResetButton();
        _leftGunButton.SetMechas(_selectedCharacter, _selectedEnemy, gun);
        _leftGunButton.SetSlider(0, gun.MaxHP);
        _leftGunButton.UpdateHP(gun.CurrentHP);
        _leftGunButton.UpdateDamagePreviewSlider();
    }

    private void ConfigureRightGunButton()
    {
        Gun gun = _selectedEnemy.GetRightGun();
        _rightGunButton.ResetButton();
        _rightGunButton.SetMechas(_selectedCharacter, _selectedEnemy, gun);
        _rightGunButton.SetSlider(0, gun.MaxHP);
        _rightGunButton.UpdateHP(gun.CurrentHP);
        _rightGunButton.UpdateDamagePreviewSlider();
    }

    private void ConfigureLegsButton()
    {
        Legs legs = _selectedEnemy.GetLegs();
        _legsButton.ResetButton();
        _legsButton.SetMechas(_selectedCharacter, _selectedEnemy, legs);
        _legsButton.SetSlider(0, legs.MaxHp);
        _legsButton.UpdateHP(legs.CurrentHP);
        _legsButton.UpdateDamagePreviewSlider();
    }

    private void UpdateExecuteAttackButtonState()
    {
        //bool status;
        //if (_bodyButton.BulletsCount > 0 || _bodyButton.BulletsCount > 0 || _bodyButton.BulletsCount > 0 | _bodyButton.BulletsCount > 0)
        //    status = true;
        //else
        //    status = false;

        //Debug.Log("ExecuteAttack status: " + status);
        //_attackButton.interactable = status;
        //_attackButton.gameObject.SetActive(status);
    }

    private void DeterminateButtonsActivation()
    {
        Gun selectedGun = _selectedCharacter.GetSelectedGun();

        if (selectedGun.GetAvailableBullets() <= 0 || _partsSelectedForAttack == selectedGun.GetAvailableSelections())
        {
            if (_bodyButton.BulletsCount <= 0)
                _bodyButton.HideButton();
            
            if (_leftGunButton.BulletsCount <= 0)
                _leftGunButton.HideButton();

            if (_rightGunButton.BulletsCount <= 0)
                _rightGunButton.HideButton();

            if (_legsButton.BulletsCount <= 0)
                _legsButton.HideButton();
        }
        else if (_partsSelectedForAttack < selectedGun.GetAvailableSelections())
        {
            if (_bodyButton.BulletsCount <= 0 && _selectedCharacter.IsEnemyBodyInSight(_selectedEnemy))
                _bodyButton.ShowButton();

            if (_leftGunButton.BulletsCount <= 0 && _selectedCharacter.IsEnemyBodyInSight(_selectedEnemy))
                _leftGunButton.ShowButton();

            if (_rightGunButton.BulletsCount <= 0 && _selectedCharacter.IsEnemyBodyInSight(_selectedEnemy))
                _rightGunButton.ShowButton();

            if (_legsButton.BulletsCount <= 0 && _selectedCharacter.IsEnemyBodyInSight(_selectedEnemy))
                _legsButton.ShowButton();
        }
    }

    private void CheckAmountOfPartsSelected()
    {
        _partsSelectedForAttack = 0;

        if (_bodyButton.BulletsCount > 0)
            _partsSelectedForAttack++;

        if (_leftGunButton.BulletsCount > 0)
            _partsSelectedForAttack++;

        if (_rightGunButton.BulletsCount > 0)
            _partsSelectedForAttack++;

        if (_legsButton.BulletsCount > 0)
            _partsSelectedForAttack++;

        if (_partsSelectedForAttack > 0)
            ShowAttackButton();
        else
            HideAttackButton();
    }

    private void ShowAttackButton()
    {
        _attackButton.interactable = true;
        _attackButton.gameObject.SetActive(true);
    }

    private void HideAttackButton()
    {
        _attackButton.interactable = false;
        _attackButton.gameObject.SetActive(false);
    }

    private void ExecuteAttackButtonClick()
    {
        _bodyButton.HideButton();
        _leftGunButton.HideButton();
        _rightGunButton.HideButton();
        _legsButton.HideButton();

        HideAttackButton();

        OnAttackButtonClicked?.Invoke();
    }

    private void ExecuteAttack()
    {
        _bodyButton.Attack();
        _leftGunButton.Attack();
        _rightGunButton.Attack();
        _legsButton.Attack();

        _selectedCharacter.DeactivateAttack();

        _selectedCharacter = null;
        _selectedEnemy = null;
    }

    private void OnDestroy()
    {
        _inputsReader.OnDeselectKeyPressed -= HidePartsButtons;
        _inputsReader.OnDeselectKeyPressed -= HideAttackButton;

        Character[] mechas = GameManager.Instance.GetMechas();

        foreach (var mecha in mechas)
        {
            mecha.OnSelectingEnemy -= OnSelectingEnemy;
        }

        _attackButton.onClick.RemoveListener(ExecuteAttackButtonClick);
        OnAttackButtonClicked = null;

        GameManager.Instance.OnMechaAttackPreparationsFinished -= ExecuteAttack;

        _bodyButton.OnButtonClicked -= UpdateExecuteAttackButtonState;
        _bodyButton.OnButtonClicked = CheckAmountOfPartsSelected;
        _bodyButton.OnButtonClicked -= DeterminateButtonsActivation;
        _bodyButton.OnAddBullets -= DestroyBulletsImage;
        _bodyButton.OnReduceBullets -= CreateBulletsImage;

        _leftGunButton.OnButtonClicked -= UpdateExecuteAttackButtonState;
        _leftGunButton.OnButtonClicked -= CheckAmountOfPartsSelected;
        _leftGunButton.OnButtonClicked -= DeterminateButtonsActivation;
        _leftGunButton.OnAddBullets -= DestroyBulletsImage;
        _leftGunButton.OnReduceBullets -= CreateBulletsImage;

        _rightGunButton.OnButtonClicked -= UpdateExecuteAttackButtonState;
        _rightGunButton.OnButtonClicked -= CheckAmountOfPartsSelected;
        _rightGunButton.OnButtonClicked -= DeterminateButtonsActivation;
        _rightGunButton.OnAddBullets -= DestroyBulletsImage;
        _rightGunButton.OnReduceBullets -= CreateBulletsImage;

        _legsButton.OnButtonClicked -= UpdateExecuteAttackButtonState;
        _legsButton.OnButtonClicked -= CheckAmountOfPartsSelected;
        _legsButton.OnButtonClicked -= DeterminateButtonsActivation;
        _legsButton.OnAddBullets -= DestroyBulletsImage;
        _legsButton.OnReduceBullets -= CreateBulletsImage;
    }
}
