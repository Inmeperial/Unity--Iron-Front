using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public class EffectsController : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] private GameObject _attackEffect;
    
    [SerializeField] private GameObject _damageEffect;

    [Header("Damage Text")]
    
    [SerializeField] private GameObject _damageText;

    [SerializeField] private float _textSpacingTime;
    private bool _canCreate;
    private List<Tuple<string, int, Vector3>> _list = new List<Tuple<string, int, Vector3>>();

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
    /// Creates the Damage Text in world. type: Miss: 0 - Normal: 1 - Critical: 2
    /// </summary>
    public void CreateDamageText(string text, int type, Vector3 position, bool last)
    {
        var t = Tuple.Create(text, type, position);
        _list.Add(t);
        if (last == true) StartCoroutine(CreateDamageTextWithSpacing());
    }

    IEnumerator CreateDamageTextWithSpacing()
    {
        for (int i = 0; i < _list.Count; i++)
        {
            var myText = _list[i];
            var tObj = Instantiate(_damageText, myText.Item3 + new Vector3(0, i/2,0), Quaternion.identity);
            var t = tObj.GetComponent<DamageText>(); 
            t.SetText(myText.Item1, myText.Item2, i);
            StartCoroutine(DestroyEffect(t.gameObject, t.GetDuration()));
            yield return new WaitForSeconds(_textSpacingTime);
        }
        FindObjectOfType<CloseUpCamera>().ResetCamera();
        _list.Clear();
        
        
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
