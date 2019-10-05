using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public SceneChange Scene;
    public List<Sprite>Light;
    public Image LightBar;
    public int LightNum;
    // Start is called before the first frame update
    void Start()
    {
        Scene = FindObjectOfType<SceneChange>();
        LightBar.sprite = Light[LightNum];
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
   
    // Update is called once per frame
    void Update()
    {
        
    }
}
