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

    List<Item>items=new List<Item>();//추가된 아이템을 담을 변수
    
    private int slotCnt; //슬롯 갯수
    private int SlotCnt
    {
        get => slotCnt;
        set { slotCnt = value; }
    }
    private int HotslotCnt; //핫바 슬롯 갯수
    private int HotSlotCnt
    {
        get => HotslotCnt;
        set { HotslotCnt = value; }
    }

    void Start()
    {
        HotslotCnt = 9;//슬롯 갯수 9개로 초기화
        slotCnt = 27;//슬롯 갯수 27개로 초기화
    }

    public bool AddItem(Item _item)
    {
        if(items.Count < HotSlotCnt)//핫바 슬롯에 빈칸이 있다면 추가
        {
            items.Add(_item);
            if(onChangeItem!=null)
            onChangeItem.Invoke();//아이템이 추가되면 슬롯에도 추가되는 기능??
            return true;//아이템 추가 성공시 true
        }
        else if (items.Count < SlotCnt) { //핫바에 자리가 없고 슬롯에 빈칸이 있다면
            items.Add(_item);
            if (onChangeItem != null)
            onChangeItem.Invoke();//아이템이 추가되면 슬롯에도 추가되는 기능??
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
