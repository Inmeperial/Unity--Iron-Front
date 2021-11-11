using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Elevator : MonoBehaviour, IObserver
{
    [Header("Stats")] 
    [SerializeField] private int _extraRange;

    [SerializeField] private int _extraCrit;

    [SerializeField] private float _maxHp;

    private float _currentHp;
    
    [Header("Others")]
    public LayerMask block;
    [SerializeField] private float _height;
    private float _startingHeight;
    [SerializeField] private float _movementDuration;

    private bool _active;

    private Tile _tileBelow;

    [SerializeField] private bool _canInteract;
    
    private delegate void Execute();
    Dictionary<string, Execute> _actionsDic = new Dictionary<string, Execute>();

    private Character _aboveCharacter;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _currentHp = _maxHp;
        _canInteract = true;
        _startingHeight = transform.position.y;
        
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
        
        _aboveCharacter = other.GetComponent<Character>();

        if (!_aboveCharacter) return;
        
        if (_aboveCharacter.CanAttack())
        {
            _aboveCharacter.OnEnterElevator(this);
            ActivateUpButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_currentHp <= 0) return;
        
        if (!_aboveCharacter) return;
        
        _aboveCharacter.OnExitElevator(this);
        _aboveCharacter = null;
    }

    public void StartMovement()
    {
        StartCoroutine(Move());
        DeactivateUpButton();
    }
    IEnumerator Move()
    {
        if (!_canInteract) yield break;
        
        Vector3 start = transform.position;
        Vector3 end = start;
        float time = 0;

        //TODO: Borrar desp el getunitabove
        _aboveCharacter = _tileBelow.GetUnitAbove();
        if (_aboveCharacter)
        {
            _aboveCharacter.transform.parent = transform;
            if (!_active)
            {
                end.y = _height;
                _active = true;
                _tileBelow.RemoveFromNeighbour();
                _canInteract = false;
                _aboveCharacter.CharacterElevatedState(true, _extraRange, _extraCrit);
            }
            else
            {
                end.y = _startingHeight;
                _active = false;
                _tileBelow.AddToNeighbour();
                _canInteract = false;
                _aboveCharacter.CharacterElevatedState(false, -_extraRange, -_extraCrit);
                _aboveCharacter.SelectThisUnit();
            }

            yield return new WaitForSeconds(2);
        
            while (time <= _movementDuration)
            {
                time += Time.deltaTime;
                var normalizedTime = time / _movementDuration;
                transform.position = Vector3.Lerp(start, end, normalizedTime);
                yield return new WaitForEndOfFrame();
            }
            DeactivateButton();
            if (!_active)
                _aboveCharacter.transform.parent = null;
        }
    }

    private void CanInteractAgain()
    {
        _canInteract = true;
    }

    private void ActivateButton()
    {
        if (_active)
        {
            DeactivateUpButton();
            ActivateDownButton();
        }
        else
        {
            DeactivateDownButton();
            ActivateUpButton();
        }
    }
    
    private void DeactivateButton()
    {
        DeactivateDownButton();
        DeactivateUpButton();
    }
    private void ActivateUpButton()
    {
        if (_currentHp <= 0) return;
        //TODO: Activar boton
    }

    private void DeactivateUpButton()
    {
        //TODO: DesactivarBoton
    }
    
    private void ActivateDownButton()
    {
        if (_currentHp <= 0) return;
        //TODO: Activar boton
    }

    private void DeactivateDownButton()
    {
        //TODO: DesactivarBoton
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
