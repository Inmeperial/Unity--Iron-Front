using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.GetComponent<Character>();
        if (obj)
        {
            obj.TakeDamageLegs(damage);

            Destroy(gameObject);
        }
    }
}
