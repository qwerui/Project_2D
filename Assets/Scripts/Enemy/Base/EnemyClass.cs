using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyClass : MonoBehaviour
{
    [SerializeField] protected int hp;
    [SerializeField] protected int atk;
    [SerializeField] protected int def;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int experience;
    [SerializeField] protected Animator ani;
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected BoxCollider2D hitbox;
    [SerializeField] protected GameObject player;
    [SerializeField] protected GameObject damageText;
    

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

    protected bool isMoving;
    protected bool isDead = false;
    protected bool isHit = false;

    
    protected abstract void Move(); //적 이동
    protected abstract void Attack(); //적 공격
    protected abstract void Think(); //적 이동 AI
    protected abstract void Chase(); //적 추적
    protected abstract IEnumerator PosDiff(); //적 추적시 방향 전환 속도 조절 (적과 캐릭터의 좌표 차이)

    protected void EnemyInit()
    {
        player = GameObject.Find("Player");
        CreateItem();
    }

    protected void Hit(int damage)
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
            isHit = true;
            StartCoroutine("FalseHit");
        }
    }
    protected IEnumerator Dead()
    {
        ani.SetTrigger("Death");
        isDead = true;
        moveSpeed = 0;
        player.GetComponent<PlayerController>().GainExprience(experience);
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(GetComponent<Rigidbody2D>());
        yield return new WaitForSeconds(1.0f);
        DropItem();
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.tag == "Player"){
            player.GetComponent<PlayerController>().Damaged(this.transform.position, this.atk);
        }

    }
    protected bool SetOnChase(float radius)
    {
        if(Physics2D.OverlapCircle((Vector2)transform.position + hitbox.offset,radius,LayerMask.GetMask("Player")))
            return true;
        else
            return false;
    }
    
    protected void PlatformCheck()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down*(hitbox.size.y+0.5f), new Color(0,1,0));
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down,hitbox.size.y+0.5f,groundLayer);
        if(raycast.collider == null){
            nextMove= nextMove*(-1); 
            CancelInvoke(); //think를 잠시 멈춘 후 재실행
            Invoke("Think",2); 
        }
    }
    protected void WallCheck()
    {
        Vector2 frontVec = hitbox.bounds.center;
        Debug.DrawRay(frontVec,new Vector2(nextMove, 0) * hitbox.size , new Color(0,1,0));
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, new Vector2(nextMove, 0) ,hitbox.size.x,groundLayer);
        if(raycast.collider != null){
            nextMove= nextMove*(-1); 
            CancelInvoke();
        }
    }
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
    bool Chance(float chance)
    {
        int chanceInt = (int)Mathf.Round(chance * 10000000);
        if(Random.Range(1,1000000000)<=chanceInt)
            return true;
        else
            return false;
    }
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
    void DropItem()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-2.0f,2.0f),10.0f),ForceMode2D.Impulse);
        }
        transform.DetachChildren();
    }
    IEnumerator FalseHit()
    {
        yield return new WaitForSeconds(0.3f);
        isHit = false;
        yield return null;
    }
}
