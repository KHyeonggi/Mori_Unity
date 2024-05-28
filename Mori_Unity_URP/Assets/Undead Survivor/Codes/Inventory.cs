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
        slotCnt = 36;//���� ���� 36���� �ʱ�ȭ
        Physics.IgnoreCollision(bulletprefab.GetComponent<Collider>(), itemprefab.GetComponent<Collider>());
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
