using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void Break()
    {
        ani.SetBool("Break", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
