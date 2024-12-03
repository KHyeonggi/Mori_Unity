using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class increaseState : MonoBehaviour
{
    public static bool openmenu = false; //�޴� ���� ����
    public GameObject StateMenu;
    public Player player;
    public GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))//altŰ �Է½� ȣ��
        {
            if (openmenu)
            {
                StateMenu.SetActive(false);
                openmenu = false;
            }
            else
            {
                StateMenu.SetActive(true);
                openmenu = true;
            }
        }
    }

    public void IncreaseBulletDamage()
    {
        Weapon bulletComponent = player.GetComponentInChildren<Weapon>();
        if (GameManager.instance.State > 0)
        {
            if (bulletComponent != null)
            {
                bulletComponent.damage += 5;
                Debug.Log("Bullet damage increased by 5. New damage: " + bulletComponent.damage);
            }
            GameManager.instance.State -= 1;
        }
    }

    public void IncreaseMaxhealth() {
        if (GameManager.instance.State > 0)
        {
            GameManager.instance.maxHealth += 5;
            GameManager.instance.State -= 1;
        }
    }

}
