using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopManager : MonoBehaviour
{
    [SerializeField] private Transform[] _mechas;
    [SerializeField] private MechaEquipmentContainerSO _equipmentContainer;
    
    private int _mechaIndex;

    public delegate void ClickAction(int index);
    public static event ClickAction OnClickPrevious;
    public static event ClickAction OnClickNext;
    public static event ClickAction OnClickEdit;
    public static event ClickAction OnClickCloseEdit;
    
    private void Start()
    {
        _mechaIndex = 3;
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
}
