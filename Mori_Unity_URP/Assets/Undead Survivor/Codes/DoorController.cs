using UnityEngine;
using System.Linq;

public class RoomController2D : MonoBehaviour
{
    public GameObject door; // �� ������Ʈ
    private int enemyCount; // ���� �� ��

    private bool playerInRoom = false;
    private bool roomCleared = false;

    void OnEnable()
    {
        Enemy.OnEnemyDeath += OnEnemyDeath;
    }

    void OnDisable()
    {
        Enemy.OnEnemyDeath -= OnEnemyDeath;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRoom = true;
            FindEnemiesInRoom();
            CloseDoor();
            Debug.Log("�÷��̾ �濡 ���Խ��ϴ�. ���� �ݽ��ϴ�.");
        }
    }

    void FindEnemiesInRoom()
    {
        BoxCollider2D roomCollider = GetComponent<BoxCollider2D>();
        if (roomCollider == null)
        {
            Debug.LogError("RoomController2D�� BoxCollider2D�� �����ϴ�!");
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, roomCollider.size, 0);
        enemyCount = colliders.Count(coll => coll.CompareTag("Enemy"));

        Debug.Log($"�濡�� {enemyCount}���� ���� ã�ҽ��ϴ�.");
    }

    void CloseDoor()
    {
        if (door != null)
        {
            door.SetActive(true);
            Debug.Log("���� �������ϴ�.");
        }
        else
        {
            Debug.LogError("door ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�!");
        }
    }

    void OpenDoor()
    {
        if (door != null)
        {
            door.SetActive(false);
            Debug.Log("���� ���Ƚ��ϴ�.");
        }
        else
        {
            Debug.LogError("door ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�!");
        }
    }

    void OnEnemyDeath(GameObject enemy)
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            roomCleared = true;
            OpenDoor();
            Debug.Log("��� ���� óġ�Ǿ����ϴ�. ���� ���ϴ�.");
        }
    }
}