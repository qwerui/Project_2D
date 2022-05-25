using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //객체 선언
    private Animator ani;
    private Rigidbody2D rigid;
    private BoxCollider2D hitBox;
    private SpriteRenderer spriteRenderer;
    private EquipItem WeaponItem;
    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject GameDirector;
    [SerializeField] private GameObject Weapon;
    [SerializeField] private GameObject WeaponSlot;
    [SerializeField] private GameObject damageText;
    [SerializeField] private GameObject LevelUpText;
    PlayerStatus stat;
    GameObject weaponRoot; //손의 위치
    GameObject damageInstance;
    GameObject levelUpInstance;

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
    private bool isDamaged = false;
    private bool isDead = false;

    //기타 변수
    private float noDamage = 0.1f;

    private void Awake() 
    {
        stat  = new PlayerStatus();
        PlayerInit();
    }

    private void Start()
    {
        ani = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        hitBox = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        weaponRoot = transform.GetChild(0).gameObject;
        StartCoroutine("Hungry");
        StartCoroutine("DeadCheck");
    }

    private void Update()
    {
        if(isDead)
            return;
        Move();
        Jump();
        Crouch();
        Attack();    
    }
    private void FixedUpdate() 
    {
        SlopeCheck();
        onGround();
    }

    //플레이어 상태 변수 초기화
    private void PlayerInit()
    {
        stat.setMaxHp(100);
        stat.setHp(100);
        stat.setAtk(0);
        stat.setDef(0);
        stat.setMaxHunger(100);
        stat.setHunger(100);
        stat.setGold(0);
        stat.setRedBall(0);
        stat.setBlueBall(0);
        stat.setYellowBall(0);
        stat.setLevel(1);
        stat.setMaxExperience(10);
        stat.setExperience(0);
    }

    //플레이어 상태 전달 (주로 UI에 사용 ex: 체력 바 등)
    public PlayerStatus GetStat()
    {
        return stat;
    }

    //이동 기능
    private void Move()
    {
            if((moveDirection = Input.GetAxisRaw("Horizontal"))!=0&&!isAttacking)
            {
                if(!(isJumping&&isAttacking)) //점프 공격 중에는 방향 전환 불가
                    this.transform.localScale = new Vector3(moveDirection,1,1);
                isMoving = true;
                if((!isJumping&&!isCrouch)) //점프, 앉기 중에는 대쉬 불가
                    Dash();
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
            
            if(!isDash&&!isDamaged) //대쉬, 피격 중이 아닐 때 일반 이동
            {
                if(!isDamaged)
                {
                    if(isOnSlope&&!isJumping) //경사로 이동
                        rigid.velocity = new Vector2(moveSpeed*slopeNormalPerp.x*-moveDirection,moveSpeed*slopeNormalPerp.y*-moveDirection);
                    else
                        rigid.velocity = new Vector2(moveSpeed * moveDirection, rigid.velocity.y);
                }
                else
                    rigid.velocity = new Vector2(rigid.velocity.x,rigid.velocity.y);
            }
    }

    //점프 기능
    private void Jump()
    {
        if(!isAttacking&&!isCrouch) //공격, 앉기 중에는 점프 불가
        {
            if(Input.GetKeyDown(KeyCode.X)&&!isJumping)//점프 시간으로 높낮이 조절
                jumpCounter = jumpTime;
            else
                jumpCounter-=Time.deltaTime;
            if(Input.GetKey(KeyCode.X))
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

    //공격 기능
    private void Attack()
    {
        if(WeaponSlot.GetComponent<EquipSlotUI>().GetItem() != null)
        {
        if(Input.GetKeyDown(KeyCode.Z)&&!isAttacking)
        {
            WeaponItem = WeaponSlot.GetComponent<EquipSlotUI>().GetItem();
            Weapon.GetComponent<SpriteRenderer>().sprite = WeaponItem.equipItemData.WeaponEffect;
            Weapon.GetComponent<BoxCollider2D>().size = WeaponItem.equipItemData.WeaponHitSize;
            ani.SetTrigger("Attack");
        }
        if(ani.GetCurrentAnimatorStateInfo(0).IsName("attack")) // 공격 출력 중
            isAttacking=true;
        else if(ani.GetCurrentAnimatorStateInfo(0).IsName("jumpAtk")) // 점프 공격 출력 중
            isAttacking=true;
        else if(ani.GetCurrentAnimatorStateInfo(0).IsName("crouchAtk")) // 앉아 공격 출력 중
            isAttacking=true;
        else
            isAttacking=false;

        if(isCrouch)
        {
            Weapon.transform.localPosition = new Vector3(1.1f,0.75f,0);
        }
        else if(isJumping)
        {
            Weapon.transform.localPosition = new Vector3(1.1f,1.75f,0);
        }
        else
        {
            Weapon.transform.localPosition = new Vector3(1.1f,1.7f,0);
        }
        }
    }

    //앉기 기능
    private void Crouch()
    {
        if(!isJumping&&!isAttacking){
            if(Input.GetKey(KeyCode.DownArrow))
            {   
                moveSpeed = 0.0f; // 이동 속도 0, 방향 전환 가능 하도록 함
                isCrouch = true;
                hitBox.offset = new Vector2(0,0.5f);
                hitBox.size = new Vector2(1,0.9f);
            }
            else
            {   
                if(!isAttacking) // 공격 중일 경우 애니메이션과 실제 히트박스의 차이를 없앰
                {
                    moveSpeed = 5.0f; // 키를 떼면 속도 원상 복귀
                    isCrouch = false;
                    hitBox.offset = new Vector2(0,1.0f);
                    hitBox.size = new Vector2(1,1.9f);
                }
            }
            ani.SetBool("Crouch", isCrouch);
        }
    }

    //플레이어가 바닥 혹은 경사에 있는지 감지
    private void onGround()
    {
        float extraHeightText = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, new Vector2(1f, 0.1f), 0f, Vector2.down,extraHeightText, groundLayer);
        if(raycastHit.collider != null) // 감지 시
            isJumping = false;
        else
            isJumping = true;
        ani.SetBool("Jump", isJumping); // 이거 없으면 착지 후 애니메이션이 안 넘어감
    }

    //대쉬 기능
    private void Dash()
    {
        if(Input.GetKeyDown(KeyCode.C)&&!isDash)
        {
            currentDashTimer = startDashTimer;
            rigid.velocity = Vector2.zero;
            isDash = true;

            //대쉬 하면 공복치 감소
            stat.setHunger(stat.getHunger()-5);
        }
        if(isDash)
        {
            rigid.velocity = transform.right * moveDirection * dashSpeed;
            currentDashTimer -= Time.deltaTime;
            Physics2D.IgnoreLayerCollision(13,25, true);
            if(currentDashTimer <= 0)
            {
                rigid.velocity = Vector2.zero;
                Physics2D.IgnoreLayerCollision(13,25, false);
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

    //피격 판정
    public void Damaged(Vector2 enemyPosition, int damage)
    {
        if(!isDead) //플레이어가 죽어도 피격되는 현상 제거
        {
            ani.SetTrigger("Damaged"); //피격 애니메이션 트리거
            isDamaged = true; //피격 동작 중 (조작 불능)

            //체력 감소
            int finalDamage = damage - stat.getDef();
            if(finalDamage<=0)
                finalDamage = 0;
            stat.setHp(stat.getHp() - finalDamage);

            //데미지 표기
            damageInstance = Instantiate(damageText).gameObject;
            damageInstance.transform.position = transform.position + Vector3.up;
            damageInstance.GetComponent<DamageText>().damage = finalDamage;

            //넉백
            rigid.velocity = Vector2.zero;
            if(enemyPosition.x > transform.position.x)
                rigid.AddForce(new Vector2(-5.0f,10.0f), ForceMode2D.Impulse);
            else
                rigid.AddForce(new Vector2(5.0f,10.0f), ForceMode2D.Impulse);
        
            //적과 충돌 방지로 무적 구현
            Physics2D.IgnoreLayerCollision(13,25, true);
            StartCoroutine("Unbeatable");
        }
        else
        {
            Physics2D.IgnoreLayerCollision(13,25, true); //플레이어가 죽었을 때 적과 충돌 방지
        }
    }

    //무적 판정
    private IEnumerator Unbeatable()
    {
        int count = 0;
        while(count < 10)
        {
            //무적 시간의 반 만큼 조작 가능(count의 2분의 1)
            if(count >= 5)
                isDamaged = false;
            
            //무적 시간중 깜빡거림
            if(count % 2 == 0)
                spriteRenderer.color = new Color32(255,255,255,90);
            else
                spriteRenderer.color = new Color32(255,255,255,255);

            yield return new WaitForSeconds(noDamage); // 무적시간은 noDamage의 10배
            count++;
        }
        spriteRenderer.color = new Color32(255,255,255,255); //투명도 원상 복귀
        Physics2D.IgnoreLayerCollision(13,25, false); //피격 가능
        yield return null;
    }

    // 공복치 감소
    private IEnumerator Hungry()
    {
        while(true)
        {
            stat.setHunger(stat.getHunger()-1);
            yield return new WaitForSeconds(5.0f); // 공복치 감소 주기
        }
    }

    // 플레이어 죽음
    private IEnumerator DeadCheck()
    {
        while(true)
        {
            if(stat.getHp() <= 0 || stat.getHunger() <= 0)
            {
                isDead = true;
                isCrouch = false;
                isJumping = false;
                isMoving = false;
                
                rigid.velocity = Vector2.zero;
                ani.SetTrigger("Dead");
                yield return new WaitForSeconds(2);
                GameDirector.GetComponent<GameDirector>().GameOver();
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void GainExprience(int experience)
    {
        experience += stat.getExperience();
        int levelCount = 0;
        while(experience >= stat.getMaxExperience())
        {
            experience-=stat.getMaxExperience();
            LevelUp();
            levelCount++;
        }
        if(levelCount != 0)
            StartCoroutine("ShowLevelUp", levelCount);
        stat.setExperience(experience);
    }
    private void LevelUp()
	{
        int level = stat.getLevel() + 1;
		stat.setLevel(level);
		stat.setAtk(stat.getAtk() + 1);
		stat.setMaxHp(stat.getMaxHp() + 5);
		if(level % 5 == 0)
		{
			stat.setMaxHunger(stat.getMaxHunger() + 5);
			stat.setDef(stat.getDef() + 1);
		}
		stat.setMaxExperience(level*(level + 2) + 10);
	}
    IEnumerator ShowLevelUp(int levelCount)
    {
        for(int i=0;i<levelCount;i++)
        {
            levelUpInstance = Instantiate(LevelUpText).gameObject;
            levelUpInstance.transform.position = transform.position + Vector3.up;
            yield return new WaitForSeconds(0.2f);
        }
    }
}