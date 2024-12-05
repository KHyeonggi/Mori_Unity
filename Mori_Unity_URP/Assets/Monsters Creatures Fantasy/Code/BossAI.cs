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
    public float attackDamage = 20f; // 보스의 공격 데미지
    public float knockBackForce = 2f; // 넉백 크기
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;
    bool isAttacking;
    bool isInvincible; // 무적 상태 플래그 추가

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
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") || !GameManager.instance.gameStarted || isAttacking)
        {
            rigid.velocity = Vector2.zero; // 이동 중단
            return;
        }

        if (target == null)
        {
            Debug.LogWarning("Target is not assigned for the boss.");
            rigid.velocity = Vector2.zero; // 이동 중단
            return;
        }

        float distanceToTarget = Vector2.Distance(rigid.position, target.position);

        // Attack 상태가 끝난 경우 Walk로 전환
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            anim.SetBool("isWalking", true); // Walk 상태로 전환
        }

        if (distanceToTarget <= attackRange)
        {
            rigid.velocity = Vector2.zero; // 공격 범위에 도달했으므로 이동 중단
            StartCoroutine(AttackWithDelayAndDoubleDelay());
        }
        else if (distanceToTarget <= moveDistance)
        {
            anim.SetBool("isWalking", true); // Walk 상태 활성화
            MoveTowardsPlayer(); // 플레이어를 추적
        }
        else
        {
            rigid.velocity = Vector2.zero; // 추적 범위를 벗어났으므로 이동 중단
            anim.SetBool("isWalking", false); // Walk 상태 비활성화
        }
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
            Debug.LogWarning("Player target not found for the Boss. Waiting for player to be set.");
        }

        isLive = true;
        health = maxHealth;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        isInvincible = false; // 무적 상태 초기화
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
        if (!collision.CompareTag("Bullet") || !isLive || isInvincible)
            return;

        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet == null) return;

        health -= bullet.damage;
        StartCoroutine(KnockBack());
        StartCoroutine(EnableInvincibility(1.0f)); // 1초 무적 상태 부여

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

            Dead();
        }
    }

    IEnumerator EnableInvincibility(float duration)
    {
        isInvincible = true; // 무적 상태 활성화

        // 스프라이트 깜빡임 효과
        float elapsed = 0f;
        while (elapsed < duration)
        {
            spriter.color = new Color(1, 1, 1, 0.5f); // 반투명
            yield return new WaitForSeconds(0.1f);
            spriter.color = Color.white; // 원래 색상
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.2f;
        }

        isInvincible = false; // 무적 상태 해제
        spriter.color = Color.white; // 원래 색상 복원
    }

    IEnumerator KnockBack()
    {
        yield return wait;

        if (GameManager.instance.player != null)
        {
            Vector3 playerPos = GameManager.instance.player.transform.position;
            Vector3 dirVec = transform.position - playerPos;
            rigid.AddForce(dirVec.normalized * knockBackForce, ForceMode2D.Impulse);
        }
    }

    public delegate void BossDeathHandler(GameObject boss);
    public event BossDeathHandler OnBossDeath;

    void Dead()
    {
        isLive = false;
        coll.enabled = false;
        rigid.simulated = false;
        spriter.sortingOrder = 1;
        anim.SetBool("Dead", true);

        if (OnBossDeath != null)
        {
            OnBossDeath(gameObject); // 이벤트 호출
        }
        GameManager.instance.BossDefeated();

        StartCoroutine(DisableAfterAnimation(1.0f)); // 1초 후 비활성화
    }

    IEnumerator DisableAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false); // 비활성화
    }

    public void SetTarget(Rigidbody2D playerTarget)
    {
        if (playerTarget == null)
        {
            Debug.LogWarning($"SetTarget called but playerTarget is null in {name}");
            return;
        }

        target = playerTarget;
        Debug.Log($"BossAI target set: {name} -> {target.name}");
    }

    IEnumerator AttackWithDelayAndDoubleDelay()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        anim.SetBool("isWalking", false); // 공격 중 Walk 비활성화
        Debug.Log("Boss is attacking the player!");

        if (GameManager.instance != null)
        {
            GameManager.instance.TakeDamage(attackDamage);
            Debug.Log($"Player took {attackDamage} damage from the boss.");
        }
        else
        {
            Debug.LogError("GameManager instance is null, unable to call TakeDamage.");
        }

        yield return new WaitForSeconds(1.0f); // 공격 간 딜레이 추가
        isAttacking = false;
        anim.SetBool("isWalking", true); // Walk 상태로 전환
    }


    void MoveTowardsPlayer()
    {
        Vector2 direction = (target.position - rigid.position).normalized;
        rigid.velocity = direction * speed; // 이동 방향에 속도 설정
    }
}
