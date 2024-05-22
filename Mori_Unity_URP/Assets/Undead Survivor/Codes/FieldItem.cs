using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    public Item item;
    public SpriteRenderer image;

    public void SetItem(Item _item){ //아이템 생성 Item.cs로 현 클래스 Item 초기화
        item.itemName=_item.itemName;
        item.itemImage=_item.itemImage;
        item.itemType=_item.itemType;

        image.sprite = item.itemImage;
    }
    public Item GetItem()//아이템 획득
    {
        return item;
    }
    public void DestroyItem()//획득한 아이템 삭제
    {
        Destroy(gameObject);
    }
}

