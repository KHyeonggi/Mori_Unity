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
        questList.Add(10, new QuestData("말을 걸어보자", new int[] { 1000, 1000 })); //10:퀘스트 id  1000:npc id //물정령한테 두번 말걸기
        questList.Add(20, new QuestData("물의 정령의 부탁", new int[] { 5000, 1000 }));//부탁한것(id:5000)과 상호작용뒤 물정령한테 말걸기
        questList.Add(30, new QuestData("퀘스트 완료 - 물의 정령의 부탁", new int[] { 3000 }));
        questList.Add(40, new QuestData("바람의 시험", new int[] { 6000,3000 })); 
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
        Debug.Log($"CheckQuest 호출: npcid = {id}, questId = {questId}, questActionIndex = {questActionIndex}");
        Debug.Log($"현재 NPC와 대화: questList[{questId}].npcId[{questActionIndex}] = {questList[questId].npcId[questActionIndex]}");

        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;
        Debug.Log($"questActionIndex 증가: {questActionIndex}");

        ControlObject(); //퀘스트 오브젝트 제어

        // 모든 NPC와 대화가 끝났으면 NextQuest 호출
        if (questActionIndex == questList[questId].npcId.Length)
        {
            Debug.Log("퀘스트 완료! 다음 퀘스트로 넘어갑니다.");
            NextQuest(); // 퀘스트 완료 후 다음 퀘스트로 이동
        }
        return questList[questId].questName;
    }
    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
        Debug.Log("NextQuest 호출: questId = " + questId);
    }
    void ControlObject()
    {
        // 물의 정령 퀘스트 로직
        if (questId == 10 && questActionIndex == 2)
        {
            questObject[0].SetActive(true); // 물의 정령 오브젝트 활성화
        }
        else if (questId == 20 && questActionIndex == 1)
        {
            questObject[0].SetActive(false); // 물의 정령 오브젝트 비활성화
        }

        // 바람의 정령 퀘스트 로직
        if (questId == 30 && questActionIndex == 2)
        {
            questObject[0].SetActive(true); 
        }
        else if (questId == 30 && questActionIndex == 1)
        {
        }
    }
}
