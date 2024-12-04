using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerUpHandler, IPointerClickHandler, IPointerDownHandler
{
    public int slotnum;
    public Item item;
    public Image itemIcon;
    public ItemType itemType;
    bool isUse;
    public changeWeapone weapon;
    public GameObject hand;
    public GameObject image;

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
        itemType = item.itemType;
    }

    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool isactive = itemIcon.gameObject.activeSelf;
        if (isactive == true)
        {
            Debug.Log(itemType);
            if(itemType == ItemType.Equipment)
            {
                SpriteRenderer spriteRenderer = hand.GetComponent<SpriteRenderer>(); //Hand 이미지
                Image thisSpriteRenderer = image.GetComponent<Image>(); // 장비창 이미지
                thisSpriteRenderer.sprite = itemIcon.sprite; //itemIcon => 인벤토리에 있는 아이템 이미지 -> 장비창을 인벤토리에 있는 이미지로 설정
                spriteRenderer.sprite = itemIcon.sprite; //Hand를 인벤토리에 있는 이미지로 설정
                Inventory.instance.RemoveItem(slotnum);
            }
            else if (itemType == ItemType.Consumables) 
            {
                isUse = item.Use();
                if (isUse)
                {
                    Inventory.instance.RemoveItem(slotnum);
                    Debug.Log(slotnum + "슬롯 사용");
                }
            }
            
            
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
