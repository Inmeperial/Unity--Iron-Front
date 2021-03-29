using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRay : MonoBehaviour
{
    /// <summary>
    /// Return the Transform of the object collided by the ray coincident with the given mask.
    /// </summary>
    public static Transform GetTargetTransform(LayerMask mask)
    {
        RaycastHit hit;
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out hit, mask))
            return hit.transform;

        return null;
    }
}
