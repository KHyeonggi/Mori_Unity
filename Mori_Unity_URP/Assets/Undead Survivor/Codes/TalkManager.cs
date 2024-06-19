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
        talkData.Add(1000, new string[] { "���� �������� ���̴� ������ �ִ�." ,"�߸� �ǵ���ٰ� ����� ó�� ������ ������ �� ������.","���߿� �ٽ� �ѹ� �ͼ� ���� �ɾ��."});
        talkData.Add(2000, new string[] { "���� �������� ���̴� ������ �ִ�." ,"�߰ſ� ���̴� ���� �������� ����","���߿� �ٽ� �ѹ� �ͼ� ���� �ɾ��."});


        //����Ʈ ��ȭ
        talkData.Add(10 + 1000, new string[] { "��Ź�� �ִµ�...","����ٰŸ� �ٽ� �� �ɾ���"  });
        talkData.Add(11 + 1000, new string[] { "����� �� ���� ������ �� �� �־�?" });
        talkData.Add(20 + 5000, new string[] { "����� ���� ���� ã�Ҵ�.", });

        talkData.Add(21 + 1000, new string[] { "����", });
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
