using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public GameObject Spawner;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    void Awake()
    {
        rigid=GetComponent<Rigidbody2D>();
        spriter=GetComponent<SpriteRenderer>();
        anim=GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        Vector2 nextVec = inputVec*speed*Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);
        if (inputVec.x != 0){
            spriter.flipX = inputVec.x < 0;
        }

    }

    void OnCollisionStay2D(Collision2D collision)//�ǰ�
    {
        if (!GameManager.instance.isLive)//�׾��ִٸ� ����x
        {
            return;
        }
        GameManager.instance.health -= Time.deltaTime * 10;//���� �� ���� 10������
        Debug.Log("�浹Stay");

        if(GameManager.instance.health <= 0)
        {
            for (int i = 1; i < transform.childCount; i++) { 
            transform.GetChild(i).gameObject.SetActive(false);
            }
            Spawner.SetActive(false);

            anim.SetTrigger("Dead");
        }
    }
}
