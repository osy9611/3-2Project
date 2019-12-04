using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornManager : MonoBehaviour
{
    //가시를 만들기 위한 것
    //오브젝트 풀링 매니저를 만들어서 사용하는게 좋지만 일단은 생략
    [Header("가시 오브젝트를 넣어주세요")]
    public GameObject Thorn;
    public List<GameObject> Thorns;
    float ThornDistance;

    [Header("가시 오브젝트를 몇개 셋팅할지 넣어주세요 자동으로 만들어집니다")]
    public float SetThornCount;
    public int ThornCount;
    public bool ThornSetOn;

    public GameObject ThornParent;
    Boss boss;

    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponent<Boss>();

        ThornDistance = (Vector2.Distance(boss.Walls.BottomWall[0], boss.Walls.BottomWall[1])) / SetThornCount;
        Thorns.Add(Thorn);
        Thorn.SetActive(false);
        for (int i = 1; i < SetThornCount; i++)
        {
            GameObject ThornDummy = Instantiate(Thorn, new Vector2(0, 0), Quaternion.identity);
            ThornDummy.transform.position = new Vector2(Thorn.transform.position.x + (ThornDistance * (i)), Thorn.transform.position.y);
            ThornDummy.SetActive(false);
            ThornDummy.transform.SetParent(ThornParent.transform);
            ThornDummy.GetComponent<Thorn>().Shaft.x = ThornDummy.transform.position.x;
            Thorns.Add(ThornDummy);
        }
    }

    public void SetThorn()
    {
        boss.Audio.Play(19);
        ThornSetOn = false;
        CancelInvoke("SetThorn");
        ThornCount++;
        Thorns[ThornCount].SetActive(true);
        ThornSetOn = true;
    }

    public void DelaySetThorn(float Time)
    {
        Invoke("SetThorn", Time);
    }
    
}
