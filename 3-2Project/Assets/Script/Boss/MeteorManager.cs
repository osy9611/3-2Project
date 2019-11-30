using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : MonoBehaviour
{
    //메테오 높이
    [Header("")]
    public GameObject MaxHeight;
    public GameObject MinHeight;

    public GameObject meteor;

    public float MaxSpeed;
    public float MinSpeed;

    //메테오 갯수
    [Header("메테오 갯수를 지정해주세요")]
    public int MeteorCount;

    public List<Meteor> meteors;

    public Boss boss;

    public GameObject MeteorParent;

    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponent<Boss>();
        for(int i=0;i<MeteorCount;i++)
        {
            GameObject dummy = Instantiate(meteor, new Vector2(0, 0), Quaternion.identity);
            dummy.SetActive(false);
            dummy.transform.SetParent(MeteorParent.transform);
            meteors.Add(dummy.GetComponent<Meteor>());       

        }  
    }

    public void SetMeteor()
    {
        for (int i = 0; i < MeteorCount; i++)
        {
            float PosX = Random.Range(boss.Walls.TopWall[0].x, boss.Walls.TopWall[1].x);
            float PosY = Random.Range(MinHeight.transform.position.y, MaxHeight.transform.position.y);
            meteors[i].transform.position = new Vector2(PosX, PosY);
            meteors[i].Speed = Random.Range(MinSpeed, MaxSpeed);
            meteors[i].EndPos = new Vector2(PosX, boss.Walls.BottomWall[0].y);
            for (int j = 0; j < i; j++)
            {
                if (meteors[i].transform.position == meteors[j].transform.position)
                {
                    i--;
                    break;
                }
            }
        }
    }

    public void MeteorOn()
    {
        for(int i=0;i<MeteorCount;i++)
        {
            meteors[i].gameObject.SetActive(true);
        }
    }
}
