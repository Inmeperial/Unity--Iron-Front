using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectorButton : CustomButton
{
    [SerializeField] private MissionDataSO _missionData;
    [SerializeField] private float _hoverScaleMultiplier = 1.5f;

    private MissionProjector _projector;

    private Vector3 _originalScale;

    protected override void Awake()
    {
        _originalScale = targetGraphic.transform.localScale;
        _projector = FindObjectOfType<MissionProjector>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        targetGraphic.color = colors.highlightedColor;

        targetGraphic.transform.localScale *= _hoverScaleMultiplier;

        _projector.ChangeMissionImage(_missionData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        targetGraphic.transform.localScale = _originalScale;

        targetGraphic.color = colors.normalColor;
    }
}
