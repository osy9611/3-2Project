using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public GameObject MenuButtons;
    public GameObject KeyUI;


    private void OnEnable()
    {
        if(KeyUI!=null)
        {
            KeyUI.SetActive(true);
        }     
    }

    public void MainTitle()
    {
        SceneManager.LoadScene("MainTitle");
    }

    public void DelayMainTitle(float Time)
    {
        Invoke("MainTitle", Time);
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
       if(KeyUI!=null)
       {
            if (Input.anyKeyDown && SceneManager.GetActiveScene().name == "MainTitle")
            {
                KeyUI.SetActive(false);
                MenuButtons.SetActive(true);
            }
        }
       
    }
}
