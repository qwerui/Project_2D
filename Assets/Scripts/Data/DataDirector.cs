using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDirector : MonoBehaviour
{
    public static DataDirector Instance;
    
    public int stage;
    public int level;
    public int resourceItem;
    public int enemySlain;

    private void Awake() {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
