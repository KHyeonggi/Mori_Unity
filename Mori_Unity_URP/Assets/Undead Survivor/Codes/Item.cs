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

    public bool Use()//������ ��밡�� ����
    {
        return false;
    }
}
