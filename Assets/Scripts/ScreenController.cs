using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenController : MonoBehaviour
{
    Image sr;
    private void Awake()
    {
        sr = GetComponent<Image>();
    }
    public void FadeIn()
    {
        StartCoroutine(ScreenFadeIn());
    }
    public void FadeOut()
    {
        StartCoroutine(ScreenFadeOut());
    }
    IEnumerator ScreenFadeIn()
    {
        for (int i = 0; i < 100; i++)
        {
            float f = i / 100.0f;

            var tempColor = sr.color;
            tempColor.a = f;
            sr.color = tempColor;

            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }
    IEnumerator ScreenFadeOut()
    {
        for (int i = 100; i > 0; i++)
        {
            float f = i / 100.0f;

            var tempColor = sr.color;
            tempColor.a = f;
            sr.color = tempColor;

            yield return new WaitForSeconds(0.01f);
        }
        gameObject.SetActive(false);
        yield return null;
    }
    public void SetAlpha(float alpha)
    {
        var tempColor = sr.color;
        tempColor.a = alpha / 100.0f;
        sr.color = tempColor;
    }
}
