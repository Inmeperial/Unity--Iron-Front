using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public abstract class OverviewData : MonoBehaviour
{    
    protected TooltipTrigger _tooltipTrigger;

    // Start is called before the first frame update
    void Start()
    {
        _tooltipTrigger = GetComponent<TooltipTrigger>();
    }

    public void SetTooltipData(ArsenalObjectSO arsenalObjectSO)
    {
        _tooltipTrigger.SetData(arsenalObjectSO.objectName, arsenalObjectSO.objectDescription);
    }
    public void SetTooltipData(string objectName = "", string objectDescription = "")
    {
        _tooltipTrigger.SetData(objectName, objectDescription);
    }
}
