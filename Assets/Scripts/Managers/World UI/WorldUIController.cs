using System.Collections.Generic;
using UnityEngine;

public class WorldUIController : Initializable
{
    [Header("Inputs")]
    [SerializeField] private InputsReader _inputsReader;

    private bool _isToggledOn = false;

    private Dictionary<Character, WorldUI> _mechaUIDictionary = new Dictionary<Character, WorldUI>();

    public override void Initialize()
    {
        WorldUI[] worldUIs = FindObjectsOfType<WorldUI>();

        foreach (WorldUI ui in worldUIs)
        {
            Character owner = ui.Owner;

            if (_mechaUIDictionary.ContainsKey(owner))
                continue;

            _mechaUIDictionary.Add(owner, ui);

            ui.OnUpdateFinished += HideMechaWorldUI;

            owner.OnMouseOverMecha += ShowMechaWorldUI;
            owner.OnMouseExitMecha += HideMechaWorldUI;

            owner.OnMechaDeath += HideMechaWorldUI;

            owner.OnMoveActionStateChange += OnMoveActionStateChange;
            owner.OnAttackActionStateChange += OnAttackActionStateChange;
            owner.OnOverweight += OnOverweightStateChange;

            owner.GetBody().OnDamageTaken += OnBodyDamageTaken;
            owner.GetLeftGun().OnDamageTaken += OnLeftGunDamageTaken;
            owner.GetRightGun().OnDamageTaken += OnRightGunDamageTaken;
            owner.GetLegs().OnDamageTaken += OnLegsDamageTaken;

            ui.Initialize();
        }

        HideWorldUI();

        _inputsReader.OnWorldUIKeyPressed += ShowWorldUI;
        _inputsReader.OnWorldUIKeyReleased += HideWorldUI;

        _inputsReader.OnToggleWorldUIKeyPressed += ToggleWorldUI;

    }

    private void ShowWorldUI()
    {
        foreach (KeyValuePair<Character, WorldUI> kvp in _mechaUIDictionary)
        {
            Character mecha = kvp.Key;

            if (mecha.IsDead())
                continue;

            WorldUI ui = kvp.Value;

            ui.Show();
        }
    }

    private void OnBodyDamageTaken(Character mecha, float damage) => _mechaUIDictionary[mecha].UpdateBodyHPBar(damage);
    private void OnLeftGunDamageTaken(Character mecha, float damage) => _mechaUIDictionary[mecha].UpdateLeftGunHPBar(damage);
    private void OnRightGunDamageTaken(Character mecha, float damage) => _mechaUIDictionary[mecha].UpdateRightGunHPBar(damage);
    private void OnLegsDamageTaken(Character mecha, float damage) => _mechaUIDictionary[mecha].UpdateLegsHPBar(damage);
    private void OnAttackActionStateChange(Character mecha, bool state) => _mechaUIDictionary[mecha].AttackActionIconStatus(state);
    private void OnMoveActionStateChange(Character mecha, bool state) => _mechaUIDictionary[mecha].MoveActionIconStatus(state);
    private void OnOverweightStateChange(Character mecha, bool state) => _mechaUIDictionary[mecha].OverweightIconStatus(state);
    private void HideWorldUI()
    {
        if (_isToggledOn)
            return;

        foreach (WorldUI ui in _mechaUIDictionary.Values)
        {
            ui.Hide();
        }
    }

    private void ToggleWorldUI()
    {
        _isToggledOn = !_isToggledOn;

        if (_isToggledOn)
            ShowWorldUI();
        else
            HideWorldUI();
    }

    public void ShowMechaWorldUI(Character mecha) => _mechaUIDictionary[mecha].Show();
    public void HideMechaWorldUI(Character mecha)
    {
        if (!mecha.IsDead() && _isToggledOn)
            return;

        _mechaUIDictionary[mecha].Hide();
    }

    private void OnDestroy()
    {
        _inputsReader.OnWorldUIKeyPressed -= ShowWorldUI;
        _inputsReader.OnWorldUIKeyReleased -= HideWorldUI;

        _inputsReader.OnToggleWorldUIKeyPressed -= ToggleWorldUI;

        foreach (KeyValuePair<Character, WorldUI> kvp in _mechaUIDictionary)
        {
            Character mecha = kvp.Key;
            WorldUI ui = kvp.Value;

            ui.OnUpdateFinished -= HideMechaWorldUI;

            mecha.OnMouseOverMecha -= ShowMechaWorldUI;
            mecha.OnMouseExitMecha -= HideMechaWorldUI;

            mecha.OnMechaDeath -= HideMechaWorldUI;

            mecha.GetBody().OnDamageTaken += OnBodyDamageTaken;
            mecha.GetLeftGun().OnDamageTaken += OnLeftGunDamageTaken;
            mecha.GetRightGun().OnDamageTaken += OnRightGunDamageTaken;
            mecha.GetLegs().OnDamageTaken += OnLegsDamageTaken;
        }
    }
}
