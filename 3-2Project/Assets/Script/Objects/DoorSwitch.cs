using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DoorType
{
    Rock,
    Wood,
    Bush
}

public class DoorSwitch : MonoBehaviour
{
    public DoorType DT;
    public SpriteRenderer renderer;
    public Door[] Doors;
    public RockFall Rock;
    public CameraMove cameraMove;

    public Player player;    
    public Trap trap;
    public bool Off;

    public float Speed;
    public Vector3 Shaft;
    public bool Fall;

    AudioManager Audio;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        cameraMove = FindObjectOfType<CameraMove>();
        Audio = FindObjectOfType<AudioManager>();
        player = FindObjectOfType<Player>();
        trap = GetComponent<Trap>();
        Rock = GetComponent<RockFall>();
    }

    public void DoorOn()
    {
        Off = true;
        renderer.flipX = true;
        trap.enabled = false; 
        if (!Fall)
        {
            for (int i = 0; i < Doors.Length; i++)
            {
                if(DT== DoorType.Rock)
                {
                    Audio.OnePlay(7);
                }
                if (DT == DoorType.Wood)
                {
                    Audio.OnePlay(14);
                }
                if (DT == DoorType.Bush)
                {
                    Audio.OnePlay(13);
                }
                Doors[i].On = true;
            }
        }
        else
        {
            Rock.FallingOn();
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

    private void OnDisable()
    {
        if(trap!=null)
        {
            transform.localPosition = trap.OriginPos;
            trap.enabled = true;
        }        
        Off = false;
    }
}
