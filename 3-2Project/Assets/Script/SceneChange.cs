using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public GameObject MenuButtons;
    public GameObject KeyUI;

    private void Start()
    {
        //Invoke("KeyOn", 6.0f);
    }

    private void OnEnable()
    {
        KeyOn();
    }

    public void KeyOn()
    {
        KeyUI.SetActive(true);
    }
    public void MainTitle()
    {
        SceneManager.LoadScene("MainTitle");
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
            KeyUI.SetActive(false);
            MenuButtons.SetActive(true);
        }
    }
}
