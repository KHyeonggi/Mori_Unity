using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()//대화 내용
    {
        talkData.Add(1000, new string[] { "물의 정령으로 보이는 뭔가가 있다." ,"잘못 건들었다간 물방울 처럼 터질것 같지만 안 터졌다.","나중에 다시 한번 와서 말을 걸어보자."});
        talkData.Add(2000, new string[] { "불의 정령으로 보이는 뭔가가 있다." ,"뜨거워 보이니 직접 만지지는 말자","나중에 다시 한번 와서 말을 걸어보자."});


        //퀘스트 대화
        talkData.Add(10 + 1000, new string[] { "부탁이 있는데...","들어줄거면 다시 말 걸어줘"  });
        talkData.Add(11 + 1000, new string[] { "흩어진 내 힘을 가져와 줄 수 있어?" });
        talkData.Add(20 + 5000, new string[] { "흩어진 물의 힘을 찾았다.", });

        talkData.Add(21 + 1000, new string[] { "고마워", });
    }


    public string GetTalk(int id, int talkIndex)
    {
        if (!talkData.ContainsKey(id))
        {
            if (!talkData.ContainsKey(id - id % 10))
                return GetTalk (id - id % 100, talkIndex); //Get First Talk
            else
                return GetTalk(id - id % 10, talkIndex);//Get First Quest Talk

        }
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];

    }
}
