using UnityEngine;

public class ParticleMechaHandler : MonoBehaviour
{
    [SerializeField] private GameObject spawnerMechaBurning;
    public GameObject[] arrParticleObj;
    private ParticleSystem[] _arrPartSystem;
    private bool _dead;
    void Start()
    {
        if (arrParticleObj != null)
        {
            _arrPartSystem = new ParticleSystem[arrParticleObj.Length];
            for (int i = 0; i < arrParticleObj.Length; i++)
            {
                if (arrParticleObj[i].GetComponent<ParticleSystem>() != null)
                {
                    _arrPartSystem[i] = arrParticleObj[i].GetComponent<ParticleSystem>();
                }
            }
        }
    }

    public void SetMachineOn(bool boolEffect)
    {
        if (_arrPartSystem != null)
        {
            for (int i = 0; i < _arrPartSystem.Length; i++)
            {
                var particleMain = _arrPartSystem[i].main;

                if (boolEffect)
                {
                    particleMain.startSize = new ParticleSystem.MinMaxCurve(2f, 4f);
                    particleMain.startLifetime = 4f;
                }
                else
                {
                    particleMain.startSize = new ParticleSystem.MinMaxCurve(1f, 2f);
                    particleMain.startLifetime = 2f;
                }
            }
        }
    }

    public void SetMachineOff()
    {
        if (_dead) return;
        
        if (_arrPartSystem != null)
        {
            for (int i = 0; i < _arrPartSystem.Length; i++)
            {
                var particleMain = _arrPartSystem[i].main;

                particleMain.startSize = new ParticleSystem.MinMaxCurve(0f, 0f);
                particleMain.startLifetime = 0f;
            }
        }
        EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Dead);
        _dead = true;
    }

    public void SetParticleWeapon(GameObject objGunParticleSpawn, EnumsClass.ParticleActionType type)
    {
        EffectsController.Instance.PlayParticlesEffect(objGunParticleSpawn, type);
    }

    public GameObject GetBurningSpawnerFromParticleMechaHandler()
    {
        return spawnerMechaBurning;
    }

}
