using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class anold : InteractableObject
{
    // Start is called before the first frame update
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
    protected override void ExitAction()
    {
        isReading = false;
        Destroy(dialogPopup);
        Destroy(gameObject);
        SceneManager.LoadScene(3);

    }   
}
