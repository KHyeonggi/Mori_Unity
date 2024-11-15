using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public float speed;
    public float moveDistance; // 플레이어를 추적하는 최대 거리
    public float attackRange;  // 플레이어를 공격할 거리
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void FixedUpdate()
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") || !GameManager.instance.gameStarted)
        {
            return;
        }

        if (target == null)
        {
            Debug.LogWarning("Target is not assigned for the boss.");
            return;
        }

        float distanceToTarget = Vector2.Distance(rigid.position, target.position);

        if (distanceToTarget <= attackRange)
        {
            // 공격 모션 실행
            Attack();
        }
        else if (distanceToTarget <= moveDistance)
        {
            // 플레이어 추적
            MoveTowardsPlayer();
        }

        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!isLive || target == null)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        if (GameManager.instance != null && GameManager.instance.player != null)
        {
            SetTarget(GameManager.instance.player.GetComponent<Rigidbody2D>());

        }
        else
        {
            Debug.LogWarning("Player target not found for the enemy.");
        }
        
        isLive = true;
        health = maxHealth;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
    }

    public void Init(SpawnDataBoss data)
    {
        anim.runtimeAnimatorController = animCon[data.spritType];
        speed = data.speed;
        moveDistance = data.moveDistance;
        attackRange = data.attackRange; // 공격 범위 설정
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0) // 살아 있을 때
        {
            anim.SetTrigger("Hit");
        }
        else // 죽었을 때
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.GetExp();

            // 적이 죽었을 때 Dead() 메서드를 일정 시간 후 호출하여 비활성화
            StartCoroutine(HandleDeath());
        }
    }

    IEnumerator HandleDeath()
    {
        // 죽음 애니메이션 재생 후 잠시 기다렸다가 Dead() 호출
        yield return new WaitForSeconds(1.0f); // 애니메이션 길이에 따라 조정 가능
        Dead();
    }

    IEnumerator KnockBack()
    {
        yield return wait;

        if (GameManager.instance.player != null)
        {
            Vector3 playerPos = GameManager.instance.player.transform.position;
            Vector3 dirVec = transform.position - playerPos;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
    }

    public delegate void BossDeathHandler(GameObject boss);
    public event BossDeathHandler OnBossDeath;

    void Dead()
    {
        if (OnBossDeath != null)
        {
            OnBossDeath(gameObject);  // 이벤트 호출
        }

        gameObject.SetActive(false);  // 보스 비활성화
    }

    public void SetTarget(Rigidbody2D playerTarget)
    {
        target = playerTarget;
        Debug.Log($"Target set for BossAI: {target}");
    }

    // 플레이어를 공격하는 메서드
    void Attack()
    {
        anim.SetTrigger("Attack");
        Debug.Log("Boss is attacking the player!");
        // 공격 로직 추가 가능 (예: 플레이어에게 데미지)
    }

    // 플레이어를 향해 이동하는 메서드
    void MoveTowardsPlayer()
    {
        Vector2 direction = (target.position - rigid.position).normalized;
        rigid.MovePosition(rigid.position + direction * speed * Time.fixedDeltaTime);
    }
}
