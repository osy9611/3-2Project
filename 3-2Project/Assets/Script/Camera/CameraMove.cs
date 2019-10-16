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
    public float DistanceSpeed;
    Vector3 CameraPos;
    Vector3 PlayerPos;
    
    public bool ZoomOn;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        Distance = MinDistance;
    }

    public void ZoomIn()
    {
        if (Distance <= MinDistance)
        {
            Distance += DistanceSpeed;
        }
    }

    public void ZoomOut()
    {
        if (Distance >= MaxDistance)
        {
            Distance -= DistanceSpeed;
        }
    }

    void FixedUpdate()
    {
        if (ZoomOn)
        {
            Invoke("ZoomOut", 0.1f);
        }
        else
        {
            Invoke("ZoomIn", 0.1f);
        }

        CameraPos = new Vector3(transform.position.x, player.gameObject.transform.position.y+Height, Distance);
     
        gameObject.transform.position = Vector3.Lerp(CameraPos, player.gameObject.transform.position, Speed * Time.smoothDeltaTime);
    }
}
