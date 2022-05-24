using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : InteractableObject
{
    [SerializeField] GameObject dialog;
    [Multiline]
    [SerializeField] string[] dialogText;
    GameObject dialogPopup;
    GameObject canvas;

    bool isReading = false;

    protected override void Interaction()
    {
        if(canvas == null)
        {
            canvas = GameObject.Find("Canvas");
        }
        if(!isReading)
        {
            dialogPopup = Instantiate(dialog, canvas.transform).gameObject;
            dialogPopup.GetComponent<DialogUI>().DialogSetting(this.gameObject, dialogText);
            isReading = true;
        }
        else
        {
            if(dialogPopup.GetComponent<DialogUI>().GoNextText())
            {
                isReading = false;
            }
        }
    }
    protected override void UpdateAction()
    {
        if(!isOnPlayer)
        {
            isReading = false;
            Destroy(dialogPopup);
        }
    }
}
