using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    public GameObject bulletprefab;
    public GameObject itemprefab;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public List<Item> items = new List<Item>();//�߰��� �������� ���� ����

    private int slotCnt; //���� ����
    private int SlotCnt
    {
        get => slotCnt;
        set { slotCnt = value; }
    }

    void Start()
    {
        slotCnt = 16;//���� ���� 36���� �ʱ�ȭ
    }

    public bool AddItem(Item _item)
    {
        if (items.Count < SlotCnt)
        { //���Կ� ��ĭ�� �ִٸ�
            items.Add(_item);
            if (onChangeItem != null)
                onChangeItem.Invoke();//onCHangeItemȣ��
            return true;//������ �߰� ������ true
        }
        return false; //������ �߰� ���н� false
    }

    public void RemoveItem(int _index)
    {
        items.RemoveAt(_index);
        onChangeItem.Invoke();
    }


    private void OnTriggerEnter2D(Collider2D collision) //�����۰� ���˽�
    {
        if (collision.gameObject.CompareTag("FieldItem"))
        {
            FieldItem fieldItem = collision.GetComponent<FieldItem>();
            if (AddItem(fieldItem.GetItem()))
            { //AddItem�� �������� �߰��Ǹ� true��ȯ
                    fieldItem.DestroyItem(); //�ʵ� ������ ����
            }
        }
    }
}
