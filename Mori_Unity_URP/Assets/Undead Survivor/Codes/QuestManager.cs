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
        questList.Add(10, new QuestData("���� �ɾ��", new int[] { 1000, 1000 })); //10:����Ʈ id  1000:npc id
        questList.Add(20, new QuestData("���� ������ ��Ź", new int[] { 5000, 1000 }));
        questList.Add(30, new QuestData("����Ʈ �Ϸ� - ���� ������ ��Ź", new int[] { 0 }));
        questList.Add(40, new QuestData("�ٶ��� ����", new int[] { 6000, 7000 })); // 6000: ������ ���, 7000: �ٶ��� ����
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
        Debug.Log("CheckQuest - id: " + id + ", questId: " + questId + ", questActionIndex: " + questActionIndex);

        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;
        
        ControlObject(); //����Ʈ ������Ʈ ����

        if (questActionIndex == questList[questId].npcId.Length)
        {
            NextQuest();
        }
 
        return questList[questId].questName;
    }
    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }
    void ControlObject()
    {
        switch (questId)
        {
            case 10:
                if (questActionIndex == 2)
                    questObject[0].SetActive(true);
                break;
            case 20:
                if (questActionIndex == 1)
                    questObject[0].SetActive(false);
                break;

            case 40:
                if (questActionIndex == 2)
                    questObject[0].SetActive(true);
                break;
            case 50:
                if (questActionIndex == 1)
                    questObject[0].SetActive(false);
                break;

        }
    }
}
