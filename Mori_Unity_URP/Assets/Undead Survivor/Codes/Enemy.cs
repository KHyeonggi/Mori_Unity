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
    Animator anim;
    SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!isLive){
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
        if (!isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }
    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
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

        if (health>0)//�������
        {

        }
        else//�׾�����
        {
            Dead();
            GameManager.instance.GetExp();
        }
    }
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
