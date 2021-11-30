using System;
using System.Collections.Generic;
using UnityEngine;

public class PiercingShot : Ability
{
	private TileHighlight _highlight;
	private int _abilityUseRange;
    private HashSet<Tile> _tilesInRange = new HashSet<Tile>();
    private List<Character> _charactersToAttack = new List<Character>();

    private PiercingShotSO _abilityData;

    public override void Initialize(Character character, EquipableSO data, Location location)
    {
        base.Initialize(character, data, location);
	    
        _abilityData = data as PiercingShotSO;
    }
    
	public override void Select()
	{
		if (_inCooldown || !_character.CanAttack()) return;
		Debug.Log("Pinto tiles de piercing shot");
        
        switch (_location)
        {
            case Location.LeftGun:
                var left = _character.GetLeftGun();
                if (left) _abilityUseRange = left.GetAttackRange();
                break;
            
            case Location.RightGun:
                var right = _character.GetRightGun();
                if (right) _abilityUseRange = right.GetAttackRange();
                break;
        }
        //_abilityUseRange = _character.GetRightGun().GetAttackRange();//Estaría bueno conseguir el rango del arma que tenga la habilidad.
        //damage = _character.GetLeftGun().GetBulletDamage() * (_character.GetLeftGun().GetAvailableBullets() / 2);//Formulita para hacer el daño dinamico según que arma tiene
		if (!_highlight)
			_highlight = FindObjectOfType<TileHighlight>();
        //_abilityUseRange = _abilityData.pushUseRange;
        //PaintUseTiles(_character.GetMyPositionTile(), 0, Vector3.zero);//Cambie porque no me pintaba todos los tiles
        PaintTilesInRange(_character.GetMyPositionTile(), 0, Vector3.forward);
        PaintTilesInRange(_character.GetMyPositionTile(), 0, -Vector3.forward);
        PaintTilesInRange(_character.GetMyPositionTile(), 0, Vector3.right);
        PaintTilesInRange(_character.GetMyPositionTile(), 0, -Vector3.right);
        _character.EquipableSelectionState(true, this);
        _character.DeselectThisUnit();
	}

	public override void Deselect()
	{
        if(_tilesInRange.Count != 0)
            _highlight.MortarClearTilesInAttackRange(_tilesInRange);
        _tilesInRange.Clear();
        _charactersToAttack.Clear();
        _character.EquipableSelectionState(false, null);
        _character.SelectThisUnit();
    }

	public override void Use(Action callback = null)
	{
		base.Use(callback);
        Debug.Log("Using Piercing Shot");
		if (Input.GetMouseButtonDown(0))
		{
            Debug.Log("Piercing Shooting");
            var selectedTile = MouseRay.GetTargetTransform(_character.block);
		
            if (!selectedTile) return;
		
            var tile = selectedTile.GetComponent<Tile>();

            if (!tile) return;
		
            if (!_tilesInRange.Contains(tile)) return;
            
            var characterTilePos = _character.GetMyPositionTile().transform.position;
            var dir = selectedTile.transform.position - characterTilePos;
            RaycastHit hit;
            Physics.Raycast(characterTilePos, dir, out hit);
            var firstTile = hit.transform.GetComponent<Tile>();
            GetCharactersInAttackDirection(firstTile, dir, 0);
            
            ShootTheShot();
            
            if (callback != null)
                callback();
            
            AbilityUsed(_abilityData);
            UpdateButtonText(_availableUses.ToString(), _abilityData);
            _button.interactable = false;
            Deselect();
		}

        if (Input.GetMouseButtonDown(1))
            Deselect();
    }

    private List<Character> GetCharactersInAttackDirection(Tile currentTile, Vector3 dir, int count)
	{
        //Primero agrego el character que esta en el rango de ataque
		if (!currentTile.IsFree())
		{
            var unitToAttack = currentTile.GetUnitAbove();
            if (unitToAttack && !unitToAttack.IsDead())
                _charactersToAttack.Add(unitToAttack);
		}

        if (count >= _abilityUseRange) return _charactersToAttack;
        count++;
        RaycastHit hit;
        Physics.Raycast(currentTile.transform.position, dir, out hit);
        var nextTile = hit.transform.GetComponent<Tile>();
		if (nextTile)//Despues chequeo cual es el proximo tile en esa dirección
		{
            Tile t = nextTile.transform.GetComponent<Tile>();

            if(t && t.IsWalkable())//Si existe el tile continuo con la cadena
			{
                return GetCharactersInAttackDirection(t, dir, count);
			}
		}
        return _charactersToAttack;
	}

    private void ShootTheShot()
	{
        foreach(var attackedCharacter in _charactersToAttack)
		{
            attackedCharacter.SetHurtAnimation();
            attackedCharacter.GetBody().TakeDamage(_abilityData.damage);
            EffectsController.Instance.PlayParticlesEffect(attackedCharacter.GetBurningSpawner(), EnumsClass.ParticleActionType.Damage);
        }
        _character.DeactivateAttack();
	}

    private void PaintTilesInRange(Tile currentTile, int count, Vector3 dir)
	{
        if(!_tilesInRange.Contains(currentTile))
            _tilesInRange.Add(currentTile);

        if (count >= _abilityUseRange)
            return;

        count++;

        RaycastHit hit;
        Physics.Raycast(currentTile.transform.position, dir, out hit);
        Tile t = hit.transform.GetComponent<Tile>();

        if (t && t.IsWalkable())
		{
            _highlight.MortarPaintTilesInAttackRange(t);
            PaintTilesInRange(t, count, dir);
		}
        return;
    }

    private void PaintUseTiles(Tile currentTile, int count, Vector3 dir)
    {
        _tilesInRange.Add(currentTile);
        if (count >= _abilityUseRange) //|| (_tilesInRangeChecked.ContainsKey(currentTile) && _tilesInRangeChecked[currentTile] <= count))
            return;

        count++;
        if (dir == Vector3.zero)
        {
            Debug.Log("comienzo");
            RaycastHit forwardHit;
            RaycastHit leftHit;
            RaycastHit rightHit;
            RaycastHit backHit;

            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.forward, out forwardHit);
            Physics.Raycast(position, currentTile.transform.right * -1, out leftHit);
            Physics.Raycast(position, currentTile.transform.right, out rightHit);
            Physics.Raycast(position, currentTile.transform.forward * -1, out backHit);

            if (forwardHit.transform)
            {
                Tile t = forwardHit.transform.GetComponent<Tile>();

                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, forwardHit.transform.forward);
                }
            }

            if (leftHit.transform)
            {
                Tile t = leftHit.transform.GetComponent<Tile>();

                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, leftHit.transform.right * -1);
                }
            }

            if (rightHit.transform)
            {
                Tile t = rightHit.transform.GetComponent<Tile>();

                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, rightHit.transform.right);
                }
            }

            if (backHit.transform)
            {
                Tile t = backHit.transform.GetComponent<Tile>();

                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, backHit.transform.forward * -1);
                }
            }
        }

        if (dir == Vector3.forward)
        {
            Debug.Log("forward count: " + count);
            RaycastHit forwardHit;

            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.forward, out forwardHit);
            if (forwardHit.transform)
            {
                Tile t = forwardHit.transform.GetComponent<Tile>();

                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, forwardHit.transform.forward);
                }
            }
            else return;
        }

        if (dir == Vector3.left)
        {
            RaycastHit leftHit;

            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.right * -1, out leftHit);

            if (leftHit.transform)
            {
                Tile t = leftHit.transform.GetComponent<Tile>();

                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, leftHit.transform.right * -1);
                }
            }
            else return;
        }

        if (dir == Vector3.right)
        {
            RaycastHit rightHit;

            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.right * -1, out rightHit);

            if (rightHit.transform)
            {
                Tile t = rightHit.transform.GetComponent<Tile>();

                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, rightHit.transform.right);
                }
            }
            else return;
        }

        if (dir == Vector3.back)
        {
            RaycastHit backHit;

            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.forward * -1, out backHit);

            if (backHit.transform)
            {
                Tile t = backHit.transform.GetComponent<Tile>();

                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, backHit.transform.forward * -1);
                }
            }
            else return;
        }
    }
}
