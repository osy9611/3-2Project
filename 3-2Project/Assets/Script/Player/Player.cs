﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum PlayerState
{
    Idle,
    Run,
    Dash,
    Attack,
    Jump,
    D_Jump,
    Sliding,
    Climb,
    Die,
}
public class Player : MonoBehaviour
{
    public PlayerState PS;
    
    //달리는 속도
    public float Speed;
    public float MinSpeed;
    public float MaxSpeed;
    public float DashTime;
    public int DashCount;
    public int PrevDashCount;
    public int MaxDashCount;
    //방향
    bool FacingRight = true;
    //점프
    public float JumpPower;
    public float variableJumpHeightMultiplier = 0.5f; //가변 점프 높이 비율 

    //애니메이션
    public Animator Ani;

    //키 입력을 받기위한 함수
    public float x;
    float ItemX, ItemY;
    //스프라이트 랜더러
    public SpriteRenderer render;

    //리지드 바디
    public Rigidbody2D rigidbody;

    //빛 수치
    public float LightCount;
    public float MaxLightCount = 100f;

    //땅을 체크하기 위함
    public bool IsGround;

    //벽타기
    public Transform WallCheck;
    public bool Touchingwall;
    public Vector2 wallHopeDirection;
    public Vector2 wallJumpDirection;
    public Vector2 wallLeapDirection;
    public float wallHopeForce;
    public float wallJumpForce;
    public float wallLeapForce;
    public float WallCheckDistance;
    private int WallDir;
    public bool WallOff;
    public bool WallClimb;

    //벽 슬라이딩 
    public bool WallSliding;
    public bool StopSliding;
    public float WallSlidingSpeed;
    private int FacingDirection = 1;
    //UI
    public UI ui;

    //바닥을 체크하는 함수
    public Transform GroundCheck;
    public float GroundCheckRaidius;
    public LayerMask WhatIsGround;

    //디딤판 체크를위한 함수
    private bool StepCheck;
    
    public AffterImage Ghost;

    //아이템을 설치하는 함수
    public GameObject prism;
    private int ItemKeyDownCount;
    public bool SetItem;
    public bool PrismRotate;
    public float rotation;

    //스폰 포인트
    public Vector2 SpawnPoint;
    private bool SetSpawnPointOn;

    //이펙트
    public GameObject JumpEffect;
    public GameObject LandEffect;
    public GameObject DieEffect;
    public Transform JumpPos;

    //카메라 함수
    CameraMove camera;

    //벽 각도 체크를 위한 레이케스트
    RaycastHit2D hit;
    public float hitRange;
    public Transform AngleCheck;

    //프리즘
    public List<GameObject> Prisms;
    public int PrismCount;
    int NowPrism;

    //비디오를 실행하는 함수
    public CutSceneManager CutManager;

    // Start is called before the first frame update
    void Start()
    {
        PS = PlayerState.Idle;
        //Ani = GetComponent<Animator>();
        Speed = MinSpeed;
        render = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();

        wallHopeDirection.Normalize();
        wallJumpDirection.Normalize();

        SpawnPoint = transform.position;
        camera = FindObjectOfType<CameraMove>();

        CutManager = FindObjectOfType<CutSceneManager>();
        for (int i=0;i< PrismCount; i++)
        {
            GameObject PrismDummy = Instantiate(prism, new Vector2(0, 0), Quaternion.identity);
            Prisms.Add(PrismDummy);
            Prisms[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PS != PlayerState.Die)
        {
            Move();
            Jump();
            Slide();
            CheckWallSliding();
            SetItems();
            LightCountCheck();
        }
        else if(PS == PlayerState.Die)
        {
            Invoke("ResetPos", 3.0f);
        }
    }

    private void FixedUpdate()
    {
        CheckSurroundings();
    }

    public void Move()
    {
        if(!SetItem)
        {
            x = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            x = 0;
            ItemX = Input.GetAxisRaw("Horizontal");
            ItemY = Input.GetAxisRaw("Vertical");
        }
        if(x!=0)
        {
            if (PS != PlayerState.Jump)
            {
                PS = PlayerState.Run;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if(DashCount != MaxDashCount)
                {
                    DashCount++;
                    StartCoroutine(ChangeSpeed());
                    Ghost.MakeGhost = true;
                }                
            }
            if (FacingRight && x < 0)
            {
                Flip();
            }
            else if (!FacingRight && x > 0)
            {
                Flip();
            }
            Ani.SetBool("Idle", false);
            Ani.SetBool("Run", true);
        }
        else if (x==0)
        {
            Ani.SetBool("Idle", true);
            Ani.SetBool("Run", false);
            if (PS !=PlayerState.Jump)
            {
                PS = PlayerState.Idle;
            }
        }

        Ani.SetBool("IsGround", IsGround);
        Ani.SetFloat("yVelocity", rigidbody.velocity.y);
        Ani.SetBool("Holding", Touchingwall);
        Ani.SetBool("Sliding", WallSliding);
        Ani.SetBool("StopHolding", StopSliding);
    }

    public void SetItems()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            switch (ItemKeyDownCount)
            {
                case 0:
                    SetItem = true;
                    Prisms[NowPrism].GetComponent<Prism>().RazerON = false;
                    Prisms[NowPrism].SetActive(false);
                    Prisms[NowPrism].transform.position = new Vector2(transform.position.x + 3.0f * FacingDirection, transform.position.y);
                    Prisms[NowPrism].transform.rotation = Quaternion.Euler(0, 0, 0);
                   
                    Prisms[NowPrism].SetActive(true);
                    ItemKeyDownCount++;
                    break;
                case 1:
                    SetItem = false;
                    Prisms[NowPrism].transform.rotation = Quaternion.Euler(0, 0, rotation);
                    if(NowPrism!=PrismCount-1)
                    {
                        NowPrism++;
                    }
                    else
                    {
                        NowPrism = 0;
                    }                    
                    ItemKeyDownCount = 0;
                    rotation = 0;
                    break;
            }
        }

        if(SetItem)
        {
            Prisms[NowPrism].transform.Translate(new Vector2(ItemX, ItemY) * Time.deltaTime * 15, Space.World);
        }
       

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(SetItem && Prisms[NowPrism] != null)
            {
                PrismRotate = true;
                rotation += 90;
            }
        }

        if(PrismRotate && Prisms[NowPrism] != null)
        {
            if(Prisms[NowPrism].transform.eulerAngles.z <= rotation)
            {
                Prisms[NowPrism].transform.rotation = Quaternion.Slerp(Prisms[NowPrism].transform.rotation, Quaternion.Euler(Prisms[NowPrism].transform.eulerAngles.x,
 Prisms[NowPrism].transform.eulerAngles.y, rotation), 5.0f * Time.unscaledDeltaTime);
            }
        }
    }

    void ResetCount()
    {
        Prisms[NowPrism].SetActive(true);
    }

    IEnumerator ChangeSpeed()
    {
        if (Speed != MaxSpeed)
        {
            float StoreGravityScale = rigidbody.gravityScale;
            rigidbody.gravityScale = 0;
            Speed = MaxSpeed;
            yield return new WaitForSeconds(DashTime);
          
            rigidbody.gravityScale = StoreGravityScale;
            Speed = MinSpeed;
            if (DashCount == MaxDashCount)
            {
                yield return new WaitForSeconds(1.0f);
                DashCount = 0;
            }
            else
            {
                PrevDashCount = DashCount;
                yield return new WaitForSeconds(0.4f); 
                if(PrevDashCount == DashCount)
                {
                    DashCount = 0;
                    PrevDashCount = 0;
                }

            }
        }       
    }

    void Flip()
    {
        if(!WallSliding)
        {
            FacingDirection *= -1;
            FacingRight = !FacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    void Slide()
    {
        if (WallSliding && !IsGround)
        {
            PS = PlayerState.Sliding;
            float direction = x;
            if (direction == x)
            {
                if (rigidbody.velocity.y < -WallSlidingSpeed)
                {
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, -WallSlidingSpeed);
                }
            }
        }
        else if(!WallOff && !WallClimb)
        {
            rigidbody.velocity = new Vector2(Speed * x, rigidbody.velocity.y);
        }
    }

    public void Jump()
    {
        if(!SetItem)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!WallSliding && IsGround)
                {
                    PS = PlayerState.Jump;

                    if (JumpEffect.activeSelf == false)
                    {
                        JumpEffect.transform.position = JumpPos.position;
                        JumpEffect.SetActive(true);
                    }
                    else
                    {
                        JumpEffect.transform.position = JumpPos.position;
                        JumpEffect.SetActive(false);
                        JumpEffect.SetActive(true);
                    }

                    Ani.SetTrigger("Jump");
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, JumpPower);

                }
                else if (WallSliding && x == 0)
                {
                    StartCoroutine(JumpOff());
                    Ani.SetBool("Idle", true);
                    Vector2 forceToAdd = new Vector2(wallHopeForce * wallHopeDirection.x * -FacingDirection, wallHopeForce * wallHopeDirection.y);
                    rigidbody.velocity = forceToAdd;
                }
                else if ((WallSliding || Touchingwall) && x != 0)
                {
                    Ani.SetTrigger("Jump");
                    PS = PlayerState.Climb;
                    if (WallDir == x)
                    {
                        //StartCoroutine(JumpOff());
                        WallOff = true;
                        Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * x, wallJumpForce * wallJumpDirection.y);
                        rigidbody.velocity = forceToAdd;
                        WallOff = false;
                    }
                    else
                    {
                        WallClimb = true;
                        rigidbody.velocity = new Vector2(wallLeapForce * wallJumpDirection.x * -x, wallJumpDirection.y * wallJumpForce);
                        StartCoroutine(ClimbWall());
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y * variableJumpHeightMultiplier);
            }
        }
      
    }

    IEnumerator JumpOff()
    {
        WallOff = true;      
        yield return new WaitForSeconds(0.5f);
        WallOff = false;
    }

    IEnumerator ClimbWall()
    {
        yield return new WaitForSeconds(0.18f);
        WallClimb = false;
    }

    void CheckWallSliding()
    {
        if(Touchingwall && !IsGround && rigidbody.velocity.y < 0)
        {
            WallSliding = true;
            WallDir = FacingRight ? -1 : 1;
        }
        else
        {
            WallSliding = false;
        }
    }

    void CheckSurroundings()
    {
        IsGround = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRaidius, WhatIsGround);

        if (IsGround)
        {
            if (LandEffect.activeSelf==false)
            {
                LandEffect.SetActive(true);
                LandEffect.transform.position = JumpPos.position;
            }
        }
        else
        {
            LandEffect.SetActive(false);
        }
        Touchingwall = Physics2D.Raycast(WallCheck.position, WallCheck.right,WallCheckDistance, WhatIsGround);
        if(!transform.parent)
        {
            StopSliding = false;
        }
        else
        {
            StopSliding = true;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GroundCheck.position, GroundCheckRaidius);
        Gizmos.DrawLine(transform.position, new Vector3(WallCheck.position.x + WallCheckDistance, transform.position.y, transform.position.z));
    }

    public void LightCheck(float Damage,bool Prism)
    {
        if (LightCount > 0)
        {
            LightCount -= Damage;
            ui.LightNum = (int)LightCount;
        }
        else if (LightCount == 0)
        {
            LightCount = 0;
            ui.LightBar.sprite = ui.Light[0];
            if (!Prism)
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                PS = PlayerState.Die;
                render.color = new Color(render.color.r, render.color.b, render.color.b, 0);
                DieEffect.transform.position = transform.position;
                DieEffect.SetActive(true);
                rigidbody.simulated = false;
            }
        }
    }

    public void LightCountCheck()
    {
        if(LightCount == 0)
        {
            ui.LightBar.sprite = ui.Light[0];
        }
        else if(LightCount >0)
        {
            if((int)(LightCount / 20) == 0)
            {
                ui.LightBar.sprite = ui.Light[1];
            }
            else if((int)(LightCount / 20) + 1<= ui.Light.Count)
            {
                int a = (int)(LightCount / 20) + 1;
                ui.LightBar.sprite = ui.Light[a];
            }
        }
        else if (LightCount < 0)
        {
            LightCount = 0;
            ui.LightBar.sprite = ui.Light[0];
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            PS = PlayerState.Die;
            render.color = new Color(render.color.r, render.color.b, render.color.b, 0);
            DieEffect.transform.position = transform.position;
            DieEffect.SetActive(true);
            rigidbody.simulated = false;
        }

    }

    IEnumerator AttackOn()
    {
        PS = PlayerState.Attack;
        yield return new WaitForSeconds(0.1f);
        PS = PlayerState.Run;
    }
    
    public void ResetPos()
    {
        if(PS==PlayerState.Die)
        {
            transform.position = SpawnPoint;
            //transform.rotation = Quaternion.Euler(0, 0, 0);
            camera.SaveZoomSet();
            render.color = new Color(render.color.r, render.color.b, render.color.b, 1);
            rigidbody.simulated = true;
            DieEffect.SetActive(false);
            PS = PlayerState.Idle;
        }
      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trap")
        {
            LightCheck(MaxLightCount, false);
        }
        if(collision.gameObject.tag == "LightIn"&& SetItem)
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
        if (collision.gameObject.tag == "LightOut" && SetItem)
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
        if (collision.gameObject.tag == "Door")
        {
            if((int)collision.contacts[0].point.y == (int)GroundCheck.transform.transform.position.y)
            {
                DoorSwitch Door = collision.gameObject.GetComponent<DoorSwitch>();
                if (Door.Off == false)
                {
                    Door.DoorOn();
                }
            }
           
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //빛 관련하여 구현
        if (collision.gameObject.tag == "Light")
        {
            if (LightCount < MaxLightCount)
            {
                LightCount += 1;
                collision.gameObject.SetActive(false);
            }
        }
       

        if (collision.gameObject.tag == "Trap")
        {
            LightCheck(MaxLightCount, false);
        }

        if(collision.gameObject.tag == "Item")
        {
            collision.gameObject.SetActive(false);
            collision.enabled = false;
            CutManager.GameSet();
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="SavePoint")
        {
            if(Input.GetKeyDown(KeyCode.W))
            { 
                SpawnPoint = collision.GetComponent<Trap>().SavePointPos;
                if( LightCount<=MaxLightCount)
                {
                    LightCount += 20;
                    camera.StoreDistance = camera.Distance;
                    collision.enabled=false;
                }
               
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SavePoint")
        {
            SetSpawnPointOn = false;
        }
    }

   
}
