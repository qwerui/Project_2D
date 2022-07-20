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

    private void Start() {
        hitBox = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    protected override void Interaction()
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
    public void DisableGacha()
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
    public void DisableLoadedGacha()
    {
        isUsed = true;
        sprite.color = new Color(0.7f, 0.5f, 0.5f);
        Destroy(hitBox);
    }
}
