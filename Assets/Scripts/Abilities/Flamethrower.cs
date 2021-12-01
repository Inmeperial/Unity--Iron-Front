using System;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Ability
{
    
    private TileHighlight _highlight;
    private Vector3 _position;
    private Vector3 _mouseDir;
    private Vector3 _facingDir;
    private Camera _mainCam;
    private HashSet<Tile> _tilesInRange = new HashSet<Tile>();
    private LineRenderer _lineRenderer;

    private FlamethrowerSO _abilityData;

    public override void Initialize(Character character, EquipableSO data, Location location)
    {
	    base.Initialize(character, data, location);
	    
	    _abilityData = data as FlamethrowerSO;
        if (!_lineRenderer) _lineRenderer = gameObject.AddComponent<LineRenderer>();
    }
    
    public override void Select()
	{
        if (_inCooldown || !_character.CanAttack()) return;
        if (!_highlight)
        {
            _highlight = FindObjectOfType<TileHighlight>();
        }
        
        _lineRenderer.positionCount = 11;
        _lineRenderer.material = _abilityData.lineMaterial;
        _mainCam = Camera.main;
        _position = _character.transform.position;
        _character.EquipableSelectionState(true, this);
        _character.DeselectThisUnit();
    }

	public override void Deselect()
	{
        if(_tilesInRange.Count != 0)
            _highlight.MortarClearTilesInAttackRange(_tilesInRange);
        _tilesInRange.Clear();
        _lineRenderer.positionCount = 0;
        _character.EquipableSelectionState(false, null);
        _character.SelectThisUnit();
    }

	public override void Use(Action callback = null)
	{
		var data = _abilityData as FlamethrowerSO;
        //Consigo las direcciones
        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 1000, _abilityData.gridMask))
		{
            _mouseDir = raycastHit.point;
		}
        _facingDir = (_mouseDir - _position);

        var dir = _facingDir.normalized;
        dir.y = 0;
        var angle = _abilityData.angle;
        var range = _abilityData.range;
        //Pongo los vertices del line renderer para mostrar el area donde esta el ataque.
        _lineRenderer.SetPosition(0, _position);//Necesary
		_lineRenderer.SetPosition(1, _position + Quaternion.Euler(0, angle / 2, 0) * dir * range);//Necesary
		_lineRenderer.SetPosition(2, _position + Quaternion.Euler(0, angle / 3, 0) * dir * range);
		_lineRenderer.SetPosition(3, _position + Quaternion.Euler(0, angle / 4, 0) * dir * range);
		_lineRenderer.SetPosition(4, _position + Quaternion.Euler(0, angle / 5, 0) * dir * range);
		_lineRenderer.SetPosition(5, _position + dir * range);
		_lineRenderer.SetPosition(6, _position + Quaternion.Euler(0, -angle / 5, 0) * dir * range);
		_lineRenderer.SetPosition(7, _position + Quaternion.Euler(0, -angle / 4, 0) * dir * range);
		_lineRenderer.SetPosition(8, _position + Quaternion.Euler(0, -angle / 3, 0) * dir * range);
		_lineRenderer.SetPosition(9, _position + Quaternion.Euler(0, -angle / 2, 0) * dir * range);//Necesary
        _lineRenderer.SetPosition(10, _position);//Necesary

        _character.transform.LookAt(_mouseDir);
        var rot = _character.transform.eulerAngles;
        rot.x = 0;
        _character.transform.eulerAngles = rot;

        if (Input.GetMouseButtonDown(0))
		{
            //Ataco a todas las unidades que esten en el area de ataque
            EffectsController.Instance.PlayParticlesEffect(_character.gameObject, EnumsClass.ParticleActionType.FlameThrower);
            List<Character> charactersHitted = new List<Character>();
            var collisions = Physics.OverlapSphere(_position, range, _abilityData.characterMask);
            foreach (var item in collisions)
            {
                if (Vector3.Angle(_facingDir, (item.transform.position - _position)) > angle / 2) continue;
                var tempChar = item.GetComponentInParent<Character>();
                if (!tempChar || charactersHitted.Contains(tempChar)) continue;
                charactersHitted.Add(tempChar);
            }
            DamageCharacters(charactersHitted);
            
            if (callback != null)
                callback();
            
            AbilityUsed(_abilityData);
            UpdateButtonText(_availableUses.ToString(), _abilityData);
            _button.interactable = false;
            Deselect();
        }

        if (Input.GetMouseButtonDown(1))
		{
            if (callback != null)
                callback();
            Deselect();
		}
	}

    void DamageCharacters(List<Character> charactersToDamage)
    {
        Debug.Log("Flamethrower damage");
        foreach (var item in charactersToDamage)
        {
            if (item == _character) continue;
            item.GetBody().TakeDamage(_abilityData.damage);
            item.SetHurtAnimation();
        }
        _character.DeactivateAttack();
    }

    void PaintAndClearTile(Tile tileToChange)
	{
        if (!_tilesInRange.Contains(tileToChange))
        {
            _tilesInRange.Add(tileToChange);
            _highlight.MortarPaintTilesInAttackRange(tileToChange);
        }
		else
		{
            _tilesInRange.Remove(tileToChange);
            tileToChange.MortarEndCanBeAttackedColor();
        }
    }
    
    public override string GetEquipableName()
    {
	    return _abilityData.equipableName;
    }
}
