using System.Collections;
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
    Prism Obj;
    LineRenderer laser;
    public bool RazerOn;

    public float Damage;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        laser = GetComponent<LineRenderer>();
        Target = OriginPos;
    }
    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Linecast(transform.position, Target.position);
        laser.SetPosition(0, transform.position);

        if(hit.collider!=null)
        {
            if (hit.collider.gameObject.tag != "Player")
            {
                laser.SetPosition(1, hit.point);
            }
            if (hit.collider.gameObject.tag == "LightIn")
            {
                HitPoint = hit.collider.transform;
                if (Obj == null)
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
                player.LightCheck(Damage,false);
            }
        }
        else
        {
            laser.SetPosition(1, Target.position);
        }
    }
}
