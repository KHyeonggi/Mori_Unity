using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public Text UINameText;
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
    public QuestManager questManager;
    public GameObject talkPanel;//대화창
    public Text talkText;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;

    private Coroutine typingCoroutine;
    private bool isTyping = false; // 타이핑 중인지 여부

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth;
        Player.gameObject.SetActive(false);
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
            IncreaseBulletDamage();
        }
    }
    void IncreaseBulletDamage()
    {
        Bullet bulletComponent = player.GetComponentInChildren<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.damage += 5;
            Debug.Log("Bullet damage increased by 5. New damage: " + bulletComponent.damage);
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

        scanObject = gameObj;
        string objectName = scanObject.name;
        ObjData objData = scanObject.GetComponent<ObjData>();

        UINameText.text = objectName; // 여기서 이름을 UI에 설정
        Talk(objData.id, objData.isNpc);
        
        talkPanel.SetActive(isAction);
    }
    void Talk(int id, bool isNpc)
    {
        int questTalkIndex = questManager.GetQuestTalkIndex(id);
        string talkData = talkManager.GetTalk(id+ questTalkIndex,talkIndex);
        string npcName = talkManager.GetName(id, talkIndex);

        if (talkData == null)
        {
            isAction = false; //대화가 끝나면 대화 종료
            talkIndex = 0;
            Debug.Log(questManager.CheckQuest(id));
            return;
        }

        if (isNpc)
        {
            UINameText.text = npcName; // NPC 이름을 UI에 표시
            talkText.text = talkData;
        }
        else
        {
            UINameText.text = "모리"; // 플레이어의 이름 설정
            talkText.text = talkData;
        }
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeEffect(talkData));

        isAction = true;
        talkIndex++;
    }
    // 타이핑 효과 코루틴
    IEnumerator TypeEffect(string talkData)
    {
        talkText.text = ""; // 기존 텍스트 초기화
        isTyping = true; // 타이핑 중으로 설정

        foreach (char letter in talkData.ToCharArray())
        {
            talkText.text += letter; // 한 글자씩 추가
            yield return new WaitForSecondsRealtime(0.02f); // 타이핑 속도 조절
        }

        // 타이핑 효과가 끝난 후 1초 대기
        yield return new WaitForSecondsRealtime(1f);

        isTyping = false; // 타이핑이 끝났음을 표시
        typingCoroutine = null; // 코루틴이 완료되었음을 표시
    }
}
