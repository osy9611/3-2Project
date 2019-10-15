using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Vector3 Shaft;
    public float Speed;
    public Vector3 OriginPos;
    Vector3 MovePos;
    public bool Done, None;
    // Start is called before the first frame update
    void Start()
    {
        OriginPos = transform.localPosition;
        Done = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.localPosition, Shaft) >= 0 && Done)
        {
            if (Vector2.Distance(transform.localPosition, Shaft) == 0)
            {
                None = true;
                Done = false;

            }

            transform.localPosition = Vector2.MoveTowards(transform.localPosition, Shaft, Time.deltaTime * Speed);
        }
        else if (Vector2.Distance(transform.localPosition, OriginPos) >= 0 && None)
        {
            if (Vector2.Distance(transform.localPosition, OriginPos) == 0)
            {
                None = false;
                Done = true;
            };
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, OriginPos, Time.deltaTime * Speed);
        }
    }
}
