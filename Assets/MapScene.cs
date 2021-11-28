using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapScene : MonoBehaviour
{
	static bool _firstTimePlaying = false;

	public void FirstTimePlayingLoad()
	{
		if (!_firstTimePlaying)
		{
			SceneManager.LoadScene("Level 1 NUEVO");
			_firstTimePlaying = true;
		}
		else
			LoadMap();
	}

    public void LoadScene(string sceneName)
	{
		var scene = SceneManager.GetSceneByName(sceneName);
		if (scene == null) return;
		SceneManager.LoadScene(sceneName);
	}

	public void LoadMap()
	{
		SceneManager.LoadScene("Map");//Agregar Map al build index
	}
	
	public void Quit()
	{
		Application.Quit();
	}
}
