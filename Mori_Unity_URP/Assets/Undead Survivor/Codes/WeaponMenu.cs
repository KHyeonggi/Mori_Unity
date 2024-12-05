using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class WeaponMenu : MonoBehaviour
{
    public GameObject WeaponCanvas; //���â UI
    bool activeWeapon = false;//�κ��丮 ���� ����
    public GameObject hand;
    public GameObject image;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer spriteRenderer = hand.GetComponent<SpriteRenderer>(); //Hand �̹���
        Image thisSpriteRenderer = image.GetComponent<Image>(); // ���â �̹���
        thisSpriteRenderer.sprite = spriteRenderer.sprite; // Hand �̹��� ���â�� ����
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))//EŰ �Է´��
        {
            activeWeapon = !activeWeapon;//�κ��丮 ���� ���� true
            WeaponCanvas.SetActive(activeWeapon); //�κ��丮 ����
        }
    }
}
