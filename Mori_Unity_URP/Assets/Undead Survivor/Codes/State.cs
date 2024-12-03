using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class State : MonoBehaviour
{
    private GameObject player; // 플레이어 오브젝트

    Text myText;
    public enum InfoType { Attack, Health, State }
    public InfoType type;

    private void Awake()
    {
        myText = GetComponent<Text>();
    }

    void Start()
    {
       
    }

    void LateStart()
    {
        // "Player" 태그가 붙은 게임 오브젝트를 찾아 참조합니다.
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("first latestart");
        if (player == null)
        {
            Debug.Log("Player object not found. Make sure the player has the 'Player' tag.");
        }
    }

    void LateUpdate()
    {
        if (player == null)
        {
            // Player를 찾지 못한 경우 LateStart를 다시 호출하여 시도
            LateStart();
            if (player == null)
            {
                Debug.LogWarning("Player object is still not found in LateUpdate.");
                return; // 여전히 null이라면 리턴하여 NullReferenceException 방지
            }
        }

        switch (type)
        {
            case InfoType.Health:
                myText.text = "체력: " + GameManager.instance.maxHealth;
                break;

            case InfoType.Attack: // 공격력 정보 업데이트
                UpdateAttackPowerText();
                break;

            case InfoType.State:
                myText.text = "보유 스텟: " + GameManager.instance.State;
                break;
        }
    }

    void UpdateAttackPowerText()
    {
        if (player == null)
        {
            Debug.LogWarning("Player object is null in UpdateAttackPowerText.");
            return; // player가 null이라면 실행하지 않음
        }

        Weapon bulletComponent = player.GetComponentInChildren<Weapon>();
        if (bulletComponent != null)
        {
            myText.text = "공격력: " + bulletComponent.damage;
        }
        else
        {
            myText.text = "Attack Power: N/A";
            Debug.LogError("Weapon component not found on player.");
        }
    }
}
