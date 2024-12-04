using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType //������ Ÿ��
{
    Equipment,
    Consumables,
    Etc
}

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public List<ItemEffect> efts;
    public changeWeapone effect;
    public bool Use()//������ ��밡�� ����
    {
        bool isUsed = false;
        foreach (ItemEffect eft in efts) 
        {
            isUsed = eft.ExecuteRole();
        }
        return isUsed;
        Debug.Log("item.use = true");
    }
}
