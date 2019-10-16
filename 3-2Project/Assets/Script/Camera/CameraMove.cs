using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Player player;
    public float Speed;
    public float Distance;
    public float Height;
    public float MinDistance;
    public float MaxDistance;
    Vector3 CameraPos;
    Vector3 PlayerPos;
    
    public bool ZoomOn;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        Distance = MinDistance;
    }

    void FixedUpdate()
    {
        if(ZoomOn)
        {
            Distance = MaxDistance;
        }
        else
        {
            Distance = MinDistance;
        }
        CameraPos = new Vector3(transform.position.x, player.gameObject.transform.position.y+Height, Distance);
     
        gameObject.transform.position = Vector3.Lerp(CameraPos, player.gameObject.transform.position, Speed * Time.smoothDeltaTime);
    }
}
