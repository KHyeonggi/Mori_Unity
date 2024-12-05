using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData; // ��ȭ �����͸� ��� ��ųʸ�
    Dictionary<int, string> nameData;
    public UIManager uiManager;
    public Inventory inventory; // �κ��丮 ����
    public static TalkManager Instance;

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
        talkData.Add(2000, new string[] { "�ȳ�","Ȥ�� �����ʿ� �ִ� ���� ������ �����ټ�������?", "���� �������� ���̴� ������ �ִ�.", "���߿� �ٽ� �ѹ� �ͼ� ���� �ɾ��." });
        talkData.Add(3000, new string[] {"�ȳ�"});// �ٶ��� ���� ��ȭ �߰�


        // ���� ���� ����Ʈ ��ȭ
        talkData.Add(10 + 1000, new string[] { "��Ź�� �ִµ�... ����ٰŸ� �ٽ� �� �ɾ���" });
        talkData.Add(11 + 1000, new string[] { "����� �� ���� ������ �� �� �־�?  (���������� ���� �ذ��� �� ����������.)" });
        talkData.Add(20 + 5000, new string[] { "����� ���� ���� ã�Ҵ�.", });
        talkData.Add(21 + 1000, new string[] { "����", });
        
        // �ٶ��� ���� ����Ʈ ��ȭ
        talkData.Add(30 + 3000, new string[] { 
            "�ȳ�, �ʵ� �̰��� �����Ϸ� �Ծ�?" ,//0
            "�ʵ�? �� ���� �� ���� �־���?", //1
            "�ϳ� ������. ������ ���� ��� �ƴ����� �𸣰�����..." ,//2
            "���⿡ ������ ������ �� �̻� �������� ���ϰ� �Ǿ��� ���� �ְ�", //3
            "�ƴϸ� ���ݵ� ��� �����ϰ� ���� ���� �־�.",//4
            "..." ,//5
            "�ʵ� �̰��� �����Ϸ� �Դٸ� �� ���� ������ ���ٰ�." ,
            "���� ������ ���� �� �ٸ� ���� �־�. �ű⿡�� �� ã�ƺ�.",
        }); 

        // �ٶ��� ���� ����Ʈ �Ϸ� ��ȭ
        talkData.Add(40 + 3000, new string[] { "ã�ұ���. ���߾�!" ,"���� �� ������ �� źź������ �ž�. ����� ����!"}); // ����Ʈ �Ϸ�


        // ��ȭ ��� �̸� �߰�
        nameData.Add(1000, "���� ����"); // ���� ����
        nameData.Add(2000, "���� ����"); // ���� ����
        nameData.Add(10 + 1000, "���� ����"); // ���� ����
        nameData.Add(11 + 1000, "���� ����"); // ���� ����
        nameData.Add(20 + 5000, "��"); // ��
        nameData.Add(21 + 1000, "���� ����"); // ���� ����

        nameData.Add(3000, "�ٶ��� ����"); // �ٶ��� ����
        nameData.Add(40 + 3000, "�ٶ��� ����");
        nameData.Add(50 + 3000, "�ٶ��� ����");  
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
        if (!talkData.ContainsKey(id)) 
        {
            if(!talkData.ContainsKey(id - id % 10)){
                //����Ʈ ��ó�� ��絵 ������
                if (talkIndex == talkData[id - id % 100].Length)
                    return null;
                else
                    return talkData[id - id % 100][talkIndex];
            }
            else
            {   //����Ʈ ���� ���� ��� ���� �� //����Ʈ ��ó�� ��� ������
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
        // ���� ���� ��ȭ�� ���, Ư�� �ε��������� ���� �̸� ��ȯ
        if (id == 2000)
        {
            if (talkIndex == 2 || talkIndex == 3) // Ư�� ��ȭ �ε����� ���� �̸� ��ȯ
            {
                return "��";
            }
            return "���� ����";
        }

        // ���� ���� ��ȭ�� ���, Ư�� �ε��������� ���� �̸� ��ȯ
        if (id == 1000)
        {
            if (talkIndex == 1 || talkIndex == 2 || talkIndex == 3) // Ư�� ��ȭ �ε����� ���� �̸� ��ȯ
            {
                return "��";
            }
            return "���� ����";
        }


        // �ٶ��� ���� ��ȭ
        if (id == 3000)
        {
            if (talkIndex == 1 || talkIndex == 5) // Ư�� ��ȭ �ε����� ���� �̸� ��ȯ
            {
                return "��";
            }
            return "�ٶ��� ����";
        }
        if (id == 40 + 3000)
        {
            if (talkIndex == 1 || talkIndex == 5) // Ư�� ��ȭ �ε����� ���� �̸� ��ȯ
            {
                return "��";
            }
            return "�ٶ��� ����";
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


