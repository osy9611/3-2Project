using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Vector3 Shaft;
    public float Speed;
    public Vector3 OriginPos;
    public Vector3 SavePointPos;
    Vector3 MovePos;
    public bool Done, None;
    // Start is called before the first frame update
    void Start()
    {
        OriginPos = transform.localPosition;
        SavePointPos = transform.position;
        Shaft.z = OriginPos.z;
        Done = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if((int)collision.contacts[0].point.y > transform.position.y)
            collision.transform.SetParent(this.transform);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(null);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.localPosition, Shaft) >= 0 && Done)
        {
            if (Vector3.Distance(transform.localPosition, Shaft) == 0)
            {
                None = true;
                Done = false;
            }

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Shaft, Time.deltaTime * Speed);
        }
        else if (Vector3.Distance(transform.localPosition, OriginPos) >= 0 && None)
        {
            if (Vector3.Distance(transform.localPosition, OriginPos) == 0)
            {
                None = false;
                Done = true;
            };
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, OriginPos, Time.deltaTime * Speed);
        }
    }
}
