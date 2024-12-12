using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthkDisplay : MonoBehaviour
{
    public Text attackPowerText; // 공격력을 표시할 UI 텍스트
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
