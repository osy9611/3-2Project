﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public Player player;
    RaycastHit2D hit;
    public Transform HitPoint;
    public Transform LayHit;
    public Transform OriginPos;
    public Transform Target;
    public Prism Obj;
    LineRenderer laser;
    public bool RazerOn;
    
    float Range;
   
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        laser = GetComponent<LineRenderer>();
        Target = OriginPos;
        Range = Vector2.Distance(transform.position, Target.position);
    }

    private void OnDisable()
    {
        if(Obj!=null)
        {
            Obj.RazerON = false;
            Obj = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        int Mask = 1 << 11;
        Mask = ~Mask;
        hit = Physics2D.Raycast(transform.position, transform.up,Range, Mask);
     
        laser.SetPosition(0, transform.position);

        if(hit.collider!=null)
        {
            if (hit.collider.gameObject.tag != "Player")
            {
                laser.SetPosition(1, hit.point);
            }
            if (hit.collider.gameObject.tag == "LightIn")
            {
                if (Obj == null)
                {
                    HitPoint = hit.collider.transform;
                    Obj = hit.collider.gameObject.transform.parent.GetComponent<Prism>();
                    Obj.HitObj = GetComponent<Prism>();
                    Obj.RazerON = true;
                }
                else if (HitPoint.position != hit.collider.transform.position)
                {
                    HitPoint = null;
                    Obj.RazerON = false;
                    Obj = null;
                }
            }
            else
            {
                if (Obj != null)
                {
                    Obj.RazerON = false;
                    Obj = null;
                }
            }
            if (hit.collider.gameObject.tag == "LightOut")
            {
                if (Obj == null)
                {
                    Obj = hit.collider.gameObject.transform.parent.GetComponent<Prism>();
                }
                else
                {
                    if (Obj.transform.position != hit.collider.gameObject.transform.position)
                    {
                        Obj.laser.enabled = false;
                    }
                }
                Obj = null;
            }

            if (hit.collider.gameObject.tag == "Player")
            {
                player.LightCheck(player.MaxLightCount, false);
            }
        }
        else
        {
            laser.SetPosition(1, Target.position);
        }
    }
}
