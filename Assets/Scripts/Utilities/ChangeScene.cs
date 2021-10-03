using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    //public KeyCode changeWorkshopKey;

    private void Update()
    {
        /*
		if (Input.GetKeyDown(KeyCode.F1))
			SceneManager.LoadScene("Level");

		if (Input.GetKeyDown(changeWorkshopKey))
		{
			SceneManager.LoadScene("TallerScene");
		}
		
		if (Input.GetKeyDown(KeyCode.F3))
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		*/
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void LoadWorkShop()
    {
        SceneManager.LoadScene("TallerScene");
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Win()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Win");
    }

    public void Defeat()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Defeat");
    }

    public void Quit()
    {
        Debug.Log("Bye");
        Application.Quit();
    }
}
