using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
	public KeyCode changeWorkshopKey;
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
			SceneManager.LoadScene("Level");

		if (Input.GetKeyDown(changeWorkshopKey))
		{
			SceneManager.LoadScene("TallerScene");
		}
	}

	public void LoadGame()
	{
		SceneManager.LoadScene("Level");
	}

	public void Win()
	{
		SceneManager.LoadScene("Win");
	}

	public void Defeat()
	{
		SceneManager.LoadScene("Defeat");
	}


	public void Quit()
	{
		Debug.Log("Bye");
		Application.Quit();
	}
}
