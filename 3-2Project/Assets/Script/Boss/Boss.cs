using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public BossState BS;
    
    //보스의 버프상태
    public BossBuff BB;

    //애니메이션 관련
    //public Animator[] Ani;
    public BossAniManager bossAniManager;
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
    
    //페이즈 2 가시 관련
    public ThornManager thorn;

    //페이즈 3 관련
    bool EclipsOn;
    //public GameObject Ecilps;
    public Animator EclipsAni;

    //페이즈 4 메테오 관련
    public MeteorManager meteor;

    //마지막 페이즈
    public FinalMoon finalMoon;

    public AudioManager Audio;

    void Start()
    {
        player = FindObjectOfType<Player>();
        camera = FindObjectOfType<CameraMove>();
        meteor = GetComponent<MeteorManager>();
        thorn = GetComponent<ThornManager>();
        finalMoon = GetComponent<FinalMoon>();
        bossAniManager = GetComponent<BossAniManager>();
        Audio = FindObjectOfType<AudioManager>();
        Invoke("Complete", 2.0f);
    }

    //페이즈를 렌덤으로 돌릴 수 있도록 하는 것
    void SetPhase()
    {
        CompeltePhase = false;
        PrevPhase = Phase;

        while (true)
        {
            if (Phase == 0)
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
                bossAniManager.AttackOn();
                LazerCount = 0;
                LazerSetOn = true;
                break;
            case 2:
                bossAniManager.AttackOn();
                break;
            case 3:
                Audio.Play(22);
                EclipsOn = true;                
                EclipsAni.SetBool("EclipsOn", true);
                BB = BossBuff.Infinity;
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
        Audio.OnePlay(17);
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
        Audio.OnePlay(16);
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

        if (thorn.Thorns.Count - 1 != thorn.ThornCount)
        {
            thorn.DelaySetThorn(0.1f);
        }
        else
        {
            thorn.ThornSetOn = false;
            if (SubPhase == 0)
            {
                Invoke("Complete", Time.Phase2);
            }
        }     
       
    }

    //페이즈3
    void Phase03()
    {
        BB = BossBuff.Normal;
        SubPhase = 1;
        CheckPhase(1);      
        Invoke("Complete", Time.Phase3);
    }

    //페이즈4
    void Phase04()
    {
        Audio.OnePlay(23);
        for (int i = 0; i < bossAniManager.Ani.Length; i++)
        {
            bossAniManager.Ani[i].SetTrigger("Attack");
        }
        meteor.MeteorOn();
        Invoke("Complete", Time.Phase4);
    }

    //마지막 페이즈
    void LastPhase()
    {
        finalMoon.FinalMoonOn = true;
    }

    //페이즈가 완료될 때 실행됨
    public void Complete()
    {
        if (Phase == 3)
        {
            EclipsAni.SetBool("EclipsOn", false);
        }
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
    
   

    //보스 체력관련
    public void CountCheck()
    {
        if(HitCount !=0)
        {
            bossAniManager.Hit = true;
            Audio.Play(21);
        }

        switch (HitCount)
        {
            case 0:
                BS = BossState.Die;
                for (int i = 0; i < bossAniManager.Ani.Length; i++)
                {
                    bossAniManager.Ani[i].SetTrigger("Die");
                }
                Audio.Play(20);
                camera.CameraShake();
                finalMoon.FinalMoonOn = false;
                finalMoon.ResetMoon();
                break;
            case 1:
                LastPhase();
                break;
            case 3:
                MaxPhase++;
                break;
        }
    }
    
   

    // Update is called once per frame
    void Update()
    {
        if(BS!=BossState.Die)
        {
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

            if (bossAniManager.Ani[0].GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {               
                if (bossAniManager.Ani[0].GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
                {
                    if (Phase==2)
                    {                       
                        thorn.ThornSetOn = true;                       
                        camera.CameraShake();
                    }
                }
                else if (bossAniManager.Ani[0].GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.45f)
                {
                    Audio.OnePlay(18);
                    if(Phase == 2)
                    {
                        thorn.ThornCount = 0;
                    }
                }
            }
        
            if (thorn.ThornSetOn)
            {
                Phase02();               
            }

            if(EclipsOn)
            {               
                if (EclipsAni.GetCurrentAnimatorStateInfo(0).IsName("EclipsOn") && EclipsAni.GetCurrentAnimatorStateInfo(0).normalizedTime>=1f)
                {
                    EclipsOn = false;
                    Invoke("Phase03", 0.5f);
                }
            }

            if(BB==BossBuff.Infinity)
            {
                bossAniManager.InfinityOn = true;
            }
            else
            {
                bossAniManager.InfinityOn = false;
            }           
        }

       
    }
}
