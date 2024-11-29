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

    public TalkManager talkManager;
    public QuestManager questManager;
    public GameObject talkPanel;//��ȭâ
    public Text talkText;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;

    private Coroutine typingCoroutine;
    private bool isTyping = false; // Ÿ���� ������ ����

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
}
