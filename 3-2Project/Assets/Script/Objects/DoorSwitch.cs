using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorSwitch : MonoBehaviour
{
    public SpriteRenderer renderer;
    public Door[] Doors;
    public CameraMove cameraMove;

    public Player player;    
    public Trap trap;
    public bool Off;

    public float Speed;
    public Vector3 Shaft;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        cameraMove = FindObjectOfType<CameraMove>();
        player = FindObjectOfType<Player>();
        trap = GetComponent<Trap>();
    }

    public void DoorOn()
    {
        Off = true;
        renderer.flipX = true;
        trap.enabled = false;       
        for (int i=0;i<Doors.Length;i++)
        {
            Doors[i].On = true;
        }
        cameraMove.CameraShake();
    }

    private void Update()
    {
        if(Off)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Shaft, Time.deltaTime * Speed);
        }
    }
}
