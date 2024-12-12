using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopUI; // 상점 UI 오브젝트

    public void OpenShop()
    {
        shopUI.SetActive(true); // 상점 UI 활성화
    }

    public void CloseShop()
    {
        shopUI.SetActive(false); // 상점 UI 비활성화
    }
}

