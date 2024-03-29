using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyClass : MonoBehaviour //적 기본 클래스
{
    [SerializeField] protected int hp;
    [SerializeField] protected int atk;
    [SerializeField] protected int def;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int experience;
    [SerializeField] protected Animator ani;
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Collider2D hitbox;
    [SerializeField] protected GameObject player;
    [SerializeField] protected GameObject damageText;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip[] clip;
    /*
     * 0 : 피격음
     * 1 : 사망음
     */

    //아이템 확률 입력은 백분율로 입력, 최소 1e-07 (0.00000001), 최대 100
    [SerializeField] protected GameObject[] dropItem; //아이템 객체
    [SerializeField] protected float[] dropChance; //아이템 드랍 확률 입력
    [SerializeField] protected ResourceItemGroup resourceItem; //자원 객체
    [Tooltip("0: 빨강최소, 1: 빨강최대, 2: 파랑최소, 3: 파랑최대, 4: 노랑최소, 5: 노랑최대, 6: 골드최소, 7: 골드최대")]
    [SerializeField] protected int[] rangeOfResource = new int[8]; //골드 등의 자원 드랍 개수
    [Tooltip("0: 빨강, 1: 파랑, 2: 노랑, 3: 골드")]
    [SerializeField] protected float[] chanceOfResource = new float[4]; //자원 드랍 확률 입력

    protected int nextMove;
    protected float posDiff;
    protected GameObject item;
    protected GameObject damageInstance;
    protected DataDirector data;

    protected bool isMoving;
    protected bool isDead = false;
    protected bool isHit = false;
    protected bool isAttack = false;

    
    protected abstract void Move(); //적 이동
    protected abstract void Attack(); //적 공격
    protected abstract void Think(); //적 이동 AI
    protected abstract void Chase(); //적 추적
    protected abstract IEnumerator PosDiff(); //적 추적시 방향 전환 속도 조절 (적과 캐릭터의 좌표 차이)

    protected virtual void StatusInit(){}

    protected void EnemyInit()
    {
        player = GameObject.Find("Player");
        data = DataDirector.Instance;
        StatusInit();
        CreateItem();
    }
    //피격
    public void Hit(int damage)
    {
        damageInstance = Instantiate(damageText).gameObject;
        damageInstance.transform.position = transform.position;
        int finalDamage = damage - def;
        if(finalDamage<=0)
            finalDamage = 0;
        damageInstance.GetComponent<DamageText>().damage = finalDamage;
        hp -= finalDamage;
        if(hp<=0)
            StartCoroutine("Dead");
        else
        {
            rigid.velocity = Vector2.zero;
            audioSource.PlayOneShot(clip[0]);
            isHit = true;
            StartCoroutine("FalseHit");
        }
    }
    //사망
    protected IEnumerator Dead()
    {
        ani.SetTrigger("Death");
        audioSource.PlayOneShot(clip[1]);
        isDead = true;
        moveSpeed = 0;
        data.enemySlain += 1;
        player.GetComponent<PlayerController>().GainExprience(experience);
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(GetComponent<Rigidbody2D>());
        yield return new WaitForSeconds(1.0f);
        DropItem();
        Destroy(gameObject);
    }
    //몸통박치기 공격
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.tag == "Player"){
            player.GetComponent<PlayerController>().Damaged(this.transform.position, this.atk);
        }

    }
    //플레이어 추적
    protected bool SetOnChase(float radius)
    {
        if(Physics2D.OverlapCircle((Vector2)transform.position + hitbox.offset,radius,LayerMask.GetMask("Player")))
            return true;
        else
            return false;
    }
    //바닥 체크
    protected void PlatformCheck()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down*(transform.localScale.y * 1.5f), new Color(0,1,0));
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, transform.localScale.y * 1.5f, groundLayer);
        if(raycast.collider == null){
            nextMove= nextMove*(-1); 
            CancelInvoke(); //think를 잠시 멈춘 후 재실행
            Invoke("Think",2); 
        }
    }
    //벽 체크
    protected void WallCheck()
    {
        Vector2 frontVec = hitbox.bounds.center;
        Debug.DrawRay(frontVec, new Vector2(nextMove * transform.localScale.y, 0) , new Color(0,1,0));
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, new Vector2(nextMove*transform.localScale.y, 0) ,transform.localScale.y,groundLayer);
        if(raycast.collider != null){
            nextMove= nextMove*(-1); 
            CancelInvoke();
        }
    }
    //드랍 아이템 생성
    protected virtual void CreateItem()
    {
        BallAndGold();
        for(int i=0;i<dropItem.Length;i++)
        {
            if(Chance(dropChance[i]))
            {
                item = Instantiate(dropItem[i]).gameObject;
                item.transform.localPosition = transform.position;
                item.SetActive(false);
                item.transform.SetParent(transform);
            }
        }
    }
    //아이템 드랍 결정
    bool Chance(float chance)
    {
        int chanceInt = (int)Mathf.Round(chance * 10000000);
        if(Random.Range(1,1000000000)<=chanceInt)
            return true;
        else
            return false;
    }
    //자원 드랍 양 결정
    void BallAndGold()
    {
        int resourceAmount;
        for(int i=0;i<4;i++)
        {
            if(Chance(chanceOfResource[i]))
            {
                resourceAmount = Random.Range(rangeOfResource[i],rangeOfResource[i+1]+1);
                if(resourceAmount!=0)
                {
                    item = Instantiate(resourceItem.GetResource(i)).gameObject;
                    item.GetComponent<ItemPrefab>().SetDrop(resourceAmount);
                    item.transform.localPosition = transform.position;
                    item.SetActive(false);
                    item.transform.SetParent(transform);
                }
            }
        }
    }
    //아이템 드랍
    void DropItem()
    {
        int childIndex = 0;
        if(transform.childCount != 0)
        {
            if(transform.GetChild(0).gameObject.tag=="EnemyAttack")
            {
                    Destroy(transform.GetChild(0).gameObject);
                    childIndex++;
            }
        }
        for(int i=childIndex;i<transform.childCount;i++)
        {
            GameObject tempItem = transform.GetChild(i).gameObject;
            tempItem.SetActive(true);
            tempItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-2.0f,2.0f),10.0f),ForceMode2D.Impulse);
            Destroy(tempItem, 10f);
        }
        transform.DetachChildren();
    }
    //피격 상태 OFF
    IEnumerator FalseHit()
    {
        yield return new WaitForSeconds(0.7f);
        isHit = false;
        yield return null;
    }
    //공격 끝
    void AttackEnd()
    {
        isAttack=false;
    }
    //소리 출력
    public void PlaySound(int index)
    {
        audioSource.PlayOneShot(clip[index]);
    }
}
