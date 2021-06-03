using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string sceneName;

    public void Change()
    {
        SceneManager.LoadScene(sceneName);
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			Change();
		}

		if (Input.GetKeyDown(KeyCode.F))
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
