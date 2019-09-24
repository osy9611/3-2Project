using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : MonoBehaviour
{
    public Player player;
    public bool RazerON;
    RaycastHit2D hit;
    public Transform raserPos;
    public Transform LayHit;
    public Prism Obj;
    public Prism HitObj;
    public LineRenderer laser;

    public Transform HitPoint;
    public Transform StoreTrans;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (RazerON)
        {
            laser.enabled = true;
            hit = Physics2D.Linecast(raserPos.position, raserPos.up * 200);
            laser.SetPosition(0, raserPos.position);
            if (hit.collider)
            {
                if (hit.collider.gameObject.tag != "Player")
                {
                    laser.SetPosition(1, hit.point);
                }
                if (hit.collider.gameObject.tag == "LightIn")
                {
                    HitPoint = hit.collider.transform;
                    if (Obj==null)
                    {
                        Obj = hit.collider.gameObject.transform.parent.GetComponent<Prism>();
                        Obj.HitObj = GetComponent<Prism>();
                    }
                    else
                    {
                        Obj.RazerON = true;
                    }
                }
                else
                {
                    if(Obj!=null)
                    {
                        Obj.RazerON = false;
                    }
                }
                if (hit.collider.gameObject.tag == "LightOut")
                {
                    if(Obj ==null)
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
                    
                }
            }
            else
            {
                laser.SetPosition(1, new Vector2(raserPos.position.x, -raserPos.position.y * 200));
            }
        }
        else
        {
            laser.enabled = false;
        }
    }
}
