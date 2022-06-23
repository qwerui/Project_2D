using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGroup", menuName = "Project_2D/EnemyGroup", order = 7)]
public class EnemyGroup : ScriptableObject {
    public GameObject[] WeakMonster;
    public GameObject[] NormalMonster;
    public GameObject[] StrongMonster;
    public GameObject[] BossMonster;
}