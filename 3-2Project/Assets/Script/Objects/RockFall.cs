using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RockData
{
    public Rigidbody2D rb;
    public Collider2D col;
    public Vector3 OriginPos;
}


public class RockFall : MonoBehaviour
{
    [SerializeField]
    public List<RockData> Data;
    public List<GameObject> Rocks;

    public float Speed;
    public float Delay;
    public bool Done;

    public Player player;

    AudioManager Audio;
    void Start()
    {
        Audio = FindObjectOfType<AudioManager>();
        for(int i=0;i<Rocks.Count;i++)
        {
            RockData dummy;
            dummy.rb = Rocks[i].GetComponent<Rigidbody2D>();
            dummy.col = Rocks[i].GetComponent<Collider2D>();
            dummy.OriginPos = Rocks[i].transform.position;
            Data.Add(dummy);
        }
    }

    public void FallingOn()
    {
        StartCoroutine(Falling());
    }

    IEnumerator Falling()
    {
        for (int i = 0; i < Data.Count; i++)
        {
            yield return new WaitForSeconds(Delay);
            Audio.Play(15);
            Data[i].rb.bodyType = RigidbodyType2D.Dynamic;
            Data[i].rb.gravityScale = Speed;
        }
        yield return new WaitForSeconds(1.0f);
        Done = true;
        for(int i=0;i<Rocks.Count;i++)
        {
            Rocks[i].gameObject.SetActive(false);
            Data[i].rb.bodyType = RigidbodyType2D.Static;
        }
    }

    private void OnDisable()
    {
        Done = false;
        for(int i=0;i<Rocks.Count;i++)
        {
            Rocks[i].transform.position = Data[i].OriginPos;
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < Rocks.Count; i++)
        {
            Rocks[i].SetActive(true);
        }
    }
}
