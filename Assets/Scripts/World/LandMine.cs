using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        Legs legs = other.GetComponent<Legs>();
        Character selectedEnemy = other.gameObject.transform.parent.GetComponent<Character>();

        if (!legs) return;
        
        selectedEnemy.SetHurtAnimation();
        legs.TakeDamageLegs(damage);
        DestroyMine();
    }

    public void DestroyMine()
    {
        EffectsController.Instance.PlayParticlesEffect(gameObject, "Mine");
        Destroy(gameObject); 
    }
}
