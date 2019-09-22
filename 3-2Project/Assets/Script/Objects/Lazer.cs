using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public Player player;
    RaycastHit2D hit;
    public Vector3 RayTrans;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {        
        hit = Physics2D.Raycast(transform.position, transform.right);
        Debug.DrawRay(transform.position, transform.right, new Color(1, 0, 0),Mathf.Infinity);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "LightIn")
            {
                if (RayTrans != hit.collider.gameObject.transform.position)
                {
                    RayTrans = hit.collider.gameObject.transform.position;
                    hit.collider.gameObject.transform.parent.GetComponent<Prism>().RazerON = true;
                }               
            }
            else
            {
                RayTrans = Vector2.zero;
            }

            if(hit.collider.gameObject.tag == "Player")
            {

            }
        }
    }
}
