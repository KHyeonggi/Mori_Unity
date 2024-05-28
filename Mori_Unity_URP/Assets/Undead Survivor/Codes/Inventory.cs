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

    public List<Item> items = new List<Item>();//추가된 아이템을 담을 변수

    private int slotCnt; //슬롯 갯수
    private int SlotCnt
    {
        get => slotCnt;
        set { slotCnt = value; }
    }

    void Start()
    {
        slotCnt = 36;//슬롯 갯수 36개로 초기화
        Physics.IgnoreCollision(bulletprefab.GetComponent<Collider>(), itemprefab.GetComponent<Collider>());
    }

    public bool AddItem(Item _item)
    {
        if (items.Count < SlotCnt)
        { //슬롯에 빈칸이 있다면
            items.Add(_item);
            if (onChangeItem != null)
                onChangeItem.Invoke();//onCHangeItem호출
            return true;//아이템 추가 성공시 true
        }
        return false; //아이템 추가 실패시 false
    }


    private void OnTriggerEnter2D(Collider2D collision) //아이템과 접촉시
    {
        Debug.Log("아이템 접촉");
        if (collision.gameObject.CompareTag("FieldItem"))
        {
            FieldItem fieldItem = collision.GetComponent<FieldItem>();
            Debug.Log("태그비교");
            if (AddItem(fieldItem.GetItem()))
            { //AddItem이 아이템이 추가되면 true반환
                    fieldItem.DestroyItem(); //필드 아이템 삭제
            }
        }
    }
}
