using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

// GameManager는 게임의 전반적인 상태와 동작을 관리하는 스크립트입니다.
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글턴 패턴을 사용하여 전역 접근 가능

    // 게임 상태 관련 변수
    public float gameTime; // 현재 게임 시간
    public float maxGameTime = 20f; // 게임 최대 시간
    public bool isLive = false; // 게임이 활성 상태인지 여부

    // 외부 연결된 오브젝트와 UI
    public PoolManeger pool; // 풀 매니저 (객체 풀링 관리)
    public Player player; // 플레이어 객체
    public GameObject CoverImage; // 덮개 이미지
    public GameObject IconImage; // 아이콘 이미지
    public Spawner spawner; // 적 스포너
    public GameObject Player; // 플레이어 오브젝트
    public Text UINameText; // NPC 또는 오브젝트 이름 표시 텍스트
    public PauseMenu pauseMenu; // 일시정지 메뉴
    public GameObject uiResult; // 결과 화면 UI
    public Animation anim; // 애니메이션 객체
    public GameObject WeaponeMenu;


    // 플레이어 정보
    [Header("# Player Info")]
    public int level; // 플레이어 레벨
    public int exp; // 현재 경험치
    public int[] nextExp = { 10, 30, 70, 150, 310 }; // 다음 레벨까지 필요한 경험치
    public float health; // 현재 체력
    public float maxHealth = 100; // 최대 체력
    public int State = 0; // 플레이어 상태 (레벨 업 시 증가)

    // 대화 및 퀘스트 시스템
    public TalkManager talkManager; // 대화 매니저
    public QuestManager questManager; // 퀘스트 매니저
    public GameObject talkPanel; // 대화 UI 패널
    public Text talkText; // 대화 내용 표시 텍스트
    public GameObject scanObject; // 상호작용 중인 오브젝트
    public bool isAction; // 현재 대화 중인지 여부
    public int talkIndex; // 대화 진행 상태 인덱스

    // 대화 효과
    private Coroutine typingCoroutine; // 타이핑 효과 코루틴
    private bool isTyping = false; // 타이핑 진행 상태
    public bool gameStarted = false; // 게임 시작 여부

    // 클리어 화면
    public GameObject clearScreen; // 클리어 화면 UI
    public CinemachineVirtualCamera virtualCamera;
    // 싱글턴 초기화
    void Awake()
    {
       
        if (instance == null)
        {
            instance = this; // 인스턴스 할당
            Debug.Log("GameManager instance initialized.");
        }
        else
        {
            Debug.LogWarning("Multiple GameManager instances detected!");
        }
        
    }

    // 게임 시작 처리
    public void StartGame()
    {
        gameStarted = true; // 게임 시작 상태로 설정
        WeaponeMenu.SetActive(true);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    // 게임 시작 시 초기화
    void Start()
    {
        health = maxHealth; // 체력 초기화
        //Player.gameObject.SetActive(false); // 플레이어 비활성화 (테스트용 주석 처리)
    }

    // 플레이어 설정 및 적과 보스의 타겟 할당
    public void SetPlayer(GameObject player)
    {
        Player = player; // 플레이어 오브젝트 설정

        Debug.Log($"SetPlayer called. Player: {player.name}");
        if (virtualCamera != null)
        {
            virtualCamera.Follow = player.transform;
            Debug.Log($"Virtual camera now follows: {player.name}");
        }
        else
        {
            Debug.LogWarning("Virtual camera is not assigned in the inspector.");
        }
        // 모든 Enemy에게 플레이어를 타겟으로 설정
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.SetTarget(player.GetComponent<Rigidbody2D>());
            Debug.Log($"Enemy target set: {enemy.name} -> {player.name}");
        }

        // 모든 BossAI에게 플레이어를 타겟으로 설정
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

    // 일시정지 메뉴 캔버스
    public GameObject pauseMenuCanvas;

    // 매 프레임 호출
    void Update()
    {
        gameTime += Time.deltaTime; // 게임 시간 증가

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime; // 최대 시간을 초과하지 않도록 제한
        }

        // 대화 패널 상태에 따라 게임 중단 및 재개
        if (talkPanel.activeSelf)
        {
            Stop(); // 대화 중이면 게임 중단
        }
        else if (!talkPanel.activeSelf)
        {
            Resume(); // 대화가 끝나면 게임 재개
        }
    }

    // 경험치 획득 및 레벨 업 처리
    public void GetExp()
    {
        exp++;
        if (exp == nextExp[level])
        {
            level++; // 레벨 업
            exp = 0; // 경험치 초기화
            State += 3; // 추가 상태 포인트
        }
    }

    // 게임 재개 처리
    public void Resume()
    {
        gameStarted = true;
        isLive = true; // 게임 활성화
        Time.timeScale = 1; // 시간 흐름 재개
    }

    // 게임 오버 처리
    public void GameOver()
    {
        if (!isLive) return; // 이미 게임 오버 상태면 중단

        uiResult.SetActive(true); // 결과 화면 활성화
        Debug.Log("Game Over!");
        Stop(); // 게임 중단

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    // 게임 중단 처리
    public void Stop()
    {
        gameStarted = false; // 게임 시작 상태 해제
        isLive = false; // 게임 비활성화
        Time.timeScale = 0; // 시간 정지
        Debug.Log("Game stopped. Time.timeScale: " + Time.timeScale);
    }

    // 게임 재시작 처리
    public void GameRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 다시 로드
        Time.timeScale = 1; // 시간 흐름 재개
    }

    // 상호작용 처리
    public void Action(GameObject gameObj)
    {
        scanObject = gameObj; // 스캔 대상 설정
        string objectName = scanObject.name;
        ObjData objData = scanObject.GetComponent<ObjData>();

        UINameText.text = objectName; // 오브젝트 이름 표시
        Talk(objData.id, objData.isNpc); // 대화 시작

        talkPanel.SetActive(isAction); // 대화 패널 활성화
    }

    // 대화 진행 처리
    void Talk(int id, bool isNpc)
    {
        int questTalkIndex = questManager.GetQuestTalkIndex(id); // 퀘스트 대화 인덱스 가져오기
        string talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex); // 대화 데이터 가져오기
        string npcName = talkManager.GetName(id, talkIndex); // NPC 이름 가져오기

        if (talkData == null)
        {
            isAction = false; // 대화 종료
            talkIndex = 0; // 대화 인덱스 초기화
            Debug.Log(questManager.CheckQuest(id)); // 퀘스트 완료 여부 확인
            return;
        }

        if (isNpc)
        {
            UINameText.text = npcName; // NPC 이름 표시
            talkText.text = talkData; // 대화 내용 표시
        }
        else
        {
            UINameText.text = "모리"; // 기본 이름 표시
            talkText.text = talkData;
        }

        // 타이핑 효과 처리
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeEffect(talkData));

        isAction = true; // 대화 진행 중 상태
        talkIndex++; // 대화 인덱스 증가
    }

    // 타이핑 효과 코루틴
    IEnumerator TypeEffect(string talkData)
    {
        talkText.text = ""; // 초기화
        isTyping = true;

        foreach (char letter in talkData.ToCharArray())
        {
            talkText.text += letter; // 한 글자씩 출력
            yield return new WaitForSecondsRealtime(0.02f); // 출력 속도
        }

        yield return new WaitForSecondsRealtime(1f); // 대화 유지 시간

        isTyping = false;
        typingCoroutine = null; // 코루틴 초기화
    }

    // 보스 처치 후 처리
    public void BossDefeated()
    {
        if (isLive)
        {
            isLive = false; // 게임 비활성화
            clearScreen.SetActive(true); // 클리어 화면 표시
            Time.timeScale = 0; // 시간 정지
            Debug.Log("Boss defeated! Game Clear!");

            AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
        }
    }
    public void TakeDamage(float damage)
    {   
        SpriteRenderer playerSprite =player.GetComponent<SpriteRenderer>();
        playerSprite.color = new Color(1, 1, 1, 0.5f);
        health -= damage;
        Debug.Log($"Player health: {health}");
        if (health <= 0)
        {
            if (isLive) // 처음 죽었을 때만 실행
            {
                isLive = false; // 플레이어 사망 상태로 변경
               
                GameOver();
            }
        }
        Invoke("TakeDamage", 3f);
        playerSprite.color = new Color(1, 1, 1, 1);
    }
}
