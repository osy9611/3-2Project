using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void MainTitle()
    {
        SceneManager.LoadScene("MainTitle");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Game()
    {
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Scene()
    {

    }
    private void Update()
    {
       if(Input.anyKeyDown && SceneManager.GetActiveScene().name == "MainTitle")
        {
            MainMenu();
        }
    }
}
