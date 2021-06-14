﻿using System;
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
    
    [SerializeField] private GameObject _mineExplosionEffect;

    [SerializeField] private GameObject _shootGunEffect;

    [Header("Sounds")]
    [SerializeField] private AudioClip _shootGunSound;

    [Header("Damage Text")]
    
    [SerializeField] private GameObject _damageText;

    [SerializeField] private float _textSpacingTime;
    private bool _canCreate;
    private List<Tuple<string, int, Vector3>> _list = new List<Tuple<string, int, Vector3>>();
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    /// <summary>
    /// Spawn and reproduce the particle effect.
    /// </summary>
    /// <param name="pos">Position the particle will spawn.</param>
    /// <param name="type">Damage - Attack - Mine - Shotgun</param>
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
            
            case "Mine":
                effect = Instantiate(_mineExplosionEffect, pos, transform.rotation, transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;

            case "ShotGun":
                effect = Instantiate(_shootGunEffect, pos, transform.rotation, transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                AudioManager.audioManagerInstance.PlaySound(_shootGunSound, this.gameObject);
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;
        }
        
    }
    
    /// <summary>
    /// Creates the Damage Text in world. type: 
    /// </summary>
    /// <param name="text">Text that will be shown.</param>
    /// <param name="type">Miss: 0 - Normal: 1 - Critical: 2.</param>
    /// <param name="position">The position it will spawn.</param>
    /// <param name="last">Is the last text to spawn?</param>
    public void CreateDamageText(string text, int type, Vector3 position, bool last)
    {
        var t = Tuple.Create(text, type, position);
        _list.Add(t);
        if (last) StartCoroutine(CreateDamageTextWithSpacing());
    }

    //Retrasa la creación de los textos para que no salgan pegados
    IEnumerator CreateDamageTextWithSpacing()
    {
        for (int i = 0; i < _list.Count; i++)
        {
            var myText = _list[i];

            //Creo un objeto vacio que mire a la camara en la posicion donde va a crearse el texto 
            var rot = Instantiate(new GameObject(), myText.Item3, Quaternion.identity);
            rot.transform.LookAt(rot.transform.position + _cam.transform.forward);
            
            //Uso la rotacion del objeto vacio para que el texto se instancie con esa rotacion
            var tObj = Instantiate(_damageText, myText.Item3 + new Vector3(0, 0,0), rot.transform.rotation);
            var t = tObj.GetComponent<DamageText>(); 
            t.SetText(myText.Item1, myText.Item2, i);
            StartCoroutine(DestroyEffect(t.gameObject, t.GetDuration()));
            yield return new WaitForSeconds(_textSpacingTime);
        }
        FindObjectOfType<CloseUpCamera>().ResetCamera();
        _list.Clear();
    }
    
    //Destruye los efectos para que no queden siempre activos
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
