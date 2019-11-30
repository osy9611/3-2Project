using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum BossState
{
    Idle,
    Attack,
    Die,
}

public enum BossBuff
{
    Infinity,
    Normal
}


[System.Serializable]
public struct WallDistance
{
    public Vector2[] LeftWall;
    public Vector2[] RightWall;
    public Vector2[] TopWall;
    public Vector2[] BottomWall;
}

[System.Serializable]
public struct BossTime
{
    public float Infinity;
    public float Phase1;
    public float Phase2;
    public float Phase3;
    public float Phase4;
}


public class Boss : MonoBehaviour
{
    public Text BoostText;
    
    public BossState BS;
    public BossBuff BB;
    //애니메이션 관련
    public Animator Ani;

    //보스 공격 카운트
    [Header("보스 피통입니다")]
    public int HitCount;
    
    Player player;

    public bool CompeltePhase;
    public int Phase;
    public int SubPhase;
    int PrevPhase;

    [Header("레이저들을 여기에 넣어주세요")]
    public GameObject[] Lazers;

    [Header("더미 레이저 입니다")]
    public GameObject[] DummyLazers;

    //벽 범위를 지정해주는 함수
    [SerializeField]
    [Header("레이저를 나오게할 범위를 지정해주세요")]
    public WallDistance Walls;

    [SerializeField]
    [Header("페이즈 지속 시간을 설정해주세요")]
    public BossTime Time;

    //레이저를 셋팅하기 위한 값
    public bool LazerSetOn;

    //레이저를 몇번 셋팅할건지 정하는 함수
    public int SetLazerCount;
    public int LazerCount;

    public int MaxPhase;
    CameraMove camera;

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

    //페이즈 4 메테오 관련
    public MeteorManager meteor;

    void Start()
    {
        player = FindObjectOfType<Player>();
        camera = FindObjectOfType<CameraMove>();
        Ani = GetComponent<Animator>();
        meteor = GetComponent<MeteorManager>();
        //가시 생성하기 위한 풀링
        ThornDistance = (Vector2.Distance(Walls.BottomWall[0], Walls.BottomWall[1])) / SetThornCount;
        Thorns.Add(Thorn);
        Thorn.SetActive(false);
        for (int i=1;i< SetThornCount; i++)
        {
            GameObject ThornDummy = Instantiate(Thorn, new Vector2(0, 0), Quaternion.identity);
            ThornDummy.transform.position = new Vector2(Thorn.transform.position.x + (ThornDistance * (i)), Thorn.transform.position.y);
            ThornDummy.SetActive(false);
            ThornDummy.transform.SetParent(this.transform);
            ThornDummy.GetComponent<Thorn>().Shaft.x = ThornDummy.transform.position.x;
            Thorns.Add(ThornDummy);    
        }

        Invoke("Complete", 2.0f);
    }

    //페이즈를 렌덤으로 돌릴 수 있도록 하는 것
    void SetPhase()
    {
        CompeltePhase = false;
        PrevPhase = Phase;
        while(true)
        {
            if(Phase==0)
            {
                
                Phase = Random.Range(1, MaxPhase);               
                SubPhase = 0;
                CheckPhase(Phase);
                break;
            }
            else
            {
                Phase = Random.Range(1, MaxPhase);
                if (PrevPhase != Phase)
                {
                    SubPhase = 0;
                    CheckPhase(Phase);
                    break;
                }
            }
            
        }
      
       
       
    }

    void CheckPhase(int Phase)
    {
        switch (Phase)
        {
            case 1:
                BS = BossState.Attack;
                Ani.SetTrigger("Attack");
                LazerCount = 0;
                LazerSetOn = true;
                break;
            case 2:
                BS = BossState.Attack;
                Ani.SetTrigger("Attack");               
                camera.CameraShake();
                ThornCount = 0;
                ThornSetOn = true;
                break;
            case 3:
                Invoke("Phase03", 0.5f);
                break;
            case 4:
                meteor.SetMeteor();
                Invoke("Phase04", 0.5f);
                break;
        }
    }


    //페이즈 1
    void Phase01()
    {
        OffDummyLazer();
        camera.CameraShake();      
        for (int i=0;i< Lazers.Length;i++)
        {
            Lazers[i].SetActive(true);
        }
      
        if(SubPhase==0)
        {
            Invoke("Complete", Time.Phase1);
        }
        else
        {
            Invoke("SubComplete", Time.Phase1);
        }
    }

    void SetLazer()
    {
        LazerSetOn = false;
        CancelInvoke("SetLazer");
        LazerCount++;
        for (int i = 0; i < DummyLazers.Length; i++)
        {
            DummyLazers[i].SetActive(false);
            int WallPos = Random.Range(1, 4);
            switch (WallPos)
            {
                //위
                case 1:
                    DummyLazers[i].transform.position = new Vector2(Random.Range(Walls.TopWall[0].x, Walls.TopWall[1].x),
                       Random.Range(Walls.TopWall[0].y, Walls.TopWall[1].y));
                    DummyLazers[i].transform.rotation = Quaternion.Euler(0, 0, -180);
                    break;
                //아래
                case 2:
                    DummyLazers[i].transform.position = new Vector2(Random.Range(Walls.BottomWall[0].x, Walls.BottomWall[1].x),
                      Random.Range(Walls.BottomWall[0].y, Walls.BottomWall[1].y));
                    DummyLazers[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                //왼쪽
                case 3:
                    DummyLazers[i].transform.position = new Vector2(Random.Range(Walls.LeftWall[0].x, Walls.LeftWall[1].x),
                      Random.Range(Walls.LeftWall[0].y, Walls.LeftWall[1].y));
                    DummyLazers[i].transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
                //오른쪽
                case 4:
                    DummyLazers[i].transform.position = new Vector2(Random.Range(Walls.RightWall[0].x, Walls.RightWall[1].x),
                      Random.Range(Walls.RightWall[0].y, Walls.RightWall[1].y));
                    DummyLazers[i].transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
            }
            for (int j = 0; j < i; j++)
            {
                if (DummyLazers[i] == DummyLazers[j])
                {
                    i--;
                    break;
                }
            }

            if (!DummyLazers[i].activeSelf)
            {
                DummyLazers[i].SetActive(true);
            }
        }
        LazerSetOn = true;
    }

    void OffDummyLazer()
    {
        for (int i=0;i<DummyLazers.Length;i++)
        {
            DummyLazers[i].SetActive(false);
            Lazers[i].transform.position = DummyLazers[i].transform.position;
            Lazers[i].transform.rotation = Quaternion.Euler(DummyLazers[i].transform.eulerAngles.x
                                            , DummyLazers[i].transform.eulerAngles.y
                                            , DummyLazers[i].transform.eulerAngles.z);
        }
    }

    //페이즈2
    void Phase02()
    {
        if (Thorns.Count-1 != ThornCount )
        {
            
            Invoke("SetThorn", 0.2f);
        }
        else 
        {
            ThornSetOn = false;
            if(SubPhase==0)
            {
                Invoke("Complete", Time.Phase2);
            }
        }
       
    }

    void SetThorn()
    {
        ThornSetOn = false;
        CancelInvoke("SetThorn");
        ThornCount++;
        Thorns[ThornCount].SetActive(true);
        ThornSetOn = true;
    }

    //페이즈3
    void Phase03()
    {
        BB = BossBuff.Infinity;
        SubPhase = 1;
        CheckPhase(1);

        Invoke("Complete", Time.Phase3);
        Invoke("ChangeBossBuff", Time.Infinity);
    }

    //페이즈4
    void Phase04()
    {
        Ani.SetTrigger("Attack");
        meteor.MeteorOn();
        Invoke("Complete", Time.Phase4);
    }

    //마지막 페이즈
    void LastPhase()
    {

    }

    //페이즈가 완료될 때 실행됨
    public void Complete()
    {
        CompeltePhase = true;

        if (Phase == 1)
        {
            for (int i = 0; i < Lazers.Length; i++)
            {
                Lazers[i].SetActive(false);
            }
        }              
    }
    
    //3페이즈 서브 페이즈가 완료될때 실행됨
    public void SubComplete()
    {
        SubPhase = 0;

        for (int i = 0; i < Lazers.Length; i++)
        {
            Lazers[i].SetActive(false);
        }
    }
    
    //보스 상태를 Normal로 바꿈
    void ChangeBossBuff()
    {
        BB = BossBuff.Normal;
    }

    //포스 체력관련
    public void CountCheck()
    {
        if(HitCount == 3)
        {
            MaxPhase++;
        }
        if(HitCount==0)
        {
            BS = BossState.Die;
        }
    }
    
   

    // Update is called once per frame
    void Update()
    {
        if(BS!=BossState.Die)
        {
            BoostText.text = BB.ToString();
            if (CompeltePhase)
            {
                SetPhase();
            }

            if(LazerSetOn)
            {
                if(LazerCount != SetLazerCount)
                {
                    Invoke("SetLazer", 1.0f);
                }
                else
                {
                    LazerSetOn = false;
                    Invoke("Phase01", 1.5f);                       
                }
            }
            if(ThornSetOn)
            {
                Phase02();            
            }
        }
    }
}
