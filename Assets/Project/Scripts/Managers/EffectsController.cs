using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EffectsController : MonoBehaviour
{
    [Header("Camera Shake")]
    [SerializeField] private float _shakeMagnetude = 0.05f, _shakeTime = 0.5f;

    private Vector3 _cameraInitialPosition;

    [Header("Damage Text")]

    [SerializeField] private GameObject _damageText;

    [SerializeField] private float _textSpacingTime;
    private bool _canCreate;
    private List<Tuple<string, int, Vector3>> _list = new List<Tuple<string, int, Vector3>>();
    private Camera _cam;

    public static EffectsController Instance;

    private List<ParticleSystem> _particlesToDestroy = new List<ParticleSystem>();
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        _cam = Camera.main;

        StartCoroutine(DestroyParticles());
    }

    public void PlayParticlesEffect(ParticleSystem particlePrefab, Vector3 position, Vector3 forward)
    {
        PlayParticlesEffect(particlePrefab, position, forward, out ParticleSystem particle);

        if (!particle)
            return;

        _particlesToDestroy.Add(particle);
    }

    public void PlayParticlesEffect(ParticleSystem particlePrefab, Vector3 position, Vector3 forward, out ParticleSystem particle)
    {
        if (particlePrefab == null)
        {
            Debug.LogError("No particle prefab given!");
            particle = null;
            return;
        }

        particle = SpawnParticle(particlePrefab, position, forward, transform);
        _particlesToDestroy.Add(particle);
    }

    public void PlayPersistentParticles(ParticleSystem particlePrefab, Vector3 position, Vector3 forward, Transform parent, out ParticleSystem particle)
    {
        particle = SpawnParticle(particlePrefab, position, forward, parent);
    }

    private ParticleSystem SpawnParticle(ParticleSystem particlePrefab, Vector3 position, Vector3 forward, Transform parent)
    {
        ParticleSystem particle = Instantiate(particlePrefab, position, Quaternion.identity, parent);

        particle.transform.forward = forward;
        particle.time = 0f;
        particle.Play();

        return particle;
    }
    private IEnumerator DestroyParticles()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        WaitUntil waitForParticles = new WaitUntil(() => _particlesToDestroy.Count > 0);

        List<ParticleSystem> toRemove = new List<ParticleSystem>();

        while (true)
        {
            yield return waitForParticles;

            foreach (ParticleSystem particleSystem in _particlesToDestroy)
            {
                if (particleSystem.isPlaying)
                    continue;

                toRemove.Add(particleSystem);
            }

            foreach (ParticleSystem particleSystem in toRemove)
            {
                _particlesToDestroy.Remove(particleSystem);
                Destroy(particleSystem);
            }

            toRemove.Clear();

            yield return waitForEndOfFrame;
        }
    }

    /// <summary>
    /// Creates the Damage Text in world. Type: Miss: 0 - Normal: 1 - Critical: 2 - Heal: 3
    /// </summary>
    /// <param name="text">Text that will be shown.</param>
    /// <param name="type"></param>
    /// <param name="position">The position it will spawn.</param>
    public void CreateDamageText(string text, int type, Vector3 position)
    {
        Tuple<string, int, Vector3> myText = Tuple.Create(text, type, position);

        GameObject obj = new GameObject();
        //Creo un objeto vacio que mire a la camara en la posicion donde va a crearse el texto 
        GameObject rot = Instantiate(obj, myText.Item3, Quaternion.identity);
        Destroy(obj);
        rot.transform.LookAt(rot.transform.position + _cam.transform.forward);
        rot.name = "name: " + myText.Item1;
        //Uso la rotacion del objeto vacio para que el texto se instancie con esa rotacion
        GameObject tObj = Instantiate(_damageText, myText.Item3 + new Vector3(0, 0, 0), rot.transform.rotation);
        DamageText t = tObj.GetComponent<DamageText>();
        t.SetText(myText.Item1, myText.Item2, 0);
        Destroy(rot);
        StartCoroutine(DestroyEffect(tObj, t.GetDuration()));
    }

    /// <summary>
    /// Creates the Damage Text in world. type: 
    /// </summary>
    /// <param name="text">Text that will be shown.</param>
    /// <param name="type">Miss: 0 - Normal: 1 - Critical: 2 - Heal: 3.</param>
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

    public void CameraShake()
    {
        _cameraInitialPosition = _cam.transform.position;
        InvokeRepeating("StartCameraShaking", 0f, 0.005f);
        Invoke("StopCameraShaking", _shakeTime);
    }

    private void StopCameraShaking()
    {
        CancelInvoke("StartCameraShaking");
        _cam.transform.position = _cameraInitialPosition;
    }

    private void StartCameraShaking()
    {
        float cameraShakingOffsetX = Random.value * _shakeMagnetude * 2 - _shakeMagnetude;
        float cameraShakingOffsetY = Random.value * _shakeMagnetude * 2 - _shakeMagnetude;
        Vector3 cameraIntermadiatePosition = _cam.transform.position;
        cameraIntermadiatePosition.x += cameraShakingOffsetX;
        cameraIntermadiatePosition.y += cameraShakingOffsetY;
        _cam.transform.position = cameraIntermadiatePosition;
    }

}
