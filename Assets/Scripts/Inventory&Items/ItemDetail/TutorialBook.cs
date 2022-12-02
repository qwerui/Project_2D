using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialBook : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(Item item, bool used) //튜토리얼 책 효과
    {
        SceneManager.LoadScene("GameScene");
    }
}
