using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaUI : MonoBehaviour
{
    RectTransform rect;
    Vector3 pos;
    [SerializeField] InputField redText;
    [SerializeField] InputField blueText;
    [SerializeField] InputField yellowText;
    [SerializeField] InputField goldText;
    InputField txt;
    PlayerStatus stat;
    ItemData itemOutput;
    Gacha Gacha;

    int redMax;
    int blueMax;
    int yellowMax;
    int goldMax;

    int maxResource;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate() {
        rect.position = Camera.main.WorldToScreenPoint(pos+Vector3.up * 1.8f);
        redText.text = Mathf.Clamp(int.Parse(redText.text),0,redMax).ToString();
        blueText.text = Mathf.Clamp(int.Parse(blueText.text),0,blueMax).ToString();
        yellowText.text = Mathf.Clamp(int.Parse(yellowText.text),0,yellowMax).ToString();
        goldText.text = Mathf.Clamp(int.Parse(goldText.text),0,goldMax).ToString();
    }
    public void ShowGachaUI(Gacha _gacha, GameObject _player)
    {
        pos = _gacha.gameObject.transform.position;
        Gacha = _gacha;
        stat = _player.GetComponent<PlayerController>().GetStat();
        redMax = stat.getRedBall();
        blueMax = stat.getBlueBall();
        yellowMax = stat.getYellowBall();
        goldMax = stat.getGold();
    }
    public void IncreaseResources(int txtIndex)
    {

        if(txtIndex == 0)
            txt = redText;
        else if(txtIndex == 1)
            txt = blueText;
        else if(txtIndex == 2)
            txt = yellowText;
        else
            txt = goldText;
        txt.text = (int.Parse(txt.text)+1).ToString();
    }
    public void SetResourceIndex(int txtIndex)
    {

        if(txtIndex == 0)
        {
            txt = redText;
            maxResource = redMax;
        }
        else if(txtIndex == 1)
        {
            txt = blueText;
            maxResource = blueMax;
        }
        else if(txtIndex == 2)
        {
            txt = yellowText;
            maxResource = yellowMax;
        }
        else
        {
            txt = goldText;
            maxResource = goldMax;
        }
    }
    public void SetResourceValue(int value)
    {
        txt.text = Mathf.Clamp(int.Parse(txt.text)+value,0,maxResource).ToString();
    }
    public void GachaItem()
    {
        itemOutput = Gacha.Gacha_ctl.ItemGacha(int.Parse(goldText.text),int.Parse(redText.text),int.Parse(blueText.text),int.Parse(yellowText.text));
        GameObject output = Instantiate(itemOutput.DropItemPrefab).gameObject;
        output.transform.position = Gacha.gameObject.transform.position+Vector3.up;
        output.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-2.0f,2.0f), 10.0f), ForceMode2D.Impulse);
        Gacha.DisableGacha();
        Destroy(gameObject);
    }
}
