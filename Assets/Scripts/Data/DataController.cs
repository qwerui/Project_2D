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

    PlayerStatus stat;
    DataDirector data;
    InventoryController inventory;
    List<RoomInfo> roomList;

    private void Awake()
    {
        
        gameData = new GameData();
        data = DataDirector.Instance;
        if(data.isLoadedGame == true)
        {
            gameData = JsonDirector.LoadGameData();
            data.stage = gameData.stage;
            data.level = gameData.level;
            data.enemySlain = gameData.enemySlain;
            data.resourceItem = gameData.resourceItem;
            data.playerPos = gameData.playerPos;
            data.playerPosIndex = gameData.playerPosIndex;
        }
    }
    private void Start() {
        stat = player.GetComponent<PlayerController>().GetStat();
        inventory = Inventory.GetComponent<InventoryController>();
        roomList = new List<RoomInfo>();
    }
    public void SaveGameData()
    {
        GameData saveData = new GameData();
        roomList = room.GetComponent<RoomController>().GetRoomList();
        SaveDataDirector(saveData);
        SaveStat(saveData);
        SaveItem(saveData);
        SaveRoom(saveData);
        JsonDirector.SaveGameData(saveData);
        director.ScreenFadeIn(0);
        Invoke("ReturnMainMenu", 2.0f);
    }
    void SaveDataDirector(GameData gameData)
    {
        gameData.stage = data.stage;
        gameData.enemySlain = data.enemySlain;
        gameData.resourceItem = data.resourceItem;
        gameData.playerPos = player.transform.position;
        gameData.playerPosIndex = data.playerPosIndex;
    }
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
    }
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
    }
    
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
    void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }
}
