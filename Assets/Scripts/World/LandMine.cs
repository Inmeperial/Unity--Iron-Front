using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.GetComponent<Legs>();
        Character objCharacter = other.gameObject.transform.parent.GetComponent<Character>();

        if (!obj) return;
        
        objCharacter.SetHurtAnimation();
        obj.TakeDamageLegs(damage);
        FindObjectOfType<EffectsController>().PlayParticlesEffect(transform.position, "Mine");
        Destroy(gameObject);
    }
}
