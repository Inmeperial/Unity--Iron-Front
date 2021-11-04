﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorkshopManager : MonoBehaviour
{
    [SerializeField] private MechaEquipmentContainerSO _equipmentContainer;

    [SerializeField] private LayerMask _characterLayer;
    private int _mechaIndex;
    private bool _isEditing;

    public delegate void ClickAction(int mechaIndex);
    public static event ClickAction OnClickPrevious;
    public static event ClickAction OnClickNext;
    public static event ClickAction OnClickEdit;
    public static event ClickAction OnClickCloseEdit;
    public static event ClickAction OnClickMecha;

    public delegate void Save();
    public static event Save OnChangesMade;
    public static event Save OnSave;

    public WorkshopMecha[] mechas;

    private SoundsMenu _soundMenu;

    [SerializeField] private Button _editButton;

    [SerializeField] private Button _closeButton;

    [Space]
    [SerializeField] private WorkshopObjectButton _workshopObjectPrefab;
    private List<WorkshopObjectButton> _createdObjectButtonList = new List<WorkshopObjectButton>();

    private void Awake()
    {
        SetEquipment();
    }

    private void Start()
    {
        _isEditing = false;
        _mechaIndex = 3;
        _soundMenu = GetComponent<SoundsMenu>();
        WorkshopUIManager.OnChangeEquippable += UpdateEquippable;
        WorkshopUIManager.OnBodyColorChange += UpdateBodyColor;
        WorkshopUIManager.OnLegsColorChange += UpdateLegsColor;
        WorkshopUIManager.OnNameChange += UpdateName;
    }

    private void Update()
    {
        if (_isEditing)
        {
            if (Input.GetMouseButtonDown(1))
            {
                _closeButton.onClick?.Invoke();
            }
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            var obj= MouseRay.GetTargetTransform(_characterLayer);
            
            if (obj)
            {
                var mecha = obj.GetComponent<WorkshopMecha>();

                if (mecha)
                {
                    MoveToPosition(mecha.GetIndex());
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            PreviousButton();

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            NextButton();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Level 1 NUEVO");
        }
    }

    public void PreviousButton()
    {
        AudioManager.audioManagerInstance.PlaySound(_soundMenu.GetClickSound(), _soundMenu.GetObjectToAddAudioSource());
        if (_mechaIndex == 0)
            _mechaIndex = mechas.Length-1;
        else _mechaIndex--;

        OnClickPrevious?.Invoke(_mechaIndex);
    }
    
    public void NextButton()
    {
        AudioManager.audioManagerInstance.PlaySound(_soundMenu.GetClickSound(), _soundMenu.GetObjectToAddAudioSource());
        if (_mechaIndex >= mechas.Length - 1)
            _mechaIndex = 0;
        else _mechaIndex++;
        
        OnClickNext?.Invoke(_mechaIndex);
    }

    public void EditButton()
    {
        AudioManager.audioManagerInstance.PlaySound(_soundMenu.GetClickSound(), _soundMenu.GetObjectToAddAudioSource());
        _isEditing = true;
        OnClickEdit?.Invoke(_mechaIndex);
    }

    public void CloseEditionButton()
    {
        AudioManager.audioManagerInstance.PlaySound(_soundMenu.GetClickSound(), _soundMenu.GetObjectToAddAudioSource());
        _isEditing = false;
        OnClickCloseEdit?.Invoke(_mechaIndex);
    }

    void MoveToPosition(int index)
    {
        AudioManager.audioManagerInstance.PlaySound(_soundMenu.GetClickSound(), _soundMenu.GetObjectToAddAudioSource());

        if (index == _mechaIndex)
        {
            _editButton.onClick?.Invoke();
        }
        else
        {
            _mechaIndex = index;
            OnClickMecha?.Invoke(index); 
        }
        
    }

    //TODO: asignar al boton cuando esté.
    public void ApplyChangesButton()
    {
        LoadSaveUtility.SaveEquipment(_equipmentContainer);
        OnSave?.Invoke();
    }

    public MechaEquipmentSO GetMechaEquipment(int index)
    {
        return mechas[index].GetEquipment();
    }

    private void SetEquipment()
    {
        var loadedEquipment = LoadSaveUtility.LoadEquipment();
        MechaEquipmentContainerSO equipmentToUse = null;

        if (loadedEquipment == null)
        {
            equipmentToUse = _equipmentContainer;
        }
        else
        {
            _equipmentContainer = loadedEquipment;
            equipmentToUse = loadedEquipment;
        }

        for (int i = 0; i < mechas.Length; i++)
        {
            mechas[i].SetEquipment(equipmentToUse.GetEquipment(i), i);
            mechas[i].SpawnParts();
        }
    }

    public void UpdateBody(BodySO body)
    {
        mechas[_mechaIndex].ChangeBody(body);
        
        _equipmentContainer.equipments[_mechaIndex].body = body;
        OnChangesMade?.Invoke();
    }
    
    public void UpdateLeftGun(GunSO gun)
    {
        mechas[_mechaIndex].ChangeLeftGun(gun);
        _equipmentContainer.equipments[_mechaIndex].leftGun = gun;
        OnChangesMade?.Invoke();
    }
    
    public void UpdateRightGun(GunSO gun)
    {
        mechas[_mechaIndex].ChangeRightGun(gun);
        _equipmentContainer.equipments[_mechaIndex].rightGun = gun;
        OnChangesMade?.Invoke();
    }
    
    public void UpdateLegs(LegsSO legs)
    {
        mechas[_mechaIndex].ChangeLegs(legs);
        _equipmentContainer.equipments[_mechaIndex].legs = legs;
        OnChangesMade?.Invoke();
    }

    public void UpdateEquippable(EquipableSO equippable, string location)
    {
        switch (location) 
        {
             case "Body":
                 _equipmentContainer.equipments[_mechaIndex].body.ability = equippable as AbilitySO;
                break;
             
             case "LeftArm":
                 _equipmentContainer.equipments[_mechaIndex].leftGun.ability = equippable as AbilitySO;
                break;
                
             case "RightArm":
                 _equipmentContainer.equipments[_mechaIndex].rightGun.ability = equippable as AbilitySO;
                break;
             
             case "Legs":
                 _equipmentContainer.equipments[_mechaIndex].legs.ability = equippable as AbilitySO;
                break;
             
             case "Item":
                 _equipmentContainer.equipments[_mechaIndex].body.item = equippable as ItemSO;
                 break;
        }
        OnChangesMade?.Invoke();
    }

    public void UpdateBodyColor(Color color)
    {
        var c = _equipmentContainer.equipments[_mechaIndex].bodyColor;
        c.red = color.r;
        c.green = color.g;
        c.blue = color.b;
        _equipmentContainer.equipments[_mechaIndex].bodyColor = c;
        OnChangesMade?.Invoke();
    }
    
    public void UpdateLegsColor(Color color)
    {
        var c =_equipmentContainer.equipments[_mechaIndex].legsColor;
        c.red = color.r;
        c.green = color.g;
        c.blue = color.b;
        _equipmentContainer.equipments[_mechaIndex].legsColor = c;
        OnChangesMade?.Invoke();
    }
    
    private void UpdateName(string n)
    {
        _equipmentContainer.equipments[_mechaIndex].name = n;
        OnChangesMade?.Invoke();
    }
    
    public WorkshopObjectButton CreateWorkshopObject(PartSO part, Transform parent)
    {
        var obj = Instantiate(_workshopObjectPrefab, parent);
        obj.transform.localPosition = Vector3.zero;
        obj.SetObjectName(part.partName);
        obj.SetObjectSprite(part.icon);
      
        _createdObjectButtonList.Add(obj);
        return obj;
    }
   
    public WorkshopObjectButton CreateWorkshopObject(GunSO gun, Transform parent)
    {
        var obj = Instantiate(_workshopObjectPrefab, parent);
        obj.transform.localPosition = Vector3.zero;
        obj.SetObjectName(gun.gunName);
        obj.SetObjectSprite(gun.gunImage);
      
        _createdObjectButtonList.Add(obj);
        return obj;
    }
   
    public WorkshopObjectButton CreateWorkshopObject(EquipableSO equipable, Transform parent)
    {
        var obj = Instantiate(_workshopObjectPrefab, parent);
        obj.transform.localPosition = Vector3.zero;
        obj.SetObjectName(equipable.equipableName);
        obj.SetObjectSprite(equipable.equipableIcon);
        _createdObjectButtonList.Add(obj);
        return obj;
    }

    public void DestroyWorkshopObjects()
    {
        foreach (var obj in _createdObjectButtonList)
        {
            Destroy(obj.gameObject);
        }

        _createdObjectButtonList = new List<WorkshopObjectButton>();
    }

    public int GetIndex()
    {
        return _mechaIndex;
    }
}
