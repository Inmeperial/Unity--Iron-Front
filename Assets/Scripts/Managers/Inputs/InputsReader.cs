using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputsReader : MonoBehaviour
{
    [Header("Keys")]
    [SerializeField] private KeyCode _deselectKey;
    [SerializeField] private KeyCode _undoPathKey;
    [SerializeField] private KeyCode _selectLeftGunKey;
    [SerializeField] private KeyCode _selectRightGunKey;
    [SerializeField] private KeyCode _showWorldUIKey;
    [SerializeField] private KeyCode _toggleWorldUIKey;
    [SerializeField] private KeyCode _menuKey;

    private Dictionary<KeyCode, Action> _keysDictionary = new Dictionary<KeyCode, Action>();

    public Action OnDeselectKeyPressed;
    public Action OnUndoKeyPressed;
    public Action OnSelectLeftGunKeyPressed;
    public Action OnSelectRightGunKeyPressed;
    public Action OnWorldUIKeyPressed;
    public Action OnWorldUIKeyReleased;
    public Action OnToggleWorldUIKeyPressed;
    public Action OnMenuKeyPressed;

    private bool _canCheckKeys;
    public bool CanCheckKeys => _canCheckKeys;

    // Start is called before the first frame update
    void Start()
    {
        _keysDictionary.Add(_deselectKey, DeselectKeyPress);
        _keysDictionary.Add(_undoPathKey, UndoKeyPress);
        _keysDictionary.Add(_selectLeftGunKey, SelectLeftGunKeyPress);
        _keysDictionary.Add(_selectRightGunKey, SelectRightGunKeyPress);
        _keysDictionary.Add(_showWorldUIKey, WorldUIKeyPress);
        _keysDictionary.Add(_toggleWorldUIKey, ToggleWorldUIKeyPress);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_menuKey))
            OnMenuKeyPressed?.Invoke();

        if (!_canCheckKeys)
            return;

        foreach (KeyValuePair<KeyCode, Action> kvp in _keysDictionary)
        {
            KeyCode key = kvp.Key;
            
            if (!Input.GetKeyDown(key))
                continue;
            
            Action action = kvp.Value;
            action?.Invoke();
        }

        if (Input.GetKeyUp(_showWorldUIKey))
            WorldUIKeyRelease();

    }

    public void EnableKeysCheck() => _canCheckKeys = true;
    public void DisableKeysCheck() => _canCheckKeys = false;

    private void DeselectKeyPress()
    {
        if (IsPointerOverGameObject())
            return;

        OnDeselectKeyPressed?.Invoke();
    }

    private void UndoKeyPress()
    {
        if (IsPointerOverGameObject())
            return;

        OnUndoKeyPressed?.Invoke();
    }

    private void SelectLeftGunKeyPress()
    {
        OnSelectLeftGunKeyPressed?.Invoke();
    }

    private void SelectRightGunKeyPress()
    {
        OnSelectRightGunKeyPressed?.Invoke();
    }

    private void WorldUIKeyPress()
    {
        OnWorldUIKeyPressed?.Invoke();
    }

    private void WorldUIKeyRelease()
    {
        OnWorldUIKeyReleased?.Invoke();
    }

    private void ToggleWorldUIKeyPress()
    {
        OnToggleWorldUIKeyPressed?.Invoke();
    }

    private void OnDestroy()
    {
        OnDeselectKeyPressed = null;
        OnUndoKeyPressed = null;
        OnSelectLeftGunKeyPressed = null;
        OnSelectRightGunKeyPressed = null;
        OnWorldUIKeyPressed = null;
        OnWorldUIKeyReleased = null;
        OnToggleWorldUIKeyPressed = null;
    }

    private bool IsPointerOverGameObject() => EventSystem.current.IsPointerOverGameObject();
}
