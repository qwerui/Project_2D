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
    
    public int stage;
    public int level;
    public int resourceItem;
    public int enemySlain;

    public bool isLoadedGame;

    public DataDirector()
    {
        stage = 1;
        level = 1;
        resourceItem = 0;
        enemySlain = 0;

        isLoadedGame = false;
    }

}
