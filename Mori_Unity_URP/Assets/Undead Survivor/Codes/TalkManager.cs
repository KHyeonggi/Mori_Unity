using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData; // 대화 데이터를 담는 딕셔너리
    Dictionary<int, string> nameData;
    public UIManager uiManager;

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

        talkData.Add(2000, new string[] { "오른쪽에 있는 물의 정령을 구해줘", "불의 정령으로 보이는 뭔가가 있다.", "나중에 다시 한번 와서 말을 걸어보자." });

        //퀘스트 대화
        talkData.Add(10 + 1000, new string[] { "부탁이 있는데... 들어줄거면 다시 말 걸어줘" });
        talkData.Add(11 + 1000, new string[] { "흩어진 내 힘을 가져와 줄 수 있어? (오른쪽으로 가면 해결할 수 있을꺼같다.)" });
        talkData.Add(20 + 5000, new string[] { "흩어진 물의 힘을 찾았다.", });


        talkData.Add(21 + 1000, new string[] { "고마워", });

        // 대화 상대 이름 추가
        nameData.Add(1000, "물의 정령"); // 물의 정령
        nameData.Add(2000, "불의 정령"); // 불의 정령
        nameData.Add(10 + 1000, "물의 정령"); // 물의 정령
        nameData.Add(11 + 1000, "물의 정령"); // 물의 정령
        nameData.Add(20 + 5000, "모리"); // 모리
        nameData.Add(21 + 1000, "물의 정령"); // 물의 정령
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
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
    public string GetName(int id, int talkIndex)
    {
        // 대사에 따라 다른 이름을 반환하도록 수정
        if (id == 2000 && (talkIndex == 1 || talkIndex == 2))
        {
            return "모리"; // 특정 대사에 대해 모리의 이름 반환
        }

        // 대사에 따라 다른 이름을 반환하도록 수정
        if (id == 1000 && (talkIndex == 1 || talkIndex == 2 || talkIndex == 3))
        {
            return "모리"; // 특정 대사에 대해 모리의 이름 반환
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


