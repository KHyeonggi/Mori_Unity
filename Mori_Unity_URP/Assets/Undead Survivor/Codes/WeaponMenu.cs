using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class WeaponMenu : MonoBehaviour
{
    public GameObject WeaponCanvas; //장비창 UI
    bool activeWeapon = false;//인벤토리 열림 여부
    public GameObject hand;
    public GameObject image;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer spriteRenderer = hand.GetComponent<SpriteRenderer>(); //Hand 이미지
        Image thisSpriteRenderer = image.GetComponent<Image>(); // 장비창 이미지
        thisSpriteRenderer.sprite = spriteRenderer.sprite; // Hand 이미지 장비창에 설정
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))//E키 입력대기
        {
            activeWeapon = !activeWeapon;//인벤토리 열림 여부 true
            WeaponCanvas.SetActive(activeWeapon); //인벤토리 열림
        }
    }
}
