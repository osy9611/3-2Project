using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RockData
{
    public Rigidbody2D rb;
    public Collider2D col;

}


public class RockFall : MonoBehaviour
{
    [SerializeField]
    public List<RockData> Data;
    public List<GameObject> Rocks;

    public float Speed;
    public float Delay;
    public bool Done;
    void Start()
    {
        for(int i=0;i<Rocks.Count;i++)
        {
            RockData dummy;
            dummy.rb = Rocks[i].GetComponent<Rigidbody2D>();
            dummy.col = Rocks[i].GetComponent<Collider2D>();
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
            Data[i].rb.bodyType = RigidbodyType2D.Dynamic;
            Data[i].rb.gravityScale = Speed;
        }
        yield return new WaitForSeconds(1.0f);
        Done = true;
        for(int i=0;i<Rocks.Count;i++)
        {
            Rocks[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
