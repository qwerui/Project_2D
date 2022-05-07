using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    //객체 선언
    private Animator ani;
    private Rigidbody2D rigid;
    private BoxCollider2D hitBox;
    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private LayerMask groundLayer;

    //플레이어 이동 관련 수치(Inspector에서 조정)
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpSpeed = 5.0f;
    [SerializeField] private float jumpTime = 0.1f;
    [SerializeField] private float dashSpeed = 8.0f;
    [SerializeField] private float startDashTimer = 0.25f;
    [SerializeField] private float maxSlopeAngle = 45.0f;

    //이동 관련 변수
    private float jumpCounter;
    private float moveDirection = 0.0f;
    private float currentDashTimer;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;
    private Vector2 slopeNormalPerp;
    
    
    //상태 bool
    private bool isJumping = false;
    private bool isAttacking = false;
    private bool isCrouch = false;
    private bool isDash = false;
    private bool isMoving = false;
    private bool isOnSlope = false;

    private void Start()
    {
        ani = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        hitBox = GetComponent<BoxCollider2D>();
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        Move();
        Jump();
        Crouch();
        Attack();
        onGround();
    }
    private void FixedUpdate() {
        SlopeCheck();
    }

    private void Move()
    {
            if((moveDirection = Input.GetAxisRaw("Horizontal"))!=0&&!isAttacking)
            {
                    this.transform.localScale = new Vector3(moveDirection,1,1);
                    isMoving = true;
            }
            else
            {
                if(!isJumping) //점프 중에는 이동할 수 있도록 함
                {
                    moveDirection = 0;
                    isMoving = false;
                } 
            }
            if(isOnSlope&&!isMoving)
                rigid.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
            else
                rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            ani.SetBool("Move", isMoving);
            if((!isJumping&&!isCrouch)) //점프, 앉기 중에는 대쉬 불가
                Dash();
            if(!isDash) //대쉬 중이 아닐 때 일반 이동
            {
                if(isOnSlope&&!isJumping) //경사로 이동
                    rigid.velocity = new Vector2(moveSpeed*slopeNormalPerp.x*-moveDirection,moveSpeed*slopeNormalPerp.y*-moveDirection);
                else
                    rigid.velocity = new Vector2(moveSpeed * moveDirection, rigid.velocity.y);
            }
    }

    private void Jump()
    {
        if(!isAttacking&&!isCrouch) //공격, 앉기 중에는 점프 불가
        {
            if(!isJumping) //점프 시간으로 높낮이 조절
                jumpCounter = jumpTime;
            else
                jumpCounter-=Time.deltaTime;
            if(Input.GetKey(KeyCode.X)) // 연타하면 2번 입력 가능성 있지만 0.2초라서 체감 불가
            {
                if(jumpCounter>0)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, jumpSpeed);
                }
            }
            ani.SetBool("Jump",isJumping);
            ani.SetFloat("Velocity_y", rigid.velocity.y);
        }
    }

    private void Attack()
    {
        if(Input.GetKeyDown(KeyCode.Z)&&!isAttacking)
            ani.SetTrigger("Attack");
        if(ani.GetCurrentAnimatorStateInfo(0).IsName("attack")) // 공격 출력 중
            isAttacking=true;
        else if(ani.GetCurrentAnimatorStateInfo(0).IsName("jumpAtk")) // 점프 공격 출력 중
            isAttacking=true;
        else if(ani.GetCurrentAnimatorStateInfo(0).IsName("crouchAtk")) // 앉아 공격 출력 중
            isAttacking=true;
        else
            isAttacking=false;
    }

    private void Crouch()
    {
        if(!isJumping){
            if(Input.GetKey(KeyCode.DownArrow))
            {   
                moveSpeed = 0.0f; // 이동 속도 0, 방향 전환 가능 하도록 함
                isCrouch = true;
                hitBox.offset = new Vector2(0,0.5f);
                hitBox.size = new Vector2(1,1.0f);
            }
            else
            {   
                if(!isAttacking) // 공격 중일 경우 애니메이션과 실제 히트박스의 차이를 없앰
                {
                    moveSpeed = 5.0f; // 키를 떼면 속도 원상 복귀
                    isCrouch = false;
                    hitBox.offset = new Vector2(0,1.0f);
                    hitBox.size = new Vector2(1,2.0f);
                }
            }
            ani.SetBool("Crouch", isCrouch);
        }
    }

    //플레이어가 바닥 혹은 경사에 있는지 감지
    private void onGround()
    {
        float extraHeightText = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(hitBox.bounds.center, hitBox.bounds.size, 0f, Vector2.down,extraHeightText, LayerMask.GetMask("Ground"));
        if(raycastHit.collider != null) // 감지 시
            isJumping = false;
        else
            isJumping = true;
        ani.SetBool("Jump", isJumping); // 이거 없으면 착지 후 애니메이션이 안 넘어감
    }

    //대쉬 기능
    private void Dash()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            currentDashTimer = startDashTimer;
            rigid.velocity = Vector2.zero;
            isDash = true;
        }
        if(isDash)
        {
            rigid.velocity = transform.right * moveDirection * dashSpeed;
            currentDashTimer -= Time.deltaTime;
            if(currentDashTimer <= 0)
            {
                rigid.velocity = Vector2.zero;
                isDash=false;
            }
        }
        ani.SetBool("Dash", isDash);
    }
    //경사로 체크
    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position; 

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }
    //경사로의 각도를 구함
    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, groundLayer);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, groundLayer);

        if (slopeHitFront)
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        else if (slopeHitBack)
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        else
            slopeSideAngle = 0.0f;

        if(slopeSideAngle<=maxSlopeAngle&&slopeSideAngle!=0)
            isOnSlope = true;
        else
            isOnSlope = false;
    }


    private void SlopeCheckVertical(Vector2 checkPos)
    {      
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);

        if (hit)
        {
            //충돌한 레이어의 법선 벡터와 수직인 선을 구함
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;            

            //충돌한 레이어와 벡터 (0,1) 사이의 각도를 구함
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }                       

            lastSlopeAngle = slopeDownAngle;
           
            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue); //충돌한 레이어와 평행
            Debug.DrawRay(hit.point, hit.normal, Color.green); //충돌한 레이어와 수직

        }
    }
}