using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.GetComponent<Legs>();
        if (obj)
        {
            obj.TakeDamageFromMine(damage);
            FindObjectOfType<EffectsController>().PlayParticlesEffect(transform.position, "Mine");
            Destroy(gameObject);
        }
    }
}
