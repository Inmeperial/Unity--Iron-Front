using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] private GameObject _assaultRifleEffect;
    [SerializeField] private GameObject _rifleEffect;
    [SerializeField] private GameObject _deadMechaEffect;
    [SerializeField] private GameObject _hitMechaEffect;
    [SerializeField] private GameObject _burningMechaEffect;
    [SerializeField] private GameObject _dustEffect;


    [Header("Sounds")]
    [SerializeField] private AudioClip _shootGunSound;
    [SerializeField] private AudioClip _assaultRifleSound;
    [SerializeField] private AudioClip _assaultRifleFinalShootSound;
    [SerializeField] private AudioClip _rifleSound;
    [SerializeField] private AudioClip _mineSound;
    [SerializeField] private AudioClip _mechaExplosionSound;

    [Header("Damage Text")]

    [SerializeField] private GameObject _damageText;

    [SerializeField] private float _textSpacingTime;
    private bool _canCreate;
    private List<Tuple<string, int, Vector3>> _list = new List<Tuple<string, int, Vector3>>();
    private Camera _cam;

    public static EffectsController Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _cam = Camera.main;
    }

    /// <summary>
    /// Spawn and reproduce the particle effect.
    /// </summary>
    /// <param name="pos">Position the particle will spawn.</param>
    /// <param name="Enum ParticleActionType">Damage - Attack - Mine - Shootgun</param>
    public void PlayParticlesEffect(GameObject obj, EnumsClass.ParticleActionType type)
    {
        GameObject effect;
        GameObject effect2;
        ParticleSystem particle;
        switch (type)
        {
            //case EnumsClass.ParticleActionType.Attack: //not called
            //    effect = Instantiate(_attackEffect, obj.transform.position, transform.rotation, transform);
            //    particle = effect.GetComponent<ParticleSystem>();
            //    particle.time = 0f;
            //    particle.Play();
            //    StartCoroutine(DestroyEffect(effect, particle.main.duration));
            //    break;

            case EnumsClass.ParticleActionType.Damage:
                effect = Instantiate(_damageEffect, obj.transform.position, transform.rotation, transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;

            case EnumsClass.ParticleActionType.Mine:
                effect = Instantiate(_mineExplosionEffect, obj.transform.position, transform.rotation, transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                AudioManager.audioManagerInstance.PlaySound(_mineSound, this.gameObject);
                break;

            case EnumsClass.ParticleActionType.ShootGun:
                effect = Instantiate(_shootGunEffect, obj.transform.position, obj.transform.rotation, transform);
                //effect = Instantiate(_attackEffect, obj.transform.position, transform.rotation, transform);
                effect.transform.SetParent(obj.transform);
                particle = effect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                AudioManager.audioManagerInstance.PlaySound(_shootGunSound, this.gameObject);
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;

            case EnumsClass.ParticleActionType.AssaultRifle:
                effect = Instantiate(_assaultRifleEffect, obj.transform.position, obj.transform.rotation, transform);
                //effect = Instantiate(_attackEffect, obj.transform.position, transform.rotation, transform);
                effect.transform.SetParent(obj.transform);
                particle = effect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                AudioManager.audioManagerInstance.PlaySound(_assaultRifleSound, this.gameObject);
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;

            case EnumsClass.ParticleActionType.AssaultRifleFinalShot:
                effect = Instantiate(_assaultRifleEffect, obj.transform.position, obj.transform.rotation, transform);
                //effect = Instantiate(_attackEffect, obj.transform.position, transform.rotation, transform);
                effect.transform.SetParent(obj.transform);
                particle = effect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                AudioManager.audioManagerInstance.StopSoundWithFadeOut(_assaultRifleSound, this.gameObject);
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;

            case EnumsClass.ParticleActionType.Rifle:
                effect = Instantiate(_rifleEffect, obj.transform.position, transform.rotation, transform);
                //effect = Instantiate(_attackEffect, obj.transform.position, transform.rotation, transform);
                effect.transform.SetParent(obj.transform);
                particle = effect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                AudioManager.audioManagerInstance.PlaySound(_rifleSound, this.gameObject);
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;

            case EnumsClass.ParticleActionType.Dead:
                effect = Instantiate(_deadMechaEffect, obj.transform.position, transform.rotation, transform);
                effect.transform.SetParent(obj.transform);
                particle = effect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                ShakeIt();
                AudioManager.audioManagerInstance.PlaySound(_mechaExplosionSound, this.gameObject);
                GameObject spawnerObj = obj.GetComponent<Character>().GetBurningSpawner();
                effect2 = Instantiate(_burningMechaEffect, spawnerObj.transform.position, spawnerObj.transform.rotation, obj.transform);
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;

            case EnumsClass.ParticleActionType.Hit:
                effect = Instantiate(_hitMechaEffect, obj.transform.position, transform.rotation, transform);
                effect.transform.SetParent(obj.transform);
                particle = effect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                AudioManager.audioManagerInstance.PlaySound(_rifleSound, this.gameObject);
                StartCoroutine(DestroyEffect(effect, particle.main.duration));
                break;

            case EnumsClass.ParticleActionType.MortarHit:
                effect = Instantiate(_dustEffect, obj.transform.position, obj.transform.rotation, transform);
                effect.transform.SetParent(obj.transform);
                particle = effect.GetComponent<ParticleSystem>();
                particle.time = 0f;
                particle.Play();
                ShakeIt();
                //AudioManager.audioManagerInstance.PlaySound(_rifleSound, this.gameObject);
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
        Tuple<string, int, Vector3> t = Tuple.Create(text, type, position);
        _list.Add(t);
        if (last) StartCoroutine(CreateDamageTextWithSpacing());
    }

    //Retrasa la creación de los textos para que no salgan pegados
    IEnumerator CreateDamageTextWithSpacing()
    {
        for (int i = 0; i < _list.Count; i++)
        {
            Tuple<string, int, Vector3> myText = _list[i];

            GameObject obj = new GameObject();
            //Creo un objeto vacio que mire a la camara en la posicion donde va a crearse el texto 
            GameObject rot = Instantiate(obj, myText.Item3, Quaternion.identity);
            Destroy(obj);
            rot.transform.LookAt(rot.transform.position + _cam.transform.forward);
            rot.name = "name: " + i;
            //Uso la rotacion del objeto vacio para que el texto se instancie con esa rotacion
            GameObject tObj = Instantiate(_damageText, myText.Item3 + new Vector3(0, 0, 0), rot.transform.rotation);
            DamageText t = tObj.GetComponent<DamageText>();
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
            GetComponent<MeshRenderer>().material.SetInt("_isFullCover", 1);
        }
        else
        {
            GetComponent<MeshRenderer>().material.SetInt("_isFullCover", 0);
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
