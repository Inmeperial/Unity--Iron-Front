﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PortraitsController : MonoBehaviour
{
    public static PortraitsController Instance;
    [SerializeField] private Transform _canvasTransform;
    [SerializeField] private float _portraitPivotHeightForAnimation = 0.75f;
    [SerializeField] private float _portraitMoveTime;
    [SerializeField] private float _animationCurveWeight;
    private List<Tuple<Character, FramesUI>> _charAndFramesList = new List<Tuple<Character, FramesUI>>();
    [SerializeField] private List<FramesUI> _portraits = new List<FramesUI>();
    [SerializeField] private List<RectTransform> _portraitsPositions = new List<RectTransform>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public RectTransform GetMyRect(Character c)
    {
        foreach (var t in _charAndFramesList)
        {
            if (t.Item1 == c)
            {
                return t.Item2.GetComponent<RectTransform>();
            }
        }

        return null;
    }
    
    public void PortraitsActiveState(bool state)
    {
        foreach (var p in _portraits)
        {
            p.gameObject.SetActive(state);
        }
    }
    public void ActivatePortraitsButtons()
    {
        foreach (var portrait in _portraits)
        {
            portrait.selectionButton.interactable = true;
        }
    }
    
    public void DeactivatePortraitsButtons()
    {
        foreach (var portrait in _portraits)
        {
            portrait.selectionButton.interactable = false;
        }
    }

    public void DeadPortrait(Character character)
    {
        foreach (var c in _charAndFramesList)
        {
            if (c.Item1 == character)
                c.Item2.mechaImage.color = Color.red;
        }
        
    }

    public void MovePortrait(Character character, int currentPos = 0, int newPos = 0)
    {
        StartCoroutine(PortraitMovement(GetMyRect(character), currentPos, newPos));
    }
    
    IEnumerator PortraitMovement(RectTransform myRect, int currentPos, int newPos)
    {
        var startV = _portraitsPositions[currentPos].gameObject.GetComponent<RectTransform>();
        var end = _portraitsPositions[newPos].gameObject.GetComponent<RectTransform>();
        var midRectA = new RectTransform();
        var midGoA = new GameObject();

        AnimationCurve curveMinX = null;
        AnimationCurve curveMaxX = null;
        AnimationCurve curveMinY = null;
        AnimationCurve curveMaxY = null;
        if (Mathf.Abs(currentPos - newPos) > 1)
        {
            midGoA.transform.parent = _canvasTransform;
            midRectA = midGoA.AddComponent<RectTransform>();
            midRectA.gameObject.name = "INSTANCIA";
            midRectA.anchorMax = Vector2.Lerp(startV.anchorMax, end.anchorMax, 0.5f);
            midRectA.anchorMin = Vector2.Lerp(startV.anchorMin, end.anchorMin, 0.5f);
            var anchorMax = midRectA.anchorMax;
            anchorMax.y = _portraitPivotHeightForAnimation;
            anchorMax.x = Mathf.Lerp(startV.anchorMax.x, end.anchorMax.x, 0.5f);
            midRectA.anchorMax = anchorMax;
            var anchorMin = midRectA.anchorMin;
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
            //curveMinY.SmoothTangents(1, 0);
            curveMaxY = new AnimationCurve(kMaxY);
            //curveMaxY.SmoothTangents(1, 0);
            //Debug.Break();
        }

        var time = 0f;
        
        while (time <= _portraitMoveTime)
        {
            time += Time.deltaTime;
            var normalized = time / _portraitMoveTime;
            
            if (Mathf.Abs(currentPos - newPos) > 1)
            {
                //myRect.anchoredPosition = new Vector2(animationCurveX.Evaluate(time), animationCurveY.Evaluate(time));
                myRect.anchorMax = new Vector2(curveMaxX.Evaluate(time), curveMaxY.Evaluate(time));
                myRect.anchorMin = new Vector2(curveMinX.Evaluate(time), curveMinY.Evaluate(time));

                // if (normalized <= 0.5f)
                // {
                //     myRect.anchorMax = Vector2.Lerp(startV.anchorMax, midRectA.anchorMax, normalized * 2);
                //     myRect.anchorMin = Vector2.Lerp(startV.anchorMin, midRectA.anchorMin, normalized * 2);
                // }
                // else
                // {
                //     myRect.anchorMax = Vector2.Lerp(midRectA.anchorMax, end.anchorMax, normalized);
                //     myRect.anchorMin = Vector2.Lerp(midRectA.anchorMin, end.anchorMin, normalized);
                // }
            }
            else
            {
                myRect.anchorMax = Vector2.Lerp(startV.anchorMax, end.anchorMax, normalized);
                myRect.anchorMin = Vector2.Lerp(startV.anchorMin, end.anchorMin, normalized);
            }
            yield return new WaitForEndOfFrame();
        }

        myRect.anchoredPosition = end.anchoredPosition;
        
        Destroy(midGoA);
    }

    public void AddCharAndFrame(Tuple<Character, FramesUI> t)
    {
        _charAndFramesList.Add(t);
    }

    public List<FramesUI> GetPortraits()
    {
        return _portraits;
    }

    public List<RectTransform> GetPortraitsRectPosition()
    {
        return _portraitsPositions;
    }
}
