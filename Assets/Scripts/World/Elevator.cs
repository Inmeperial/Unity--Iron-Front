using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class Elevator : MonoBehaviour, IObserver
{
    [Header("Stats")] 
    [SerializeField] private int _extraRange;

    [SerializeField] private int _extraCrit;

    [SerializeField] private float _maxHp;

    private float _currentHp;
    
    [Header("Others")]
    [SerializeField] private Transform _platform;
    [SerializeField] private GameObject _button;
    [SerializeField] private TextMeshProUGUI _buttonText;
    
    public LayerMask block;
    [SerializeField] private float _platformMaxHeight;
    private Vector3 _startingPos;
    [SerializeField] private float _movementDuration;

    private bool _active;

    private Tile _tileBelow;

    private bool _canInteract;

    private bool _isMoving;
    
    private delegate void Execute();
    Dictionary<string, Execute> _actionsDic = new Dictionary<string, Execute>();

    private Character _aboveCharacter;

    
    // Start is called before the first frame update
    void Start()
    {
        _currentHp = _maxHp;
        _canInteract = true;
        _startingPos = _platform.position;
        _button.SetActive(false);
        var coll = Physics.OverlapBox(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
        
        foreach (var tile in coll)
        {
            var t = tile.GetComponent<Tile>();
            if (t)
            {
                _tileBelow = t;
                break;
            }
        }
        
        _actionsDic.Add("EndTurn", CanInteractAgain);
        _actionsDic.Add("AboveTurn", ActivateButton);
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartMovement();
        }
    }

    //TODO: hacer el popup del boton para activar
    private void OnTriggerEnter(Collider other)
    {
        if (_currentHp <= 0) return;

        if (_isMoving) return;
        
        StartCoroutine(CheckCharacterDelay());
    }

    IEnumerator CheckCharacterDelay()
    {
        yield return new WaitUntil(() => _tileBelow.GetUnitAbove() != null);

        _aboveCharacter = _tileBelow.GetUnitAbove();

        if (_aboveCharacter.CanAttack())
        {
            _aboveCharacter.OnEnterElevator(this);
            ActivateButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_currentHp <= 0) return;

        if (_isMoving) return;
        
        if (!_aboveCharacter) return;
        
        _aboveCharacter.OnExitElevator(this);
        _aboveCharacter = null;
        DeactivateButton();
    }

    public void StartMovement()
    {
        StartCoroutine(Move());
        DeactivateButton();
    }
    IEnumerator Move()
    {
        if (!_canInteract) yield break;

        _isMoving = true;
        Vector3 start;
        Vector3 end;
        float time = 0;

        _aboveCharacter.transform.parent = _platform.transform;
        if (!_active)
        {
            start = _startingPos;
            end = start;
            end.y += _platformMaxHeight;
            _active = true;
            _tileBelow.RemoveFromNeighbour();
            _canInteract = false;
        }
        else
        {
            start = _platform.position;
            end = _startingPos;
            _active = false;
            _tileBelow.AddToNeighbour();
            _canInteract = false;
        }

        while (time <= _movementDuration)
        {
            time += Time.deltaTime;
            var normalizedTime = time / _movementDuration;
            _platform.position = Vector3.Lerp(start, end, normalizedTime);
            yield return new WaitForEndOfFrame();
        }

        _isMoving = false;
        if (!_active)
        {
            _aboveCharacter.transform.parent = null; 
            _aboveCharacter.CharacterElevatedState(false, -_extraRange, -_extraCrit);
            _aboveCharacter.SelectThisUnit();
        }
        else
        {
            _aboveCharacter.CharacterElevatedState(true, _extraRange, _extraCrit);
            _aboveCharacter.ResetTilesInAttackRange();
            yield return new WaitForEndOfFrame();
            _aboveCharacter.PaintTilesInAttackRange(_tileBelow, 0);
        }
    }

    private void CanInteractAgain()
    {
        _canInteract = true;
        _button.SetActive(false);
    }

    private void ActivateButton()
    {
        if (_currentHp <= 0) return;
        
        if (_active)
        {
            _buttonText.text = "DOWN";
        }
        else
        {
            _buttonText.text = "UP";
        }
        _button.SetActive(true);
    }
    
    private void DeactivateButton()
    {
        _button.SetActive(false);
    }
    public void Notify(string action)
    {
        if (_actionsDic.ContainsKey(action))
            _actionsDic[action]();
    }

    public void TakeDamage(List<Tuple<int, int>> damages)
    {
        int total = 0;
        for (int i = 0; i < damages.Count; i++)
        {
            total += damages[i].Item1;
            float hp = _currentHp - damages[i].Item1;
            _currentHp = hp > 0 ? hp : 0;
        }
    }

    public void TakeDamage(int damage)
    {
        float hp = _currentHp - damage;
        _currentHp = hp > 0 ? hp : 0;
    }
}
