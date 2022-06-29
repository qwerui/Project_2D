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

    PlayerStatus stat;
    DataDirector data;
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
        }
    }
    private void Start() {
        stat = player.GetComponent<PlayerController>().GetStat();
        roomList = new List<RoomInfo>();
    }
    public void SaveGameData()
    {
        GameData saveData = new GameData();
        roomList = room.GetComponent<RoomController>().GetRoomList();
        SaveDataDirector(saveData);
        SaveStat(saveData);
        SaveRoom(saveData);
        JsonDirector.SaveGameData(saveData);
        StartCoroutine("FadeIn");
    }
    void SaveDataDirector(GameData gameData)
    {
        gameData.stage = data.stage;
        gameData.enemySlain = data.enemySlain;
        gameData.resourceItem = data.resourceItem;
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
    IEnumerator FadeIn()
    {
        FadeScreen.SetActive(true);
        Image sr;
        sr = FadeScreen.GetComponent<Image>();

        for(int i=0;i<100;i++)
        {
            float f = i / 100.0f;

            var tempColor = sr.color;
            tempColor.a = f;
            sr.color = tempColor;
            
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainScene");
        yield return null;
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
}
