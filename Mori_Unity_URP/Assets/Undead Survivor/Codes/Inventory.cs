using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    List<Item>items=new List<Item>();//�߰��� �������� ���� ����
    
    private int slotCnt; //���� ����
    private int SlotCnt
    {
        get => slotCnt;
        set { slotCnt = value; }
    }
    private int HotslotCnt; //�ֹ� ���� ����
    private int HotSlotCnt
    {
        get => HotslotCnt;
        set { HotslotCnt = value; }
    }

    void Start()
    {
        HotslotCnt = 9;//���� ���� 9���� �ʱ�ȭ
        slotCnt = 27;//���� ���� 27���� �ʱ�ȭ
    }

    public bool AddItem(Item _item)
    {
        if(items.Count < HotSlotCnt)//�ֹ� ���Կ� ��ĭ�� �ִٸ� �߰�
        {
            items.Add(_item);
            if(onChangeItem!=null)
            onChangeItem.Invoke();//�������� �߰��Ǹ� ���Կ��� �߰��Ǵ� ���??
            return true;//������ �߰� ������ true
        }
        else if (items.Count < SlotCnt) { //�ֹٿ� �ڸ��� ���� ���Կ� ��ĭ�� �ִٸ�
            items.Add(_item);
            if (onChangeItem != null)
            onChangeItem.Invoke();//�������� �߰��Ǹ� ���Կ��� �߰��Ǵ� ���??
            return true;//������ �߰� ������ true
        } 
        return false; //������ �߰� ���н� false
    }

    private void OnTriggerEnter2D(Collider2D collision) //�����۰� ���˽�
    {
        Debug.Log("������ ����");
        if (collision.gameObject.CompareTag("FieldItem"))
        {
            FieldItem fieldItem = collision.GetComponent<FieldItem>();
            Debug.Log("�±׺�");
            if (AddItem(fieldItem.GetItem()))
            { //AddItem�� �������� �߰��Ǹ� true��ȯ
                fieldItem.DestroyItem(); //�ʵ� ������ ����
            }
        }
    }
}
