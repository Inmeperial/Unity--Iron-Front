using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FramesUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Image mechaImage;
	public Image overWeight;
	public Image colorBorder;
	public Image colorNamePlate;
	public Color playerColor;
	public Color enemyColor;
	public TextMeshProUGUI mechaName;
	public CustomButton selectionButton;
	public Character _mecha;

	private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    public void ChangeData(Image newMechaImage, TextMeshProUGUI newMechaName)
	{
		mechaImage = newMechaImage;
		mechaName.text = newMechaName.text;
	}

    public void RemoveButtonLeftClickListeners() => selectionButton.OnLeftClick.RemoveAllListeners();

    public void AddButtonLeftClickListener(UnityAction action) => selectionButton.OnLeftClick.AddListener(action);

    public void RemoveButtonRightClickListeners() => selectionButton.OnRightClick.RemoveAllListeners();

    public void AddButtonRightClickListener(UnityAction action) => selectionButton.OnRightClick.AddListener(action);

    public FramesUI SetMecha(Character mecha)
	{
		_mecha = mecha;

		_mecha.OnOverweight += OverweightIconState;
		return this;
	}
	public FramesUI SetSprite(Sprite sprite)
	{
		mechaImage.sprite = sprite;
		return this;
	}

	public FramesUI SetName(string unitName)
	{
		mechaName.text = unitName;
		return this;
	}

	public FramesUI SetBorderColor(EnumsClass.Team team)
	{
        Color color =  team == EnumsClass.Team.Green ? playerColor : enemyColor;
		colorBorder.color = color;
		colorNamePlate.color = color;
		
		return this;
	}

    public void ChangeMechaImageColor(Color color) => mechaImage.color = color;

    public void OverweightIconState(Character mecha, bool state) => overWeight.gameObject.SetActive(state);

	public void ShowFrame() => gameObject.SetActive(true);
	public void HideFrame() => gameObject.SetActive(false);
	public void EnableFrame() => selectionButton.enabled = true;
	public void DisableFrame() => selectionButton.enabled = false;
	public RectTransform GetRectTransform() => _rectTransform;
    public void OnPointerEnter(PointerEventData eventData)
	{
		if (_mecha.IsDead())
			return;

		_mecha.SetShaderForAllParts(SwitchTextureEnum.TextureFresnel);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (_mecha.IsDead())
			return;

		_mecha.SetShaderForAllParts(SwitchTextureEnum.TextureClean);
	}
}
