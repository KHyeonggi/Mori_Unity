using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;
    public GameObject[] questObject;

    Dictionary<int, QuestData> questList;
    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateDeta();
    }

    void GenerateDeta() //����Ʈ ����
    {
        questList.Add(10, new QuestData("���� �ɾ��", new int[] { 1000, 1000 })); //10:����Ʈ id  1000:npc id
        questList.Add(20, new QuestData("���� ������ ��Ź", new int[] { 5000, 1000 }));
        questList.Add(30, new QuestData("����Ʈ �Ϸ� - ���� ������ ��Ź", new int[] { 0 }));
    }
    public int GetQuestTalkIndex(int id)
    {
         return questId + questActionIndex;
    }
    public string CheckQuest(int id)
    {
        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;
        
        ControlObject(); //����Ʈ ������Ʈ ����

        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();

        return questList[questId].questName;
    }
    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }
    void ControlObject()
    {
        switch (questId) {
            case 10:
                if (questActionIndex == 2)
                    questObject[0].SetActive(true);
                break;
            case 20:
                if (questActionIndex == 1)
                    questObject[0].SetActive(false);
                break;
        }
    }
}
