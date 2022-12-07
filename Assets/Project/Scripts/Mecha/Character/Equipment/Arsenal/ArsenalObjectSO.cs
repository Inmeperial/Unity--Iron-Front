using UnityEngine;

public class ArsenalObjectSO : ScriptableObject
{
    public string objectName;
    [TextArea]
    public string objectDescription;
    public Sprite objectImage;
}
