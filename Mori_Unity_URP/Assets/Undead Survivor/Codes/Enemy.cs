using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy Ŭ������ ���� �ൿ �� ���¸� �����ϴ� ��ũ��Ʈ�Դϴ�.
public class Enemy : MonoBehaviour
{
    // public ������: �ܺο��� ���� ������ ���� �Ӽ���
    public float speed; // ���� �̵� �ӵ�
    public float moveDistance; // �÷��̾ �߰��� �Ÿ� ����
    public float health; // ���� ���� ü��
    public float maxHealth; // ���� �ִ� ü��
    public RuntimeAnimatorController[] animCon; // �ִϸ��̼� ��Ʈ�ѷ� �迭
    public Rigidbody2D target; // ��ǥ�� �Ǵ� �÷��̾��� Rigidbody2D

    // ���� ���� ������
    public bool isLive; // ���� ����ִ��� ����

    // Unity ������Ʈ ���� ������
    public Rigidbody2D rigid; // ���� Rigidbody2D
    public Collider2D coll; // ���� �浹 ü��
    public Animator anim; // ���� �ִϸ�����
    public SpriteRenderer spriter; // ���� ��������Ʈ ������
    public WaitForFixedUpdate wait; // FixedUpdate�� �Բ� ����ϴ� Wait ��ü

    // Awake �޼���: ������Ʈ �ʱ�ȭ
    void Awake()
    {
        coll = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    // FixedUpdate: ���� ���� ���� ����
    void FixedUpdate()
    {
        // ���� ������� �ʰų�, Hit �ִϸ��̼� ���¶�� ���� �ߴ�
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") || !GameManager.instance.gameStarted)
        {
            return;
        }

        // Ÿ���� �������� ���� ��� ��� ����ϰ� �ߴ�
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned for the enemy.");
            return;
        }

        // Ÿ�ٰ��� �Ÿ� ���
        float distanceToTarget = Vector2.Distance(rigid.position, target.position);

        // Ÿ���� moveDistance ���� ���� ��� ���� �̵�
        if (distanceToTarget <= moveDistance)
        {
            Vector2 dirVec = target.position - rigid.position; // �̵� ���� ���
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; // �̵� ���� ���
            rigid.MovePosition(rigid.position + nextVec); // ��ġ �̵�
        }
        rigid.velocity = Vector2.zero; // �̵� �� �ӵ� �ʱ�ȭ
    }

    // LateUpdate: ������ ���� ����
    void LateUpdate()
    {
        if (!isLive || target == null)
            return;

        // ���� �÷��̾ �ٶ󺸰Բ� ��������Ʈ ���� ����
        spriter.flipX = target.position.x < rigid.position.x;
    }

    // OnEnable: ���� Ȱ��ȭ�� �� �ʱ�ȭ
    void OnEnable()
    {
        // GameManager�κ��� �÷��̾� Ÿ�� ����
        if (GameManager.instance != null && GameManager.instance.player != null)
        {
            target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogWarning("Player target not found for the enemy.");
        }

        // �� ���� �ʱ�ȭ
        isLive = true;
        health = maxHealth;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
    }

    // Init: ���� ������ �ʱ�ȭ
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spritType]; // �ִϸ��̼� ��Ʈ�ѷ� ����
        speed = data.speed; // �̵� �ӵ� ����
        moveDistance = data.moveDistance; // �̵� �Ÿ� ����
        maxHealth = data.health; // �ִ� ü�� ����
        health = data.health; // ���� ü�� ����
    }

    // OnTriggerEnter2D: �浹 ���� ó��
    void OnTriggerEnter2D(Collider2D collision)
    {
        // �Ѿ˰��� �浹�� ó��
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        // �Ѿ��� ��������ŭ ü�� ����
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack()); // �˹� ȿ�� ó��

        if (health > 0)
        {
            anim.SetTrigger("Hit"); // Hit �ִϸ��̼� ����
        }
        else
        {
            // ���� �׾��� �� ó��
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.GetExp(); // ����ġ ȹ��
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            StartCoroutine(HandleDeath()); // ���� ó�� �ڷ�ƾ ����
        }
    }

    // HandleDeath: ���� ���� ó��
    IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(1.0f); // 1�� ���
        Dead(); // Dead �޼��� ȣ��
    }

    // KnockBack: �÷��̾�κ��� �ݵ� ȿ��
    public IEnumerator KnockBack()
    {
        yield return wait;

        if (GameManager.instance.player != null)
        {
            Vector3 playerPos = GameManager.instance.player.transform.position; // �÷��̾� ��ġ
            Vector3 dirVec = transform.position - playerPos; // �ݵ� ���� ���
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); // �� �߰�
        }
    }

    // EnemyDeathHandler: ���� ������ �˸��� ��������Ʈ
    public delegate void EnemyDeathHandler(GameObject enemy);
    public event EnemyDeathHandler OnEnemyDeath;

    // Dead: �� ���� ����
    void Dead()
    {
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath(gameObject); // ���� ������ �˸�
        }

        gameObject.SetActive(false); // �� ��Ȱ��ȭ
    }

    // OnDestroy: ���� �ı��� �� ȣ��
    void OnDestroy()
    {
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath(gameObject); // ���� ������ �˸�
        }
    }

    // SetTarget: �÷��̾� Ÿ�� ����
    public void SetTarget(Rigidbody2D playerTarget)
    {
        target = playerTarget;
        Debug.Log($"Enemy target set: {name} -> {target.name}");
    }
}
