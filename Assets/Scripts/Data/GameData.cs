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
    
    public int[] roomId;
    public int[] roomType;
}
