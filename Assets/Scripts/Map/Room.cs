using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomInfo roomInfo;
    public RoomController controller;

    public GameObject EnemySpawn;
    public EnemyGroup enemyGroup;
    private List<GameObject> enemyList;

    public int roomIndex;

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
        if(DataDirector.Instance.isLoadedGame)
        {
            if(roomInfo.roomType == RoomType.ItemShop)
            {
                SetItem();
            }
            if(roomIndex == DataDirector.Instance.playerPosIndex)
            {
                RoomInit();
            }
            else
            {
                transform.GetChild(2).gameObject.SetActive(false);
            }
        }
        else
        {
            if (roomInfo.roomType != RoomType.Start)
                transform.GetChild(2).gameObject.SetActive(false);
            else
                RoomInit();
        }
        
    }
    public void SetRoomInfo(RoomInfo info, RoomController ctl, int index)
    {
        roomInfo = info;
        roomInfo.Width = 50;
        roomInfo.Height = 50;
        controller = ctl;
        roomIndex = index;
    }
    
    public void HideAll()
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
            if(tempTrans.childCount != 0)
            {
                Destroy(tempTrans.GetChild(0).gameObject);
            }
            Instantiate(enemyList[i], tempTrans);
        }
    }
    public string GetRoomItemId()
    {
        Transform itemPosGroup = transform.GetChild(2).GetChild(1);
        if(itemPosGroup.childCount == 0)
        {
            return "0,0";
        }
        string roomItem = "";
        
        for(int i=0;i<itemPosGroup.childCount;i++)
        {
            if(itemPosGroup.GetChild(i).childCount != 0)
            {
                roomItem+=itemPosGroup.GetChild(i).GetChild(0).GetComponent<ItemPrefab>().data.ID.ToString();
            }
            else
            {
                roomItem += "0";
            }
            if(i==itemPosGroup.childCount-1)
            {
                break;
            }
            roomItem += ",";
        }
        return roomItem;
    }
    void SetItem()
    {
        DataController data = GameObject.Find("DataController").GetComponent<DataController>();
        if(data.GetGachaUsed(roomIndex))
        {
            foreach(Gacha g in transform.GetChild(2).GetChild(2).GetComponentsInChildren<Gacha>())
            {
                g.DisableLoadedGacha();
            }
        }
        string shopStr = data.GetShopList(roomIndex);
        string[] shopList = shopStr.Split('/');
        int index = 0;
        foreach(Shop s in transform.GetChild(2).GetChild(2).GetComponentsInChildren<Shop>())
        {
            string[] shopItem = shopList[index].Split(',');
            for(int i=0;i<8;i++)
            {
                int itemId = int.Parse(shopItem[i]);
                if(itemId == 0)
                {
                    s.shopItem[i]=null;
                }
                else
                {
                    s.shopItem[i]=ItemLoader.Instance.GetSubItem(itemId);
                }
            }
        }
    }
}
