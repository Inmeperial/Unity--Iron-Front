using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public abstract class OverviewData : MonoBehaviour
{    
    protected TooltipTrigger _tooltipTrigger;

    public void SetTooltipData(ArsenalObjectSO arsenalObjectSO)
    {
        if (!_tooltipTrigger)
            GetTooltipTrigger();

        _tooltipTrigger.SetData(arsenalObjectSO.objectName, arsenalObjectSO.objectDescription);
    }
    public void SetTooltipData(string objectName = "", string objectDescription = "")
    {
        if (!_tooltipTrigger)
            GetTooltipTrigger();

        _tooltipTrigger.SetData(objectName, objectDescription);
    }

    private void GetTooltipTrigger()
    {
        _tooltipTrigger = GetComponent<TooltipTrigger>();
    }
}
