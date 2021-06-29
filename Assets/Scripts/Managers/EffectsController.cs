using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EffectsController : MonoBehaviour
{
    [Header("Camera Shake")]
    public float shakeMagnetude = 0.05f, shakeTime = 0.5f;
    private Vector3 _cameraInitialPosition;
    
    //private Camera mainCamera;

    [Header("Particles")]
    [SerializeField] private GameObject _attackEffect;
    [SerializeField] private GameObject _damageEffect;
    [SerializeField] private GameObject _mineExplosionEffect;
    [SerializeField] private GameObject _shootGunEffect;
    [SerializeField] private GameObject _assalutRifleEffect;
    [SerializeField] private GameObject _rifleEffect;
    [SerializeField] private GameObject _deadMechaEffect;
    [SerializeField] private GameObject _hitMechaEffect;

    [Header("Sounds")]
    [SerializeField] private AudioClip _shootGunSound;
    [SerializeField] private AudioClip _assalutRifleSound;
    [SerializeField] private AudioClip _rifleSound;
    [SerializeField] private AudioClip _mineSound;

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
    /// <param name="type">Damage - Attack - Mine - Shootgun</param>
    public void PlayParticlesEffect(GameObject obj, string type)
    {
        GameObject effect;
        ParticleSystem particle;
        switch (type)
        {
            case "Damage":
                effect = Instantiate(_damageEffect, obj.transform.position, transform.rotation, transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;
            
            case "Attack":
                effect = Instantiate(_attackEffect, obj.transform.position, transform.rotation, transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;
            
            case "Mine":
                effect = Instantiate(_mineExplosionEffect, obj.transform.position, transform.rotation, transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                AudioManager.audioManagerInstance.PlaySound(_mineSound, this.gameObject);
                break;

            case "ShootGun":
                effect = Instantiate(_shootGunEffect.transform.GetChild(0).gameObject, obj.transform.position, transform.rotation, transform);
                effect.transform.SetParent(obj.transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                AudioManager.audioManagerInstance.PlaySound(_shootGunSound, this.gameObject);
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;

            case "AssaultRifle":
                effect = Instantiate(_assalutRifleEffect.transform.GetChild(0).gameObject, obj.transform.position, transform.rotation, transform);
                effect.transform.SetParent(obj.transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                AudioManager.audioManagerInstance.PlaySound(_assalutRifleSound, this.gameObject);
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;

            case "Rifle":
                effect = Instantiate(_rifleEffect.transform.GetChild(0).gameObject, obj.transform.position, transform.rotation, transform);
                effect.transform.SetParent(obj.transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                AudioManager.audioManagerInstance.PlaySound(_rifleSound, this.gameObject);
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;

            case "Dead":
                effect = Instantiate(_deadMechaEffect.transform.GetChild(0).gameObject, obj.transform.position, transform.rotation, transform);
                effect.transform.SetParent(obj.transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                ShakeIt();
                AudioManager.audioManagerInstance.PlaySound(_rifleSound, this.gameObject);
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;

            case "Hit":
                effect = Instantiate(_hitMechaEffect.transform.GetChild(0).gameObject, obj.transform.position, transform.rotation, transform);
                effect.transform.SetParent(obj.transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                AudioManager.audioManagerInstance.PlaySound(_rifleSound, this.gameObject);
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

            var obj = new GameObject();
            //Creo un objeto vacio que mire a la camara en la posicion donde va a crearse el texto 
            var rot = Instantiate(obj, myText.Item3, Quaternion.identity);
            Destroy(obj);
            rot.transform.LookAt(rot.transform.position + _cam.transform.forward);
            rot.name = "name: " + i;
            //Uso la rotacion del objeto vacio para que el texto se instancie con esa rotacion
            var tObj = Instantiate(_damageText, myText.Item3 + new Vector3(0, 0,0), rot.transform.rotation);
            var t = tObj.GetComponent<DamageText>(); 
            t.SetText(myText.Item1, myText.Item2, i);
            Destroy(rot);
            StartCoroutine(DestroyEffect(tObj, t.GetDuration()));
            yield return new WaitForSeconds(_textSpacingTime);
        }
        FindObjectOfType<CloseUpCamera>().ResetCamera();
        _list.Clear();
    }
    
    //Destruye los efectos para que no queden siempre activos
    IEnumerator DestroyEffect(GameObject effect, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(effect);
    }

    private void SetFullCover(bool isFullCoverOn)
    {
        if (isFullCoverOn)
        {
            this.GetComponent<MeshRenderer>().material.SetInt("_isFullCover", 1);
        }
        else
        {
            this.GetComponent<MeshRenderer>().material.SetInt("_isFullCover", 0);
        }
    }

    private void ShakeIt()
    {
        _cameraInitialPosition = _cam.transform.position;
        InvokeRepeating("StartCameraShaking", 0f, 0.005f);
        Invoke("StopCameraShaking", shakeTime);
    }

    private void StopCameraShaking()
    {
        CancelInvoke("StartCameraShaking");
        _cam.transform.position = _cameraInitialPosition;
    }

    private void StartCameraShaking()
    {
        float cameraShakingOffsetX = Random.value * shakeMagnetude * 2 - shakeMagnetude;
        float cameraShakingOffsetY = Random.value * shakeMagnetude * 2 - shakeMagnetude;
        Vector3 cameraIntermadiatePosition = _cam.transform.position;
        cameraIntermadiatePosition.x += cameraShakingOffsetX;
        cameraIntermadiatePosition.y += cameraShakingOffsetY;
        _cam.transform.position = cameraIntermadiatePosition;
    }

}
