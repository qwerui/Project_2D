using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaUI : MonoBehaviour
{
    //아이템 뽑기 UI
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
    //아이템 수량 감시
    private void FixedUpdate() {
        rect.position = Camera.main.WorldToScreenPoint(pos+Vector3.up * 1.8f);
        redText.text = Mathf.Clamp(int.Parse(redText.text),0,redMax).ToString();
        blueText.text = Mathf.Clamp(int.Parse(blueText.text),0,blueMax).ToString();
        yellowText.text = Mathf.Clamp(int.Parse(yellowText.text),0,yellowMax).ToString();
        goldText.text = Mathf.Clamp(int.Parse(goldText.text),0,goldMax).ToString();
    }
    //UI초기화
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
    //아이템 수량 증가
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
    //키보드로 입력시 자원 투입량 설정
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
    //투입 자원 수량 최소,최대치 고정
    public void SetResourceValue(int value)
    {
        txt.text = Mathf.Clamp(int.Parse(txt.text)+value,0,maxResource).ToString();
    }
    //아이템 뽑기
    public void GachaItem()
    {
        itemOutput = Gacha.Gacha_ctl.ItemGacha(int.Parse(goldText.text),int.Parse(redText.text),int.Parse(blueText.text),int.Parse(yellowText.text));
        GameManager.Instance.stat.setBlueBall(GameManager.Instance.stat.getBlueBall()-int.Parse(blueText.text));
        GameManager.Instance.stat.setRedBall(GameManager.Instance.stat.getRedBall()-int.Parse(redText.text));
        GameManager.Instance.stat.setYellowBall(GameManager.Instance.stat.getYellowBall()-int.Parse(yellowText.text));
        GameManager.Instance.stat.setGold(GameManager.Instance.stat.getGold()-int.Parse(goldText.text));
        GameObject output = Instantiate(itemOutput.DropItemPrefab).gameObject;
        output.transform.position = Gacha.gameObject.transform.position+Vector3.up;
        output.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-2.0f,2.0f), 10.0f), ForceMode2D.Impulse);
        Gacha.DisableGacha();
        Destroy(gameObject);
    }
}
