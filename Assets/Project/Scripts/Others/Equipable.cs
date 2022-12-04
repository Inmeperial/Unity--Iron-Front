using System;
using UnityEngine;

public abstract class Equipable : MonoBehaviour
{
    protected EquipableSO.EquipableType _equipableType;
    protected int _availableUses;
    protected Character _character;
    protected EquipmentButton _button;
    protected Sprite _icon;
    public Action OnEquipableUsed;
    public Action OnEquipableSelected;
    public Action OnEquipableDeselected;

    public abstract void Initialize(Character character, EquipableSO data);

    public abstract void Select();

    public abstract void Deselect();

    public abstract void Use();

    public abstract string GetEquipableName();

    public abstract string GetEquipableDescription();

    public void SetButton(EquipmentButton button)
    {
        _button = button;
    }

    public Sprite GetIcon()
    {
        return _icon;
    }
    
    public int GetAvailableUses()
    {
        return _availableUses;
    }

    protected virtual void UpdateButtonText(string text, EquipableSO data)
    {
        _button.SetButtonText(text, _equipableType);
    }

    public abstract void UpdateEquipableState();

    public abstract bool CanBeUsed();

    public virtual bool IsInteractable()
    {
        return _equipableType != EquipableSO.EquipableType.Passive;
    }

    public EquipableSO.EquipableType GetEquipableType()
    {
        return _equipableType;
    }

    public virtual void PlaySound(SoundData sound, GameObject soundSource)
    {
        AudioManager.Instance.PlaySound(sound, soundSource);
    }

    public virtual void PlayVFX(ParticleSystem particlePrefab, Vector3 position, Vector3 forward)
    {
        EffectsController.Instance.PlayParticlesEffect(particlePrefab, position, forward);
    }

    protected virtual void OnDestroy()
    {
        OnEquipableUsed = null;
        OnEquipableSelected = null;
        OnEquipableDeselected = null;
    }
}
