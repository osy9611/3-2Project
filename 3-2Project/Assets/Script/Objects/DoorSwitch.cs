﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorSwitch : MonoBehaviour
{
    public SpriteRenderer renderer;
    public Door[] Doors;
    public CameraMove cameraMove;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        cameraMove = FindObjectOfType<CameraMove>();
    }

    public void DoorOn()
    {
        renderer.flipX = true;
        for(int i=0;i<Doors.Length;i++)
        {
            Doors[i].On = true;
        }
        cameraMove.CameraShake();

    }
}
