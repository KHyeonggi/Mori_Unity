using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class State : MonoBehaviour
{
    private GameObject player; // �÷��̾� ������Ʈ

    Text myText;
    public enum InfoType { Attack, Health, State }
    public InfoType type;

    private void Awake()
    {
        myText = GetComponent<Text>();
    }
    void Start()
    {
        // "Player" �±װ� ���� ���� ������Ʈ�� ã�� �����մϴ�.
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found. Make sure the player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            UpdateAttackPowerText();
        }
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Health:
                myText.text = "ü��: " + GameManager.instance.maxHealth;
                break;

            case InfoType.Attack://ü�¹� ü�� ��ġ
                UpdateAttackPowerText();
                break;

            case InfoType.State:
                myText.text = "���� ����: " + GameManager.instance.State;
                break;
        }
    }

    void UpdateAttackPowerText()
    {
        Bullet bulletComponent = player.GetComponentInChildren<Bullet>();
        if (bulletComponent != null)
        {
            myText.text = "���ݷ�: " + bulletComponent.damage;
        }
        else
        {
            myText.text = "Attack Power: N/A";
            Debug.LogError("Bullet component not found on player.");
        }
    }
}
