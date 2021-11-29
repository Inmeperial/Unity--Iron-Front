using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] public MenuOptionsInGame menuOptions = default;
    public static bool _firstTimePlaying = false;


    public void MenuLoadScene()
    {
        if (!_firstTimePlaying)
        {
            _firstTimePlaying = true;
            LoadLevel1();
        }
        else
            LoadMap();
    }

    public void LoadLevel1()
    {
        if(menuOptions)
            menuOptions.CloseAllMenu();
        SceneManager.LoadScene("Level 1 NUEVO");
    }

    public void LoadLevel2()
    {
        if (menuOptions)
            menuOptions.CloseAllMenu();
        SceneManager.LoadScene("Level 2");
    }

    public void LoadLevel3()
    {
        if (menuOptions)
            menuOptions.CloseAllMenu();
        SceneManager.LoadScene("Level 3");
    }

    public void LoadWorkShop()
    {
        if (menuOptions)
            menuOptions.CloseAllMenu();
        SceneManager.LoadScene("TallerScene");
    }

    public void LoadMap()
    {
        SceneManager.LoadScene("Map");
    }

    public void ReloadLevel()
    {
        if (menuOptions)
            menuOptions.CloseAllMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        if (menuOptions)
            menuOptions.CloseAllMenu();
        Debug.Log("Bye");
        Application.Quit();
    }
}
