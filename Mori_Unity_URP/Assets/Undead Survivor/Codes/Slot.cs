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
    bool isUse;

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
        bool isactive = itemIcon.gameObject.activeSelf;
        if (isactive == true)
        {
            isUse = item.Use();
            if (isUse)
            {
                Inventory.instance.RemoveItem(slotnum);
                Debug.Log(slotnum + "½½·Ô »ç¿ë");
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
