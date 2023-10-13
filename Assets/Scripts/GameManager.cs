using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    //싱글톤 활용, 각종 정보 컨테이너
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameManager();
                instance.stat = new PlayerStatus();
                instance.identified = new IdentifiedItem();
            }
            return instance;
        }
    }
    public PlayerStatus stat;
    public IdentifiedItem identified;
    public PlayerController controller;
}
