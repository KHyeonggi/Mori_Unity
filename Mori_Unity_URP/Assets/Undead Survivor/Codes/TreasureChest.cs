using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public int chestId = 7000; // �������� ID
    public Item rewardItem; // ���� ������
    private GameObject player; // �÷��̾� ������Ʈ
    private Inventory inventory; // �κ��丮 ����

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventory = FindObjectOfType<Inventory>();

        if (player == null)
        {
            Debug.LogError("�÷��̾� ��ü�� ã�� �� �����ϴ�. �÷��̾ '�÷��̾�' �±װ� �ִ��� Ȯ���մϴ�.");
        }

        if (inventory == null)
        {
            Debug.LogError("�κ��丮 ��ü�� ã�� �� �����ϴ�.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance <= 2.0f && Input.GetKeyDown(KeyCode.Space))
            {
                InteractWithChest();
            }
        }
    }

    void InteractWithChest()
    {
        if (inventory.AddItem(rewardItem))
        {
            Debug.Log("Item added to inventory.");
            Destroy(gameObject); // �������� ����
            QuestManager.Instance.CheckQuest(chestId); // ����Ʈ ����
        }
        else
        {
            Debug.Log("Inventory is full.");
        }
    }
}
