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
    Prism Obj;

    public LineRenderer laser;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.SetItem)
        {
            if (RazerON)
            {
                laser.enabled = true;
                hit = Physics2D.Raycast(raserPos.position, raserPos.up);

                laser.SetPosition(0, raserPos.position);
                if (hit.collider)
                {
                    LayHit = hit.collider.gameObject.transform;
                    laser.SetPosition(1, hit.point);
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
                        if (Obj != null)
                        {
                            Obj.RazerON = false;
                            Obj = null;
                        }
                    }
                    if (hit.collider.gameObject.tag == "Player")
                    {

                    }
                }
                else
                {
                    laser.SetPosition(1, raserPos.up * 200);
                }
            }
            else
            {
                laser.enabled = false;
            }
        }
    }
}
