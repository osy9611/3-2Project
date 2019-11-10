using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StepData
{
    public Rigidbody2D rb;
    public SpriteRenderer render;
    public Collider2D col;
    public Vector3 OriginPos;
}
public class StepFloor : MonoBehaviour
{
    public List<GameObject> Floors;
    Collider2D col;

    [SerializeField]
    public List<StepData> Data;
    public Player player;
    public float Delay;
    public float Speed;
    public bool Starting;
    public bool Done;
    public bool Stop;
    public bool WallTouch;

    AudioManager Audio;

    // Start is called before the first frame update
    void Start()
    {
        Audio = FindObjectOfType<AudioManager>();
        for (int i=0;i<Floors.Count;i++)
        {
            StepData dummy;
            dummy.rb=Floors[i].GetComponent<Rigidbody2D>();
            dummy.render =Floors[i].GetComponent<SpriteRenderer>();
            dummy.col=Floors[i].GetComponent<Collider2D>();
            dummy.OriginPos = Floors[i].transform.position;
            Data.Add(dummy);
        }

        col = GetComponent<Collider2D>();
        player = FindObjectOfType<Player>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if ((int)player.GroundCheck.transform.transform.position.y == collision.contacts[0].point.y)
            {
                Stop = true;
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!Starting && !Stop)
            {
                Starting = true;
                col.enabled = false;
                StartCoroutine(Falling());
            }
            Stop = false;
        }
    }

    IEnumerator Falling()
    {
        for (int i = 0; i < Floors.Count; i++)
        {
            yield return new WaitForSeconds(Delay);
            Audio.Play(15);
            Data[i].rb.bodyType = RigidbodyType2D.Dynamic;
            Data[i].rb.gravityScale = Speed;
        }
        yield return new WaitForSeconds(1.0f);
        Done = true;       
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Done = false;
        Starting = false;
        col.enabled = true;
        for (int i=0;i< Data.Count;i++)
        {
            Floors[i].transform.position = Data[i].OriginPos;
            Data[i].rb.bodyType = RigidbodyType2D.Static;
        }
    }
}
