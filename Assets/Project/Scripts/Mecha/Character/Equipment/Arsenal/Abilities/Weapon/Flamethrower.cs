using System;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : WeaponAbility
{
    private Vector3 _position;
    private Vector3 _mouseDir;
    private Vector3 _facingDir;
    private Camera _mainCam;
    private HashSet<Tile> _tilesInRange = new HashSet<Tile>();
    private LineRenderer _lineRenderer;

    private FlamethrowerSO _abilityData;

    public override void Initialize(Character character, EquipableSO data)
    {
	    base.Initialize(character, data);
	    
	    _abilityData = data as FlamethrowerSO;

        if (!_lineRenderer)
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
    }
    
    public override void Select()
	{
        if (_inCooldown || !_character.CanAttack())
            return;
        
        _lineRenderer.positionCount = 11;

        _lineRenderer.material = _abilityData.lineMaterial;

        _mainCam = Camera.main;

        _position = _character.transform.position;

        _character.EquipableSelectionState(true, this);

        _character.DeselectThisUnit();

        OnEquipableSelected?.Invoke();
    }

	public override void Deselect()
	{
        OnEquipableDeselected?.Invoke();

        if(_tilesInRange.Count != 0)
            TileHighlight.Instance.MortarClearTilesInAttackRange(_tilesInRange);

        _tilesInRange.Clear();

        _lineRenderer.positionCount = 0;

        _character.EquipableSelectionState(false, null);

        _character.SelectThisUnit();
    }

	public override void Use()
	{
        DrawArc();

        if (Input.GetMouseButtonDown(0))
            ExecuteAbility();

        if (Input.GetMouseButtonDown(1))
            Deselect();
	}

    private void ExecuteAbility()
    {
        //Ataco a todas las unidades que esten en el area de ataque

        PlayVFX(_abilityData.particleEffect, _character.GetSelectedGun().GetParticleSpawn().transform.position, _facingDir);
        PlaySound(_abilityData.sound, gameObject);

        List<Character> charactersHitted = new List<Character>();

        Collider[] collisions = Physics.OverlapSphere(_position, _abilityData.range, _abilityData.characterMask);

        foreach (Collider item in collisions)
        {
            if (Vector3.Angle(_facingDir, (item.transform.position - _position)) > _abilityData.angle / 2)
                continue;

            Character tempChar = item.GetComponentInParent<Character>();

            if (!tempChar || charactersHitted.Contains(tempChar)) 
                continue;

            charactersHitted.Add(tempChar);
        }

        DamageCharacters(charactersHitted);

        OnEquipableUsed?.Invoke();

        AbilityUsed(_abilityData);

        UpdateButtonText(_availableUses.ToString(), _abilityData);

        _button.interactable = false;

        Deselect();
    }

    private void DrawArc()
    {
        //Consigo las direcciones
        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000, _abilityData.gridMask))
            _mouseDir = raycastHit.point;

        _facingDir = (_mouseDir - _position);

        Vector3 dir = _facingDir.normalized;

        dir.y = 0;

        float angle = _abilityData.angle;

        float range = _abilityData.range;

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

        Vector3 rot = _character.transform.eulerAngles;

        rot.x = 0;

        _character.transform.eulerAngles = rot;
    }

    private void DamageCharacters(List<Character> charactersToDamage)
    {
        Debug.Log("Flamethrower damage");
        foreach (Character item in charactersToDamage)
        {
            if (item == _character) 
                continue;

            Body body = item.GetBody();
            body.ReceiveDamage(_abilityData.damage);
        }
        _character.DoAttackAction();
    }

    private void PaintAndClearTile(Tile tileToChange)
	{
        if (!_tilesInRange.Contains(tileToChange))
        {
            _tilesInRange.Add(tileToChange);
            TileHighlight.Instance.MortarPaintTilesInAttackRange(tileToChange);
        }
		else
		{
            _tilesInRange.Remove(tileToChange);
            tileToChange.MortarEndCanBeAttackedColor();
        }
    }

    public override string GetEquipableName() => _abilityData.objectName;
}
