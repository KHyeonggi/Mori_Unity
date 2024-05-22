using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public GameObject Inventory;//인벤토리UI객체
    bool activeInventory=false;//인벤토리 열림 여부

    public Slot[] slots;
    public Transform slotHolder;

    private void Start()
    {
        slots = GetComponentsInChildren<Slot>();//slot 자식요소 불러오기
        Inventory.SetActive(activeInventory);//게임 시작시 인벤토리 비활성화
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))//E키 입력대기
        {
            activeInventory = !activeInventory;//인벤토리 열림 여부 true
            Inventory.SetActive(activeInventory); //인벤토리 열림
        }    
    }
}
