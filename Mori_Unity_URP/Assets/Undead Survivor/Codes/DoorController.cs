using UnityEngine;
using System.Linq;

public class RoomController2D : MonoBehaviour
{
    public GameObject[] doors; // 여러 개의 문 오브젝트
    private int enemyCount; // 방의 적 수
    private bool playerInRoom = false;
    private bool roomCleared = false;

    private BoxCollider2D roomCollider;  // BoxCollider2D 캐시
    private Collider2D[] roomEnemies;  // 방 안의 적들 캐시

    void Start()
    {
        // BoxCollider2D 캐시
        roomCollider = GetComponent<BoxCollider2D>();
        if (roomCollider == null)
        {
            Debug.LogError("RoomController2D에 BoxCollider2D가 없습니다!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !roomCleared)
        {
            playerInRoom = true;
            FindEnemiesInRoom();
            CloseDoors();
            Debug.Log("플레이어가 방에 들어왔습니다. 문을 닫습니다.");
        }
    }

    void FindEnemiesInRoom()
    {
        if (roomCollider == null)
        {
            Debug.LogError("RoomController2D에 BoxCollider2D가 없습니다!");
            return;
        }

        // 방 안의 모든 적을 찾음
        roomEnemies = Physics2D.OverlapBoxAll(transform.position, roomCollider.size, 0).Where(coll => coll.CompareTag("Enemy")).ToArray();
        enemyCount = roomEnemies.Length;

        // 각 적의 OnEnemyDeath 이벤트에 구독
        foreach (var enemy in roomEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.OnEnemyDeath += OnEnemyDeath;  // 방의 각 적이 죽을 때 이 방에서 처리
            }
        }

        Debug.Log($"방에서 {enemyCount}개의 적을 찾았습니다.");
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
                    Debug.Log($"{door.name} 문이 닫혔습니다.");
                }
            }
        }
        else
        {
            Debug.LogError("문 오브젝트가 할당되지 않았거나 비어 있습니다!");
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
                    Debug.Log($"{door.name} 문이 열렸습니다.");
                }
            }
        }
        else
        {
            Debug.LogError("문 오브젝트가 할당되지 않았거나 비어 있습니다!");
        }
    }

    // 해당 방의 적이 죽을 때만 이벤트 발생
    void OnEnemyDeath(GameObject enemy)
    {
        enemyCount--;

        Debug.Log($"{enemy.name}가 죽었습니다. 남은 적 수: {enemyCount}");

        if (enemyCount <= 0)
        {
            roomCleared = true;
            OpenDoors();
            Debug.Log("모든 적이 처치되었습니다. 문을 엽니다.");

            // BoxCollider2D 삭제
            if (roomCollider != null)
            {
                Destroy(roomCollider);
                Debug.Log("방이 클리어되어 BoxCollider2D가 삭제되었습니다.");
            }
        }
    }

    // 방이 파괴되거나 비활성화될 때 이벤트 구독 해제
    void OnDisable()
    {
        // 방 안의 적들이 있을 경우 OnEnemyDeath 이벤트 구독 해제
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
