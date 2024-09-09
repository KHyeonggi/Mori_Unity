using UnityEngine;
using System.Linq;

public class RoomController2D : MonoBehaviour
{
    public GameObject door; // 문 오브젝트
    private int enemyCount; // 방의 적 수

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
            Debug.Log("플레이어가 방에 들어왔습니다. 문을 닫습니다.");
        }
    }

    void FindEnemiesInRoom()
    {
        BoxCollider2D roomCollider = GetComponent<BoxCollider2D>();
        if (roomCollider == null)
        {
            Debug.LogError("RoomController2D에 BoxCollider2D가 없습니다!");
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, roomCollider.size, 0);
        enemyCount = colliders.Count(coll => coll.CompareTag("Enemy"));

        Debug.Log($"방에서 {enemyCount}개의 적을 찾았습니다.");
    }

    void CloseDoor()
    {
        if (door != null)
        {
            door.SetActive(true);
            Debug.Log("문이 닫혔습니다.");
        }
        else
        {
            Debug.LogError("door 오브젝트가 할당되지 않았습니다!");
        }
    }

    void OpenDoor()
    {
        if (door != null)
        {
            door.SetActive(false);
            Debug.Log("문이 열렸습니다.");
        }
        else
        {
            Debug.LogError("door 오브젝트가 할당되지 않았습니다!");
        }
    }

    void OnEnemyDeath(GameObject enemy)
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            roomCleared = true;
            OpenDoor();
            Debug.Log("모든 적이 처치되었습니다. 문을 엽니다.");
        }
    }
}