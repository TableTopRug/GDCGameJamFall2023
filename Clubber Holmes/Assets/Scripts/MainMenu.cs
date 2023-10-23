using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;

    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void RunAlleys()
    {
        SceneManager.LoadScene("Alley");
    }

    public void EnterCafe()
    {
        SceneManager.LoadScene("Cafe");
    }

    public void HitTheClub()
    {
        SceneManager.LoadScene("Club");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
