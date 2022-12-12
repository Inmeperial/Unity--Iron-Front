using UnityEngine;

public class EquipablesHUD : Initializable
{
    [Header("Others")]
    [SerializeField] private GameObject _container;
    [SerializeField] private Sprite _noneIcon;

    [Header("Buttons")]
    [SerializeField] private EquipmentButton _bodyAbilityButton;
    [SerializeField] private EquipmentButton _leftGunAbilityButton;
    [SerializeField] private EquipmentButton _rightGunAbilityButton;
    [SerializeField] private EquipmentButton _legsAbilityButton;
    [SerializeField] private EquipmentButton _itemButton;

    private void Awake()
    {
        HideContainer();
    }

    public override void Initialize()
    {
        Character[] mechas = GameManager.Instance.GetMechas();

        foreach (Character mecha in mechas)
        {
            mecha.OnMechaSelected += OnMechaSelected;
            mecha.OnBeginMove += DeactivateButtons;
            mecha.OnEndMove += CheckButtonsState;
        }
        HideContainer();

        GameManager.Instance.OnBeginTurn += ShowContainer;
        GameManager.Instance.OnEndTurn += DisableButtonsInteraction;
        GameManager.Instance.OnEndTurn += HideContainer;
        GameManager.Instance.OnEnemyMechaSelected += HideContainer;
        GameManager.Instance.OnEnemyMechaDeselected += ShowContainer;
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

        ShowContainer();
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

            equipable.OnEquipableSelected += CheckButtonsState;
            equipable.OnEquipableDeselected += CheckButtonsState;
            equipable.OnEquipableUsed += CheckButtonsState;

            button.GetComponent<OverviewData>().SetTooltipData(equipable.GetEquipableName(), equipable.GetEquipableDescription());

            if (mecha.GetUnitTeam() != EnumsClass.Team.Green)
            {
                button.interactable = false;
                return;
            }

            UpdateButtonState(button, equipable);
        }
        else
            ConfigureEmptyButton(button);
    }

    private void CheckButtonsState()
    {
        Character mecha = GameManager.Instance.CurrentTurnMecha;

        if (mecha.IsDead())
        {
            DisableButtonsInteraction();
            return;
        }

        UpdateButtonState(_bodyAbilityButton, mecha.GetBody().GetAbility());
        UpdateButtonState(_rightGunAbilityButton, mecha.GetRightGun().GetAbility());
        UpdateButtonState(_leftGunAbilityButton, mecha.GetLeftGun().GetAbility());
        UpdateButtonState(_legsAbilityButton, mecha.GetLegs().GetAbility());
        UpdateButtonState(_itemButton, mecha.GetItem());

        if (mecha.GetUnitTeam() == EnumsClass.Team.Red)
            DisableButtonsInteraction();
    }
    private void UpdateButtonState(EquipmentButton button, Equipable equipable)
    {
        if (!equipable)
        {
            ConfigureEmptyButton(button);
            return;
        }

        button.interactable = equipable.CanBeUsed() && equipable.IsInteractable();

        EquipableSO.EquipableType type = equipable.GetEquipableType();
        if (type == EquipableSO.EquipableType.Item)
            button.SetButtonText(equipable.GetAvailableUses().ToString(), type);
        else
        {
            Ability ability = equipable as Ability;

            if (ability.GetRemainingCooldown() > 0)
                button.SetButtonText(ability.GetRemainingCooldown().ToString(), type);
            else
                button.HideAbilityCooldownText();
        }
    }

    private void ConfigureEmptyButton(EquipmentButton button)
    {
        button.interactable = false;
        button.SetButtonIcon(_noneIcon);
        button.HideAbilityCooldownText();
        button.HideItemUsesText();

        button.GetComponent<OverviewData>().SetTooltipData();
    }

    private void DeactivateButtons()
    {
        _bodyAbilityButton.interactable = false;
        _rightGunAbilityButton.interactable = false;
        _leftGunAbilityButton.interactable = false;
        _legsAbilityButton.interactable = false;
        _itemButton.interactable = false;
    }

    private void OnButtonSelected(EquipmentButton button)
    {
        DeactivateButtons();
        button.interactable = true;
    }
    private void OnButtonDeselected()
    {
        CheckButtonsState();
    }

    private void ResetEquipmentButtons()
    {
        _bodyAbilityButton.ResetButton();
        _leftGunAbilityButton.ResetButton();
        _rightGunAbilityButton.ResetButton();
        _legsAbilityButton.ResetButton();
        _itemButton.ResetButton();
    }

    public void EnableButtonsInteraction()
    {
        _bodyAbilityButton.interactable = true;
        _leftGunAbilityButton.interactable = true;
        _rightGunAbilityButton.interactable = true;
        _legsAbilityButton.interactable = true;
        _itemButton.interactable = true;
    }

    public void DisableButtonsInteraction()
    {
        _bodyAbilityButton.interactable = false;
        _leftGunAbilityButton.interactable = false;
        _rightGunAbilityButton.interactable = false;
        _legsAbilityButton.interactable = false;
        _itemButton.interactable = false;
    }

    private void ShowContainer()
    {
        _container.SetActive(true);
    }

    private void HideContainer()
    {
        _container.SetActive(false);
    }

    private void OnDestroy()
    {
        Character[] mechas = GameManager.Instance.GetMechas();

        foreach (Character mecha in mechas)
        {
            mecha.OnMechaSelected -= OnMechaSelected;
            mecha.OnBeginMove -= DeactivateButtons;
            mecha.OnEndMove -= CheckButtonsState;
        }

        GameManager.Instance.OnBeginTurn -= ShowContainer;
        GameManager.Instance.OnEndTurn -= DisableButtonsInteraction;
        GameManager.Instance.OnEndTurn -= HideContainer;
        GameManager.Instance.OnEnemyMechaSelected -= HideContainer;
        GameManager.Instance.OnEnemyMechaDeselected -= ShowContainer;
    }
}
