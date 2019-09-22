using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public Player player;
    RaycastHit2D hit;
    public Vector3 RayTrans;
    public Transform LayHit;
    Prism Obj;

    LineRenderer laser;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        laser = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(transform.position, transform.right);
        Debug.DrawRay(transform.position, hit.point, new Color(1, 0, 0), Vector3.Distance(transform.position, hit.point));
        laser.SetPosition(0, transform.position);
        if (hit.collider)
        {
            if (hit.collider.gameObject.tag != "Player")
            {
                LayHit = hit.collider.gameObject.transform;
                laser.SetPosition(1, hit.point);
            }
            if (hit.collider.gameObject.tag == "LightIn")
            {
               
                if (Obj != hit.collider.gameObject)
                {
                    Obj = hit.collider.gameObject.transform.parent.GetComponent<Prism>();
                    Obj.RazerON = true;
                }
            }
            else
            {
                if(Obj!=null)
                {
                    Obj.RazerON = false;
                    Obj = null;
                }
            }
            if (hit.collider.gameObject.tag == "Player")
            {
               
            }

            if(hit.collider.gameObject.tag == "LightOut")
            {
                if (Obj != hit.collider.gameObject)
                {
                    Obj = hit.collider.gameObject.transform.parent.GetComponent<Prism>();
                    Obj.RazerON = false;
                }
            }
        }

    }
}
