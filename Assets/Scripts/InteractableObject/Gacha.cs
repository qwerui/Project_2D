using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gacha : InteractableObject
{
    [SerializeField] GameObject GachaPrefab;
    GameObject MainUI;
    GameObject GachaPopup;
    public GachaController Gacha_ctl;

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
}
