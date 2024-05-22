using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float gameTime;
    public float maxGameTime = 2 *10f;
    public bool isLive = true;

    public PoolManeger pool;
    public Player player;
    public GameObject CoverImage;
    public GameObject IconImage;
    public Spawner spawner;
    public GameObject Player;
    public PauseMenu pauseMenu;
    public GameObject uiResult;
    Animation anim;


    [Header("# Player Info")]
    public int level; //레벨
    public int exp; //경험치
    public int[] nextExp = { 10, 30, 70, 150, 310 }; //경험치 최대량
    public float health;
    public float maxHealth = 100;

    public TalkManager talkManager;
    public GameObject talkPanel;//대화창
    public Text talkText;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;

    public void OnClickButton()
    {
        
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth;
    }

    public GameObject pauseMenuCanvas;
    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }

        if (talkPanel.activeSelf == true)//대화창 열리면 게임 멈춤
        {
            Stop();
        }
        else if (talkPanel.activeSelf == false)//대화창 닫히면 게임 진행
        {
            Resume();
        }
    }

   public void GetExp()
    {
        exp++;//몬스터 처치시 경험치 1제공
        if(exp == nextExp[level]) { //경험치가 현제 레벨 최대 경험치와 같다면
            level++; //레벨 1증가
            exp = 0; //경험치 초기화
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive=true;
        Time.timeScale = 1;
    }


    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.2f);

        uiResult.SetActive(true);
        Stop();
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void Action(GameObject gameObj)
    {
        if (isAction)
        {
            isAction = false;
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
            isAction = true;
            scanObject = gameObj;
            ObjData objData = scanObject.GetComponent<ObjData>();
            Talk(objData.id, objData.isNpc);
        }
        talkPanel.SetActive(isAction);
    }
    void Talk(int id, bool isNpc)
    {
        string talkData = talkManager.GetTalk(id, talkIndex);


        if (isNpc)
        {
            talkText.text = talkData;
        }
        else
        {
            talkText.text = talkData;
        }

    }
}
