using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthkDisplay : MonoBehaviour
{
    public Text attackPowerText; // ���ݷ��� ǥ���� UI �ؽ�Ʈ
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
            UpdateAttackPowerText();
        }
    }

    void UpdateAttackPowerText()
    {
        Bullet bulletComponent = player.GetComponentInChildren<Bullet>();
        if (bulletComponent != null)
        {
            attackPowerText.text = "Attack Power: " + bulletComponent.damage;
        }
        else
        {
            attackPowerText.text = "Attack Power: N/A";
            Debug.LogError("Bullet component not found on player.");
        }
    }
}
