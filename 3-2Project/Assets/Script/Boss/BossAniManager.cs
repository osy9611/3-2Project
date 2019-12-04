using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAniManager : MonoBehaviour
{
    public Animator StateAni;
    public Animator[] Ani;
    public SpriteRenderer[] render;
    Boss boss;
    
    //보스 히트관련
    public int HitCount;
    public float HitSpeed;
    int Count;
    public bool Hit;  

    public bool InfinityOn;
    
    // Start is called before the first frame update
    void Start()
    {
        StateAni = GetComponent<Animator>();
        boss = GetComponent<Boss>();

        render = new SpriteRenderer[Ani.Length];
       
        for (int i=0;i<Ani.Length;i++)
        {
            render[i] = Ani[i].GetComponent<SpriteRenderer>();
        }
    }

    public void HitOn()
    {
        CancelInvoke("HitOn");
        if (HitCount != Count)
        {
            Count++;
            for (int i = 0; i < render.Length; i++)
            {
                if (render[i].enabled)
                {
                    render[i].enabled = false;
                }
                else
                {
                    render[i].enabled = true;
                }
            }
        }
        else
        {
            Count = 0;
            Hit = false;
            for (int i = 0; i < render.Length; i++)
            {
               render[i].enabled = true;
            }
        }
    }

    public void AttackOn()
    {
        for (int i = 0; i < Ani.Length; i++)
        {
            Ani[i].SetTrigger("Attack");
        }
    }

    public void AttackOn(float time)
    {
        Invoke("AttackOn", time);
    }

    private void Update()
    {
        if(Hit)
        {
            Invoke("HitOn", 0.1f);           
        }

        StateAni.SetBool("Infinity", InfinityOn);
        if(boss.BS == BossState.Die)
        {
            StateAni.SetBool("Die", true);
        }

    }

}
