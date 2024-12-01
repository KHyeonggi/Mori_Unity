using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Health:
                myText.text = "체력: " + GameManager.instance.maxHealth;
                break;

            case InfoType.Attack://체력바 체력 수치
                UpdateAttackPowerText();
                break;

            case InfoType.State:
                myText.text = "보유 스텟: " + GameManager.instance.State;
                break;
        }
    }

    void UpdateAttackPowerText()
    {
        Bullet bulletComponent = player.GetComponentInChildren<Bullet>();
        if (bulletComponent != null)
        {
            myText.text = "공격력: " + bulletComponent.damage;
        }
        else
        {
            myText.text = "Attack Power: N/A";
            Debug.LogError("Bullet component not found on player.");
        }
    }
}
