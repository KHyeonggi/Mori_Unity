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

    void GenerateData()//��ȭ ����
    {
        talkData.Add(1033, new string[] { "���� �������� ���̴� ������ �ִ�." ,"�߸� �ǵ���ٰ� ����� ó�� ������ ������ �� ������.","���߿� �ٽ� �ѹ� �ͼ� ���� �ɾ��."});
        talkData.Add(1034, new string[] { "���� �������� ���̴� ������ �ִ�." ,"�߰ſ� ���̴� ���� �������� ����"});
    }
    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
