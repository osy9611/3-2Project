using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Player player;
    public Image Gage;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
