using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public float speed;
    public float moveDistance; // �÷��̾ �����ϴ� �ִ� �Ÿ�
    public float attackRange;  // �÷��̾ ������ �Ÿ�
    public float health;
    public float maxHealth;
    public float attackDamage = 20f; // ������ ���� ������
    public float knockBackForce = 2f; // �˹� ũ��
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;
    bool isAttacking;
    bool isInvincible; // ���� ���� �÷��� �߰�

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
            rigid.velocity = Vector2.zero; // �̵� �ߴ�
            return;
        }

        if (target == null)
        {
            Debug.LogWarning("Target is not assigned for the boss.");
            rigid.velocity = Vector2.zero; // �̵� �ߴ�
            return;
        }

        float distanceToTarget = Vector2.Distance(rigid.position, target.position);

        // Attack ���°� ���� ��� Walk�� ��ȯ
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            anim.SetBool("isWalking", true); // Walk ���·� ��ȯ
        }

        if (distanceToTarget <= attackRange)
        {
            rigid.velocity = Vector2.zero; // ���� ������ ���������Ƿ� �̵� �ߴ�
            StartCoroutine(AttackWithDelayAndDoubleDelay());
        }
        else if (distanceToTarget <= moveDistance)
        {
            anim.SetBool("isWalking", true); // Walk ���� Ȱ��ȭ
            MoveTowardsPlayer(); // �÷��̾ ����
        }
        else
        {
            rigid.velocity = Vector2.zero; // ���� ������ ������Ƿ� �̵� �ߴ�
            anim.SetBool("isWalking", false); // Walk ���� ��Ȱ��ȭ
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
        isInvincible = false; // ���� ���� �ʱ�ȭ
    }

    public void Init(SpawnDataBoss data)
    {
        anim.runtimeAnimatorController = animCon[data.spritType];
        speed = data.speed;
        moveDistance = data.moveDistance;
        attackRange = data.attackRange; // ���� ���� ����
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
        StartCoroutine(EnableInvincibility(1.0f)); // 1�� ���� ���� �ο�

        if (health > 0) // ��� ���� ��
        {
            anim.SetTrigger("Hit");
        }
        else // �׾��� ��
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
        isInvincible = true; // ���� ���� Ȱ��ȭ

        // ��������Ʈ ������ ȿ��
        float elapsed = 0f;
        while (elapsed < duration)
        {
            spriter.color = new Color(1, 1, 1, 0.5f); // ������
            yield return new WaitForSeconds(0.1f);
            spriter.color = Color.white; // ���� ����
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.2f;
        }

        isInvincible = false; // ���� ���� ����
        spriter.color = Color.white; // ���� ���� ����
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
            OnBossDeath(gameObject); // �̺�Ʈ ȣ��
        }
        GameManager.instance.BossDefeated();

        StartCoroutine(DisableAfterAnimation(1.0f)); // 1�� �� ��Ȱ��ȭ
    }

    IEnumerator DisableAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false); // ��Ȱ��ȭ
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
        anim.SetBool("isWalking", false); // ���� �� Walk ��Ȱ��ȭ
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

        yield return new WaitForSeconds(1.0f); // ���� �� ������ �߰�
        isAttacking = false;
        anim.SetBool("isWalking", true); // Walk ���·� ��ȯ
    }


    void MoveTowardsPlayer()
    {
        Vector2 direction = (target.position - rigid.position).normalized;
        rigid.velocity = direction * speed; // �̵� ���⿡ �ӵ� ����
    }
}
