using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverviewDataImage : OverviewData
{
    [SerializeField] private Image _overviewImage;
    public void SetData(Sprite image)
    {
        _overviewImage.sprite = image;
    }
}
