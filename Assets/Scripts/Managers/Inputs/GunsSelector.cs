using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsSelector : Initializable
{
    [Header("References")]
    [SerializeField] private InputsReader _inputsReader;

    private bool _canChangeGun;
    private Character _selectedMecha;
    public Action OnLeftGunSelected;
    public Action OnRightGunSelected;

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

        if (!_selectedMecha || !_selectedMecha.GetLeftGun())
            return;

        OnLeftGunSelected?.Invoke();

        //AudioManager.audioManagerInstance.PlaySound(_soundsMenuManager.GetClickSound(), _soundsMenuManager.GetObjectToAddAudioSource());
    }

    public void SelectRightGun()
    {
        if (!_canChangeGun)
            return;

        if (!_selectedMecha || !_selectedMecha.GetRightGun())
            return;

        OnRightGunSelected?.Invoke();

        //AudioManager.audioManagerInstance.PlaySound(_soundsMenuManager.GetClickSound(), _soundsMenuManager.GetObjectToAddAudioSource());
    }

    private void SetSelectedMecha(Character mecha)
    {
        if (_selectedMecha)
        {
            OnLeftGunSelected -= _selectedMecha.SelectLeftGun;
            OnRightGunSelected -= _selectedMecha.SelectRightGun;
        }
        _selectedMecha = mecha;

        OnLeftGunSelected += _selectedMecha.SelectLeftGun;
        OnRightGunSelected += _selectedMecha.SelectRightGun;
    }

    public void EnableGunSelection() => _canChangeGun = true;
    public void DisableGunSelection() => _canChangeGun = false;

    private void OnDestroy()
    {
        _inputsReader.OnSelectLeftGunKeyPressed -= SelectLeftGun;
        _inputsReader.OnSelectRightGunKeyPressed -= SelectRightGun;

        GameManager.Instance.OnTurnMechaSelected -= SetSelectedMecha;
    }
}
