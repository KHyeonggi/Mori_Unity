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

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
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
        
        bool isUse = item.Use();
        if (isUse)
        {
            Inventory.instance.RemoveItem(slotnum);
            Debug.Log(slotnum);
        }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
