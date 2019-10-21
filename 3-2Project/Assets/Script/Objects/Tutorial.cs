using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Player player;
    public bool FadeOn;

    public SpriteRenderer render;
    public float Alpha;
    public float AlphaSpeed;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        render = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
       if(FadeOn)
       {
            if (render.color.a < 1)
            {
                Invoke("FadeIn", 0.1f);
            }
        }
       else
        {
            Invoke("FadeOut", 0.1f);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FadeOn = true;
        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FadeOn = false;
        }
    }

    public void FadeIn()
    {
        render.color = new Color(render.color.r, render.color.g, render.color.b, render.color.a + AlphaSpeed);
    }

    public void FadeOut()
    {
        if (render.color.a >= Alpha)
        {
            render.color = new Color(render.color.r, render.color.g, render.color.b, render.color.a - AlphaSpeed);
        }
    }
}
