using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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
        coll =GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void FixedUpdate()
    {
        if (!isLive|| anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")){
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
        if (!isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }
    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
        isLive =true;
        coll.enabled = true;
        rigid.simulated=true;
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

        if (health>0)//살았을때
        {
            anim.SetTrigger("Hit");
        }
        else//죽었을때
        {
            isLive =false;
            coll.enabled = false;
            rigid.simulated=false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.GetExp();
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
    public delegate void EnemyDeathHandler(GameObject enemy);
    public static event EnemyDeathHandler OnEnemyDeath;

    void OnDestroy()
    {
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath(gameObject);
        }
    }
}
