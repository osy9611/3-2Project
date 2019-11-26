using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Idle,
    Attack,
    Die,
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
public struct PhaseTime
{
    public float Phase1;
    public float Phase2;
    public float Phase3;
}


public class Boss : MonoBehaviour
{
    public BossState BS;

    //애니메이션 관련
    public Animator Ani;

    //보스 공격 카운트
    [Header("보스 피통입니다")]
    public int HitCount;

    Player player;

    public bool CompeltePhase;
    public int Phase;

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
    public PhaseTime Time;

    //레이저를 셋팅하기 위한 값
    public bool LazerSetOn;

    //레이저를 몇번 셋팅할건지 정하는 함수
    public int SetLazerCount;
    public int LazerCount;

    void Start()
    {
        player = FindObjectOfType<Player>();

        Ani = GetComponent<Animator>();
    }

    //페이즈를 렌덤으로 돌릴 수 있도록 하는 것
    void SetPhase()
    {
        CompeltePhase = false;
        Phase = Random.Range(1, 3);
        //Phase = 1;
        CheckPhase(Phase);      
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
                Phase02();
                break;
            case 3:
                Phase03();
                break;
        }
    }


    void Phase01()
    {
        for (int i=0;i< Lazers.Length;i++)
        {
            Lazers[i].SetActive(true);
        }
      
        Invoke("Complete", Time.Phase1);
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
        for(int i=0;i<DummyLazers.Length;i++)
        {
            DummyLazers[i].SetActive(false);
            Lazers[i].transform.position = DummyLazers[i].transform.position;
            Lazers[i].transform.rotation = Quaternion.Euler(DummyLazers[i].transform.eulerAngles.x
                                            , DummyLazers[i].transform.eulerAngles.y
                                            , DummyLazers[i].transform.eulerAngles.z);
        }
    }

    void Phase02()
    {
        Invoke("Complete", Time.Phase2);
    }

    void Phase03()
    {
        Invoke("Complete", Time.Phase3);
    }

    void LastPhase()
    {

    }

    public void Complete()
    {
        CompeltePhase = true;
        if(Phase == 1)
        {
            for(int i=0;i<Lazers.Length;i++)
            {
                Lazers[i].SetActive(false);
            }
        }
    }

    void CountCheck()
    {
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
            CountCheck();
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
                    Invoke("OffDummyLazer", 1.0f);
                    Invoke("Phase01", 1.5f);
                }
            }
        }
    }
}
