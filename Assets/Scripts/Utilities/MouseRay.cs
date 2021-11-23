using UnityEngine;

public class MouseRay : MonoBehaviour
{
    /// <summary>
    /// Return the Transform of the object collided by the ray coincident with the given mask.
    /// </summary>
    public static Transform GetTargetTransform(LayerMask mask)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(mouseRay, out var hit, Mathf.Infinity, mask) ? hit.transform : null;
    }

    /// <summary>
    /// Return the GameObject of the object collided by the ray coincident with the given mask.
    /// </summary>
    public static GameObject GetTargetGameObject(LayerMask mask)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(mouseRay, out var hit, Mathf.Infinity, mask) ? hit.transform.gameObject : null;
    }

    public static bool CheckIfType(LayerMask mask)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(mouseRay, mask);
    }
}
