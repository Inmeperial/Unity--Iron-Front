using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUI : MonoBehaviour
{
    [SerializeField] private GameObject _container;

    [SerializeField] private Slider _bodySlider;
    [SerializeField] private Slider _leftArmSlider;
    [SerializeField] private Slider _rightArmSlider;
    [SerializeField] private Slider _legsSlider;
    [SerializeField] private GameObject _moveActionIcon;
    [SerializeField] private GameObject _attackActionIcon;
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _container.SetActive(false);
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_container.activeInHierarchy)
        {
            _container.transform.LookAt(transform.position + _camera.transform.forward);
        }
    }

    public void ActivateWorldUI(float bodyCurr, float rArmCurr, float lArmCurr, float legsCurr, bool moveStatus, bool attackStatus)
    {
        SetBodySlider(bodyCurr);
        SetLeftArmSlider(rArmCurr);
        SetRightSlider(lArmCurr);
        SetLegsSlider(legsCurr);
        MoveActionIcon(moveStatus);
        AttackActionIcon(attackStatus);
        ContainerActivation(true);
    }

    public void DeactivateWorldUI()
    {
        ContainerActivation(false);
    }

    public void SetLimits(float bodyMax, float rArmMax, float lArmMax, float legsMax)
    {
        _bodySlider.maxValue = bodyMax;
        _bodySlider.minValue = 0;

        _rightArmSlider.maxValue = rArmMax;
        _rightArmSlider.minValue = 0;

        _leftArmSlider.maxValue = lArmMax;
        _leftArmSlider.minValue = 0;

        _legsSlider.maxValue = legsMax;
        _legsSlider.minValue = 0;
    }

    void ContainerActivation(bool status)
    {
        _container.SetActive(status);
    }

   void SetBodySlider(float quantity)
    {
        if (quantity < 0)
            _bodySlider.value = 0;
        else _bodySlider.value = quantity;
    }

    void SetLeftArmSlider(float quantity)
    {
        if (quantity < 0)
            _leftArmSlider.value = 0;
        else _leftArmSlider.value = quantity;
    }

    void SetRightSlider(float quantity)
    {
        if (quantity < 0)
            _rightArmSlider.value = 0;
        else _rightArmSlider.value = quantity;
    }

    void SetLegsSlider(float quantity)
    {
        if (quantity < 0)
            _legsSlider.value = 0;
        else _legsSlider.value = quantity;
    }

    void MoveActionIcon(bool status)
    {
        _moveActionIcon.SetActive(status);
    }

    void AttackActionIcon(bool status)
    {
        _attackActionIcon.SetActive(status);
    }
}
