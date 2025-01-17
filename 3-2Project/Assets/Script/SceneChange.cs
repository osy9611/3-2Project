﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public GameObject MenuButtons;
    public GameObject KeyUI;

    public AudioManager Audio;

    private void OnEnable()
    {
        if(KeyUI!=null)
        {
            KeyUI.SetActive(true);
        }     
    }

    public void MainTitle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainTitle");
    }

    public void DelayMainTitle(float Time)
    {
        Invoke("MainTitle", Time);
    }

    public void BossStage()
    {
        SceneManager.LoadScene("BossStage");
    }

    public void BossStageDelay(float Time)
    {
        Invoke("BossStage", Time);
    }

    public void Game()
    {
        if (Audio != null)
        {
            Audio.Play("Start");
        }
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        if (Audio != null)
        {
            Audio.Play("Exit");
        }
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
