using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData; // ��ȭ �����͸� ��� ��ųʸ�
    Dictionary<int, string> nameData;
    public UIManager uiManager;

    void Awake()
    { 
        talkData = new Dictionary<int, string[]>();
        nameData = new Dictionary<int, string>();

        GenerateData();

    }


    void GenerateData()//��ȭ ����
    {    
        // ��ȭ ������ �߰�
        talkData.Add(1000, new string[] {"�ȳ�", "���� �������� ���̴� ������ �ִ�.", "�߸� �ǵ���ٰ� �����ó�� ���� �� ������ �� ������.", "���߿� �ٽ� �ѹ� �ͼ� ���� �ɾ��." });

        talkData.Add(2000, new string[] { "�����ʿ� �ִ� ���� ������ ������", "���� �������� ���̴� ������ �ִ�.", "���߿� �ٽ� �ѹ� �ͼ� ���� �ɾ��." });

        //����Ʈ ��ȭ
        talkData.Add(10 + 1000, new string[] { "��Ź�� �ִµ�... ����ٰŸ� �ٽ� �� �ɾ���" });
        talkData.Add(11 + 1000, new string[] { "����� �� ���� ������ �� �� �־�? (���������� ���� �ذ��� �� ����������.)" });
        talkData.Add(20 + 5000, new string[] { "����� ���� ���� ã�Ҵ�.", });


        talkData.Add(21 + 1000, new string[] { "����", });

        // ��ȭ ��� �̸� �߰�
        nameData.Add(1000, "���� ����"); // ���� ����
        nameData.Add(2000, "���� ����"); // ���� ����
        nameData.Add(10 + 1000, "���� ����"); // ���� ����
        nameData.Add(11 + 1000, "���� ����"); // ���� ����
        nameData.Add(20 + 5000, "��"); // ��
        nameData.Add(21 + 1000, "���� ����"); // ���� ����
    }




    public string GetTalk(int id, int talkIndex)
    {

        if (!talkData.ContainsKey(id))
        {
            if (!talkData.ContainsKey(id - id % 10))
                return GetTalk (id - id % 100, talkIndex); // ù ��° ��ȭ ��������
            else
                return GetTalk(id - id % 10, talkIndex);// ù ��° ����Ʈ ��ȭ ��������

        }
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
    public string GetName(int id, int talkIndex)
    {
        // ��翡 ���� �ٸ� �̸��� ��ȯ�ϵ��� ����
        if (id == 2000 && (talkIndex == 1 || talkIndex == 2))
        {
            return "��"; // Ư�� ��翡 ���� ���� �̸� ��ȯ
        }

        // ��翡 ���� �ٸ� �̸��� ��ȯ�ϵ��� ����
        if (id == 1000 && (talkIndex == 1 || talkIndex == 2 || talkIndex == 3))
        {
            return "��"; // Ư�� ��翡 ���� ���� �̸� ��ȯ
        }


        if (nameData.ContainsKey(id))
        {
            return nameData[id];
        }
        return "��"; // ID�� �ش��ϴ� �̸��� ������ �⺻ �̸� ��ȯ
    }


    public void StartDialogue(int id, int talkIndex)
    {
        string talk = GetTalk(id, talkIndex);
        string name = GetName(id, talkIndex); // �̸� ��������

        if (talk != null)
        {
            // �̸��� ��ȭ �ؽ�Ʈ�� UI�� ǥ��
            uiManager.SetNameText(name);   // �̸��� ����
            uiManager.SetTalkText(talk);   // ��ȭ ������ ����
        }
    }
}


