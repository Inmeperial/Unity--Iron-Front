﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkshopManager : MonoBehaviour
{
    [SerializeField] private string _savePath;
    [SerializeField] private Transform[] _mechas;
    [SerializeField] private MechaEquipmentContainerSO _equipmentContainer;
    
    private int _mechaIndex;
    private bool _isEditing;

    public delegate void ClickAction(int mechaIndex);
    public static event ClickAction OnClickPrevious;
    public static event ClickAction OnClickNext;
    public static event ClickAction OnClickEdit;
    public static event ClickAction OnClickCloseEdit;
    
    public WorkshopMecha[] mechas;

    private void Awake()
    {
        SetEquipment();
    }

    private void Start()
    {
        _isEditing = false;
        _mechaIndex = 3;
    }

    private void Update()
    {
        if (_isEditing) return;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            PreviousButton();

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            NextButton();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("TEST NUEVO PREFAB");
        }
    }

    public void PreviousButton()
    {
        if (_mechaIndex == 0)
            _mechaIndex = _mechas.Length-1;
        else _mechaIndex--;

        OnClickPrevious?.Invoke(_mechaIndex);
    }
    
    public void NextButton()
    {
        if (_mechaIndex >= _mechas.Length - 1)
            _mechaIndex = 0;
        else _mechaIndex++;
        
        OnClickNext?.Invoke(_mechaIndex);
    }

    public void EditButton()
    {
        _isEditing = true;
        OnClickEdit?.Invoke(_mechaIndex);
    }

    public void CloseEditionButton()
    {
        _isEditing = false;
        OnClickCloseEdit?.Invoke(_mechaIndex);
    }

    public MechaEquipmentSO GetSelectedMechaEquipment()
    {
        return _equipmentContainer.GetEquipment(_mechaIndex);
    }

    private void SetEquipment()
    {
        _equipmentContainer = LoadSaveUtility.LoadEquipment(_equipmentContainer);
        for (int i = 0; i < mechas.Length; i++)
        {
            mechas[i].SetEquipment(_equipmentContainer.GetEquipment(i));
            mechas[i].SpawnParts();
        }
    }

    public void UpdateBody(BodySO body)
    {
        mechas[_mechaIndex].ChangeBody(body);
        _equipmentContainer.equipments[_mechaIndex].body = body;
        LoadSaveUtility.SaveEquipment(_equipmentContainer);
    }
    
    public void UpdateLeftGun(GunSO gun)
    {
        mechas[_mechaIndex].ChangeLeftGun(gun);
        _equipmentContainer.equipments[_mechaIndex].leftGun = gun;
        LoadSaveUtility.SaveEquipment(_equipmentContainer);
    }
    
    public void UpdateRightGun(GunSO gun)
    {
        mechas[_mechaIndex].ChangeRightGun(gun);
        _equipmentContainer.equipments[_mechaIndex].rightGun = gun;
        LoadSaveUtility.SaveEquipment(_equipmentContainer);
    }
    
    public void UpdateLegs(LegsSO legs)
    {
        mechas[_mechaIndex].ChangeLegs(legs);
        _equipmentContainer.equipments[_mechaIndex].legs = legs;
        LoadSaveUtility.SaveEquipment(_equipmentContainer);
    }
}
