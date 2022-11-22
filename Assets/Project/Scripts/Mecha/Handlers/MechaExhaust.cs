using System.Collections.Generic;
using UnityEngine;

public class MechaExhaust : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _exhaust;

    [Header("Configs")]
    [SerializeField] private float _powerfulExhaustMinSize = 2f;
    [SerializeField] private float _powerfulExhaustMaxSize = 4f;
    [SerializeField] private float _powerfulExhaustLifetime = 4f;
    [SerializeField] private float _normalExhaustMinSize = 1f;
    [SerializeField] private float _normalExhaustMaxSize = 2f;
    [SerializeField] private float _normalExhaustLifetime = 2f;

    [Header("Particles")]
    [SerializeField] private ParticleSystem[] _particlesPrefabs;
    private List<ParticleSystem> _createdParticles = new List<ParticleSystem>();

    void Start()
    {
        if (_particlesPrefabs.Length < 1)
            return;

        Character mecha = GetComponent<Character>();

        mecha.OnBeginMove += PowerfulExhaust;
        mecha.OnEndMove += NormalExhaust;

        mecha.OnMechaDeath += SetMachineOff;

        foreach (ParticleSystem prefab in _particlesPrefabs)
        {
            EffectsController.Instance.PlayPersistentParticles(prefab, _exhaust.transform.position, transform.forward, _exhaust.transform, out ParticleSystem particle);

            if (particle)
                _createdParticles.Add(particle);
        }
        
    }

    private void PowerfulExhaust()
    {
        foreach (ParticleSystem particle in _createdParticles)
        {
            ParticleSystem.MainModule particleMain = particle.main;

            particleMain.startSize = new ParticleSystem.MinMaxCurve(_powerfulExhaustMinSize, _powerfulExhaustMaxSize);
            particleMain.startLifetime = _powerfulExhaustLifetime;
        }
    }

    private void NormalExhaust()
    {
        foreach(ParticleSystem particle in _createdParticles)
        {
            ParticleSystem.MainModule particleMain = particle.main;

            particleMain.startSize = new ParticleSystem.MinMaxCurve(_normalExhaustMinSize, _normalExhaustMaxSize);
            particleMain.startLifetime = _normalExhaustLifetime;
        }
    }

    private void SetMachineOff(Character mecha)
    {
        foreach (ParticleSystem particle in _createdParticles)
        {
            Destroy(particle.gameObject);
        }
    }

    private void OnDestroy()
    {
        
    }
}
