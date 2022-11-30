﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Elevator : MonoBehaviour, IDamageable, IEndActionNotifier
{
    protected const int MissHit = 0;
    protected const int NormalHit = 1;
    protected const int CriticalHit = 2;
    
    [Header("Stats")] [SerializeField] private int _extraRange;

    [SerializeField] private int _extraCrit;

    [SerializeField] private float _maxHp;

    [SerializeField] private float _currentHp;

    [SerializeField] private int _fallDamagePercentage;

    [SerializeField] private float _platformMaxHeight;
    [SerializeField] private float _movementDuration;
    [SerializeField] private float _fallMovementDuration;
    [SerializeField] private float _timeToDestroy;

    [Header("References")]
    [SerializeField] private Transform _platform;
    [SerializeField] private GameObject _button;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private GameObject _colliderForAttack;
    [SerializeField] private GameObject _shaderObj;

    [Header("Data")]
    public LayerMask block;
    [SerializeField] private SoundData _moveSound;
    [SerializeField] private SoundData _damageSound;
    [SerializeField] private SoundData _destroySound;
    [SerializeField] private ParticleSystem _damageParticle;
    [SerializeField] private ParticleSystem _destroyParticle;

    private Vector3 _startingPos;
    private bool _active;

    private Tile _tileBelow;

    private bool _canInteract;

    private bool _isMoving;
    private bool _usedThisTurn;

    private Character _aboveCharacter; 

    public Action<IEndActionNotifier> OnEndAction { get ; set ; }

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

        GameManager.Instance.OnEndTurn += DeactivateButton;
        GameManager.Instance.OnBeginTurn += Restart;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_currentHp <= 0)
            return;

        if (_isMoving)
            return;
        
        if (!_canInteract)
            return;

        if (_aboveCharacter)
        {
            if (!_aboveCharacter.IsMyTurn())
                return;

            if (_aboveCharacter.GetUnitTeam() == EnumsClass.Team.Red)
                return;
        }

        StartCoroutine(CheckCharacterDelay());
    }

    IEnumerator CheckCharacterDelay()
    {
        yield return new WaitUntil(() => _tileBelow.GetUnitAbove() != null);

        _aboveCharacter = _tileBelow.GetUnitAbove();

        _aboveCharacter.OnMechaTurnStart += ActivateButton;
        
        ActivateButton();
    }

    private void OnTriggerExit(Collider other)
    {
        if (_currentHp <= 0)
            return;

        if (_isMoving)
            return;
        
        if (!_aboveCharacter) 
            return;

        _aboveCharacter.OnMechaTurnStart -= ActivateButton;

        _aboveCharacter = null;
        DeactivateButton();
    }

    public void StartMovement()
    {
        _usedThisTurn = true;

        DisableShaderObject();

        StartCoroutine(Move());
        DeactivateButton();
    }
    private IEnumerator Move()
    {
        if (!_canInteract)
            yield break;

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
            _colliderForAttack.SetActive(true);

            _aboveCharacter.OnMechaAttack += DeactivateButton;
            _aboveCharacter.OnMechaAttack += DisableShaderObject;
        }
        else
        {
            start = _platform.position;
            end = _startingPos;
            _active = false;
            _tileBelow.AddToNeighbour();
            _canInteract = false;
            _colliderForAttack.SetActive(false);

            _aboveCharacter.OnMechaAttack -= DeactivateButton;
            _aboveCharacter.OnMechaAttack -= DisableShaderObject;
        }

        AudioManager.Instance.PlaySound(_moveSound, gameObject);

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
        //AudioManager.Instance.StopSoundWithFadeOut(this.gameObject.GetComponent<AudioSource>().clip, this.gameObject);

    }

    private void Restart()
    {
        _usedThisTurn = false;
        
        if (GameManager.Instance.ActiveTeam == EnumsClass.Team.Green)
        {
            if (!_aboveCharacter || _aboveCharacter == GameManager.Instance.CurrentTurnMecha)
            {
                _canInteract = true;
                EnableShaderObject();
            }

            if (_aboveCharacter == GameManager.Instance.CurrentTurnMecha)
                ActivateButton();
        }
        else
        {
            _canInteract = false;
            DisableShaderObject();
            _button.SetActive(false);

            DeactivateButton();
        }
    }

    private void ActivateButton()
    {
        if (_currentHp <= 0)
            return;

        if (GameManager.Instance.ActiveTeam == EnumsClass.Team.Red)
            return;

        if (_usedThisTurn)
            return;

        if (_active)
            _buttonText.text = "DOWN";
        else
            _buttonText.text = "UP";

        _button.SetActive(true);

        EnableShaderObject();
    }
    
    private void DeactivateButton()
    {
        _button.SetActive(false);
    }

    private void EnableShaderObject()
    {
        _shaderObj.SetActive(true);
    }
    private void DisableShaderObject()
    {
        _shaderObj.SetActive(false);
    }

    public void ReceiveDamage(List<Tuple<int, int>> damages)
    {
        int total = 0;
        for (int i = 0; i < damages.Count; i++)
        {
            total += damages[i].Item1;
            float hp = _currentHp - damages[i].Item1;
            _currentHp = hp > 0 ? hp : 0;
            
            EffectsController.Instance.PlayParticlesEffect(_damageParticle, transform.position, transform.forward);            
            AudioManager.Instance.PlaySound(_damageSound, gameObject);

            int item = damages[i].Item2;
            switch (item)
            {
                case MissHit:
                    EffectsController.Instance.CreateDamageText("Miss", 0, _platform.position, i == damages.Count - 1 ? true : false);
                    break;

                case NormalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 1, _platform.position, i == damages.Count - 1 ? true : false);
                    break;

                case CriticalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 2, _platform.position, i == damages.Count - 1 ? true : false);
                    break;
            }
        }

        if (_currentHp <= 0)
            Dead();
    }

    public void ReceiveDamage(int damage)
    {
        float hp = _currentHp - damage;
        _currentHp = hp > 0 ? hp : 0;

        EffectsController.Instance.CreateDamageText(damage.ToString(), 1, transform.position, true);

        EffectsController.Instance.PlayParticlesEffect(_damageParticle, transform.position, transform.forward);
        AudioManager.Instance.PlaySound(_damageSound, gameObject);

        if (_currentHp <= 0)
            Dead();
    }

    private void Dead()
    {
        _colliderForAttack.SetActive(false);
        _aboveCharacter.transform.parent = null;
        _aboveCharacter.CharacterElevatedState(false, -_extraRange, -_extraCrit);
        _aboveCharacter.GetComponent<Rigidbody>().isKinematic = false;

        AudioManager.Instance.PlaySound(_destroySound, gameObject);
        EffectsController.Instance.PlayParticlesEffect(_destroyParticle, transform.position, transform.forward);

        DisableShaderObject();

        StartCoroutine(Fall());
    }

    public GameObject GetColliderForAttack()
    {
        return _colliderForAttack;
    }

    private IEnumerator Fall()
    {
        float time = 0;
        var startPos = _platform.position;
        var endPos = _startingPos;
        while (time <= _fallMovementDuration)
        {
            time += Time.deltaTime;
            var normalizedTime = time / _fallMovementDuration;
            _platform.position = Vector3.Lerp(startPos, endPos, normalizedTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(_timeToDestroy);
        _aboveCharacter.TakeFallDamage(_fallDamagePercentage);

        GameManager.Instance.OnEndTurn -= DeactivateButton;
        GameManager.Instance.OnBeginTurn -= Restart;

        _aboveCharacter.OnMechaAttack -= DeactivateButton;
        _aboveCharacter.OnMechaAttack -= DisableShaderObject;

        _aboveCharacter = null;

        Destroy(gameObject);
    }
}
