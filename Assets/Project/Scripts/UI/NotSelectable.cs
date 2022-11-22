using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NotSelectable : Selectable
{
    public Selectable previousSelection;
    public override void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForEndOfFrame();

        if (previousSelection) previousSelection.Select();
    }
}
