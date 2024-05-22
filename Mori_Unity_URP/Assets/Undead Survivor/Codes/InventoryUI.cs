using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public GameObject Inventory;//�κ��丮UI��ü
    bool activeInventory=false;//�κ��丮 ���� ����

    public Slot[] slots;
    public Transform slotHolder;

    private void Start()
    {
        slots = GetComponentsInChildren<Slot>();//slot �ڽĿ�� �ҷ�����
        Inventory.SetActive(activeInventory);//���� ���۽� �κ��丮 ��Ȱ��ȭ
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))//EŰ �Է´��
        {
            activeInventory = !activeInventory;//�κ��丮 ���� ���� true
            Inventory.SetActive(activeInventory); //�κ��丮 ����
        }    
    }
}
