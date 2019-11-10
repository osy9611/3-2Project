using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public Animator Ani;

    public void CheckAni()
    {
        Ani.SetBool("SaveOn", true);
    }
}
