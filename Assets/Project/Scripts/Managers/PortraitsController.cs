using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortraitsController : Initializable
{
    //public static PortraitsController Instance;
    [SerializeField] private GameObject _container;
    [SerializeField] private float _portraitPivotHeightForAnimation = 0.75f;
    [SerializeField] private float _portraitMoveTime;
    [SerializeField] private float _animationCurveWeight;
    private List<Tuple<Character, FramesUI>> _charAndFramesList = new List<Tuple<Character, FramesUI>>();
    [SerializeField] private List<FramesUI> _portraits = new List<FramesUI>();
    [SerializeField] private List<RectTransform> _portraitsPositions = new List<RectTransform>();

    public float PortraitMoveTime => _portraitMoveTime;
    public int PortraitsCount => _portraits.Count;

    public Action OnPortraitStoppedMoving;
    public override void Initialize()
    {
        GameManager.Instance.OnEnemyMechaSelected += HidePortraits;
        GameManager.Instance.OnEnemyMechaDeselected += ShowPortraits;
        GameManager.Instance.OnMechaAttackPreparationsFinished += ShowPortraits;
    }

    public RectTransform GetMechaFrameRectTransform(Character mecha)
    {
        foreach (Tuple<Character, FramesUI> tuple in _charAndFramesList)
        {
            if (tuple.Item1 != mecha)
                continue;

            return tuple.Item2.GetRectTransform();
        }

        return null;
    }

    public void ShowPortraits() => _container.SetActive(true);
    public void HidePortraits() => _container.SetActive(false);
    public void ActivatePortraitsButtons()
    {
        foreach (Tuple<Character, FramesUI> tuple in _charAndFramesList)
        {
            if (!tuple.Item1.IsUnitEnabled())
                continue;

            tuple.Item2.selectionButton.interactable = true;
        }
    }
    
    public void DeactivatePortraitsButtons()
    {
        foreach (FramesUI portrait in _portraits)
        {
            portrait.selectionButton.interactable = false;
        }
    }

    public void DisableDeadMechaPortrait(Character deadMecha)
    {
        foreach (Tuple<Character, FramesUI> tuple in _charAndFramesList)
        {
            Character mecha = tuple.Item1;

            if (mecha != deadMecha)
                continue;

            FramesUI portrait = tuple.Item2;

            portrait.ChangeMechaImageColor(Color.red);

            portrait.RemoveButtonLeftClickListeners();
            portrait.RemoveButtonRightClickListeners();
        }
        
    }

    public void MovePortraitOfMechaFromTo(Character mecha, int fromPos = 0, int toPos = 0)
    {
        StartCoroutine(MovePortrait(GetMechaFrameRectTransform(mecha), fromPos, toPos));
    }
    
    private IEnumerator MovePortrait(RectTransform myRect, int fromPos, int toPos)
    {
        RectTransform startV = _portraitsPositions[fromPos];
        RectTransform end = _portraitsPositions[toPos];
        RectTransform midRectA = new RectTransform();
        GameObject midGoA = new GameObject();

        AnimationCurve curveMinX = null;
        AnimationCurve curveMaxX = null;
        AnimationCurve curveMinY = null;
        AnimationCurve curveMaxY = null;
        if (Mathf.Abs(fromPos - toPos) > 1)
        {
            midGoA.transform.parent = _container.transform;
            midRectA = midGoA.AddComponent<RectTransform>();
            midRectA.gameObject.name = "INSTANCIA";
            midRectA.anchorMax = Vector2.Lerp(startV.anchorMax, end.anchorMax, 0.5f);
            midRectA.anchorMin = Vector2.Lerp(startV.anchorMin, end.anchorMin, 0.5f);
            Vector2 anchorMax = midRectA.anchorMax;
            anchorMax.y = _portraitPivotHeightForAnimation;
            anchorMax.x = Mathf.Lerp(startV.anchorMax.x, end.anchorMax.x, 0.5f);
            midRectA.anchorMax = anchorMax;
            Vector2 anchorMin = midRectA.anchorMin;
            anchorMin.y = _portraitPivotHeightForAnimation - (startV.anchorMax.y - startV.anchorMin.y);
            anchorMin.x = anchorMax.x - (startV.anchorMax.x - startV.anchorMin.x);
            midRectA.anchorMin = anchorMin;

            Keyframe[] kMinX = new Keyframe[3];
            kMinX[0] = new Keyframe(0, startV.anchorMin.x);
            kMinX[1] = new Keyframe(_portraitMoveTime/2, midRectA.anchorMin.x);
            kMinX[2] = new Keyframe(_portraitMoveTime, end.anchorMin.x);
            
            Keyframe[] kMaxX = new Keyframe[3];
            kMaxX[0] = new Keyframe(0, startV.anchorMax.x);
            kMaxX[1] = new Keyframe(_portraitMoveTime/2, midRectA.anchorMax.x);
            kMaxX[2] = new Keyframe(_portraitMoveTime, end.anchorMax.x);
            
            Keyframe[] kMinY = new Keyframe[3];
            kMinY[0] = new Keyframe(0, startV.anchorMin.y);
            kMinY[1] = new Keyframe(_portraitMoveTime/2, midRectA.anchorMin.y);
            kMinY[2] = new Keyframe(_portraitMoveTime, end.anchorMin.y);
            
            Keyframe[] kMaxY = new Keyframe[3];
            kMaxY[0] = new Keyframe(0, startV.anchorMax.y);
            kMaxY[1] = new Keyframe(_portraitMoveTime/2, midRectA.anchorMax.y);
            kMaxY[2] = new Keyframe(_portraitMoveTime, end.anchorMax.y);

            curveMinX = new AnimationCurve(kMinX);
            curveMinX.SmoothTangents(1, _animationCurveWeight);
            curveMaxX = new AnimationCurve(kMaxX);
            curveMaxX.SmoothTangents(1, _animationCurveWeight);
            curveMinY = new AnimationCurve(kMinY);
            curveMaxY = new AnimationCurve(kMaxY);
        }

        float time = 0f;
        
        while (time <= _portraitMoveTime)
        {
            time += Time.deltaTime;
            float normalized = time / _portraitMoveTime;
            
            if (Mathf.Abs(fromPos - toPos) > 1)
            {
                myRect.anchorMax = new Vector2(curveMaxX.Evaluate(time), curveMaxY.Evaluate(time));
                myRect.anchorMin = new Vector2(curveMinX.Evaluate(time), curveMinY.Evaluate(time));
            }
            else
            {
                myRect.anchorMax = Vector2.Lerp(startV.anchorMax, end.anchorMax, normalized);
                myRect.anchorMin = Vector2.Lerp(startV.anchorMin, end.anchorMin, normalized);
            }
            yield return new WaitForEndOfFrame();
        }

        myRect.anchoredPosition = end.anchoredPosition;

        OnPortraitStoppedMoving?.Invoke();

        Destroy(midGoA);
    }

    public void AddCharAndFrame(Tuple<Character, FramesUI> t) => _charAndFramesList.Add(t);

    public List<FramesUI> GetPortraits() => _portraits;

    public FramesUI GetMechaPortrait(Character mecha)
    {
        foreach (Tuple<Character, FramesUI> tuple in _charAndFramesList)
        {
            if (tuple.Item1 != mecha)
                continue;

            return tuple.Item2;
        }

        return null;
    }

    public List<RectTransform> GetPortraitsPositionsRectTransform() => _portraitsPositions;

    public FramesUI SetPortrait(Character mecha, int position, Sprite sprite, string name, EnumsClass.Team team, Action leftButtonAction = null, Action rightButtonAction = null)
    {
        FramesUI portrait = _portraits[position].SetSprite(sprite).SetName(name).SetBorderColor(team).SetMecha(mecha);

        if (leftButtonAction != null)
        {
            portrait.RemoveButtonLeftClickListeners();
            portrait.AddButtonLeftClickListener(() => leftButtonAction?.Invoke());
        }
        
        if (rightButtonAction != null)
        {
            portrait.RemoveButtonRightClickListeners();
            portrait.AddButtonRightClickListener(() => rightButtonAction?.Invoke());
        }
        return portrait;
    }

    private void OnDestroy()
    {
        foreach (FramesUI portrait in _portraits)
        {
            portrait.RemoveButtonLeftClickListeners();
            portrait.RemoveButtonRightClickListeners();
        }
    }
}
