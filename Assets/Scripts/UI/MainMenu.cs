using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void One()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Two()
    {
      SceneManager.LoadScene("MainScene");
    }

    public void Menu()
    {
      SceneManager.LoadScene("Menu");
    }

    public void Tutorial()
    {
      SceneManager.LoadScene("Tutorial");
    }

    public void Exit()
    {
        Application.Quit();
    }

}
