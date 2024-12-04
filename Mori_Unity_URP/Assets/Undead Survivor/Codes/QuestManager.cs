using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; } // 싱글턴 인스턴스
    public int questId;
    public int questActionIndex;
    public GameObject[] questObject;

    Dictionary<int, QuestData> questList;
    void Awake()
    {
        // 이미 인스턴스가 존재하면 새로운 인스턴스를 만들지 않음
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 이미 존재하는 인스턴스를 파괴
        }
        else
        {
            Instance = this; // 인스턴스를 할당
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
        }

        questList = new Dictionary<int, QuestData>();
        GenerateDeta();
    }
    

    void GenerateDeta() //퀘스트 내용
    {
        questList.Add(10, new QuestData("말을 걸어보자", new int[] { 1000, 1000 })); //10:퀘스트 id  1000:npc id
        questList.Add(20, new QuestData("물의 정령의 부탁", new int[] { 5000, 1000 }));
        questList.Add(30, new QuestData("퀘스트 완료 - 물의 정령의 부탁", new int[] { 0 }));
        questList.Add(40, new QuestData("바람의 시험", new int[] { 6000, 7000 })); // 6000: 숨겨진 통로, 7000: 바람의 정령
        questList.Add(50, new QuestData("퀘스트 완료 - 바람의 시험", new int[] { 0 }));
    }
    
    public bool IsQuestActive(int questId)
    {
        return questList.ContainsKey(questId); // 퀘스트가 존재하는지 확인
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
        
        ControlObject(); //퀘스트 오브젝트 제어

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
