using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

    void Awake()
    {
        rigid=GetComponent<Rigidbody2D>();
        spriter=GetComponent<SpriteRenderer>();
        anim=GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) //위에있는 오브젝트 인식
            dirVec = Vector3.up;
        if (Input.GetKey("up"))
            dirVec = Vector3.up;

        else if (Input.GetKeyDown(KeyCode.S)) //아래에 있는 오브젝트 인식
            dirVec = Vector3.down;
        else if (Input.GetKey("down"))
            dirVec = Vector3.down;

        else if (Input.GetKeyDown(KeyCode.A)) //왼쪽에 있는 오브젝트 인식
            dirVec = Vector3.left;
        else if (Input.GetKey("left"))
            dirVec = Vector3.left;

        else if ((Input.GetKeyDown(KeyCode.D))) //오른쪽에 있는 오브젝트 인식
            dirVec = Vector3.right;
        else if (Input.GetKey("right"))
            dirVec = Vector3.right;
        
            //Scan Object 
        if (Input.GetButtonDown("Jump") && scanObject != null) //오브젝트 앞에서 점프를 누르면 대화창이 나옴
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
