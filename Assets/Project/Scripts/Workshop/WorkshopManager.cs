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

    [Header("Configs")]
    [SerializeField] private SoundData _clickSound;

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

        _workshopUIManager.OnCopyAbilityToAll += CopyAbilityToAll;
        _workshopUIManager.OnCopyItemToAll += CopyItemToAll;
    }

    private void Update()
    {
        if (_isEditing)
        {
            if (Input.GetMouseButtonDown(1))
                _closeButton.onClick?.Invoke();
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Transform mechaTransform = MouseRay.GetTargetTransform(_characterLayer);

                if (mechaTransform)
                {
                    WorkshopMecha mecha = mechaTransform.GetComponent<WorkshopMecha>();

                    if (mecha)
                        MoveToPosition(mecha.GetPositionIndex());
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            PreviousButton();


        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            NextButton();

    }

    public void PreviousButton()
    {
        PlayClickSound();

        if (_mechaIndex == 0)
            _mechaIndex = mechas.Length - 1;
        else
            _mechaIndex--;

        OnClickPrevious?.Invoke(_mechaIndex);
    }
   
    public void NextButton()
    {
        PlayClickSound();

        if (_mechaIndex >= mechas.Length - 1)
            _mechaIndex = 0;
        else
            _mechaIndex++;

        OnClickNext?.Invoke(_mechaIndex);
    }

    public void EditButton()
    {
        PlayClickSound();


        _isEditing = true;
        
        OnClickEdit?.Invoke(_mechaIndex);
    }

    public void CloseEditionButton()
    {
        PlayClickSound();

        _isEditing = false;
        
        OnClickCloseEdit?.Invoke(_mechaIndex);
    }

    void MoveToPosition(int index)
    {
        PlayClickSound();

        if (index == _mechaIndex)
            _editButton.onClick?.Invoke();
        else
        {
            _mechaIndex = index;
            OnClickMecha?.Invoke(index); 
        }
        
    }

    private void PlayClickSound()
    {
        AudioManager.Instance.PlaySound(_clickSound, gameObject);
    }

    public void ApplyChangesButton() => LoadSaveUtility.SaveEquipment(_equipmentContainer);

    public MechaEquipmentSO GetMechaEquipment(int index) => mechas[index].GetEquipment();

    private void SetEquipment()
    {
        _equipmentContainer = LoadSaveUtility.LoadEquipment();

        for (int i = 0; i < mechas.Length; i++)
        {
            mechas[i].SetEquipment(_equipmentContainer.GetEquipment(i), i);
            mechas[i].SpawnParts();
        }
    }

    public void UpdateBody(BodySO body)
    {
        mechas[_mechaIndex].ChangeBody(body);
        
        _equipmentContainer.equipments[_mechaIndex].body = body;
        
        StartCoroutine(PartFlickerDelay("Body"));

        ApplyChangesButton();
    }
    
    public void UpdateLeftGun(GunSO gun)
    {
        mechas[_mechaIndex].ChangeLeftGun(gun);

        _equipmentContainer.equipments[_mechaIndex].leftGun = gun;
        
        StartCoroutine(PartFlickerDelay("LeftArm"));

        ApplyChangesButton();
    }
    
    public void UpdateRightGun(GunSO gun)
    {
        mechas[_mechaIndex].ChangeRightGun(gun);

        _equipmentContainer.equipments[_mechaIndex].rightGun = gun;
        
        StartCoroutine(PartFlickerDelay("RightArm"));

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
                _equipmentContainer.equipments[_mechaIndex].bodyAbility = equippable as AbilitySO;
                break;

            case "LeftArm":
                _equipmentContainer.equipments[_mechaIndex].leftGunAbility = equippable as AbilitySO;
                break;

            case "RightArm":
                _equipmentContainer.equipments[_mechaIndex].rightGunAbility = equippable as AbilitySO;
                break;

            case "Legs":
                _equipmentContainer.equipments[_mechaIndex].legsAbility = equippable as AbilitySO;
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

    private void CopyItemToAll()
    {
        ItemSO item = mechas[_mechaIndex].GetEquipment().item;

        for (int i = 0; i < mechas.Length; i++)
        {
            _equipmentContainer.equipments[i].item = item;
        }

        ApplyChangesButton();
    }

    private void CopyAbilityToAll()
    {
        MechaEquipmentSO currentMechaEquipment = mechas[_mechaIndex].GetEquipment();
        for (int i = 0; i < mechas.Length; i++)
        {
            _equipmentContainer.equipments[i].bodyAbility = currentMechaEquipment.bodyAbility;

            _equipmentContainer.equipments[i].leftGunAbility = currentMechaEquipment.leftGunAbility;

            _equipmentContainer.equipments[i].rightGunAbility = currentMechaEquipment.rightGunAbility;

            _equipmentContainer.equipments[i].legsAbility = currentMechaEquipment.legsAbility;
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

    public int GetMechaIndex() => _mechaIndex;

    public void StartPartFlicker(string part)
    {
        EndPartFlicker();

        MasterShaderScript masterShader = null;

        WorkshopMecha mecha = mechas[_mechaIndex];

        switch (part)
        {
            case "Body":
                masterShader = mecha.GetBodyShader();
                break;
            
            case "LeftArm":
                masterShader = mecha.GetLeftGunShader();
                break;
                
            case "RightArm":
                masterShader = mecha.GetRightGunShader();
                break;
            
            case "Legs":
                masterShader = mecha.GetLegsShader();
                break;
        }

        masterShader.ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureOutLine);
    }

    public void EndPartFlicker()
    {
        MasterShaderScript bodyShader = mechas[_mechaIndex].GetBodyShader();
        bodyShader.ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);

        MasterShaderScript legsShader = mechas[_mechaIndex].GetLegsShader();
        legsShader.ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);

        MasterShaderScript leftGunShader = mechas[_mechaIndex].GetLeftGunShader();
        leftGunShader.ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);

        MasterShaderScript rightGunShader = mechas[_mechaIndex].GetRightGunShader();
        rightGunShader.ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);
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

        _workshopUIManager.OnCopyAbilityToAll -= CopyAbilityToAll;
        _workshopUIManager.OnCopyItemToAll -= CopyItemToAll;
    }
}
