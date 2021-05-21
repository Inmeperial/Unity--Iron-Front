using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EffectsController : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] private GameObject _attackEffect;
    
    [SerializeField] private GameObject _damageEffect;

    [Header("Damage Text")]
    
    [SerializeField] private GameObject _damageText;

    
    
    /// <summary>
    /// Reproduces the Effect. type: Damage - Attack
    /// </summary>
    public void PlayParticlesEffect(Vector3 pos, string type)
    {
        GameObject effect;
        ParticleSystem particle;
        switch (type)
        {
            case "Damage":
                effect = Instantiate(_damageEffect, pos, transform.rotation, transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                Debug.Log("damage effect");
                break;
            
            case "Attack":
                effect = Instantiate(_attackEffect, pos, transform.rotation, transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;
        }
        
    }
    /// <summary>
    /// Creates the Damage Text in world. type: 0: Miss - 1: Normal - 2: Critical
    /// </summary>
    public void CreateDamageText(string text, int type, Vector3 position)
    {
        var tObj = Instantiate(_damageText, position, Quaternion.identity);
        var t = tObj.GetComponent<DamageText>(); 
        t.SetText(text, type);
        StartCoroutine(DestroyEffect(t.gameObject, t.GetDuration()));
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
