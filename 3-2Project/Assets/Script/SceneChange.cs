using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void Awake()
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
}
