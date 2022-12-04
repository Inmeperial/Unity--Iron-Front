using System;
using System.Collections.Generic;
using UnityEngine;

public class PiercingShot : WeaponAbility
{
    private HashSet<Tile> _tilesInRange = new HashSet<Tile>();
    private List<Character> _charactersToAttack = new List<Character>();

    private PiercingShotSO _abilityData;

    public override void Initialize(Character character, EquipableSO data)
    {
        base.Initialize(character, data);
	    
        _abilityData = data as PiercingShotSO;
    }
    
	public override void Select()
	{
		if (_inCooldown || !_character.CanAttack())
            return;

        if (!_gun)
            return;

        //damage = _character.GetLeftGun().GetBulletDamage() * (_character.GetLeftGun().GetAvailableBullets() / 2);//Formulita para hacer el daño dinamico según que arma tiene

        //_abilityUseRange = _abilityData.pushUseRange;
        //PaintUseTiles(_character.GetMyPositionTile(), 0, Vector3.zero);//Cambie porque no me pintaba todos los tiles

        OnEquipableSelected?.Invoke();

        PaintTilesInRange(_character.GetPositionTile(), 0, Vector3.forward);
        PaintTilesInRange(_character.GetPositionTile(), 0, -Vector3.forward);
        PaintTilesInRange(_character.GetPositionTile(), 0, Vector3.right);
        PaintTilesInRange(_character.GetPositionTile(), 0, -Vector3.right);

        _character.EquipableSelectionState(true, this);

        _character.DeselectThisUnit();
	}

	public override void Deselect()
	{
        OnEquipableDeselected?.Invoke();

        if(_tilesInRange.Count != 0)
            TileHighlight.Instance.MortarClearTilesInAttackRange(_tilesInRange);

        _tilesInRange.Clear();
        _charactersToAttack.Clear();
        _character.EquipableSelectionState(false, null);
        _character.SelectThisUnit();
    }

	public override void Use()
	{
		base.Use();

		if (Input.GetMouseButtonDown(0))
            ExecuteAbility();

        if (Input.GetMouseButtonDown(1))
            Deselect();
    }

    private void ExecuteAbility()
    {
        Transform selectedTile = MouseRay.GetTargetTransform(_character.GetBlockLayerMask());

        if (!selectedTile)
            return;

        Tile tile = selectedTile.GetComponent<Tile>();

        if (!tile)
            return;

        if (!_tilesInRange.Contains(tile))
            return;

        Vector3 characterTilePos = _character.GetPositionTile().transform.position;

        Vector3 dir = selectedTile.transform.position - characterTilePos;

        Physics.Raycast(characterTilePos, dir, out RaycastHit hit);

        Tile firstTile = hit.transform.GetComponent<Tile>();

        GetCharactersInAttackDirection(firstTile, dir, 0);

        Shoot();

        OnEquipableUsed?.Invoke();

        AbilityUsed(_abilityData);

        UpdateButtonText(_availableUses.ToString(), _abilityData);

        _button.interactable = false;

        Deselect();
    }

    private List<Character> GetCharactersInAttackDirection(Tile currentTile, Vector3 dir, int count)
	{
        //Primero agrego el character que esta en el rango de ataque
		if (!currentTile.IsFree())
		{
            Character unitToAttack = currentTile.GetUnitAbove();

            if (unitToAttack && !unitToAttack.IsDead())
                _charactersToAttack.Add(unitToAttack);
		}

        if (count >= _gun.GetAttackRange())
            return _charactersToAttack;

        count++;

        Physics.Raycast(currentTile.transform.position, dir, out RaycastHit hit);

        Tile nextTile = hit.transform.GetComponent<Tile>();

		if (nextTile)//Despues chequeo cual es el proximo tile en esa dirección
		{
            Tile tile = nextTile.transform.GetComponent<Tile>();

            if(tile && tile.IsWalkable())//Si existe el tile continuo con la cadena
                return GetCharactersInAttackDirection(tile, dir, count);
		}

        return _charactersToAttack;
	}

    private void Shoot()
	{
        foreach(Character attackedCharacter in _charactersToAttack)
		{
            Body body = attackedCharacter.GetBody();
            body.ReceiveDamage(_abilityData.damage);
        }
        _character.DoAttackAction();
	}

    private void PaintTilesInRange(Tile currentTile, int count, Vector3 dir)
	{
        if(!_tilesInRange.Contains(currentTile))
            _tilesInRange.Add(currentTile);

        if (count >= _gun.GetAttackRange())
            return;

        count++;

        Physics.Raycast(currentTile.transform.position, dir, out RaycastHit hit);
        Tile tile = hit.transform.GetComponent<Tile>();

        if (tile && tile.IsWalkable())
		{
            TileHighlight.Instance.MortarPaintTilesInAttackRange(tile);
            PaintTilesInRange(tile, count, dir);
		}
        return;
    }

    private void PaintUseTiles(Tile currentTile, int count, Vector3 dir)
    {
        _tilesInRange.Add(currentTile);

        if (count >= _gun.GetAttackRange()) //|| (_tilesInRangeChecked.ContainsKey(currentTile) && _tilesInRangeChecked[currentTile] <= count))
            return;

        count++;

        if (dir == Vector3.zero)
        {
            Debug.Log("comienzo");

            Vector3 position = currentTile.transform.position;

            Physics.Raycast(position, currentTile.transform.forward, out RaycastHit forwardHit);
            Physics.Raycast(position, currentTile.transform.right * -1, out RaycastHit leftHit);
            Physics.Raycast(position, currentTile.transform.right, out RaycastHit rightHit);
            Physics.Raycast(position, currentTile.transform.forward * -1, out RaycastHit backHit);

            if (forwardHit.transform)
            {
                Tile tile = forwardHit.transform.GetComponent<Tile>();

                if (tile && tile.IsWalkable())
                {
                    TileHighlight.Instance.MortarPaintTilesInActivationRange(tile);
                    PaintUseTiles(tile, count, forwardHit.transform.forward);
                }
            }

            if (leftHit.transform)
            {
                Tile tile = leftHit.transform.GetComponent<Tile>();

                if (tile && tile.IsWalkable())
                {
                    TileHighlight.Instance.MortarPaintTilesInActivationRange(tile);
                    PaintUseTiles(tile, count, leftHit.transform.right * -1);
                }
            }

            if (rightHit.transform)
            {
                Tile tile = rightHit.transform.GetComponent<Tile>();

                if (tile && tile.IsWalkable())
                {
                    TileHighlight.Instance.MortarPaintTilesInActivationRange(tile);
                    PaintUseTiles(tile, count, rightHit.transform.right);
                }
            }

            if (backHit.transform)
            {
                Tile tile = backHit.transform.GetComponent<Tile>();

                if (tile && tile.IsWalkable())
                {
                    TileHighlight.Instance.MortarPaintTilesInActivationRange(tile);
                    PaintUseTiles(tile, count, backHit.transform.forward * -1);
                }
            }
        }

        if (dir == Vector3.forward)
        {
            Debug.Log("forward count: " + count);

            Vector3 position = currentTile.transform.position;

            Physics.Raycast(position, currentTile.transform.forward, out RaycastHit forwardHit);

            if (!forwardHit.transform)
                return;

            else
            {
                Tile tile = forwardHit.transform.GetComponent<Tile>();

                if (tile && tile.IsWalkable())
                {
                    TileHighlight.Instance.MortarPaintTilesInActivationRange(tile);
                    PaintUseTiles(tile, count, forwardHit.transform.forward);
                }
            }
        }

        if (dir == Vector3.left)
        {
            Vector3 position = currentTile.transform.position;

            Physics.Raycast(position, currentTile.transform.right * -1, out RaycastHit leftHit);

            if (!leftHit.transform)
                return;
            else
            {
                Tile tile = leftHit.transform.GetComponent<Tile>();

                if (tile && tile.IsWalkable())
                {
                    TileHighlight.Instance.MortarPaintTilesInActivationRange(tile);
                    PaintUseTiles(tile, count, leftHit.transform.right * -1);
                }
            }
        }

        if (dir == Vector3.right)
        {
            Vector3 position = currentTile.transform.position;
            
            Physics.Raycast(position, currentTile.transform.right * -1, out RaycastHit rightHit);

            if (!rightHit.transform)
                return;

            else
            {
                Tile tile = rightHit.transform.GetComponent<Tile>();

                if (tile && tile.IsWalkable())
                {
                    TileHighlight.Instance.MortarPaintTilesInActivationRange(tile);
                    PaintUseTiles(tile, count, rightHit.transform.right);
                }
            }
        }

        if (dir == Vector3.back)
        {
            Vector3 position = currentTile.transform.position;
         
            Physics.Raycast(position, currentTile.transform.forward * -1, out RaycastHit backHit);

            if (!backHit.transform)
                return;
            else
            {
                Tile tile = backHit.transform.GetComponent<Tile>();

                if (tile && tile.IsWalkable())
                {
                    TileHighlight.Instance.MortarPaintTilesInActivationRange(tile);
                    PaintUseTiles(tile, count, backHit.transform.forward * -1);
                }
            }
        }
    }

    public override string GetEquipableName()
    {
        return _abilityData.objectName;
    }

    public override string GetEquipableDescription()
    {
        return _abilityData.objectDescription;
    }
}
