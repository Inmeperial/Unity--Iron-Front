using UnityEngine;

public class Billboard : MonoBehaviour
{
    protected virtual void LateUpdate()
    {
		transform.LookAt(transform.position + Camera.main.transform.forward);   
    }
}
