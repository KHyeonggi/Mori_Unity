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
            Debug.LogWarning("Target is not assigned for the enemy.");
            return;
        }

        float distanceToTarget = Vector2.Distance(rigid.position, target.position);

        if (distanceToTarget <= moveDistance)
        {
            Vector2 dirVec = target.position - rigid.position; // 위치 차이 계산
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; // 다음 프레임에서 이동할 벡터 계산
            rigid.MovePosition(rigid.position + nextVec); // 새로운 위치로 이동
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

    public delegate void EnemyDeathHandler(GameObject enemy);
    public event EnemyDeathHandler OnEnemyDeath;

    void Dead()
    {
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath(gameObject);  // 이벤트 호출
        }

        gameObject.SetActive(false);  // 적 비활성화
    }

    // 적이 파괴되거나 사망 시 호출될 수 있는 메서드
    void OnDestroy()
    {
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath(gameObject);  // 적 파괴 시 이벤트 호출
        }
    }

    public void SetTarget(Rigidbody2D playerTarget)
    {
        target = playerTarget;
    }
}
