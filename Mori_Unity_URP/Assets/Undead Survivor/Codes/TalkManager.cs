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

    void GenerateData()
    {
        talkData.Add(1033, new string[] { "물의 정령으로 보이는 뭔가가 있다." });
        talkData.Add(1034, new string[] { "불의 정령으로 보이는 뭔가가 있다." });
    }
    public string GetTalk(int id, int talkIndex)
    {
        return talkData[id][talkIndex];
    }
}
