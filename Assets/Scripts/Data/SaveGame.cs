using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    GameData gameData;

    public GameObject player;
    public GameObject room;
    public GameObject jsonDirector;

    PlayerStatus stat;
    DataDirector data;
    List<RoomInfo> roomList;

    private void Start() {
        gameData = new GameData();
        stat = player.GetComponent<PlayerController>().GetStat();
        roomList = new List<RoomInfo>();
    }
    public void SaveGameData()
    {
        roomList = room.GetComponent<RoomController>().GetRoomList();
        SaveDataDirector();
        SaveStat();

    }
    void SaveDataDirector()
    {
        gameData.stage = data.stage;
        gameData.enemySlain = data.enemySlain;
        gameData.resourceItem = data.resourceItem;
    }
    void SaveStat()
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
}
