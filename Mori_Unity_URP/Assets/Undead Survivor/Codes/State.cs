using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
       
    }

    void LateStart()
    {
        // "Player" �±װ� ���� ���� ������Ʈ�� ã�� �����մϴ�.
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("first latestart");
        if (player == null)
        {
            Debug.Log("Player object not found. Make sure the player has the 'Player' tag.");
        }
    }

    void LateUpdate()
    {
        if (player == null)
        {
            // Player�� ã�� ���� ��� LateStart�� �ٽ� ȣ���Ͽ� �õ�
            LateStart();
            if (player == null)
            {
                Debug.LogWarning("Player object is still not found in LateUpdate.");
                return; // ������ null�̶�� �����Ͽ� NullReferenceException ����
            }
        }

        switch (type)
        {
            case InfoType.Health:
                myText.text = "ü��: " + GameManager.instance.maxHealth;
                break;

            case InfoType.Attack: // ���ݷ� ���� ������Ʈ
                UpdateAttackPowerText();
                break;

            case InfoType.State:
                myText.text = "���� ����: " + GameManager.instance.State;
                break;
        }
    }

    void UpdateAttackPowerText()
    {
        if (player == null)
        {
            Debug.LogWarning("Player object is null in UpdateAttackPowerText.");
            return; // player�� null�̶�� �������� ����
        }

        Weapon bulletComponent = player.GetComponentInChildren<Weapon>();
        if (bulletComponent != null)
        {
            myText.text = "���ݷ�: " + bulletComponent.damage;
        }
        else
        {
            myText.text = "Attack Power: N/A";
            Debug.LogError("Weapon component not found on player.");
        }
    }
}
