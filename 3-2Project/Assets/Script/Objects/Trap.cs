using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Vector3 Shaft;
    public float Speed;
    Vector3 OriginPos;
    Vector3 MovePos;
    public bool Done, None;
    // Start is called before the first frame update
    void Start()
    {
        OriginPos = transform.position;
        Done = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position,Shaft) >= 0  && Done)
        {
            if(Vector2.Distance(transform.position, Shaft) == 0)
            {
                None = true;
                Done = false;
            }
            
            transform.position = Vector2.MoveTowards(transform.position, Shaft, Time.deltaTime * Speed);
        }
        else if(Vector2.Distance(transform.position, OriginPos) >= 0 && None)
        {
            if (Vector2.Distance(transform.position, OriginPos) == 0)
            {
                None = false;
                Done = true;
            };
            transform.position = Vector2.MoveTowards(transform.position, OriginPos, Time.deltaTime * Speed);
        }
        
    }
}
