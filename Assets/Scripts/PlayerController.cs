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
    [SerializeField] private GameObject QuickSlotPotion;
    [SerializeField] private GameObject QuickSlotWeapon;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject DataCtl;
    [SerializeField] private GameObject MiniMap;
    public SoundDirector sound;

    PlayerStatus stat;
    GameObject damageInstance;
    GameObject levelUpInstance;

    QuickSlotUI quickSlotPotion;
    QuickSlotUI quickSlotWeapon;

    DataDirector data;

    //플레이어 이동 관련 수치(Inspector에서 조정)
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpSpeed = 5.0f;
    [SerializeField] private float jumpTime = 0.1f;
    [SerializeField] private float dashSpeed = 8.0f;
    [SerializeField] private float startDashTimer = 0.5f;
    [SerializeField] private float maxSlopeAngle = 45.01f;

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
    public bool isAttacking = false;
    private bool isCrouch = false;
    private bool isDash = false;
    private bool isMoving = false;
    private bool isOnSlope = false;
    private bool isDamaged = false;
    private bool isDead = false;

    //기타 변수
    private float noDamage = 0.1f;
    private int attackPos;

    private void Awake() 
    {
        data = DataDirector.Instance;
        GameManager.Instance.controller = this;
    }

    private void Start()
    {
        if (data.isLoadedGame == true)
        {
            transform.position = data.playerPos;
        }
        stat = GameManager.Instance.stat;
        ani = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        hitBox = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        quickSlotPotion = QuickSlotPotion.GetComponent<QuickSlotUI>();
        quickSlotWeapon = QuickSlotWeapon.GetComponent<QuickSlotUI>();
        if(SceneManager.GetActiveScene().name != "PrologueScene")
        {
            StartCoroutine("Hungry");
            StartCoroutine("DeadCheck");
        }
    }

    private void Update()
    {
        if(isDead)
            return;
        Move();
        Jump();
        Crouch();
        Attack();
        UseQuickSlot();
        UIControll();
    }
    private void FixedUpdate() 
    {
        SlopeCheck();
        onGround();
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
                {
                    this.transform.localScale = new Vector3(moveDirection, 1, 1);
                }
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
            if((!isJumping&&!isCrouch)) //점프, 앉기 중에는 대쉬 불가
                Dash();
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
                    {
                        rigid.velocity = new Vector2(moveSpeed*slopeNormalPerp.x*-moveDirection,moveSpeed*slopeNormalPerp.y*-moveDirection);
                    }
                    else
                    {
                        rigid.velocity = new Vector2(moveSpeed * moveDirection, rigid.velocity.y<=-20?-20:rigid.velocity.y);
                    }
                }
                else
                    rigid.velocity = new Vector2(rigid.velocity.x,rigid.velocity.y);
            }
    }

    //점프 기능
    private void Jump()
    {

        if((!isAttacking&&!isCrouch)&&!isDash) //공격, 앉기, 대시 중에는 점프 불가
        {
            if (Input.GetKeyDown(KeyCode.X) && !isJumping)//점프 시간으로 높낮이 조절
            {
                jumpCounter = jumpTime;
                ani.SetBool("Jump", isJumping);
                sound.PlayerSoundPlay(0);
            }
            else
                jumpCounter -= Time.deltaTime;
            if (Input.GetKey(KeyCode.X))
            {
                if(jumpCounter>0)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, jumpSpeed);
                }
            }
            ani.SetFloat("Velocity_y", rigid.velocity.y);
        }
    }

    //공격 기능
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.UpArrow))
        {
            if (!isAttacking)
            {
                quickSlotWeapon.QuickItemUse();
            }
        }
        else if (WeaponSlot.GetComponent<EquipSlotUI>().GetItem() != null)
        {
            if (Input.GetKeyDown(KeyCode.Z) && !isAttacking)
            {
                WeaponItem = WeaponSlot.GetComponent<EquipSlotUI>().GetItem();
                Weapon.GetComponent<SpriteRenderer>().sprite = (WeaponItem.equipItemData as WeaponItemData).HitBox.WeaponEffect;
                Weapon.GetComponent<BoxCollider2D>().size = (WeaponItem.equipItemData as WeaponItemData).HitBox.WeaponHitSize;
                Weapon.GetComponent<BoxCollider2D>().offset = (WeaponItem.equipItemData as WeaponItemData).HitBox.WeaponOffset;
                ani.SetTrigger("Attack");
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
            }
            else
            {   
                if(!isAttacking)
                {
                    moveSpeed = 5.0f; // 키를 떼면 속도 원상 복귀
                    isCrouch = false;
                }
            }
            ani.SetBool("Crouch", isCrouch);
        }
    }

    //플레이어가 바닥 혹은 경사에 있는지 감지
    private void onGround()
    {
        bool lastJump = isJumping;
        float extraHeightText = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, new Vector2(0.9f, 0.07f), 0f, Vector2.down,extraHeightText, groundLayer);
        if(raycastHit.collider != null) // 감지 시
        {
            if (raycastHit.collider.name == "HalfFloor")
            {
                if (rigid.velocity.y > 0)
                {
                    isJumping = true;
                }
                else
                {
                    isJumping = false;
                }
            }
            else
            {
                isJumping = false;
            }
        }
        else
            isJumping = true;
        if(lastJump == true && lastJump != isJumping)
        {
            sound.PlayerSoundPlay(3);
        }

        ani.SetBool("Jump", isJumping);
    }

    //대쉬 기능
    private void Dash()
    {
        if(Input.GetKeyDown(KeyCode.C)&&!isDash)
        {
            if(Input.GetAxisRaw("Horizontal")!=0) //정지 중에는 대쉬 불가
            {
                currentDashTimer = startDashTimer;
                rigid.velocity = Vector2.zero;
                isDash = true;

                //대쉬 하면 공복치 감소
                stat.setHunger(stat.getHunger()-5);
                sound.PlayerSoundPlay(0);
            }
        }
        if(isDash)
        {
            if(isOnSlope)
            {
                rigid.velocity = new Vector2(slopeNormalPerp.x * -moveDirection, slopeNormalPerp.y * -moveDirection) * dashSpeed;
            }
            else
            {
                rigid.velocity = new Vector2(transform.localScale.x, 0) * dashSpeed;
            }
            currentDashTimer -= Time.deltaTime;
            Physics2D.IgnoreLayerCollision(13,25, true);
            Physics2D.IgnoreLayerCollision(12,25, true);
            if(currentDashTimer <= 0)
            {
                rigid.velocity = Vector2.zero;
                Physics2D.IgnoreLayerCollision(13,25, false);
                Physics2D.IgnoreLayerCollision(12,25, false);
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
        {
            isOnSlope = true;
        }
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
            sound.PlayerSoundPlay(1);

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
            Physics2D.IgnoreLayerCollision(12,25, true);
            if(!isDead)
                StartCoroutine("Unbeatable");
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
        Physics2D.IgnoreLayerCollision(12,25, false);
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
                rigid.bodyType = RigidbodyType2D.Static;
                ani.SetTrigger("Dead");
                Physics2D.IgnoreLayerCollision(13, 25, true);
                Physics2D.IgnoreLayerCollision(12, 25, true);
                sound.PlayerSoundPlay(2);
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
        DataDirector.Instance.level++;
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
            sound.PlayerSoundPlay(4);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void UseQuickSlot()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            quickSlotPotion.QuickItemUse();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            quickSlotWeapon.QuickItemUse();
        }
    }
    private void UIControll()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(inventory.activeSelf == true)
            {
                inventory.SetActive(false);
                sound.FxPlay(1);
            }
            else
            {
                inventory.SetActive(true);
                sound.FxPlay(0);
            }
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            MinimapController mini = MiniMap.GetComponent<MinimapController>();
            mini.MinimapOnOff(!mini.minimapOn);
            sound.FxPlay(0);
        }
    }
    public void ActiveBuff(IEnumerator buff)
    {
        StartCoroutine(buff);
    }
}