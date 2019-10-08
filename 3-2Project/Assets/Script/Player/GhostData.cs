using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostData : MonoBehaviour
{
    public Transform[] Bonse;

    public Vector3[] BonsPos;

    public Vector3[] BonsRot;

    public void TransChange(Transform[] Dummy)
    {
        for(int i=0;i<Bonse.Length;i++)
        {
            Bonse[i].rotation = Quaternion.Euler(Dummy[i].eulerAngles);
        }
    }
}
