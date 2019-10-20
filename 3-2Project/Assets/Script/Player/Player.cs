using System.Collections;
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
    //방향
    bool FacingRight = true;
    //점프
    public float JumpPower;
    public float variableJumpHeightMultiplier = 0.5f; //가변 점프 높이 비율 

    //애니메이션
    public Animator Ani;

    //키 입력을 받기위한 함수
    float x,y;
    float ItemX, ItemY;
    //스프라이트 랜더러
    SpriteRenderer render;

    //리지드 바디
    public Rigidbody2D rigidbody;

    //빛 수치
    public float LightCount;
    public float MaxLightCount = 100f;

    //땅을 체크하기 위함
    public bool IsGround;

    //벽타기
    public Transform WallCheck;
    private bool Touchingwall;
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
    private bool WallSliding;
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
    public GameObject PrismDummy;
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
    public bool StopMove;
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
            ResetPos();
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
            y = Input.GetAxisRaw("Vertical");
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
                Ghost.MakeGhost = true;
                StartCoroutine(ChangeSpeed());
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
        Ani.SetBool("Holding", WallSliding);
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
                    PrismDummy = Instantiate(prism, new Vector2(transform.position.x+3.0f*FacingDirection,transform.position.y), Quaternion.identity);                    
                    ItemKeyDownCount++;
                    break;
                case 1:
                    PrismDummy.transform.rotation = Quaternion.Euler(0, 0, rotation);
                    PrismDummy = null;
                    SetItem = false;
                    ItemKeyDownCount =0;
                    rotation = 0;
                    break;
            }
        }
        if(PrismDummy !=null)
        {
            PrismDummy.transform.Translate(new Vector2(ItemX, ItemY) * Time.deltaTime * 15, Space.World);
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(SetItem && PrismDummy !=null)
            {
                PrismRotate = true;
                rotation += 90;
            }
        }

        if(PrismRotate && PrismDummy !=null)
        {
            if(PrismDummy.transform.eulerAngles.z <= rotation)
            {
                PrismDummy.transform.rotation = Quaternion.Slerp(PrismDummy.transform.rotation, Quaternion.Euler(PrismDummy.transform.eulerAngles.x,
 PrismDummy.transform.eulerAngles.y, rotation), 5.0f * Time.unscaledDeltaTime);
            }
        }
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
        if (WallSliding && !IsGround&&!StopSliding)
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
        else if(!WallOff && !WallClimb && !StopMove)
        {
            rigidbody.velocity = new Vector2(Speed * x, rigidbody.velocity.y);
        }
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!WallSliding&&IsGround)
            {
                PS = PlayerState.Jump;
               
                if (JumpEffect.activeSelf==false)
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
            else if((WallSliding||Touchingwall) && x != 0)
            {
                Ani.SetTrigger("Jump");
                PS = PlayerState.Climb;
                WallSliding = false;
                if(WallDir ==x)
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

        if(Input.GetKeyUp(KeyCode.Space))
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y * variableJumpHeightMultiplier);
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
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = SpawnPoint;
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
        if(collision.gameObject.tag == "Door")
        {
            DoorSwitch Door =collision.GetComponent<DoorSwitch>();
            if(Door.renderer.flipX==false)
            {
                Door.DoorOn();
            }
        }
        if(collision.gameObject.tag == "ZoomPointIn")
        {
            if(camera.ZoomOn)
            {
                camera.ZoomOn = false;
            }
            else
            {
                camera.ZoomOn = true;
            }
        }

        if (collision.gameObject.tag == "ZoomPointOut")
        {
            if (camera.ZoomOn)
            {
                camera.ZoomOn = false;
            }
            else
            {
                camera.ZoomOn = true;
            }
        }

        if (collision.gameObject.tag == "Trap")
        {
            LightCheck(MaxLightCount, false);
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
