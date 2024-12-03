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
    public int level; //����
    public int exp; //����ġ
    public int[] nextExp = { 10, 30, 70, 150, 310 }; //����ġ �ִ뷮
    public float health;
    public float maxHealth = 100;
    public int State = 0;

    public TalkManager talkManager;
    public QuestManager questManager;
    public GameObject talkPanel;//��ȭâ
    public Text talkText;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;

    private Coroutine typingCoroutine;
    private bool isTyping = false; // Ÿ���� ������ ����
    public bool gameStarted = false; // ������ ���۵Ǿ����� ���θ� ��Ÿ���� ���� �߰�

    public GameObject clearScreen;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("GameManager instance initialized."); // ����� �α׷� �ʱ�ȭ Ȯ��
        }
        else
        {
            Debug.LogWarning("Multiple GameManager instances detected!");
        }
    }

    public void StartGame()
    {
        // ���� ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
        gameStarted = true;
        // UI ����� �� �ٸ� ���� ���� ���� ó��
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

        // �� �� ��� Enemy�� ã�� target�� �����մϴ�.
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.SetTarget(player.GetComponent<Rigidbody2D>());
            Debug.Log($"Enemy target set: {enemy.name} -> {player.name}");
        }

        // �� �� ��� BossAI�� ã�� target�� �����մϴ�.
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

        if (talkPanel.activeSelf == true)//��ȭâ ������ ���� ����
        {
            Stop();
        }
        else if (talkPanel.activeSelf == false)//��ȭâ ������ ���� ����
        {
            Resume();
        }
    }

   public void GetExp()
    {
        exp++;//���� óġ�� ����ġ 1����
        if(exp == nextExp[level]) { //����ġ�� ���� ���� �ִ� ����ġ�� ���ٸ�
            level++; //���� 1����
            exp = 0; //����ġ �ʱ�ȭ
            State += 3;
        }
    }
    

    public void Resume()
    {
        isLive=true;
        Time.timeScale = 1;
    }
    public void GameOver()
    {
        if (!isLive) return; // �̹� ���� ���� ���¶�� �� �̻� �������� ����

        uiResult.SetActive(true); // ���� ���� UI Ȱ��ȭ
        Debug.Log("Game Over!"); // ���� ���� ���� �α� ��� (������)
        Stop(); // ��� ���� ����
    }

    public void Stop()
    {
        gameStarted = false;
        isLive = false;
        Time.timeScale = 0; // ���� ���߱�
        Debug.Log("Game stopped. Time.timeScale: " + Time.timeScale); // Time.timeScale �� Ȯ��
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

        UINameText.text = objectName; // ���⼭ �̸��� UI�� ����
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
            isAction = false; //��ȭ�� ������ ��ȭ ����
            talkIndex = 0;
            Debug.Log(questManager.CheckQuest(id));
            return;
        }

        if (isNpc)
        {
            UINameText.text = npcName; // NPC �̸��� UI�� ǥ��
            talkText.text = talkData;
        }
        else
        {
            UINameText.text = "��"; // �÷��̾��� �̸� ����
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
    // Ÿ���� ȿ�� �ڷ�ƾ
    IEnumerator TypeEffect(string talkData)
    {
        talkText.text = ""; // ���� �ؽ�Ʈ �ʱ�ȭ
        isTyping = true; // Ÿ���� ������ ����

        foreach (char letter in talkData.ToCharArray())
        {
            talkText.text += letter; // �� ���ھ� �߰�
            yield return new WaitForSecondsRealtime(0.02f); // Ÿ���� �ӵ� ����
        }

        // Ÿ���� ȿ���� ���� �� 1�� ���
        yield return new WaitForSecondsRealtime(1f);

        isTyping = false; // Ÿ������ �������� ǥ��
        typingCoroutine = null; // �ڷ�ƾ�� �Ϸ�Ǿ����� ǥ��
    }
    public void BossDefeated()
    {
        if (isLive) // ������ ������ ���� ���� ���� ó��
        {
            isLive = false; // ���� ���¸� ��Ȱ��ȭ�Ͽ� �� �̻� �÷��̾ �������� �ʵ��� ��
            clearScreen.SetActive(true); // Ŭ���� ȭ�� Ȱ��ȭ
            Time.timeScale = 0; // ���� �Ͻ� ���� (���� ����)
            Debug.Log("Boss defeated! Game Clear!");
        }
    }
}
