using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int stage;
    public int enemySlain;
    public int resourceItem;
    
    public int hp;
    public int maxHp;
    public int hunger;
    public int maxHunger;
    public int atk;
    public int def;
    public int gold;
    public int redBall;
    public int blueBall;
    public int yellowBall;
	public int level;
	public int experience;
	public int maxExperience;

    public Vector3 playerPos;
    public int playerPosIndex;
    
    public int[] roomId;
    public int[] roomType;
    public Vector2[] roomPos;
    public bool[] roomVisited;
    [SerializeField]
    public string[] roomItem;

    public Vector2[] item; //x = itemId, y = item amount
    public int[] equip;
}
