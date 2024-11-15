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
            // ���� ��� ����
            Attack();
        }
        else if (distanceToTarget <= moveDistance)
        {
            // �÷��̾� ����
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
        attackRange = data.attackRange; // ���� ���� ����
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

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

            // ���� �׾��� �� Dead() �޼��带 ���� �ð� �� ȣ���Ͽ� ��Ȱ��ȭ
            StartCoroutine(HandleDeath());
        }
    }

    IEnumerator HandleDeath()
    {
        // ���� �ִϸ��̼� ��� �� ��� ��ٷȴٰ� Dead() ȣ��
        yield return new WaitForSeconds(1.0f); // �ִϸ��̼� ���̿� ���� ���� ����
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
            OnBossDeath(gameObject);  // �̺�Ʈ ȣ��
        }

        gameObject.SetActive(false);  // ���� ��Ȱ��ȭ
    }

    public void SetTarget(Rigidbody2D playerTarget)
    {
        target = playerTarget;
        Debug.Log($"Target set for BossAI: {target}");
    }

    // �÷��̾ �����ϴ� �޼���
    void Attack()
    {
        anim.SetTrigger("Attack");
        Debug.Log("Boss is attacking the player!");
        // ���� ���� �߰� ���� (��: �÷��̾�� ������)
    }

    // �÷��̾ ���� �̵��ϴ� �޼���
    void MoveTowardsPlayer()
    {
        Vector2 direction = (target.position - rigid.position).normalized;
        rigid.MovePosition(rigid.position + direction * speed * Time.fixedDeltaTime);
    }
}
