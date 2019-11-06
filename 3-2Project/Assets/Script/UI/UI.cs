using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public SceneChange Scene;

    public GameObject Fade;
    public Animator FadeAni;

    // Start is called before the first frame update
    void Start()
    {
        Scene = FindObjectOfType<SceneChange>();
    }

    public void StartGame()
    {
        Scene.Game();
    }

    public void ExitGame()
    {
        Scene.Exit();
    }

    public void MoveScene()
    {
        Scene.Scene();
    }

    public void FadeOut()
    {
        if(FadeAni !=null && Fade.activeSelf==true)
        {
            FadeAni.SetBool("FadeOn", false);
        }
    }

    public void DelayFadeOut(float time)
    {
        Invoke("FadeOut", time);
    }

    public void FadeIn()
    {
        if (FadeAni != null && Fade.activeSelf == true)
        {
            FadeAni.SetBool("FadeOn", true);
        }
    }

    public void DelayFadeIn(float time)
    {
        Invoke("FadeIn", time);
    }
}
