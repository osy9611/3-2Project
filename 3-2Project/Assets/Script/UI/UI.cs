using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Player player;

    public List<Sprite>Light;
    public Image LightBar;
    public int LightNum;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        LightBar.sprite = Light[LightNum];
    }

   
    // Update is called once per frame
    void Update()
    {
        
    }
}
