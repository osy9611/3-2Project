using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepFloor : MonoBehaviour
{
    public List<GameObject> Floors;
    Collider2D col;
    public List<Rigidbody2D> rb;
    public List<SpriteRenderer> render;
    public List<Collider2D> cols;
    public Player player;
    public float Delay;
    public float Speed;
    public bool Starting;
    public bool Done;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<Floors.Count;i++)
        {
            rb.Add(Floors[i].GetComponent<Rigidbody2D>());
            render.Add(Floors[i].GetComponent<SpriteRenderer>());
            cols.Add(Floors[i].GetComponent<Collider2D>());
        }

        col = GetComponent<Collider2D>();
        player = FindObjectOfType<Player>();
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!Starting && !player.Touchingwall)
            {
                Starting = true;
                StartCoroutine(Falling());
            }
        }
    }

    IEnumerator Falling()
    {
        for (int i = 0; i < Floors.Count; i++)
        {
            yield return new WaitForSeconds(Delay);
            rb[i].bodyType = RigidbodyType2D.Dynamic;
            rb[i].gravityScale = Speed;
        }
        yield return new WaitForSeconds(1.0f);
        Done = true;
        col.enabled = false;
        gameObject.SetActive(true);
    }
}
