using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class increaseState : MonoBehaviour
{
    public static bool openmenu = false; // 메뉴 열림 여부
    public GameObject StateMenu;
    public Player player;
    public GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        // 일정 시간을 대기 후 Player 오브젝트를 찾기
        yield return new WaitForSeconds(0.5f);

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            Debug.LogError("Player object not found. Make sure the player has the 'Player' tag.");
        }
        else
        {
            player = playerObject.GetComponent<Player>();
            if (player == null)
            {
                Debug.LogError("Player component not found on Player object.");
            }
            else
            {
                Debug.Log("Player successfully found: " + player.name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt)) // alt키 입력시 호출
        {
            if (openmenu)
            {
                StateMenu.SetActive(false);
                openmenu = false;
            }
            else
            {
                StateMenu.SetActive(true);
                openmenu = true;
            }
        }
    }

    public void IncreaseBulletDamage()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is null. Cannot increase bullet damage.");
            return;
        }

        Weapon bulletComponent = player.GetComponentInChildren<Weapon>();
        if (GameManager.instance.State > 0)
        {
            if (bulletComponent != null)
            {
                bulletComponent.damage += 5;
                Debug.Log("Bullet damage increased by 5. New damage: " + bulletComponent.damage);
            }
            else
            {
                Debug.LogError("Weapon component not found on player.");
            }
            GameManager.instance.State -= 1;
        }
    }

    public void IncreaseMaxhealth()
    {
        if (GameManager.instance.State > 0)
        {
            GameManager.instance.maxHealth += 5;
            GameManager.instance.State -= 1;
            Debug.Log("Max health increased by 5. New max health: " + GameManager.instance.maxHealth);
        }
    }
}
