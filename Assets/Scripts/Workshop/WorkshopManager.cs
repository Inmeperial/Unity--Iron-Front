using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorkshopManager : MonoBehaviour
{
    [Header("References")]
    public WorkshopMecha[] mechas;
    [SerializeField] private WorkshopUIManager _workshopUIManager;
    [SerializeField] private WorkshopObjectButtonCreator _workshopObjectButtonCreator;
    [SerializeField] private MechaEquipmentContainerSO _equipmentContainer;

    [SerializeField] private LayerMask _characterLayer;

    [SerializeField] private Button _editButton;

    [SerializeField] private Button _closeButton;

    public Action<int> OnClickPrevious;
    public Action<int> OnClickNext;
    public Action<int> OnClickEdit;
    public Action<int> OnClickCloseEdit;
    public Action<int> OnClickMecha;
    public Action<int> OnClickArrows;//Nico

    private SoundsMenu _soundMenu;
    
    private List<WorkshopObjectButton> _createdObjectButtonList = new List<WorkshopObjectButton>();

    private int _mechaIndex;
    private bool _isEditing;

    [Header("Test")]
    public bool useForTest;

    private void Awake()
    {
        SetEquipment();
    }

    private void Start()
    {
        _isEditing = false;
        _mechaIndex = 3;
        _soundMenu = GetComponent<SoundsMenu>();

        _workshopUIManager.OnChangeEquippable += UpdateEquippable;
        _workshopUIManager.OnBodyColorChange += UpdateBodyColor;
        _workshopUIManager.OnLegsColorChange += UpdateLegsColor;
        _workshopUIManager.OnCopyColorToAllBodies += CopyColorToAllBodies;
        _workshopUIManager.OnCopyColorToAllLegs += CopyColorToAllLegs;
        _workshopUIManager.OnNameChange += UpdateName;
    }


    private void Update()
    {
        if (_isEditing)
        {
            if (Input.GetMouseButtonDown(1)) _closeButton.onClick?.Invoke();
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Transform obj = MouseRay.GetTargetTransform(_characterLayer);

                if (obj)
                {
                    WorkshopMecha mecha = obj.GetComponent<WorkshopMecha>();

                    if (mecha) MoveToPosition(mecha.GetPositionIndex());
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) PreviousButton();


        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) NextButton();

    }

    public void PreviousButton()
    {
        AudioManager.audioManagerInstance.PlaySound(_soundMenu.GetClickSound(), _soundMenu.GetObjectToAddAudioSource());
        
        if (_mechaIndex == 0) _mechaIndex = mechas.Length - 1;
        else _mechaIndex--;

        OnClickPrevious?.Invoke(_mechaIndex);
    }
   
    public void NextButton()
    {
        AudioManager.audioManagerInstance.PlaySound(_soundMenu.GetClickSound(), _soundMenu.GetObjectToAddAudioSource());
        if (_mechaIndex >= mechas.Length - 1) _mechaIndex = 0;
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

        if (index == _mechaIndex) _editButton.onClick?.Invoke();
        else
        {
            _mechaIndex = index;
            OnClickMecha?.Invoke(index); 
        }
        
    }
    
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
        MechaEquipmentContainerSO loadedEquipment = LoadSaveUtility.LoadEquipment();

        if (useForTest) loadedEquipment = null;

        MechaEquipmentContainerSO equipmentToUse = null;

        if (loadedEquipment == null) equipmentToUse = _equipmentContainer;
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
        
        StartPartFlicker("Body");
        StartCoroutine(PartFlickerDelay("Body"));
        ApplyChangesButton();
    }
    
    public void UpdateLeftGun(GunSO gun)
    {
        mechas[_mechaIndex].ChangeLeftGun(gun);
        _equipmentContainer.equipments[_mechaIndex].leftGun = gun;
        
        StartPartFlicker("LGun");
        StartCoroutine(PartFlickerDelay("LGun"));
        ApplyChangesButton();
    }
    
    public void UpdateRightGun(GunSO gun)
    {
        mechas[_mechaIndex].ChangeRightGun(gun);
        _equipmentContainer.equipments[_mechaIndex].rightGun = gun;
        
        StartPartFlicker("RGun");
        StartCoroutine(PartFlickerDelay("RGun"));
        ApplyChangesButton();
    }
    
    public void UpdateLegs(LegsSO legs)
    {
        mechas[_mechaIndex].ChangeLegs(legs);
        _equipmentContainer.equipments[_mechaIndex].legs = legs;
        
        StartCoroutine(PartFlickerDelay("Legs"));
        ApplyChangesButton();
    }

    IEnumerator PartFlickerDelay(string part)
    {
        yield return new WaitForEndOfFrame();
        StartPartFlicker(part);
        
    }

    public void UpdateEquippable(EquipableSO equippable, string location)
    {
        switch (location)
        {
            case "Body":
                _equipmentContainer.equipments[_mechaIndex].bodyAbility = equippable as BodyAbilitySO;
                break;

            case "LeftArm":
                _equipmentContainer.equipments[_mechaIndex].leftGunAbility = equippable as GunAbilitySO;
                break;

            case "RightArm":
                _equipmentContainer.equipments[_mechaIndex].rightGunAbility = equippable as GunAbilitySO;
                break;

            case "Legs":
                _equipmentContainer.equipments[_mechaIndex].legsAbility = equippable as LegsAbilitySO;
                break;

            case "Item":
                _equipmentContainer.equipments[_mechaIndex].item = equippable as ItemSO;
                break;
        }
        ApplyChangesButton();
    }

    public void UpdateBodyColor(Color color)
    {
        MechaEquipmentSO.ColorData colorData = _equipmentContainer.equipments[_mechaIndex].bodyColor;
        colorData.red = color.r;
        colorData.green = color.g;
        colorData.blue = color.b;
        _equipmentContainer.equipments[_mechaIndex].bodyColor = colorData;

        Color newColor = new Color(colorData.red, colorData.green, colorData.blue);
        mechas[_mechaIndex].UpdateBodyColor(newColor);
        
        ApplyChangesButton();
    }
    
    public void UpdateLegsColor(Color color)
    {
        MechaEquipmentSO.ColorData colorData =_equipmentContainer.equipments[_mechaIndex].legsColor;
        colorData.red = color.r;
        colorData.green = color.g;
        colorData.blue = color.b;
        _equipmentContainer.equipments[_mechaIndex].legsColor = colorData;

        Color newColor = new Color(colorData.red, colorData.green, colorData.blue);
        mechas[_mechaIndex].UpdateLegsColor(newColor);
        
        ApplyChangesButton();
    }

    public void CopyColorToAllBodies(Color color)
    {
        for (int i = 0; i < mechas.Length; i++)
        {
            MechaEquipmentSO.ColorData colorData =_equipmentContainer.equipments[i].bodyColor;
            colorData.red = color.r;
            colorData.green = color.g;
            colorData.blue = color.b;
            _equipmentContainer.equipments[i].bodyColor = colorData;

            Color newColor = new Color(colorData.red, colorData.green, colorData.blue);
            mechas[i].UpdateBodyColor(newColor);
        }
        
        ApplyChangesButton();
    }
    
    
    private void CopyColorToAllLegs(Color color)
    {
        for (int i = 0; i < mechas.Length; i++)
        {
            MechaEquipmentSO.ColorData colorData =_equipmentContainer.equipments[i].legsColor;
            colorData.red = color.r;
            colorData.green = color.g;
            colorData.blue = color.b;
            _equipmentContainer.equipments[i].legsColor = colorData;

            Color newColor = new Color(colorData.red, colorData.green, colorData.blue);
            mechas[i].UpdateLegsColor(newColor);
        }
        ApplyChangesButton();
    }
    
    private void UpdateName(string name)
    {
        _equipmentContainer.equipments[_mechaIndex].mechaName = name;
        ApplyChangesButton();
    }

    public void DestroyWorkshopObjects()
    {
        foreach (WorkshopObjectButton button in _createdObjectButtonList)
        {
            Destroy(button.gameObject);
        }

        _createdObjectButtonList = new List<WorkshopObjectButton>();
    }

    public int GetMechaIndex()
    {
        return _mechaIndex;
    }

    public void StartPartFlicker(string part)
    {
        EndPartFlicker();
        switch (part)
        {
            case "Body":
                mechas[_mechaIndex].GetBodyShader().ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureOutLine);
                //mechas[_mechaIndex]._leftArmShader.ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureOutLine);
                //mechas[_mechaIndex]._rightArmShader.ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureOutLine);
                break;
            
            case "LGun":
                //MasterShaderScript[] _leftWeaponArr = new MasterShaderScript[mechas[_mechaIndex].GetLeftWeaponShaderArray().Length];
                //_leftWeaponArr = mechas[_mechaIndex].GetLeftWeaponShaderArray();
                List<MasterShaderScript> leftWeaponArr = mechas[_mechaIndex].GetLeftWeaponShaderList();
                for (int i = 0; i < leftWeaponArr.Count; i++)
                {
                    if (leftWeaponArr[i] != null) leftWeaponArr[i].ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureOutLine);
                }
                break;
                
            case "RGun":
                //MasterShaderScript[] _rightWeaponArr = new MasterShaderScript[mechas[_mechaIndex].GetRightWeaponShaderArray().Length];
                //_rightWeaponArr = mechas[_mechaIndex].GetRightWeaponShaderArray();
                List<MasterShaderScript> rightWeaponArr = mechas[_mechaIndex].GetRightWeaponShaderList();
                for (int i = 0; i < rightWeaponArr.Count; i++)
                {
                    if (rightWeaponArr[i] != null) rightWeaponArr[i].ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureOutLine);
                }
                break;
            
            case "Legs":
                //mechas[_mechaIndex]._leftLegShader.ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureOutLine);
                //mechas[_mechaIndex]._rightLegShader.ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureOutLine);
                mechas[_mechaIndex].GetLegsShader().ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureOutLine);
                break;
        }
    }

    public void EndPartFlicker()
    {
        mechas[_mechaIndex].GetBodyShader().ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);
        mechas[_mechaIndex].GetLegsShader().ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);
        //mechas[_mechaIndex]._leftArmShader.ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);
        //mechas[_mechaIndex]._rightArmShader.ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);
        //mechas[_mechaIndex]._leftLegShader.ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);
        //mechas[_mechaIndex]._rightLegShader.ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);

        //MasterShaderScript[] _leftWeaponArr = new MasterShaderScript[mechas[_mechaIndex].GetLeftWeaponShaderArray().Length];
        //_leftWeaponArr = mechas[_mechaIndex].GetLeftWeaponShaderArray();
        List<MasterShaderScript> _leftWeaponArr = mechas[_mechaIndex].GetLeftWeaponShaderList();
        for (int i = 0; i < _leftWeaponArr.Count; i++)
        {
            if (_leftWeaponArr[i] != null) _leftWeaponArr[i].ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);
        }

        //MasterShaderScript[] _rightWeaponArr = new MasterShaderScript[mechas[_mechaIndex].GetRightWeaponShaderArray().Length];
        //_rightWeaponArr = mechas[_mechaIndex].GetRightWeaponShaderArray();
        List<MasterShaderScript> _rightWeaponArr = mechas[_mechaIndex].GetRightWeaponShaderList();
        for (int i = 0; i < _rightWeaponArr.Count; i++)
        {
            if (_rightWeaponArr[i] != null) _rightWeaponArr[i].ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);
        }
    }

    //Nico
    public void DeselectButton(Button button)
	{
        button.interactable = false;
        button.interactable = true;
    }

    //TODO: PONER BOTONES EN UN POOL
    public WorkshopObjectButton CreateWorkshopObjectButton(PartSO partSO, Transform parent)
    {
        WorkshopObjectButton button = _workshopObjectButtonCreator.CreateWorkshopObjectButton(partSO, parent);
        _createdObjectButtonList.Add(button);
        return button;
    }

    public WorkshopObjectButton CreateWorkshopObjectButton(GunSO gunSO, Transform parent)
    {
        WorkshopObjectButton button = _workshopObjectButtonCreator.CreateWorkshopObjectButton(gunSO, parent);
        _createdObjectButtonList.Add(button);
        return button;
    }

    public WorkshopObjectButton CreateWorkshopObjectButton(EquipableSO equipableSO, Transform parent)
    {
        WorkshopObjectButton button = _workshopObjectButtonCreator.CreateWorkshopObjectButton(equipableSO, parent);
        _createdObjectButtonList.Add(button);
        return button;
    }

    private void OnDestroy()
    {
        _workshopUIManager.OnChangeEquippable -= UpdateEquippable;
        _workshopUIManager.OnBodyColorChange -= UpdateBodyColor;
        _workshopUIManager.OnLegsColorChange -= UpdateLegsColor;
        _workshopUIManager.OnCopyColorToAllBodies -= CopyColorToAllBodies;
        _workshopUIManager.OnCopyColorToAllLegs -= CopyColorToAllLegs;
        _workshopUIManager.OnNameChange -= UpdateName;
    }
}
