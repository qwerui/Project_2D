using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gacha : InteractableObject
{
    [SerializeField] GameObject GachaPrefab;
    GameObject MainUI;
    GameObject GachaPopup;
    public GachaController Gacha_ctl;
    BoxCollider2D hitBox;
    SpriteRenderer sprite;
    public AudioSource sound;

    bool isUsed = false;

    private void Awake() {
        hitBox = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    protected override void Interaction() //아이템 뽑기 UI출력
    {
        if(MainUI == null)
        {
            MainUI = interactionDirector.MainUI;
            Gacha_ctl = interactionDirector.Gacha.GetComponent<GachaController>();
        }
        if(GachaPopup == null)
        {
            GachaPopup = Instantiate(GachaPrefab, MainUI.transform).gameObject;
            GachaPopup.GetComponent<GachaUI>().ShowGachaUI(this, interactionDirector.transform.parent.parent.gameObject);
        }
    }
    protected override void ExitAction()
    {
        Destroy(GachaPopup);
    }
    public void DisableGacha() //뽑기 비활성화
    {
        sound.PlayOneShot(sound.clip);
        isUsed = true;
        sprite.color = new Color(0.7f, 0.5f, 0.5f);
        Destroy(hitBox);
    }
    public bool GetGachaUsed()
    {
        return isUsed;
    }
    public void DisableLoadedGacha() //로드시 비활성화
    {
        isUsed = true;
        sprite.color = new Color(0.7f, 0.5f, 0.5f);
        Destroy(hitBox);
    }
}
