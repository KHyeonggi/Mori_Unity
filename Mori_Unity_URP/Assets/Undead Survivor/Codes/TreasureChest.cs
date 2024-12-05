using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public int chestId = 7000; // 보물상자 ID
    public Item rewardItem; // 보상 아이템
    private GameObject player; // 플레이어 오브젝트
    private Inventory inventory; // 인벤토리 참조

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventory = FindObjectOfType<Inventory>();

        if (player == null)
        {
            Debug.LogError("플레이어 개체를 찾을 수 없습니다. 플레이어에 '플레이어' 태그가 있는지 확인합니다.");
        }

        if (inventory == null)
        {
            Debug.LogError("인벤토리 개체를 찾을 수 없습니다.");
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
            Destroy(gameObject); // 보물상자 삭제
            QuestManager.Instance.CheckQuest(chestId); // 퀘스트 진행
        }
        else
        {
            Debug.Log("Inventory is full.");
        }
    }
}
