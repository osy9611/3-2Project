using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    Rock,
    Wood
}
public class Obstacle : MonoBehaviour
{
    public ObstacleType OT;
    public SpriteRenderer render;
    public Collider2D col;
    public bool DestroyOn;
    public float AlphaSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public void Break()
    {
        if (render.color.a >= 0)
        {
            render.color = new Color(render.color.r, render.color.g, render.color.b, render.color.a - AlphaSpeed);
        }
        if (render.color.a <= 0.5f)
        {
            col.enabled = false;
        }
        if(render.color.a<=0)
        {
            gameObject.SetActive(false);
            CancelInvoke("Break");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(DestroyOn)
        {
            Invoke("Break", 0.1f);
        }
    }
}
