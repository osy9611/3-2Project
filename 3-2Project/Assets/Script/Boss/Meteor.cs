using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float Speed;
    public bool On;

    public Vector2 EndPos;

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, EndPos) != 0)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, EndPos, Time.deltaTime * Speed);
        }
        else if (transform.localPosition.y == EndPos.y)
        {
            this.gameObject.SetActive(false);
        }
    }
}
