using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Menu()
    {
      SceneManager.LoadScene("Menu");
    }

    public void Exit()
    {
        Application.Quit();
    }

}
