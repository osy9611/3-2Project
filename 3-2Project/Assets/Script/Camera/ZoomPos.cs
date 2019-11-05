using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomPos : MonoBehaviour
{
    public float Distance;
    public float PrevDistance;

    public float Height;
    public float PrevHeight;

    //public float 
    public CameraMove camera;
    public Player player;
    public bool In;
    public int direction;
    public int Origindir;
    public float Speed;
    public bool NoDir;
    private void Start()
    {
        camera = FindObjectOfType<CameraMove>();
        player = FindObjectOfType<Player>();
        PrevDistance = camera.Distance;
        if (Distance == 0)
        {
            Distance = camera.Distance;
        }
        if(Height==0)
        {
            Height = camera.Height;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!NoDir)
            {
                if (Origindir == 0)
                {
                    //처음 들어온 방향
                    Origindir = (int)player.x;
                }
                //만약 들어오는 방향이 다르다면 이전에 사용했던 카메라로 변경한다
                if (Origindir != (int)player.x)
                {
                    camera.CheckDistance(PrevDistance);
                    camera.CheckHeight(PrevHeight);
                }
                else if (Origindir == (int)player.x)
                {
                    PrevDistance = camera.Distance;
                    PrevHeight = camera.Height;
                    camera.CheckDistance(Distance);
                    camera.CheckHeight(Height);
                }
            }
            else
            {
                if (Origindir == 0)
                {
                    //처음 들어온 방향
                    Origindir = (int)player.transform.position.y;
                }
                //만약 들어오는 방향이 다르다면 이전에 사용했던 카메라로 변경한다
                if (Origindir != (int)player.transform.position.y)
                {
                    camera.CheckDistance(PrevDistance);
                    camera.CheckHeight(PrevHeight);
                }
                else if (Origindir == (int)player.transform.position.y)
                {
                    PrevDistance = camera.Distance;
                    PrevHeight = camera.Height;
                    camera.CheckDistance(Distance);
                    camera.CheckHeight(Height);
                }
            }



        }
    }
}