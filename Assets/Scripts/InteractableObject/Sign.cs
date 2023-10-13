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
            canvas = interactionDirector.MainUI;
        }
        if(!isReading) //최초 상호작용시 대화창 출력
        {
            dialogPopup = Instantiate(dialog, canvas.transform).gameObject;
            dialogPopup.GetComponent<DialogUI>().DialogSetting(this.gameObject, dialogText);
            isReading = true;
        }
        else // 이후 상호작용시 다음 대화창 출력
        {
            if(dialogPopup.GetComponent<DialogUI>().GoNextText())
            {
                isReading = false;
            }
        }
    }
    protected override void ExitAction() //플레이어 벗어나면 대화창 사라짐
    {
        isReading = false;
        Destroy(dialogPopup);
    }
}
