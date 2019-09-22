﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public Transform sunrise;
    public Transform sunset;
    public float journeyTime = 1.0F;
    public float time;
    private float startTime;
    void Start()
    {
        startTime = Time.time;
    }
    void Update()
    {
        Vector3 center = (sunrise.position + sunset.position) * 0.5F;
        center -= new Vector3(-1, 0, 0);
        Vector3 riseRelCenter = sunrise.position - center;
        Vector3 setRelCenter = sunset.position - center;
        float fracComplete = (Time.time - startTime) / journeyTime;
        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, journeyTime);
        transform.position += center;
    }
}
