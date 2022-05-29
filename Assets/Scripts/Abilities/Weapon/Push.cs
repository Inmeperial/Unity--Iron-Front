using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : WeaponAbility
{
    //private Dictionary<Tile, int> _tilesInRangeChecked = new Dictionary<Tile, int>();
    private HashSet<Tile> _tilesInRange = new HashSet<Tile>();
    private bool collides;
    private Character collidingUnit;
    
    private PushSO _abilityData;

    public override void Initialize(Character character, EquipableSO data)
    {
        base.Initialize(character, data);
	    
        _abilityData = data as PushSO;
    }
    
    public override void Select()
    {
        if (_inCooldown || !_character.CanAttack())
            return;

        PaintUseTiles(_character.GetMyPositionTile(), 0, Vector3.zero);

        _character.EquipableSelectionState(true, this);

        _character.DeselectThisUnit();
    }

    public override void Deselect()
    {
        if(_tilesInRange.Count != 0)
            TileHighlight.Instance.MortarClearTilesInAttackRange(_tilesInRange);

        _tilesInRange.Clear();

        _character.EquipableSelectionState(false, null);

        _character.SelectThisUnit();
    }

    public override void Use(Action callback = null)
    {
        Debug.Log("Using Push");
		if (Input.GetMouseButtonDown(0))
            UseAbility(callback);

        if (Input.GetMouseButtonDown(1))
            Deselect();
    }

    private void UseAbility(Action callback = null)
    {
        Debug.Log("Pushing");

        Transform selectedTile = MouseRay.GetTargetTransform(_character.GetBlockLayerMask());

        if (!selectedTile)
            return;

        Tile tile = selectedTile.GetComponent<Tile>();

        if (!tile)
            return;

        if (!_tilesInRange.Contains(tile))
            return;

        Character selectedUnit = tile.GetUnitAbove();

        if (!selectedUnit)
            return;

        //Para que solo puedas empujar enemigos
        EnumsClass.Team unitTeam = selectedUnit.GetUnitTeam();
        //TODO: Cambiar para que pueda usar tambien la ia
        if (unitTeam == EnumsClass.Team.Green) 
            return;

        Tile tileToPushTo = GetTileToPushTo(tile, (selectedUnit.transform.position - _character.transform.position).normalized, 0);

        PushAction(tileToPushTo, selectedUnit);

        callback?.Invoke();

        AbilityUsed(_abilityData);

        UpdateButtonText(_availableUses.ToString(), _abilityData);

        _button.interactable = false;

        Deselect();
    }
    /// <summary>
    /// Calls the push movement and does the damage
    /// </summary>
    /// <param name="tileToPush"> The tile the unit is pushed to</param>
    /// <param name="enemy"> The unit that is pushed</param>
    private void PushAction(Tile tileBeignPushedTo, Character enemy)
	{
        //Hago un Lerp del enemy hacia el tileBeignPushedTo
        StartCoroutine(LerpPush(enemy.transform, tileBeignPushedTo.transform.position, _abilityData.pushLerpDuration));

        enemy.ChangeMyPosTile(tileBeignPushedTo);

        Body enemyBody = enemy.GetBody();
        enemyBody.TakeDamage(_abilityData.pushDamage);

        GameObject enemyBurningSpawner = enemy.GetBurningSpawner();
        EffectsController.Instance.PlayParticlesEffect(enemyBurningSpawner, EnumsClass.ParticleActionType.Damage);

        if (collides)
		{
            enemyBody.TakeDamage(_abilityData.collisionDamage);

            EffectsController.Instance.PlayParticlesEffect(enemyBurningSpawner, EnumsClass.ParticleActionType.Damage);

            if (collidingUnit)
            {
                //Hacer daño a la otra unidad
                collidingUnit.GetBody().TakeDamage(_abilityData.collisionDamage);

                EffectsController.Instance.PlayParticlesEffect(collidingUnit.GetBurningSpawner(), EnumsClass.ParticleActionType.Damage);
            }
        }
        _character.DeactivateAttack();
	}

    private IEnumerator LerpPush(Transform movingTarget, Vector3 targetPos, float duration)
	{
        float lerpTime = 0;

        Vector3 startPos = movingTarget.position;

        targetPos += (Vector3.up * 4);

        while(lerpTime < duration)
		{
            movingTarget.position = Vector3.Lerp(startPos, targetPos, lerpTime / duration);

            lerpTime += Time.deltaTime;

            yield return null;
		}
        movingTarget.position = targetPos;
	}


    /// <summary>
    /// Returns the tile the unit is pushed to
    /// </summary>
    /// <param name="currentTile"></param>
    /// <param name="dir"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Tile GetTileToPushTo(Tile currentTile, Vector3 dir, int count)
	{
        if (count >= _abilityData.pushDistance)
            return currentTile;

        Debug.Log("Push To dir = " + dir);

        count++;

        Vector3 position = currentTile.transform.position;

        Physics.Raycast(position, dir, out RaycastHit nextHit);

        if (nextHit.transform)
        {
            Tile tile = nextHit.transform.GetComponent<Tile>();

            if (!tile) return 
                    currentTile;

            if (tile && tile.IsWalkable() && tile.IsFree())
                return GetTileToPushTo(tile, dir, count);

            else if (!tile.IsWalkable() || !tile.IsFree())
            {
                collides = true;
                collidingUnit = tile.GetUnitAbove();
                return currentTile;
            }
        }
        return currentTile;
    }

    private void PaintUseTiles(Tile currentTile, int count, Vector3 dir)
    {
        _tilesInRange.Add(currentTile);

        if (count >= _abilityData.pushUseRange) //|| (_tilesInRangeChecked.ContainsKey(currentTile) && _tilesInRangeChecked[currentTile] <= count))
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
                    TileHighlight.Instance.MortarPaintTilesInAttackRange(tile);

                    PaintUseTiles(tile, count, forwardHit.transform.forward);                    
                }
            }
            
            if (leftHit.transform)
            {
                Tile tile = leftHit.transform.GetComponent<Tile>();
    
                if (tile && tile.IsWalkable())
                {
                    TileHighlight.Instance.MortarPaintTilesInAttackRange(tile);

                    PaintUseTiles(tile, count, leftHit.transform.right * -1);                    
                }
            }
            
            if (rightHit.transform)
            {
                Tile tile = rightHit.transform.GetComponent<Tile>();
    
                if (tile && tile.IsWalkable())
                {
                    TileHighlight.Instance.MortarPaintTilesInAttackRange(tile);

                    PaintUseTiles(tile, count, rightHit.transform.right);                    
                }
            }
            
            if (backHit.transform)
            {
                Tile tile = backHit.transform.GetComponent<Tile>();
    
                if (tile && tile.IsWalkable())
                {
                    TileHighlight.Instance.MortarPaintTilesInAttackRange(tile);

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

            Tile tile = forwardHit.transform.GetComponent<Tile>();

            if (tile && tile.IsWalkable())
            {
                TileHighlight.Instance.MortarPaintTilesInAttackRange(tile);

                PaintUseTiles(tile, count, forwardHit.transform.forward);
            }
        }

        if (dir == Vector3.left)
        {
            Vector3 position = currentTile.transform.position;

            Physics.Raycast(position, currentTile.transform.right * -1, out RaycastHit leftHit);

            if (!leftHit.transform)
                return;

            Tile tile = leftHit.transform.GetComponent<Tile>();

            if (tile && tile.IsWalkable())
            {
                TileHighlight.Instance.MortarPaintTilesInAttackRange(tile);

                PaintUseTiles(tile, count, leftHit.transform.right * -1);
            }
        }
        
        if (dir == Vector3.right)
        {
            Vector3 position = currentTile.transform.position;

            Physics.Raycast(position, currentTile.transform.right * -1, out RaycastHit rightHit);

            if (!rightHit.transform)
                return;

            Tile tile = rightHit.transform.GetComponent<Tile>();

            if (tile && tile.IsWalkable())
            {
                TileHighlight.Instance.MortarPaintTilesInAttackRange(tile);

                PaintUseTiles(tile, count, rightHit.transform.right);
            }
        }
        
        if (dir == Vector3.back)
        {
            Vector3 position = currentTile.transform.position;

            Physics.Raycast(position, currentTile.transform.forward * -1, out RaycastHit backHit);

            if (!backHit.transform)
                return;

            Tile tile = backHit.transform.GetComponent<Tile>();

            if (tile && tile.IsWalkable())
            {
                TileHighlight.Instance.MortarPaintTilesInAttackRange(tile);

                PaintUseTiles(tile, count, backHit.transform.forward * -1);
            }
        }
    }
}
