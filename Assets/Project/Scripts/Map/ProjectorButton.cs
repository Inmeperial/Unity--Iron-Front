using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectorButton : CustomButton
{
    [SerializeField] private MissionDataSO _missionData;

    private MissionProjector _projector;
    protected override void Awake()
    {
        _projector = FindObjectOfType<MissionProjector>();

        OnLeftClick.AddListener(LoadScene);
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

    private void LoadScene()
    {
        ChangeScene.Instance.LoadScene(_missionData.mission.sceneName);
    }

    protected override void OnDestroy()
    {
        OnLeftClick.RemoveListener(LoadScene);
    }
}
