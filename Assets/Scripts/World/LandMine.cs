using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.GetComponent<Legs>();
        Character selectedEnemy = other.gameObject.transform.parent.GetComponent<Character>();

        if (!obj) return;
        
        selectedEnemy.SetHurtAnimation();
        obj.TakeDamageLegs(damage);
        DestroyMine();
    }

    public void DestroyMine()
    {
        FindObjectOfType<EffectsController>().PlayParticlesEffect(gameObject, "Mine");
        Destroy(gameObject); 
    }
}
