using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuDirector : MonoBehaviour
{
    int sceneIndex;
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject TutorialPopup;
    [SerializeField] GameObject LoadGamePopup;
    [SerializeField] SoundDirector sound;
    RectTransform arrowPos;

    bool onTutorial;
    bool PopupOn;
    //최초 실행시 필요한 폴더와 파일 생성
    private void Awake() {
        if(!Directory.Exists(Application.persistentDataPath+"/saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath+"/saves/");
        }
        if(!File.Exists(Application.persistentDataPath+"/saves/ranking.json"))
        {
            InitRankingFile();
        }

    }
    void Start()
    {
        arrowPos = arrow.GetComponent<RectTransform>();
        sceneIndex = 0;
        if (PlayerPrefs.GetInt("Tutorial", 1) == 1)
            onTutorial = true;
        else
            onTutorial = false;
    }

    private void Update() {
        GetKeyboard();
        GetKeyPopup();
    }
    private void FixedUpdate() {
        ArrowMove();
    }
    //화살표 출력
    void ArrowMove()
    {
        arrowPos.anchoredPosition = new Vector2(arrowPos.anchoredPosition.x, -160-60*sceneIndex);
    }
    //키보드 입력
    void GetKeyboard()
    {
        if(!PopupOn)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
                if (sceneIndex < 2)
                {
                    sceneIndex++;
                    sound.FxPlay(0);
                }
            if (Input.GetKeyDown(KeyCode.UpArrow))
                if (sceneIndex > 0)
                {
                    sceneIndex--;
                    sound.FxPlay(0);
                }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (sceneIndex == 0)
                    StartBtnDown();
                else if (sceneIndex == 1)
                    SetBtnDown();
                else if (sceneIndex == 2)
                    RecBtnDown();
            }
        }
    }
    //팝업창 활성화 상태의 키보드 입력
    void GetKeyPopup()
    {
        if(TutorialPopup.activeSelf == true)
        {
            if(Input.GetKeyDown(KeyCode.Y))
                LoadTutorial();
            else if(Input.GetKeyDown(KeyCode.N))
            {
                NotGoTutorial();
            }
                
        }
        else if(LoadGamePopup.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Y))
                LoadGame();
            else if (Input.GetKeyDown(KeyCode.N))
            {
                GameStart();
            }
                
        }
    }
    //시작 버튼
    public void StartBtnDown()
    {
        PopupOn = true;

        if (onTutorial)
        {
            TutorialPopup.SetActive(true);
            sound.FxPlay(0);
        }
        else
        {
            if(JsonDirector.CheckSaveFile())
            {
                sound.FxPlay(0);
                LoadGamePopup.SetActive(true);
            }
            else
            {
                GameStart();
            }
        }
    }
    //설정 버튼
    public void SetBtnDown()
    {
        sound.FxPlay(0);
        SceneManager.LoadScene("SettingScene");
    }
    //게임 기록 버튼
    public void RecBtnDown()
    {
        sound.FxPlay(0);
        LoadingSceneManager.LoadScene("RecordScene");
    }
    //튜토리얼 이동
    public void LoadTutorial()
    {
        sound.FxPlay(0);
        LoadingSceneManager.LoadScene("PrologueScene");
    }
    //게임 이어하기
    public void LoadGame()
    {
        sound.FxPlay(0);
        DataDirector.Instance.isLoadedGame = true;
        LoadingSceneManager.LoadScene("GameScene");
    }
    //게임 처음 시작
    public void GameStart()
    {
        sound.FxPlay(0);
        DataDirector.Instance.isLoadedGame = false;
        LoadingSceneManager.LoadScene("GameScene");
    }
    //튜토리얼 팝업에서 아니오 누른경우
    public void NotGoTutorial()
    {
        sound.FxPlay(0);
        TutorialPopup.SetActive(false);
        if(JsonDirector.CheckSaveFile())
        {
            LoadGamePopup.SetActive(true);
        }
        else
        {
            GameStart();
        }
    }
    //게임 종료
    public void ExitGame()
    {
        Application.Quit();
    }
    //랭킹 파일 생성
    void InitRankingFile()
    {
        LocalRanking localRank = new LocalRanking();
        localRank.rank = new int[10];
        localRank.score = new int[10];
        for(int i=0;i<10;i++)
        {
            localRank.rank[i]=i+1;
            localRank.score[i]=localRank.score[i];
        }

        JsonDirector.SaveRanking(localRank);
    }
}
