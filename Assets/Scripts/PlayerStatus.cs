using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus
{
    //플레이어 상태 변수
    int hp;
    int maxHp;
    int hunger;
    int maxHunger;
    int atk;
    int def;
    int gold;
    int redBall;
    int blueBall;
    int yellowBall;
    float noDamage;

    //Getter, Setter
	public int getHp() 
    {
		return this.hp;
	}

	public void setHp(int hp) 
    {
		this.hp = hp;
	}

	public int getMaxHp() 
    {
		return this.maxHp;
	}

	public void setMaxHp(int maxHp) 
    {
		this.maxHp = maxHp;
	}

	public int getHunger() 
    {
		return this.hunger;
	}

	public void setHunger(int hunger) 
    {
		this.hunger = hunger;
	}

    public int getMaxHunger() 
    {
		return this.maxHunger;
	}

	public void setMaxHunger(int maxHunger) 
    {
		this.maxHunger = maxHunger;
	}

	public int getAtk() 
    {
		return this.atk;
	}

	public void setAtk(int atk) 
    {
		this.atk = atk;
	}

	public int getDef() 
    {
		return this.def;
	}

	public void setDef(int def) 
    {
		this.def = def;
	}

	public int getGold() 
    {
		return this.gold;
	}

	public void setGold(int gold) 
    {
		this.gold = gold;
	}

	public int getRedBall() 
    {
		return this.redBall;
	}

	public void setRedBall(int redBall) 
    {
		this.redBall = redBall;
	}

	public int getBlueBall() 
    {
		return this.blueBall;
	}

	public void setBlueBall(int blueBall) 
    {
		this.blueBall = blueBall;
	}

	public int getYellowBall() 
    {
		return this.yellowBall;
	}

	public void setYellowBall(int yellowBall) 
    {
		this.yellowBall = yellowBall;
	}

	public float getNoDamage() 
    {
		return this.noDamage;
	}

	public void setNoDamage(float noDamage) 
    {
		this.noDamage = noDamage;
	}
}