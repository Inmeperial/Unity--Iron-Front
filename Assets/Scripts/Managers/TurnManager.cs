﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.Experimental.PlayerLoop;

public class TurnManager : EnumsClass, IObservable, IObserver
{
    [SerializeField] private Transform _canvasTransform;
    private List<Character> _greenTeam = new List<Character>();
    private int _greenDeadCount;
    private List<Character> _redTeam = new List<Character>();
    private int _redDeadCount;
    private Character[] _allUnits;
    private CharacterSelection _charSelect;
    private TileHighlight _highlight;
    private ButtonsUIManager _buttonsUIManager;

    private Team _activeTeam;

	[SerializeField] private List<Character> _currentTurnOrder = new List<Character>();
    
    private List<IObserver> _observers = new List<IObserver>();

    private int _turnCounter;
    private Character _actualCharacter;
    private CameraMovement _cameraMovement;

    [SerializeField] private List<FramesUI> _portraits = new List<FramesUI>();
    [SerializeField] private List<RectTransform> _portraitsPositions = new List<RectTransform>();
    [SerializeField] private float _moveTime;
    private List<Tuple<Character, FramesUI>> _charAndFramesList = new List<Tuple<Character, FramesUI>>();

    public bool mortarAttack;

    public float waitForMortarAttack;
    
    private delegate void Execute();
    Dictionary<string, Execute> _actionsDic = new Dictionary<string, Execute>();
    private void Awake()
    {
        _cameraMovement = FindObjectOfType<CameraMovement>();
        var units = FindObjectsOfType<Character>();
        _allUnits = new Character[units.Length];
        _allUnits = units;
        
    }

    private void Start()
    {
        SeparateByTeam(_allUnits);
        _charSelect = FindObjectOfType<CharacterSelection>();
        _highlight = FindObjectOfType<TileHighlight>();
        _buttonsUIManager = FindObjectOfType<ButtonsUIManager>();
        _activeTeam = Team.Green;

        var mortars = FindObjectsOfType<Mortar>();
        foreach (var mortar in mortars)
        {
            Subscribe(mortar);
        }
        SetFirstTurn();

        _actualCharacter = _currentTurnOrder[0];
        _actualCharacter.SetTurn(true);
        _activeTeam = _actualCharacter.GetUnitTeam();
        
        _actionsDic.Add("GreenDead", GreenUnitDied);
        _actionsDic.Add("RedDead", RedUnitDied);
        
        Action toDo = () =>
        {
            _buttonsUIManager.ActivateEndTurnButton();
            _charSelect.Selection(_actualCharacter);
        };
        _cameraMovement.MoveTo(_actualCharacter.transform, toDo, _actualCharacter.transform);
    }

    public void UnitIsMoving()
    {
        _charSelect.ActivateCharacterSelection(false);
    }

    public void UnitStoppedMoving()
    {
        _charSelect.ActivateCharacterSelection(true);
    }

    private void SeparateByTeam(Character[] units)
    {
        foreach (var item in units)
        {
            if (item.GetUnitTeam() == Team.Green)
                _greenTeam.Add(item);
            else _redTeam.Add(item);
        }
    }

    public List<Character> GetEnemies(Team myTeam)
    {
        return myTeam == Team.Green ? _redTeam : _greenTeam;
    }

    public void EndTurn()
    {
        var character = _charSelect.GetSelectedCharacter();
        if (character != null && character.IsMoving() != false) return;

        NotifyObserver("EndTurn");
        NotifyObserver("Deselect");
        
        StartCoroutine(OnEndTurn());
    }

    IEnumerator OnEndTurn()
    {
        if (mortarAttack)
        {
            yield return new WaitUntil(() => !mortarAttack);
            yield return new WaitForSeconds(waitForMortarAttack);
        }
        
        MoveToLast();

        ResetTurn(_actualCharacter);

        _actualCharacter = _currentTurnOrder[0];

        while (_actualCharacter.body.GetCurrentHp() <= 0)
        {
            MoveToLast();
            yield return new WaitForSeconds(1);
            _actualCharacter = _currentTurnOrder[0];
        }
        _actualCharacter.SetTurn(true);
        _activeTeam = _actualCharacter.GetUnitTeam();
        Action toDo = () =>
        {
            _buttonsUIManager.ActivateEndTurnButton();
            _charSelect.Selection(_actualCharacter);
        };
        _cameraMovement.MoveTo(_actualCharacter.transform, toDo, _actualCharacter.transform);
    }

    void MoveToLast()
    {
        _buttonsUIManager.DeactivateEndTurnButton();
        StartCoroutine(MovePortrait(GetMyRect(_actualCharacter), 0, _portraitsPositions.Count-1));
        _currentTurnOrder.RemoveAt(0);
        _currentTurnOrder.Insert(_currentTurnOrder.Count, _actualCharacter);
        
        var previous = new Dictionary<Character, int>();

        for (var i = 0; i < _currentTurnOrder.Count; i++)
        {
            var c = _currentTurnOrder[i];
            previous.Add(c, i);
        }
        
        for (int i = 0; i < _portraits.Count-1; i++)
        {
            var c = _currentTurnOrder[i];
            StartCoroutine(MovePortrait(GetMyRect(c),previous[c], i));
        }
    }

    private void ResetTurn(List<Character> team)
    {
        _charSelect.ResetSelector();
        _highlight.EndPreview();
        foreach (var unit in team)
        {
            unit.NewTurn();
        }
    }

    /// <summary>
    /// Reset stats for new turn of the given unit.
    /// </summary>
    private void ResetTurn(Character unit)
    {
        _charSelect.ResetSelector();
        _highlight.EndPreview();
        unit.NewTurn();
    }

    public Team GetActiveTeam()
    {
        return _activeTeam;
    }

    public void UnitCanBeAttacked(Character unit)
    {
        unit.MakeAttackable();
    }

    public void UnitCantBeAttacked(Character unit)
    {
        unit.MakeNotAttackable();
    }

    public Tile GetUnitTile(Character unit)
    {
        return unit.GetTileBelow();
    }

    private void SetFirstTurn()
    {
        var ordered = GetOrderedUnits();

        int count = 0;

        foreach (var character in ordered)
        {
            if (count == _portraits.Count)
                return;
            
            var c = character.Item1;
            var p = _portraits[count];
            p.mechaImage.sprite = c._myIcon;
            p.mechaName.text = c._myName;
            c.gameObject.name = c._myName;
            // p.leftGunIcon.sprite = c.GetLeftGun().GetIcon();
            // p.rightGunIcon.sprite = c.GetRightGun().GetIcon();
            p.selectionButton.OnLeftClick.RemoveAllListeners();
            p.selectionButton.OnLeftClick.AddListener(() => _cameraMovement.MoveTo(c.transform));
            count++;
            _currentTurnOrder.Add(c);
            _charAndFramesList.Add(Tuple.Create(c,p));
        }
    }

    public void OrderTurns()
    {
        var previous = new Dictionary<Character, int>();

        for (var i = 0; i < _currentTurnOrder.Count; i++)
        {
            var character = _currentTurnOrder[i];
            previous.Add(character, i);
        }

        var ordered = GetOrderedUnits();
        
        var pos = 0;
        for (int i = 0; i < ordered.Count; i++)
        {
            if (ordered[i].Item1 == _actualCharacter)
            {
                pos = i;
                break;
            }
        }

        var c = ordered[pos];
        ordered.RemoveAt(pos);
        ordered.Insert(0, c);
        _currentTurnOrder.Clear();

        for (int i = 0; i < ordered.Count; i++)
        {
            var character = ordered[i].Item1;
            _currentTurnOrder.Add(character);
            StartCoroutine(MovePortrait(GetMyRect(character), previous[character], i));
        }
    }

    public void ReducePosition(Character unit)
    {
        var oldPos = GetMyTurn(unit);
        
        //If last, return
        if (oldPos == _currentTurnOrder.Count -1) return;
        
        //Move unit portrait one position behind

        var newPos = 0;

        if (unit.legs.GetCurrentHp() > 0)
        {
            newPos = oldPos + 1;
            StartCoroutine(MovePortrait(GetMyRect(unit), oldPos, newPos));

            //Move portrait that was behind to one position ahead
            var other = _currentTurnOrder[newPos];
            StartCoroutine(MovePortrait(GetMyRect(other), newPos, oldPos));
            _currentTurnOrder.RemoveAt(oldPos);
            _currentTurnOrder.Insert(newPos, unit);
        }
        else
        {
            newPos = _portraitsPositions.Count - 1;
            StartCoroutine(MovePortrait(GetMyRect(unit), oldPos, newPos));
            _currentTurnOrder.RemoveAt(oldPos);
            _currentTurnOrder.Insert(newPos, unit);

            for (int i = oldPos; i < newPos; i++)
            {
                var u = _currentTurnOrder[i];
                StartCoroutine(MovePortrait(GetMyRect(u), i + 1, i));
            }
        } 
        
        
    }

    List<Tuple<Character, float>> GetOrderedUnits()
    {
        var unitsList = new List<Tuple<Character, float>>();
        
        //Adds them to a collection with their initiative
        foreach (var character in _allUnits)
        {
            var t = Tuple.Create(character, character.GetCharacterInitiative());
            unitsList.Add(t);
        }
        
        return unitsList.OrderByDescending(x => x.Item2).ToList();
    }

    public int GetMyTurn(Character unit)
    {
        for (int i = 0; i < _currentTurnOrder.Count; i++)
        {
            if (_currentTurnOrder[i] == unit)
                return i;
        }

        return -1;
    }

    public RectTransform GetMyRect(Character c)
    {
        //var asd = _charAndFramesList.Where(x => x.Item1 == c).Select(y => y.Item2).FirstOrDefault().GetComponent<RectTransform>();
        
        foreach (var t in _charAndFramesList)
        {
            if (t.Item1 == c)
            {
                return t.Item2.GetComponent<RectTransform>();
            }
        }

        return null;
    }


    IEnumerator MovePortrait(RectTransform myRect, int currentPos, int newPos)
    {
        var startV = _portraitsPositions[currentPos].gameObject.GetComponent<RectTransform>();
        var end = _portraitsPositions[newPos].gameObject.GetComponent<RectTransform>();
        var midRectA = new RectTransform();
        var midGoA = new GameObject();
        if (Mathf.Abs(currentPos - newPos) > 1)
        {
            midGoA.transform.parent = _canvasTransform;
            midRectA = midGoA.AddComponent<RectTransform>();
            midRectA.gameObject.name = "INSTANCIA";
            midRectA.anchorMax = Vector2.Lerp(startV.anchorMax, end.anchorMax, 0.5f);
            midRectA.anchorMin = Vector2.Lerp(startV.anchorMin, end.anchorMin, 0.5f);
            var anchorMax = midRectA.anchorMax;
            anchorMax.y = 0.75f;
            anchorMax.x = Mathf.Lerp(startV.anchorMax.x, end.anchorMax.x, 0.5f);
            midRectA.anchorMax = anchorMax;
            var anchorMin = midRectA.anchorMin;
            anchorMin.y = 0.75f - (startV.anchorMax.y - startV.anchorMin.y);
            anchorMin.x = anchorMax.x - (startV.anchorMax.x - startV.anchorMin.x);
            midRectA.anchorMin = anchorMin;
            //Debug.Break();
        }

        var time = 0f;
        
        while (time <= _moveTime)
        {
            time += Time.deltaTime;
            var normalized = time / _moveTime;

            if (Mathf.Abs(currentPos - newPos) > 1)
            {
                if (normalized <= 0.5f)
                {
                    myRect.anchorMax = Vector2.Lerp(startV.anchorMax, midRectA.anchorMax, normalized * 2);
                    myRect.anchorMin = Vector2.Lerp(startV.anchorMin, midRectA.anchorMin, normalized * 2);
                }
                else
                {
                    myRect.anchorMax = Vector2.Lerp(midRectA.anchorMax, end.anchorMax, normalized);
                    myRect.anchorMin = Vector2.Lerp(midRectA.anchorMin, end.anchorMin, normalized);
                }
            }
            else
            {
                myRect.anchorMax = Vector2.Lerp(startV.anchorMax, end.anchorMax, normalized);
                myRect.anchorMin = Vector2.Lerp(startV.anchorMin, end.anchorMin, normalized);
            }
            yield return new WaitForEndOfFrame();
        }

        myRect.anchoredPosition = end.anchoredPosition;
        
        Destroy(midGoA);
    }

    public void PortraitsActiveState(bool state)
    {
        foreach (var p in _portraits)
        {
            p.gameObject.SetActive(state);
        }
    }

    public void ActivatePortraitsButtons()
    {
        foreach (var portrait in _portraits)
        {
            portrait.selectionButton.interactable = true;
        }
    }
    
    public void DeactivatePortraitsButtons()
    {
        foreach (var portrait in _portraits)
        {
            portrait.selectionButton.interactable = false;
        }
    }

    public void DeadPortrait(Character character)
    {
        foreach (var c in _charAndFramesList)
        {
            if (c.Item1 == character)
                c.Item2.mechaImage.color = Color.red;
        }
        
    }

    public Character[] GetAllUnits()
    {
        return _allUnits;
    }

    private void GreenUnitDied()
    {
        _greenDeadCount++;
        Debug.Log("green count: " + _greenTeam.Count);
        Debug.Log("green  dead count: " + _greenDeadCount);
        if (_greenDeadCount < _greenTeam.Count) return;
        Debug.Log("Lose");
        FindObjectOfType<ChangeScene>().Defeat();

    }

    private void RedUnitDied()
    {
        _redDeadCount++;
        Debug.Log("red count: " + _redTeam.Count);
        Debug.Log("red  dead count: " + _redDeadCount);
        if (_redDeadCount < _redTeam.Count) return;
        Debug.Log("Win");
        FindObjectOfType<ChangeScene>().Win();
    }

    public void Notify(string action)
    {
        _actionsDic[action]();
    }
    
    
    public void Subscribe(IObserver observer)
    {
        if (_observers.Contains(observer) == false)
            _observers.Add(observer);
    }

    public void Unsubscribe(IObserver observer)
    {
        if (_observers.Contains(observer))
            _observers.Remove(observer);
    }

    public void NotifyObserver(string action)
    {
        for (int i = _observers.Count - 1; i >= 0; i--)
        {
            _observers[i].Notify(action);
        }
    }
}
