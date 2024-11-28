using UnityEngine;
using System.Linq;

public class RoomController2D : MonoBehaviour
{
    public GameObject[] doors; // ���� ���� �� ������Ʈ
    private int enemyCount; // ���� �� ��
    private bool playerInRoom = false;
    private bool roomCleared = false;

    private BoxCollider2D roomCollider;  // BoxCollider2D ĳ��
    private Collider2D[] roomEnemies;  // �� ���� ���� ĳ��

    void Start()
    {
        // BoxCollider2D ĳ��
        roomCollider = GetComponent<BoxCollider2D>();
        if (roomCollider == null)
        {
            Debug.LogError("RoomController2D�� BoxCollider2D�� �����ϴ�!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !roomCleared)
        {
            playerInRoom = true;
            FindEnemiesInRoom();
            CloseDoors();
            Debug.Log("�÷��̾ �濡 ���Խ��ϴ�. ���� �ݽ��ϴ�.");
        }
    }

    void FindEnemiesInRoom()
    {
        if (roomCollider == null)
        {
            Debug.LogError("RoomController2D�� BoxCollider2D�� �����ϴ�!");
            return;
        }

        // �� ���� ��� ���� ã��
        roomEnemies = Physics2D.OverlapBoxAll(transform.position, roomCollider.size, 0).Where(coll => coll.CompareTag("Enemy")).ToArray();
        enemyCount = roomEnemies.Length;

        // �� ���� OnEnemyDeath �̺�Ʈ�� ����
        foreach (var enemy in roomEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.OnEnemyDeath += OnEnemyDeath;  // ���� �� ���� ���� �� �� �濡�� ó��
            }
        }

        Debug.Log($"�濡�� {enemyCount}���� ���� ã�ҽ��ϴ�.");
    }

    void CloseDoors()
    {
        if (doors != null && doors.Length > 0)
        {
            foreach (GameObject door in doors)
            {
                if (door != null)
                {
                    door.SetActive(true);
                    Debug.Log($"{door.name} ���� �������ϴ�.");
                }
            }
        }
        else
        {
            Debug.LogError("�� ������Ʈ�� �Ҵ���� �ʾҰų� ��� �ֽ��ϴ�!");
        }
    }

    void OpenDoors()
    {
        if (doors != null && doors.Length > 0)
        {
            foreach (GameObject door in doors)
            {
                if (door != null)
                {
                    door.SetActive(false);
                    Debug.Log($"{door.name} ���� ���Ƚ��ϴ�.");
                }
            }
        }
        else
        {
            Debug.LogError("�� ������Ʈ�� �Ҵ���� �ʾҰų� ��� �ֽ��ϴ�!");
        }
    }

    // �ش� ���� ���� ���� ���� �̺�Ʈ �߻�
    void OnEnemyDeath(GameObject enemy)
    {
        enemyCount--;

        Debug.Log($"{enemy.name}�� �׾����ϴ�. ���� �� ��: {enemyCount}");

        if (enemyCount <= 0)
        {
            roomCleared = true;
            OpenDoors();
            Debug.Log("��� ���� óġ�Ǿ����ϴ�. ���� ���ϴ�.");

            // BoxCollider2D ����
            if (roomCollider != null)
            {
                Destroy(roomCollider);
                Debug.Log("���� Ŭ����Ǿ� BoxCollider2D�� �����Ǿ����ϴ�.");
            }
        }
    }

    // ���� �ı��ǰų� ��Ȱ��ȭ�� �� �̺�Ʈ ���� ����
    void OnDisable()
    {
        // �� ���� ������ ���� ��� OnEnemyDeath �̺�Ʈ ���� ����
        if (roomEnemies != null)
        {
            foreach (var enemy in roomEnemies)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.OnEnemyDeath -= OnEnemyDeath;
                }
            }
        }
    }
}
