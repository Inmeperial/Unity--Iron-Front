using System.Collections;
using UnityEngine;

public class LandMine : MonoBehaviour
{
    public int damage;
    [SerializeField] private SoundData _sound;
    [SerializeField] private ParticleSystem _particleEffect;

    private Collider _collider;
    private Renderer _renderer;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Legs legs = other.GetComponent<Legs>();

        if (!legs)
            return;
        
        legs.ReceiveDamage(damage);
        DestroyMine();
    }

    public void DestroyMine()
    {
        StartCoroutine(DestroyTimer());
    }

    private IEnumerator DestroyTimer()
    {
        _collider.enabled = false;
        _renderer.enabled = false;

        PlayVFX();
        PlaySound();

        yield return new WaitForSeconds(_sound.Clip.length);

        Destroy(gameObject);
    }

    private void PlaySound()
    {
        AudioManager.Instance.PlaySound(_sound, gameObject);
    }

    private void PlayVFX()
    {
        EffectsController.Instance.PlayParticlesEffect(_particleEffect, transform.position, transform.forward);
    }
}
