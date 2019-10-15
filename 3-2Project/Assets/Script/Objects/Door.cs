using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 Shaft;
    public float Speed;
    public Vector3 OriginPos;
    Vector3 MovePos;
    public bool On;
    public bool Done;
    // Start is called before the first frame update
    void Start()
    {
        OriginPos = transform.localPosition;
        Done = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(On)
        {
            if (Vector2.Distance(transform.localPosition, Shaft) >= 0 && Done)
            {
                if (Vector2.Distance(transform.localPosition, Shaft) == 0)
                {
                    Done = false;

                }
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, Shaft, Time.deltaTime * Speed);
            }
        }
    }
}
