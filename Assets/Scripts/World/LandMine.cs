using UnityEngine;

public class LandMine : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        Legs legs = other.GetComponent<Legs>();

        if (!legs) return;
        
        legs.GetCharacter().SetHurtAnimation();
        legs.TakeDamage(damage);
        DestroyMine();
    }

    public void DestroyMine()
    {
        EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Mine);
        Destroy(gameObject); 
    }
}
