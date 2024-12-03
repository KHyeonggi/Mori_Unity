using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float moveDistance;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    public bool isLive;

    public Rigidbody2D rigid;
    public Collider2D coll;
    public Animator anim;
    public SpriteRenderer spriter;
    public WaitForFixedUpdate wait;

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
            Debug.LogWarning("Target is not assigned for the enemy.");
            return;
        }

        float distanceToTarget = Vector2.Distance(rigid.position, target.position);

        if (distanceToTarget <= moveDistance)
        {
            Vector2 dirVec = target.position - rigid.position; // ��ġ ���� ���
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; // ���� �����ӿ��� �̵��� ���� ���
            rigid.MovePosition(rigid.position + nextVec); // ���ο� ��ġ�� �̵�
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
            target = GameManager.instance.player.GetComponent<Rigidbody2D>();
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

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spritType];
        speed = data.speed;
        moveDistance = data.moveDistance;
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

    public IEnumerator KnockBack()
    {
        yield return wait;

        if (GameManager.instance.player != null)
        {
            Vector3 playerPos = GameManager.instance.player.transform.position;
            Vector3 dirVec = transform.position - playerPos;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
    }

    public delegate void EnemyDeathHandler(GameObject enemy);
    public event EnemyDeathHandler OnEnemyDeath;

    void Dead()
    {
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath(gameObject);  // �̺�Ʈ ȣ��
        }

        gameObject.SetActive(false);  // �� ��Ȱ��ȭ
    }

    // ���� �ı��ǰų� ��� �� ȣ��� �� �ִ� �޼���
    void OnDestroy()
    {
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath(gameObject);  // �� �ı� �� �̺�Ʈ ȣ��
        }
    }

    public void SetTarget(Rigidbody2D playerTarget)
    {
        target = playerTarget;
        Debug.Log($"Enemy target set: {name} -> {target.name}");
    }
}
