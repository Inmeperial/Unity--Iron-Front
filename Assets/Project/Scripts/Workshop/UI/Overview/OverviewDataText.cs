using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OverviewDataText : OverviewData
{
    [SerializeField] private TextMeshProUGUI _dataText;

    public void SetData(string text)
    {
        _dataText.text = text;
    }
}
