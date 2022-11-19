using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsSelector : Initializable
{
    [Header("References")]
    [SerializeField] private InputsReader _inputsReader;

    [Header("Sound")]
    [SerializeField] private SoundData _gunSelectionSound;

    private bool _canChangeGun;
    private Character _selectedMecha;
    public Action OnLeftGunSelected;
    public Action OnRightGunSelected;
    public Action OnSelectionDisabled;
    public Action OnSelectionEnabled;

    public override void Initialize()
    {
        _inputsReader.OnSelectLeftGunKeyPressed += SelectLeftGun;
        _inputsReader.OnSelectRightGunKeyPressed += SelectRightGun;

        GameManager.Instance.OnTurnMechaSelected += SetSelectedMecha;
        GameManager.Instance.OnEnemyMechaSelected += DisableGunSelection;
        GameManager.Instance.OnEnemyMechaDeselected += EnableGunSelection;
        GameManager.Instance.OnMechaAttackPreparationsFinished += EnableGunSelection;
    }
    public void SelectLeftGun()
    {
        if (!_canChangeGun)
            return;

        if (!_selectedMecha || !_selectedMecha.IsLeftGunAlive())
            return;

        OnLeftGunSelected?.Invoke();

        AudioManager.Instance.PlaySound(_gunSelectionSound, gameObject);
    }

    public void SelectRightGun()
    {
        if (!_canChangeGun)
            return;

        if (!_selectedMecha || !_selectedMecha.IsRightGunAlive())
            return;

        OnRightGunSelected?.Invoke();
        AudioManager.Instance.PlaySound(_gunSelectionSound, gameObject);
    }

    private void SetSelectedMecha(Character mecha)
    {
        if (_selectedMecha)
        {
            OnLeftGunSelected -= _selectedMecha.SelectLeftGun;
            OnRightGunSelected -= _selectedMecha.SelectRightGun;
        }
        _selectedMecha = mecha;

        if (_selectedMecha.GetLeftGun().CurrentHP > 0)
            OnLeftGunSelected += _selectedMecha.SelectLeftGun;

        if (_selectedMecha.GetRightGun().CurrentHP > 0)
            OnRightGunSelected += _selectedMecha.SelectRightGun;
    }

    public void EnableGunSelection()
    {
        _canChangeGun = true;
        OnSelectionEnabled?.Invoke();
    }

    public void DisableGunSelection()
    {
        _canChangeGun = false;
        OnSelectionDisabled?.Invoke();
    }

    private void OnDestroy()
    {
        _inputsReader.OnSelectLeftGunKeyPressed -= SelectLeftGun;
        _inputsReader.OnSelectRightGunKeyPressed -= SelectRightGun;

        GameManager.Instance.OnTurnMechaSelected -= SetSelectedMecha;
    }
}
