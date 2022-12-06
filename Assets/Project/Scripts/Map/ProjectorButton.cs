using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectorButton : CustomButton
{
    [SerializeField] private MissionDataSO _missionData;

    private MissionProjector _projector;
    protected override void Awake()
    {
        _projector = FindObjectOfType<MissionProjector>();

        OnLeftClick.AddListener(SetMissionImageToProjector);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        _projector.ChangeMissionImage(_missionData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

    private void SetMissionImageToProjector()
    {
        //_projector.ChangeMissionImage(_missionData);
    }

    protected override void OnDestroy()
    {
        OnLeftClick.RemoveListener(SetMissionImageToProjector);
    }
}
