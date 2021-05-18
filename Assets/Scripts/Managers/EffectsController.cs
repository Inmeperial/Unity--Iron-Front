using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{

    public GameObject attackEffect;

    public GameObject damageEffect;

    /// <summary>
    /// Reproduces the Effect. type: Damage - Attack
    /// </summary>
    public void PlayEffect(Vector3 pos, string type)
    {
        GameObject effect;
        ParticleSystem particle;
        switch (type)
        {
            case "Damage":
                effect = Instantiate(damageEffect, pos, transform.rotation, transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                Debug.Log("damage effect");
                break;
            
            case "Attack":
                effect = Instantiate(attackEffect, pos, transform.rotation, transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;
        }
        
    }

    IEnumerator DestroyEffect(GameObject effect, float time)
    {
        if (effect)
        {
            yield return new WaitForSeconds(time);
            Destroy(effect);
        }
        else yield return null;

    }
}
