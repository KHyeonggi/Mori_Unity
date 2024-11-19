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

    public bool gameStarted = false; // 게임이 시작되었는지 여부를 나타내는 변수 추가

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("GameManager instance initialized."); // 디버그 로그로 초기화 확인
        }
        else
        {
            Debug.LogWarning("Multiple GameManager instances detected!");
        }
    }

    public void StartGame()
    {
        // 게임 시작 버튼 클릭 시 호출되는 메서드
        gameStarted = true;
        // UI 숨기기 등 다른 시작 관련 로직 처리
    }
    void Start()
    {
        health = maxHealth;
        //Player.gameObject.SetActive(false);
    }
    public void SetPlayer(GameObject player)
    {
        Player = player;

        Debug.Log($"SetPlayer called. Player: {player.name}");

        // 씬 내 모든 Enemy를 찾아 target을 설정합니다.
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.SetTarget(player.GetComponent<Rigidbody2D>());
            Debug.Log($"Enemy target set: {enemy.name} -> {player.name}");
        }

        // 씬 내 모든 BossAI를 찾아 target을 설정합니다.
        BossAI[] bosses = FindObjectsOfType<BossAI>();
        if (bosses.Length == 0)
        {
            Debug.LogWarning("No BossAI instances found in the scene.");
        }
        else
        {
            foreach (BossAI boss in bosses)
            {
                boss.SetTarget(player.GetComponent<Rigidbody2D>());
                Debug.Log($"Boss target set: {boss.name} -> {player.name}");
            }
        }
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

    public void Resume()
    {
        isLive=true;
        Time.timeScale = 1;
    }
    public void GameOver()
    {
        if (!isLive) return; // 이미 게임 오버 상태라면 더 이상 실행하지 않음

        uiResult.SetActive(true); // 게임 오버 UI 활성화
        Debug.Log("Game Over!"); // 게임 오버 상태 로그 출력 (디버깅용)
        Stop(); // 즉시 게임 정지
    }

    public void Stop()
    {
        gameStarted = false;
        isLive = false;
        Time.timeScale = 0; // 게임 멈추기
        Debug.Log("Game stopped. Time.timeScale: " + Time.timeScale); // Time.timeScale 값 확인
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

        isAction = true;
        talkIndex++;

    }
}
