using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType //아이템 타입
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
    public bool Use()//아이템 사용가능 여부
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
