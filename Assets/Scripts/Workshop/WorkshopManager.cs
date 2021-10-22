using UnityEngine;

public class WorkshopManager : MonoBehaviour
{
    [SerializeField] private Transform[] _mechas;
    [SerializeField] private MechaEquipmentContainerSO _equipmentContainer;
    
    private int _mechaIndex;

    public delegate void ClickAction(int mechaIndex);
    public static event ClickAction OnClickPrevious;
    public static event ClickAction OnClickNext;
    public static event ClickAction OnClickEdit;
    public static event ClickAction OnClickCloseEdit;
    
    public WorkshopMecha[] mechas;
    private void Start()
    {
        _mechaIndex = 3;
        SetEquipment();
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
        OnClickEdit?.Invoke(_mechaIndex);
    }

    public void CloseEditionButton()
    {
        OnClickCloseEdit?.Invoke(_mechaIndex);
    }

    public MechaEquipmentSO GetSelectedMechaEquipment()
    {
        return _equipmentContainer.GetEquipment(_mechaIndex);
    }

    private void SetEquipment()
    {
        for (int i = 0; i < mechas.Length; i++)
        {
            mechas[i].SetEquipment(_equipmentContainer.GetEquipment(i));
            mechas[i].SpawnParts();
        }
    }

    public void UpdateBody(BodySO body)
    {
        Debug.Log("update body");
        mechas[_mechaIndex].ChangeBody(body);
    }
    
    public void UpdateLeftArm(ArmSO arm)
    {
        Debug.Log("update l arm");
        mechas[_mechaIndex].ChangeLeftArm(arm);
    }
    
    public void UpdateRightArm(ArmSO arm)
    {
        Debug.Log("update r arm");
        mechas[_mechaIndex].ChangeRightArm(arm);
    }
    
    public void UpdateLegs(LegsSO legs)
    {
        Debug.Log("update legs");
        mechas[_mechaIndex].ChangeLegs(legs);
    }
}
