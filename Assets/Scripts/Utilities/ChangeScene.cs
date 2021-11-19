using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] public MenuOptionsInGame menuOptions = default;

    public void LoadGame()
    {
        menuOptions.CloseAllMenu();
        SceneManager.LoadScene("Level 1 NUEVO");
    }

    public void LoadLevel2()
    {
        menuOptions.CloseAllMenu();
        SceneManager.LoadScene("Level 2");
    }

    public void LoadLevel3()
    {
        menuOptions.CloseAllMenu();
        SceneManager.LoadScene("Level 3");
    }
    public void LoadWorkShop()
    {
        menuOptions.CloseAllMenu();
        SceneManager.LoadScene("TallerScene");
    }

    public void ReloadLevel()
    {
        menuOptions.CloseAllMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Win()
    {
        menuOptions.CloseAllMenu();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Win");
    }

    public void Defeat()
    {
        menuOptions.CloseAllMenu();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Defeat");
    }

    public void Quit()
    {
        menuOptions.CloseAllMenu();
        Debug.Log("Bye");
        Application.Quit();
    }
}
