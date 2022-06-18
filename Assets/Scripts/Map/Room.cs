using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomInfo roomInfo;
    RoomController controller;

    public GameObject EnemySpawn;
    public EnemyGroup enemyGroup;
    private List<GameObject> enemyList;

    private void Awake() {
        enemyList = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(roomInfo.path[0] != true)
        {
            transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        }
        if(roomInfo.path[1] != true)
        {
            transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        }
        if(roomInfo.path[2] != true)
        {
            transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
        }
        if(roomInfo.path[3] != true)
        {
            transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
        }

        for(int i=0;i<EnemySpawn.transform.childCount;i++)
        {
            if(EnemySpawn.transform.GetChild(i).gameObject.tag == "WeakMonster")
            {
                enemyList.Add(enemyGroup.WeakMonster[Random.Range(0,enemyGroup.WeakMonster.Length)]);
            }
            else if(EnemySpawn.transform.GetChild(i).gameObject.tag == "NormalMonster")
            {
                enemyList.Add(enemyGroup.NormalMonster[Random.Range(0,enemyGroup.NormalMonster.Length)]);
            }
            else if(EnemySpawn.transform.GetChild(i).gameObject.tag == "StrongMonster")
            {
                enemyList.Add(enemyGroup.StrongMonster[Random.Range(0,enemyGroup.StrongMonster.Length)]);
            }
            else if(EnemySpawn.transform.GetChild(i).gameObject.tag == "BossMonster")
            {
                GameObject BossSpawn;
                BossSpawn = Instantiate(enemyGroup.BossMonster[Random.Range(0,enemyGroup.BossMonster.Length)],EnemySpawn.transform.GetChild(i)) as GameObject;
                BossSpawn.SetActive(false);
            }
        }
        if(roomInfo.roomType != RoomType.Start)
            transform.GetChild(2).gameObject.SetActive(false);
        else
            RoomInit();
    }
    public void SetRoomInfo(RoomInfo info, RoomController ctl)
    {
        roomInfo = info;
        roomInfo.Width = 50;
        roomInfo.Height = 50;
        controller = ctl;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            controller.SetNextRoom(this);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            controller.SetCurrentRoom();
            HideAll();
        }
    }
    void HideAll()
    {
        for(int i=0;i<EnemySpawn.transform.childCount;i++)
        {
            Transform tempTrans;
            tempTrans = EnemySpawn.transform.GetChild(i);
            if(tempTrans.childCount != 0)
            {
                if(tempTrans.GetChild(0).gameObject.tag == "Enemy")
                {
                    tempTrans.GetChild(0).position = tempTrans.position;
                }
            }
        }
        transform.GetChild(2).gameObject.SetActive(false);
    }
    public void RoomInit()
    {
        transform.GetChild(2).gameObject.SetActive(true);
        for(int i=0;i<enemyList.Count;i++)
        {
            Transform tempTrans;
            tempTrans = EnemySpawn.transform.GetChild(i);
            if(tempTrans.childCount == 0)
            {
                Instantiate(enemyList[i],tempTrans);
            }
            else if(tempTrans.GetChild(0).gameObject.tag != "Enemy")
            {
                for(int j=0;j<tempTrans.childCount;i++)
                {
                    Destroy(tempTrans.GetChild(0).gameObject);
                }
                Instantiate(enemyList[i],tempTrans);
            }
        }
    }
}
