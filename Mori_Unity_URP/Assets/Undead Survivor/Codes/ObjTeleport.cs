using UnityEngine;

public class ObjTeleport : MonoBehaviour
{
    public Transform targetLocation; // 순간이동할 위치
    public float interactionDistance = 2.0f; // 상호작용 가능한 거리
    private GameObject player; // 플레이어 오브젝트

    void Start()
    {
        // "Player" 태그가 붙은 게임 오브젝트를 찾아 참조합니다.
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
            // 플레이어와 이 오브젝트 사이의 거리 계산
            float distance = Vector3.Distance(player.transform.position, transform.position);

            // 상호작용 거리 내에 있고 스페이스바를 눌렀을 때
            if (distance <= interactionDistance && Input.GetKeyDown(KeyCode.Space))
            {
                TeleportPlayer();
            }
        }
    }

    void TeleportPlayer()
    {
        // targetLocation으로 플레이어의 위치를 순간이동
        if (targetLocation != null)
        {
            player.transform.position = targetLocation.position;
            Debug.Log("Player teleported to target location.");
        }
        else
        {
            Debug.LogError("Target location not set.");
        }
    }
}
