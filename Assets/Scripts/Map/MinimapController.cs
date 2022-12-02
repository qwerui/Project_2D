using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    RectTransform rect;
    public bool minimapOn;
    private void Start() {
        rect = GetComponent<RectTransform>();
    }
    //미니맵 토글
    public void MinimapOnOff(bool toggle)
    {
        if(toggle)
        {
            rect.anchoredPosition = new Vector2(-155, 130);
            minimapOn = true;
        }
        else
        {
            rect.anchoredPosition = new Vector2(-25,10);
            minimapOn = false;
        }
    }
}
