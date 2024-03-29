using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataController : MonoBehaviour
{
    GameData gameData;

    public GameObject player;
    public GameObject room;
    public GameObject FadeScreen;
    public GameObject Inventory;
    public GameDirector director;

    public EquipSlotUI weapon;
    public EquipSlotUI armor;
    public EquipSlotUI accesory;

    public GameObject roomObject;

    public QuickSlotUI quickPotion;
    public QuickSlotUI quickWeapon;

    PlayerStatus stat;
    DataDirector data;
    InventoryController inventory;
    List<RoomInfo> roomList;
    PlayerController controller;

    private void Awake()
    {
        GameManager.Instance.stat = new PlayerStatus();
        gameData = new GameData();
        data = DataDirector.Instance;
        GameManager.Instance.identified = new IdentifiedItem();
        if(data.isLoadedGame == true) //게임 이어하기 선택시 데이터 불러오기
        {
            gameData = JsonDirector.LoadGameData();
            data.stage = gameData.stage;
            data.level = gameData.level;
            data.enemySlain = gameData.enemySlain;
            data.resourceItem = gameData.resourceItem;
            data.playerPos = gameData.playerPos;
            data.playerPosIndex = gameData.playerPosIndex;
            GameManager.Instance.identified.effect = gameData.capsuleEffect;
            GameManager.Instance.identified.identified = gameData.capsuleIdentified;
            GetLoadPlayer(GameManager.Instance.stat);
            JsonDirector.DeleteSaveFile();
        }
        else
        {
            GameManager.Instance.identified.Init();
            DataDirector.Instance.Init();
        }
    }
    private void Start() {
        stat = GameManager.Instance.stat;
        controller = player.GetComponent<PlayerController>();
        inventory = Inventory.GetComponent<InventoryController>();
        roomList = new List<RoomInfo>();
        weapon.LoadEquipItem();
        armor.LoadEquipItem();
        accesory.LoadEquipItem();
    }
    //게임 저장
    public void SaveGameData()
    {
        GameData saveData = new GameData();
        controller.StopAllCoroutines();
        roomList = room.GetComponent<RoomController>().GetRoomList();
        SaveDataDirector(saveData);
        SaveItem(saveData);
        SaveStat(saveData);
        SaveRoom(saveData);
        SaveItemRoom(saveData);
        SaveIdentify(saveData);
        JsonDirector.SaveGameData(saveData);
        director.ScreenFadeIn(0);
        Invoke("ReturnMainMenu", 2.0f);
    }
    //게임 진행 상황 저장
    void SaveDataDirector(GameData gameData)
    {
        gameData.stage = data.stage;
        gameData.enemySlain = data.enemySlain;
        gameData.resourceItem = data.resourceItem;
        gameData.playerPos = player.transform.position;
        gameData.playerPosIndex = data.playerPosIndex;
    }
    //알약 식별 저장
    void SaveIdentify(GameData gameData)
    {
        gameData.capsuleEffect = GameManager.Instance.identified.effect;
        gameData.capsuleIdentified = GameManager.Instance.identified.identified;
    }
    //플레이어 상태 저장
    void SaveStat(GameData gameData)
    {
        gameData.hp = stat.getHp();
        gameData.maxHp = stat.getMaxHp();
        gameData.hunger = stat.getHunger();
        gameData.maxHunger = stat.getMaxHunger();
        gameData.atk = stat.getAtk();
        gameData.def = stat.getDef();
        gameData.gold = stat.getGold();
        gameData.redBall = stat.getRedBall();
        gameData.blueBall = stat.getBlueBall();
        gameData.yellowBall = stat.getYellowBall();
	    gameData.level = stat.getLevel();
	    gameData.experience = stat.getExperience();
	    gameData.maxExperience = stat.getMaxExperience();
    }
    //맵 저장
    void SaveRoom(GameData gameData)
    {
        int[] roomId = new int[roomList.Count];
        int[] roomType = new int[roomList.Count];
        Vector2[] roomPos = new Vector2[roomList.Count];
        bool[] roomVisited = new bool[roomList.Count];
        for(int i=0;i<roomList.Count;i++)
        {
            roomId[i] = roomList[i].roomId;
            switch(roomList[i].roomType)
            {
                case RoomType.Start:
                    roomType[i] = 1;
                    break;
                case RoomType.ItemShop:
                    roomType[i] = 2;
                    break;
                case RoomType.Boss:
                    roomType[i] = 3;
                    break;
                default:
                    roomType[i] = 0;
                    break;
            }
            roomPos[i] = roomList[i].pos;
            roomVisited[i] = roomList[i].isVisited;
        }
        gameData.roomId = roomId;
        gameData.roomType = roomType;
        gameData.roomPos = roomPos;
        gameData.roomVisited = roomVisited;

        gameData.roomItem = new string[roomList.Count];

        for(int i=0;i<roomList.Count;i++)
        {
            gameData.roomItem[i]=room.GetComponent<RoomController>().GetRoomItemList(i).ToString();
        }
    }
    //인벤토리, 장비 저장
    void SaveItem(GameData gameData)
    {
        Vector2[] item = new Vector2[inventory.GetExistItemCount()];
        for(int i=0;i<item.Length;i++)
        {
            Item tempItem = inventory.ReturnItem(i);
            if(tempItem.Data.ItemType==ItemType.Potion || tempItem.Data.ItemType==ItemType.Subweapon)
            {
                item[i] = new Vector2(tempItem.Data.ID, (tempItem as SubItem).Amount);
            }
            else
            {
                item[i] = new Vector2(tempItem.Data.ID, 0);
            }
        }
        gameData.item = item;

        int[] equip = new int[3];
        EquipSlotUI[] slot = {weapon, armor, accesory};
        for(int i=0;i<3;i++)
        {
            if(slot[i].GetItem() != null)
            {
                equip[i] = slot[i].GetItem().Data.ID;
                slot[i].RemoveItem();
            }
            else
                equip[i] = 0;
        }
        
        gameData.equip = equip;
        gameData.quickPotion = quickPotion.Index;
        gameData.quickWeapon = quickWeapon.Index;
    }
    //플레이어 상태 불러오가
    public void GetLoadPlayer(PlayerStatus player)
    {
        player.setHp(gameData.hp);
        player.setMaxHp(gameData.maxHp);
        player.setHunger(gameData.hunger);
        player.setMaxHunger(gameData.maxHunger);
        player.setAtk(gameData.atk);
        player.setDef(gameData.def);
        player.setGold(gameData.gold);
        player.setRedBall(gameData.redBall);
        player.setBlueBall(gameData.blueBall);
        player.setYellowBall(gameData.yellowBall);
        player.setLevel(gameData.level);
        player.setExperience(gameData.experience);
        player.setMaxExperience(gameData.maxExperience);
    }
    //맵 불러오기
    public int GetLoadedRoomCount()
    {
        return gameData.roomId.Length;
    }
    public RoomInfo LoadRoomInfo(int index)
    {
        RoomInfo info = new RoomInfo();
        info.roomId = gameData.roomId[index];
        switch(gameData.roomType[index])
        {
            case 1:
                info.roomType = RoomType.Start;
                break;
            case 2:
                info.roomType = RoomType.ItemShop;
                break;
            case 3:
                info.roomType = RoomType.Boss;
                break;
            default:
                info.roomType = RoomType.Normal;
                break;
        }
        info.pos = new Vector2Int((int)gameData.roomPos[index].x, (int)gameData.roomPos[index].y);
        info.isVisited = gameData.roomVisited[index];
        info.path = new bool[4];
        return info;
    }
    //장비 불러오기
    public ItemData LoadEquipItem(ItemType type)
    {
        if(type == ItemType.Weapon)
        {
            if(gameData.equip[0] == 0)
                return null;
            else
                return ItemLoader.Instance.GetEquipItem((int)gameData.equip[0]);
        }
        else if(type == ItemType.Armor)
        {
            if(gameData.equip[1] == 0)
                return null;
            else
                return ItemLoader.Instance.GetEquipItem((int)gameData.equip[1]);
        }
        else if(type == ItemType.Accesory)
        {
            if(gameData.equip[2] == 0)
                return null;
            else
                return ItemLoader.Instance.GetEquipItem((int)gameData.equip[2]);
        }
        else
        {
            return null;
        }
    }
    //인벤토리 불러오기
    public int GetLoadedItemCount()
    {
        return gameData.item.Length;
    }
    public Item LoadItem(int index)
    {
        if(gameData.item[index].y != 0)
        {
            SubItem subitem = ItemLoader.Instance.GetSubItem((int)gameData.item[index].x).CreateItem() as SubItem;
            subitem.SetAmount((int)gameData.item[index].y);
            subitem.SetPlayer(player);
            return subitem;
        }
        else
        {
            EquipItem equipitem = ItemLoader.Instance.GetEquipItem((int)gameData.item[index].x).CreateItem() as EquipItem;
            equipitem.SetPlayer(player);
            return equipitem;
        }
    }
    //메인메뉴 이동
    void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }
    //방 아이템 불러오기
    public string LoadRoomItem(int index)
    {
            return gameData.roomItem[index];
    }
    //아이템 방 저장하기(뽑기 상태, 상점 아이템)
    void SaveItemRoom(GameData gameData)
    {
        List<bool> gachaUsed = new List<bool>();
        List<string> shopList = new List<string>();
        for(int i=0;i<roomObject.transform.childCount;i++)
        {
            Transform tempTrans = roomObject.transform.GetChild(i);
            if (tempTrans.gameObject.name.Contains("Item"))
            {
                bool isUsed = false;
                foreach(Gacha g in tempTrans.GetChild(2).GetChild(2).GetComponentsInChildren<Gacha>())
                {
                    if(g.GetGachaUsed())
                    {
                        
                        isUsed = true;
                    }
                }
                gachaUsed.Add(isUsed);
                string shopItem = "";
                foreach (Shop s in tempTrans.GetChild(2).GetChild(2).GetComponentsInChildren<Shop>())
                {
                    for (int k=0;k<8;k++)
                    {
                        if(s.shopItem[k]==null)
                        {
                            shopItem += "0";
                        }
                        else
                        {
                            shopItem += s.shopItem[k].ID;
                        }
                        if(k==7)
                        {
                            shopItem += "/";
                        }
                        else
                        {
                            shopItem += ",";
                        }
                    }
                }
                shopList.Add(shopItem.Substring(0,shopItem.Length-1));
            }
        }
        gameData.gachaUsed = gachaUsed.ToArray();
        gameData.shopList = shopList.ToArray();
    }
    //뽑기 상태 불러오기
    public bool GetGachaUsed(int roomIndex)
    {
        int index = -1;
        for(int i=0;i<roomObject.transform.childCount;i++)
        {
            if(roomObject.transform.GetChild(i).gameObject.name.Contains("Item"))
            {
                index++;
                if(i==roomIndex)
                {
                    return gameData.gachaUsed[index];
                }
            }
        }
        return false;
    }
    //상점 아이템 불러오기
    public string GetShopList(int roomIndex)
    {
        int index = -1;
        for(int i=0;i<roomObject.transform.childCount;i++)
        {
            if(roomObject.transform.GetChild(i).gameObject.name.Contains("Item"))
            {
                index++;
                if(i==roomIndex)
                {
                    return gameData.shopList[index];
                }
            }
        }
        return null;
    }
    public int GetPotionIndex()
    {
        return gameData.quickPotion;
    }
    public int GetWeaponIndex()
    {
        return gameData.quickWeapon;
    }
}
