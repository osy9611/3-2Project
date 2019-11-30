using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float Speed;
    public bool On;

    public Vector2 EndPos;


//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if(collision.gameObject.name == "Ground")
//        { 
//}
//    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, EndPos) != 0)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, EndPos, Time.deltaTime * Speed);
        }
        else if (Vector2.Distance(transform.position, EndPos) == 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
