using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData; // 대화 데이터를 담는 딕셔너리
    Dictionary<int, string> nameData;
    public UIManager uiManager;
    public Inventory inventory; // 인벤토리 참조
    public static TalkManager Instance;

    void Awake()
    { 
        talkData = new Dictionary<int, string[]>();
        nameData = new Dictionary<int, string>();

        GenerateData();

    }


    void GenerateData()//대화 내용
    {    
        // 대화 데이터 추가
        talkData.Add(1000, new string[] {"안녕", "물의 정령으로 보이는 뭔가가 있다.", "잘못 건들었다간 물방울처럼 터질 것 같지만 안 터졌다.", "나중에 다시 한번 와서 말을 걸어보자." });
        talkData.Add(2000, new string[] { "안녕","혹시 오른쪽에 있는 물의 정령을 구해줄수있을까?", "불의 정령으로 보이는 뭔가가 있다.", "나중에 다시 한번 와서 말을 걸어보자." });
        talkData.Add(3000, new string[] {"안녕"});// 바람의 정령 대화 추가


        // 물의 정령 퀘스트 대화
        talkData.Add(10 + 1000, new string[] { "부탁이 있는데... 들어줄거면 다시 말 걸어줘" });
        talkData.Add(11 + 1000, new string[] { "흩어진 내 힘을 가져와 줄 수 있어?  (오른쪽으로 가면 해결할 수 있을꺼같다.)" });
        talkData.Add(20 + 5000, new string[] { "흩어진 물의 힘을 찾았다.", });
        talkData.Add(21 + 1000, new string[] { "고마워", });
        
        // 바람의 정령 퀘스트 대화
        talkData.Add(30 + 3000, new string[] { 
            "안녕, 너도 이곳을 모험하러 왔어?" ,//0
            "너도? 나 말고 또 누가 있었어?", //1
            "꽤나 많았지. 지금은 전부 어떻게 됐는지는 모르겠지만..." ,//2
            "위기에 직면해 모험을 더 이상 진행하지 못하게 되었을 수도 있고", //3
            "아니면 지금도 계속 진행하고 있을 수도 있어.",//4
            "..." ,//5
            "너도 이곳을 모험하러 왔다면 한 가지 시험을 내줄게." ,
            "너의 왼쪽을 보면 또 다른 방이 있어. 거기에서 잘 찾아봐.",
        }); 

        // 바람의 정령 퀘스트 완료 대화
        talkData.Add(40 + 3000, new string[] { "찾았구나. 잘했어!" ,"이제 네 모험은 더 탄탄해졌을 거야. 행운을 빌어!"}); // 퀘스트 완료


        // 대화 상대 이름 추가
        nameData.Add(1000, "물의 정령"); // 물의 정령
        nameData.Add(2000, "불의 정령"); // 불의 정령
        nameData.Add(10 + 1000, "물의 정령"); // 물의 정령
        nameData.Add(11 + 1000, "물의 정령"); // 물의 정령
        nameData.Add(20 + 5000, "모리"); // 모리
        nameData.Add(21 + 1000, "물의 정령"); // 물의 정령

        nameData.Add(3000, "바람의 정령"); // 바람의 정령
        nameData.Add(40 + 3000, "바람의 정령");
        nameData.Add(50 + 3000, "바람의 정령");  
    }


    public string GetTalk(int id, int talkIndex)
    {

        if (!talkData.ContainsKey(id))
        {
            if (!talkData.ContainsKey(id - id % 10))
                return GetTalk (id - id % 100, talkIndex); // 첫 번째 대화 가져오기
            else
                return GetTalk(id - id % 10, talkIndex);// 첫 번째 퀘스트 대화 가져오기

        }
        if (!talkData.ContainsKey(id)) 
        {
            if(!talkData.ContainsKey(id - id % 10)){
                //퀘스트 맨처음 대사도 없을때
                if (talkIndex == talkData[id - id % 100].Length)
                    return null;
                else
                    return talkData[id - id % 100][talkIndex];
            }
            else
            {   //퀘스트 진행 순서 대사 없을 때 //퀘스트 맨처음 대사 가져옴
                if (talkIndex == talkData[id - id % 10].Length)
                    return null;
                else
                    return talkData[id - id % 10][talkIndex];
            }  
        }

        //if (talkIndex == talkData[id].Length)
        //    return null;
        if (talkIndex >= talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

    public string GetName(int id, int talkIndex)
    {
        // 불의 정령 대화의 경우, 특정 인덱스에서만 모리의 이름 반환
        if (id == 2000)
        {
            if (talkIndex == 2 || talkIndex == 3) // 특정 대화 인덱스에 모리의 이름 반환
            {
                return "모리";
            }
            return "불의 정령";
        }

        // 물의 정령 대화의 경우, 특정 인덱스에서만 모리의 이름 반환
        if (id == 1000)
        {
            if (talkIndex == 1 || talkIndex == 2 || talkIndex == 3) // 특정 대화 인덱스에 모리의 이름 반환
            {
                return "모리";
            }
            return "물의 정령";
        }


        // 바람의 정령 대화
        if (id == 3000)
        {
            if (talkIndex == 1 || talkIndex == 5) // 특정 대화 인덱스에 모리의 이름 반환
            {
                return "모리";
            }
            return "바람의 정령";
        }
        if (id == 40 + 3000)
        {
            if (talkIndex == 1 || talkIndex == 5) // 특정 대화 인덱스에 모리의 이름 반환
            {
                return "모리";
            }
            return "바람의 정령";
        }


        if (nameData.ContainsKey(id))
        {
            return nameData[id];
        }
        return "모리"; // ID에 해당하는 이름이 없으면 기본 이름 반환
    }


    public void StartDialogue(int id, int talkIndex)
    {
        string talk = GetTalk(id, talkIndex);
        string name = GetName(id, talkIndex); // 이름 가져오기

        if (talk != null)
        {
            // 이름과 대화 텍스트를 UI에 표시
            uiManager.SetNameText(name);   // 이름을 설정
            uiManager.SetTalkText(talk);   // 대화 내용을 설정
        }
    }
}


