using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkshopManager : MonoBehaviour
{
    [SerializeField] private MechaEquipmentContainerSO _equipmentContainer;
    
    private int _mechaIndex;
    private bool _isEditing;

    public delegate void ClickAction(int mechaIndex);
    public static event ClickAction OnClickPrevious;
    public static event ClickAction OnClickNext;
    public static event ClickAction OnClickEdit;
    public static event ClickAction OnClickCloseEdit;

    public WorkshopMecha[] mechas;

    private SoundsMenu _soundMenu;
    
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

    //TODO: asignar al boton cuando esté.
    public void ApplyChangesButton()
    {
        LoadSaveUtility.SaveEquipment(_equipmentContainer);
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
            equipmentToUse = ScriptableObject.Instantiate(_equipmentContainer);
            for (int i = 0; i < _equipmentContainer.equipments.Count; i++)
            {
                equipmentToUse.equipments[i] = ScriptableObject.Instantiate(_equipmentContainer.equipments[i]);
                _equipmentContainer = equipmentToUse;
            }
        }
        else
        {
            _equipmentContainer = loadedEquipment;
            equipmentToUse = loadedEquipment;
        }

        for (int i = 0; i < mechas.Length; i++)
        {
            mechas[i].SetEquipment(equipmentToUse.GetEquipment(i));
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
        LoadSaveUtility.SaveEquipment(_equipmentContainer);
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
    
    
}
