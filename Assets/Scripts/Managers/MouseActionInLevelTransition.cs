using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseActionInLevelTransition : MonoBehaviour
{
    public bool allReadyPlayed = false; //so we dont re play the same level, need to make the texture in gray.
    [SerializeField] private LevelEnumList levelToLoad;
    private Material _mat = default;

    void Start()
    {
        _mat = this.GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == this.transform)
                {
                    if (levelToLoad == LevelEnumList.LevelWS || !allReadyPlayed)
                    {
                        LoadScene(levelToLoad);
                    }
                }
            }
        }
    }

    private void LoadScene(LevelEnumList levelEnum)
    {
        switch (levelEnum)
        {
            case LevelEnumList.Level1:
                SceneManager.LoadScene("Level 1");
                break;
            case LevelEnumList.Level2:
                SceneManager.LoadScene("Level 2");
                break;
            case LevelEnumList.Level3:
                SceneManager.LoadScene("Level 3");
                break;
            case LevelEnumList.LevelWS:
                SceneManager.LoadScene("TallerScene");
                break;
            default:
                SceneManager.LoadScene("Level 1");
                break;
        }
    }

    void OnMouseEnter()
    {
        if (!allReadyPlayed || levelToLoad == LevelEnumList.LevelWS)
        {
            SetOutLine(true);
        }
    }

    void OnMouseExit()
    {
        if (!allReadyPlayed || levelToLoad == LevelEnumList.LevelWS)
        {
            SetOutLine(false);
        }
    }

    private void SetOutLine(bool key)
    {
        _mat.SetInt("_isOutLineOn", key ? 1 : 0);
    }

    private enum LevelEnumList
    {
        Level1,
        Level2,
        Level3,
        LevelWS,
    }

}
