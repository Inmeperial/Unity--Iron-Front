using UnityEngine;

public class WorkshopManager : MonoBehaviour
{
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
    private void Start()
    {
        _isEditing = false;
        _mechaIndex = 3;
        SetEquipment();
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
        for (int i = 0; i < mechas.Length; i++)
        {
            mechas[i].SetEquipment(_equipmentContainer.GetEquipment(i));
            mechas[i].SpawnParts();
        }
    }

    public void UpdateBody(BodySO body)
    {
        mechas[_mechaIndex].ChangeBody(body);
    }
    
    public void UpdateLeftArm(ArmSO arm)
    {
        mechas[_mechaIndex].ChangeLeftArm(arm);
    }
    
    public void UpdateRightArm(ArmSO arm)
    {
        mechas[_mechaIndex].ChangeRightArm(arm);
    }
    
    public void UpdateLegs(LegsSO legs)
    {
        mechas[_mechaIndex].ChangeLegs(legs);
    }
}
