using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MechaEquipmentHUD : Initializable
{
    [Header("Inputs")]
    [SerializeField] private GunsSelector _gunsSelector;

    [Header("Others")]
    [SerializeField] private GameObject _container;
    [SerializeField] private Sprite _noneIcon;

    [Header("Buttons")]
    [SerializeField] private EquipmentButton _bodyAbilityButton;
    [SerializeField] private EquipmentButton _leftGunAbilityButton;
    [SerializeField] private EquipmentButton _rightGunAbilityButton;
    [SerializeField] private EquipmentButton _legsAbilityButton;
    [SerializeField] private EquipmentButton _itemButton;
    [SerializeField] private Button _leftGunButton;
    [SerializeField] private Button _rightGunButton;

    [Header("Guns")]
    [SerializeField] private GameObject _leftGunContainer;
    [SerializeField] private GameObject _leftGunCircle;
    //[SerializeField] private Button _leftGunBar;
    [SerializeField] private TextMeshProUGUI _leftGunNameText;
    [SerializeField] private TextMeshProUGUI _leftGunHitsText;
    [SerializeField] private TextMeshProUGUI _leftGunDamageText;
    [SerializeField] private TextMeshProUGUI _leftGunHitChanceText;
    [SerializeField] private GameObject _rightGunContainer;
    [SerializeField] private GameObject _rightGunCircle;
    //[SerializeField] private Button _rightGunBar;
    [SerializeField] private TextMeshProUGUI _rightGunNameText;
    [SerializeField] private TextMeshProUGUI _rightGunHitsText;
    [SerializeField] private TextMeshProUGUI _rightGunDamageText;
    [SerializeField] private TextMeshProUGUI _rightGunHitChanceText;

    private Dictionary<EquipmentButton, bool> _equipmentButtonsPreviousState = new Dictionary<EquipmentButton, bool>();

    private void Awake()
    {
        HideGunsContainer();
    }

    public override void Initialize()
    {
        Character[] mechas = GameManager.Instance.GetMechas();

        foreach (Character mecha in mechas)
        {
            mecha.OnMechaSelected += OnMechaSelected;
            mecha.OnBeginMove += DeactivateEquipablesButtons;
            mecha.OnEndMove += ActivateEquipablesButtons;
            mecha.OnRightGunSelected += OnRightGunSelected;
            mecha.OnLeftGunSelected += OnLeftGunSelected;
        }
        _container.SetActive(false);

        _equipmentButtonsPreviousState.Add(_bodyAbilityButton, false);
        _equipmentButtonsPreviousState.Add(_leftGunAbilityButton, false);
        _equipmentButtonsPreviousState.Add(_rightGunAbilityButton, false);
        _equipmentButtonsPreviousState.Add(_legsAbilityButton, false);
        _equipmentButtonsPreviousState.Add(_itemButton, false);

        _gunsSelector.OnLeftGunSelected += OnLeftGunSelected;
        _gunsSelector.OnRightGunSelected += OnRightGunSelected;

        GameManager.Instance.OnBeginTurn += ShowGunsContainer;
        GameManager.Instance.OnEndTurn += DisableButtonsInteraction;
        GameManager.Instance.OnEndTurn += HideGunsContainer;
        GameManager.Instance.OnEnemyMechaSelected += HideGunsContainer;
        GameManager.Instance.OnEnemyMechaDeselected += ShowGunsContainer;
    }

    private void OnMechaSelected(Character mecha)
    {
        ResetEquipmentButtons();

        Ability bodyAbility = mecha.GetBody().GetAbility();
        ConfigureEquipableButton(mecha, _bodyAbilityButton, bodyAbility);

        Ability leftGunAbility = null;
        if (mecha.GetLeftGun())
            leftGunAbility = mecha.GetLeftGun().GetAbility();
        ConfigureEquipableButton(mecha, _leftGunAbilityButton, leftGunAbility);

        Ability rightGunAbility = null;
        if (mecha.GetRightGun())
            rightGunAbility = mecha.GetRightGun().GetAbility();
        ConfigureEquipableButton(mecha, _rightGunAbilityButton, rightGunAbility);

        Ability legsAbility = mecha.GetLegs().GetAbility();
        ConfigureEquipableButton(mecha, _legsAbilityButton, legsAbility);

        Item item = mecha.GetItem();
        ConfigureEquipableButton(mecha, _itemButton, item);

        ConfigureGunsUI(mecha, mecha.GetLeftGun(), mecha.GetRightGun());

        ShowGunsContainer();
    }

    private void ConfigureEquipableButton(Character mecha, EquipmentButton button, Equipable equipable)
    {
        if (equipable)
        {
            equipable.SetButton(button);

            button.SetButtonIcon(equipable.GetIcon());
            button.AddLeftClick(() => OnButtonSelected(button));
            button.AddLeftClick(equipable.Select);

            button.AddRightClick(equipable.Deselect);
            button.AddRightClick(OnButtonDeselected);


            if (mecha.GetUnitTeam() != EnumsClass.Team.Green)
            {
                button.interactable = false;
                return;
            }

            if (equipable.CanBeUsed())
            {
                switch (equipable.GetEquipableType())
                {
                    case EquipableSO.EquipableType.Item:
                        button.interactable = equipable.GetAvailableUses() > 0;
                        break;
                    case EquipableSO.EquipableType.Active:
                        button.interactable = true;
                        break;
                    case EquipableSO.EquipableType.Passive:
                        button.interactable = false;
                        break;
                    default:
                        break;
                }
            }                
        }
        else
        {
            button.interactable = false;
            button.SetButtonIcon(_noneIcon);
        }
    }

    private void ConfigureGunsUI(Character mecha, Gun left, Gun right)
    {
        //_leftGunBar.onClick.RemoveAllListeners();

        if (left)
        {
            _leftGunContainer.SetActive(true);
            _leftGunNameText.text = left.GetGunName();
            _leftGunHitsText.text = left.GetHitChance().ToString();
            _leftGunDamageText.text = left.GetBulletDamage().ToString();
            _leftGunHitChanceText.text = left.GetHitChance().ToString();

            //_leftGunBar.onClick.AddListener(mecha.SelectLeftGun);
            //_leftGunBar.onClick.AddListener(OnLeftGunSelected);

            if (mecha.GetUnitTeam() == EnumsClass.Team.Green)
                _leftGunButton.interactable = true;
            else
                _leftGunButton.interactable = false;
        }
        else
            _leftGunContainer.SetActive(false);


        //_rightGunBar.onClick.RemoveAllListeners();

        if (right)
        {
            _rightGunContainer.SetActive(true);
            _rightGunNameText.text = right.GetGunName();
            _rightGunHitsText.text = right.GetHitChance().ToString();
            _rightGunDamageText.text = right.GetBulletDamage().ToString();
            _rightGunHitChanceText.text = right.GetHitChance().ToString();


            //_rightGunBar.onClick.AddListener(mecha.SelectRightGun);
            //_rightGunBar.onClick.AddListener(OnRightGunSelected);

            if (mecha.GetUnitTeam() == EnumsClass.Team.Green)
                _rightGunButton.interactable = true;
            else
                _rightGunButton.interactable = false;
        }
        else
            _rightGunContainer.SetActive(false);

    }
    private void OnButtonSelected(EquipmentButton selected)
    {
        List<EquipmentButton> buttons = new List<EquipmentButton>();

        buttons.AddRange(_equipmentButtonsPreviousState.Keys);

        foreach (EquipmentButton button in buttons)
        {
            _equipmentButtonsPreviousState[button] = button.interactable;

            if (button == selected)
                continue;

            button.interactable = false;
        }
    }

    private void OnButtonDeselected()
    {
        ActivateEquipablesButtons();
    }
    private void ResetEquipmentButtons()
    {
        _bodyAbilityButton.ResetButton();
        _leftGunAbilityButton.ResetButton();
        _rightGunAbilityButton.ResetButton();
        _legsAbilityButton.ResetButton();
        _itemButton.ResetButton();
    }

    private void DeactivateEquipablesButtons()
    {
        List<EquipmentButton> buttons = new List<EquipmentButton>();

        buttons.AddRange(_equipmentButtonsPreviousState.Keys);

        foreach (EquipmentButton button in buttons)
        {
            _equipmentButtonsPreviousState[button] = button.interactable;

            button.interactable = false;
        }
    }

    private void ActivateEquipablesButtons()
    {
        foreach (EquipmentButton button in _equipmentButtonsPreviousState.Keys)
        {
            button.interactable = _equipmentButtonsPreviousState[button];
        }
    }

    private void ShowGunsContainer() => _container.SetActive(true);
    private void HideGunsContainer() => _container.SetActive(false);

    public void EnableButtonsInteraction()
    {
        _bodyAbilityButton.interactable = true;
        _leftGunAbilityButton.interactable = true;
        _rightGunAbilityButton.interactable = true;
        _legsAbilityButton.interactable = true;

        _leftGunButton.interactable = true;
        _rightGunButton.interactable = true;
    }
    public void DisableButtonsInteraction()
    {
        _bodyAbilityButton.interactable = false;
        _leftGunAbilityButton.interactable = false;
        _rightGunAbilityButton.interactable = false;
        _legsAbilityButton.interactable = false;

        _leftGunButton.interactable = false;
        _rightGunButton.interactable = false;
    }
    public void OnRightGunSelected()
    {
        _rightGunCircle.SetActive(true);
        _leftGunCircle.SetActive(false);
    }
    public void OnLeftGunSelected()
    {
        _leftGunCircle.SetActive(true);
        _rightGunCircle.SetActive(false);
    }

    private void OnDestroy()
    {
        Character[] mechas = GameManager.Instance.GetMechas();

        foreach (var mecha in mechas)
        {
            mecha.OnMechaSelected -= OnMechaSelected;
            mecha.OnBeginMove -= DeactivateEquipablesButtons;
            mecha.OnEndMove -= ActivateEquipablesButtons;
            mecha.OnRightGunSelected -= OnRightGunSelected;
            mecha.OnLeftGunSelected -= OnLeftGunSelected;
        }

        _gunsSelector.OnLeftGunSelected -= OnLeftGunSelected;
        _gunsSelector.OnRightGunSelected -= OnRightGunSelected;

        GameManager.Instance.OnBeginTurn -= ShowGunsContainer;
        GameManager.Instance.OnEndTurn -= DisableButtonsInteraction;
        GameManager.Instance.OnEndTurn -= HideGunsContainer;
        GameManager.Instance.OnEnemyMechaSelected -= HideGunsContainer;
    }
}
