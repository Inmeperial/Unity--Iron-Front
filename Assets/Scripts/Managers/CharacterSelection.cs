using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterSelection : MonoBehaviour
{
    public LayerMask charMask;
    public LayerMask gridBlockMask;
    private Character _selection;
    private TileHighlight _highlight;
    private TurnManager _turnManager;
    ButtonsUIManager _buttonsManager;
    private bool _canSelectUnit;
    private Character _enemySelection;
    
    public bool playerSelected;

    public static CharacterSelection Instance;
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
        _canSelectUnit = true;
        _highlight = GetComponent<TileHighlight>();
        _turnManager = FindObjectOfType<TurnManager>();
        _buttonsManager = FindObjectOfType<ButtonsUIManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (Input.GetMouseButtonDown(0) && _canSelectUnit && MouseRay.CheckIfType(charMask))
            {
                SelectCharacterFromObject(charMask);

            }
            if (Input.GetMouseButtonDown(0) && _canSelectUnit && MouseRay.CheckIfType(gridBlockMask))
            {
                SelectCharacterFromTile(gridBlockMask);
            }
        }
    }

    private void SelectCharacterFromTile(LayerMask layerMask)
    {
        var tile = MouseRay.GetTargetGameObject(layerMask);
        if (!tile || !tile.CompareTag("GridBlock")) return;
        
        var c = tile.GetComponent<Tile>().GetCharacterAbove();
        if (c) Selection(c);
    }
    
    private void SelectCharacterFromObject(LayerMask mask)
    {
        var character = MouseRay.GetTargetTransform(mask);
        
        if (!character || !character.CompareTag("Character")) return;
        var c = character.GetComponent<Character>();
        Selection(c);
    }

    /// <summary>
    /// Select a Character.
    /// </summary>
    /// <param name="c">The Character to select.</param>
    public void Selection(Character c)
    {
        if (!c.CanBeSelected()) return;
        if (c.IsMyTurn())
        {
            ButtonsUIManager.Instance.DeselectActions();
            if (_selection)
                _selection.DeselectThisUnit();
            if (_enemySelection)
            {
                _enemySelection.DeselectThisUnit();
                _enemySelection = null;
            }
        
            playerSelected = true;
            _selection = c;
            _selection.SelectThisUnit();
            _highlight.ChangeActiveCharacter(_selection);
            ButtonsUIManager.Instance.SetPlayerCharacter(_selection);
            ButtonsUIManager.Instance.SetPlayerUI();
            if (_enemySelection)
            {
                _selection.RotateTowardsEnemy(_enemySelection.transform.position);
            }
        }
        else if (c.GetUnitTeam() != TurnManager.Instance.GetActiveTeam())
        {
            if (_enemySelection)
            {
                _enemySelection.DeselectThisUnit();
            }
            _enemySelection = c;
            if (_selection && _selection.CanAttack())
            {
                _selection.RotateTowardsEnemy(_enemySelection.transform.position);
                bool body = _selection.RayToPartsForAttack(_enemySelection.GetBodyPosition(), "Body") &&
                           _enemySelection.body.GetCurrentHp() > 0;
                
                bool lArm = _selection.RayToPartsForAttack(_enemySelection.GetLArmPosition(), "LArm") &&
                           _enemySelection.leftArm.GetCurrentHp() > 0;
                
                bool rArm= _selection.RayToPartsForAttack(_enemySelection.GetRArmPosition(), "RArm") &&
                          _enemySelection.rightArm.GetCurrentHp() > 0;
                
                bool legs =  _selection.RayToPartsForAttack(_enemySelection.GetLegsPosition(), "Legs") &&
                            _enemySelection.legs.GetCurrentHp() > 0;

                if (body || lArm || rArm || legs)
                {
                    _enemySelection.SelectedAsEnemy();
                    ButtonsUIManager.Instance.SetEnemy(_enemySelection);
                    ButtonsUIManager.Instance.SetEnemyUI();
                    _canSelectUnit = false;
                }
                else
                {
                    _selection.RaysOffDelay();
                    Selection(_selection);
                }
            }
        }
    }

    public Character GetSelectedCharacter()
    {
        return _selection;
    }
    
    public Character GetSelectedEnemy()
    {
        return _enemySelection;
    }

    public void ActivateCharacterSelection(bool state)
    {
        _canSelectUnit = state;
    }

    public void ResetSelector()
    {
        DeselectUnit();
        ButtonsUIManager.Instance.DeactivateExecuteAttackButton();
    }

    public void DeselectUnit()
    {
        if (_selection)
        {
            playerSelected = false;
            _selection.DeselectThisUnit();
            _selection = null;
        }

        if (!_enemySelection) return;
        
        _enemySelection.DeselectThisUnit();
        _enemySelection = null;
    }

    public bool PlayerIsSelected(Character character)
    {
        if (_selection) return character == _selection;
        return false;
    }
}