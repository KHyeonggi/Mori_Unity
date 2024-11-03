using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public GameObject InventoryPanel;//�κ��丮UI��ü
    bool activeInventory=false;//�κ��丮 ���� ����

    public Slot[] slots;
    public Transform slotHolder;

    private void Start()
    {
        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        InventoryPanel.SetActive(activeInventory);//���� ���۽� �κ��丮 ��Ȱ��ȭ
        inven.onChangeItem += ReDrawSlotUI;
        for(int i=0; i<slots.Length; i++)
        {
            slots[i].slotnum = i;
            Debug.Log("���� ����" + slots[i]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))//EŰ �Է´��
        {
            activeInventory = !activeInventory;//�κ��丮 ���� ���� true
            InventoryPanel.SetActive(activeInventory); //�κ��丮 ����
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
