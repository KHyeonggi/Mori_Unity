using UnityEngine;

public class ObjTeleport : MonoBehaviour
{
    public Transform targetLocation; // �����̵��� ��ġ
    public float interactionDistance = 2.0f; // ��ȣ�ۿ� ������ �Ÿ�
    private GameObject player; // �÷��̾� ������Ʈ

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
            // �÷��̾�� �� ������Ʈ ������ �Ÿ� ���
            float distance = Vector3.Distance(player.transform.position, transform.position);

            // ��ȣ�ۿ� �Ÿ� ���� �ְ� �����̽��ٸ� ������ ��
            if (distance <= interactionDistance && Input.GetKeyDown(KeyCode.Space))
            {
                TeleportPlayer();
            }
        }
    }

    void TeleportPlayer()
    {
        // targetLocation���� �÷��̾��� ��ġ�� �����̵�
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
