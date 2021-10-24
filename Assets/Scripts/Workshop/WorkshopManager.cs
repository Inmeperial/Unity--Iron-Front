using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

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
        Load();
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
        Save();
    }
    
    public void UpdateLeftGun(GunSO gun)
    {
        mechas[_mechaIndex].ChangeLeftGun(gun);
        _equipmentContainer.equipments[_mechaIndex].leftGun = gun;
        Save();
    }
    
    public void UpdateRightGun(GunSO gun)
    {
        mechas[_mechaIndex].ChangeRightGun(gun);
        _equipmentContainer.equipments[_mechaIndex].rightGun = gun;
        Save();
    }
    
    public void UpdateLegs(LegsSO legs)
    {
        mechas[_mechaIndex].ChangeLegs(legs);
        _equipmentContainer.equipments[_mechaIndex].legs = legs;
        Save();
    }

    void Save()
    {
        Debug.Log("save");
        int amount = _equipmentContainer.equipments.Count;
        
        //Array to save al equipments
        string equipmentSaves = "";

        //Converts al equipments to json/string
        for (int i = 0; i < amount; i++)
        {
            if (i != 0)
            {
                equipmentSaves += JsonUtility.ToJson(_equipmentContainer.equipments[i], true);
                equipmentSaves += '|';
            }
            else
            {
                equipmentSaves = JsonUtility.ToJson(_equipmentContainer.equipments[i], true);
                equipmentSaves += '|';
            }
            
        }
        

        BinaryFormatter formatter = new BinaryFormatter();
        
        FileStream file = File.Create(string.Concat(Application.dataPath, _savePath));
        
        //Serializes the string of equipments
        formatter.Serialize(file, equipmentSaves);
        file.Close();
    }

    void Load()
    {
        if (!File.Exists(string.Concat(Application.dataPath, _savePath))) return;
        
        Debug.Log("load");
        
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(string.Concat(Application.dataPath, _savePath), FileMode.Open);
        
        //Get the string of equipments
        string save = formatter.Deserialize(file).ToString();

        //Separates the string in an array to have each equipment
        char[] separator = {'|'};
        string[] allEquipments = save.Split(separator);

        //Length-1 because the last element of the array is an empty string
        for (int i = 0; i < allEquipments.Length-1; i++)
        {
            var equipmentToOverwrite = _equipmentContainer.equipments[i];
            var savedEquipment = allEquipments[i];
            
            //Overwrites the equipment with the saved one
            JsonUtility.FromJsonOverwrite(savedEquipment, equipmentToOverwrite);
        }
        //JsonUtility.FromJsonOverwrite(formatter.Deserialize(file).ToString(), _equipmentContainer);
        file.Close();
    }
}
