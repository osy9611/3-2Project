using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour
{
    public Vector3 Shaft;
    public float Speed;
    public Vector3 OriginPos;
    public bool On;
    public bool Done;

    // Start is called before the first frame update
    void Awake()
    {
        OriginPos = transform.position;
        Done = true;
    }

    private void OnDisable()
    {
        On = true;
        Done=true;
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
            else if (Vector3.Distance(transform.localPosition, OriginPos) >= 0 && !Done)
            {
                if (Vector3.Distance(transform.localPosition, OriginPos) == 0)
                {
                    On = false;
                    this.gameObject.SetActive(false);
                }
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, OriginPos, Time.deltaTime * Speed);
            }
        }
    }
}
