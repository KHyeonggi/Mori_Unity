using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public GameObject InventoryPanel;//인벤토리UI객체
    bool activeInventory=false;//인벤토리 열림 여부

    public Slot[] slots;
    public Transform slotHolder;

    private void Start()
    {
        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        InventoryPanel.SetActive(activeInventory);//게임 시작시 인벤토리 비활성화
        inven.onChangeItem += ReDrawSlotUI;
        for(int i=0; i<slots.Length; i++)
        {
            slots[i].slotnum = i;
            Debug.Log("슬롯 갯수" + slots[i]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))//E키 입력대기
        {
            activeInventory = !activeInventory;//인벤토리 열림 여부 true
            InventoryPanel.SetActive(activeInventory); //인벤토리 열림
        }    
    }

    public void ReDrawSlotUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }
        for (int i = 0; i < inven.items.Count; i++)
        {
            slots[i].item = inven.items[i];
            slots[i].UpdateSlotUI();
        }
    }
}
