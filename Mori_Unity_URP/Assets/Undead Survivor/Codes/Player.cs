using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.ShaderGraph;
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

    public GameManager manager;
    Vector3 dirVec;
    GameObject scanObject;
    public GameObject talkPanel;
    private PlayerControls controls;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }





    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey("up"))//위에있는 오브젝트 인식
            dirVec = Vector3.up;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey("down"))//아래에 있는 오브젝트 인식
            dirVec = Vector3.down;
        else if (Input.GetKey(KeyCode.A) || Input.GetKey("left"))//왼쪽에 있는 오브젝트 인식
            dirVec = Vector3.left;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey("right"))//오른쪽에 있는 오브젝트 인식
            dirVec = Vector3.right;

    }
    private void OnInspect()//오브젝트 인식
    {
        if (scanObject != null)
        {
            manager.Action(scanObject);
        }
    }

    void FixedUpdate()
    {
        Vector2 nextVec = inputVec*speed*Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        //Ray
        Debug.DrawRay(rigid.position, dirVec * 0.7f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
            scanObject = null;
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

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (GameManager.instance.health <= 0)
            {
                anim.SetTrigger("Dead");
                GameManager.instance.GameOver();
            }

            else {
                GameManager.instance.health -= Time.deltaTime * 10;//닿을 때 마다 10데미지
                //Debug.Log("충돌Stay");
            }
            
        }
        //if (GameManager.instance.isLive == false)//죽어있다면 실행x
        //{
        //    return;
        //}
    }
}

internal class PlayerControls
{
}