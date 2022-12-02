using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDirector
{
    private static DataDirector instance;
    public static DataDirector Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataDirector();

            }
            return instance;
        }
    }
    //게임 진행 상황
    public int stage;
    public int level;
    public int resourceItem;
    public int enemySlain;

    public Vector3 playerPos;
    public int playerPosIndex;

    public bool isLoadedGame;

    public DataDirector()
    {
        stage = 1;
        level = 1;
        resourceItem = 0;
        enemySlain = 0;

        isLoadedGame = false;
    }
    public void Init()
    {
        stage = 1;
        level = 1;
        resourceItem = 0;
        enemySlain = 0;

        isLoadedGame = false;
    }
}
