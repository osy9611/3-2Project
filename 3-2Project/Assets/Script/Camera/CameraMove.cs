using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Player player;
    public float Speed;
    public float Distance;
    public float Height;
    Vector3 CameraPos;
    Vector3 PlayerPos;
    
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void FixedUpdate()
    {
        CameraPos = new Vector3(transform.position.x, player.gameObject.transform.position.y+Height, Distance);
     
        gameObject.transform.position = Vector3.Lerp(CameraPos, player.gameObject.transform.position, Speed * Time.smoothDeltaTime);
    }
}
