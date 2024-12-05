using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; } // �̱��� �ν��Ͻ�
    public int questId;
    public int questActionIndex;
    public GameObject[] questObject;

    Dictionary<int, QuestData> questList;
    void Awake()
    {
        // �̹� �ν��Ͻ��� �����ϸ� ���ο� �ν��Ͻ��� ������ ����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // �̹� �����ϴ� �ν��Ͻ��� �ı�
        }
        else
        {
            Instance = this; // �ν��Ͻ��� �Ҵ�
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �ı����� �ʵ��� ����
        }

        questList = new Dictionary<int, QuestData>();
        GenerateDeta();
    }
    

    void GenerateDeta() //����Ʈ ����
    {
        questList.Add(10, new QuestData("���� �ɾ��", new int[] { 1000, 1000 })); //10:����Ʈ id  1000:npc id //���������� �ι� ���ɱ�
        questList.Add(20, new QuestData("���� ������ ��Ź", new int[] { 5000, 1000 }));//��Ź�Ѱ�(id:5000)�� ��ȣ�ۿ�� ���������� ���ɱ�
        questList.Add(30, new QuestData("����Ʈ �Ϸ� - ���� ������ ��Ź", new int[] { 3000 }));
        questList.Add(40, new QuestData("�ٶ��� ����", new int[] { 6000,3000 })); 
        questList.Add(50, new QuestData("����Ʈ �Ϸ� - �ٶ��� ����", new int[] { 0 }));
    }
    
    public bool IsQuestActive(int questId)
    {
        return questList.ContainsKey(questId); // ����Ʈ�� �����ϴ��� Ȯ��
    }

    public int GetQuestTalkIndex(int id)
    {
         return questId + questActionIndex;
    }
    public string CheckQuest(int id)
    {
        Debug.Log($"CheckQuest ȣ��: npcid = {id}, questId = {questId}, questActionIndex = {questActionIndex}");
        Debug.Log($"���� NPC�� ��ȭ: questList[{questId}].npcId[{questActionIndex}] = {questList[questId].npcId[questActionIndex]}");

        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;
        Debug.Log($"questActionIndex ����: {questActionIndex}");

        ControlObject(); //����Ʈ ������Ʈ ����

        // ��� NPC�� ��ȭ�� �������� NextQuest ȣ��
        if (questActionIndex == questList[questId].npcId.Length)
        {
            Debug.Log("����Ʈ �Ϸ�! ���� ����Ʈ�� �Ѿ�ϴ�.");
            NextQuest(); // ����Ʈ �Ϸ� �� ���� ����Ʈ�� �̵�
        }
        return questList[questId].questName;
    }
    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
        Debug.Log("NextQuest ȣ��: questId = " + questId);
    }
    void ControlObject()
    {
        // ���� ���� ����Ʈ ����
        if (questId == 10 && questActionIndex == 2)
        {
            questObject[0].SetActive(true); // ���� ���� ������Ʈ Ȱ��ȭ
        }
        else if (questId == 20 && questActionIndex == 1)
        {
            questObject[0].SetActive(false); // ���� ���� ������Ʈ ��Ȱ��ȭ
        }

        // �ٶ��� ���� ����Ʈ ����
        if (questId == 30 && questActionIndex == 2)
        {
            questObject[0].SetActive(true); 
        }
        else if (questId == 30 && questActionIndex == 1)
        {
        }
    }
}
