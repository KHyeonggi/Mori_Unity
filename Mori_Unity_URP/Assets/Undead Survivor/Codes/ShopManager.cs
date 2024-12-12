using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopUI; // ���� UI ������Ʈ

    public void OpenShop()
    {
        shopUI.SetActive(true); // ���� UI Ȱ��ȭ
    }

    public void CloseShop()
    {
        shopUI.SetActive(false); // ���� UI ��Ȱ��ȭ
    }
}

