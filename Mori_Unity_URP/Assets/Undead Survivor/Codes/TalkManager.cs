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
        talkData.Add(1033, new string[] { "물의 정령으로 보이는 뭔가가 있다." ,"잘못 건들었다간 물방울 처럼 터질것 같지만 안 터졌다.","나중에 다시 한번 와서 말을 걸어보자."});
        talkData.Add(1034, new string[] { "불의 정령으로 보이는 뭔가가 있다." ,"뜨거워 보이니 직접 만지지는 말자"});
    }
    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
