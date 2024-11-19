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
    //private PlayerControls controls;

    private bool isLive = true; // 플레이어 생존 여부

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isLive) return; // 플레이어가 죽은 경우 행동 중지

        if (Input.GetKey(KeyCode.W) || Input.GetKey("up")) // 위에 있는 오브젝트 인식
            dirVec = Vector3.up;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey("down")) // 아래에 있는 오브젝트 인식
            dirVec = Vector3.down;
        else if (Input.GetKey(KeyCode.A) || Input.GetKey("left")) // 왼쪽에 있는 오브젝트 인식
            dirVec = Vector3.left;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey("right")) // 오른쪽에 있는 오브젝트 인식
            dirVec = Vector3.right;
    }

    private void OnInspect() // 오브젝트 인식
    {
        if (!isLive) return; // 플레이어가 죽은 경우 행동 중지

        if (scanObject != null)
        {
            manager.Action(scanObject);
        }
    }

    void FixedUpdate()
    {
        if (!isLive) return; // 플레이어가 죽은 경우 이동 중지

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        // Ray
        Debug.DrawRay(rigid.position, dirVec * 0.7f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }
    }

    void OnMove(InputValue value)
    {
        if (!isLive) return; // 플레이어가 죽은 경우 이동 중지

        inputVec = value.Get<Vector2>();
    }

    void LateUpdate()
    {
        if (!isLive) return; // 플레이어가 죽은 경우 애니메이션 중지

        anim.SetFloat("Speed", inputVec.magnitude);
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!isLive) return; // 이미 죽은 경우 로직 실행하지 않음

        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("BossAI"))
        {
            GameManager.instance.health -= Time.deltaTime * 10; // 닿을 때마다 10데미지
            if (GameManager.instance.health <= 0)
            {
                if (isLive) // 처음 죽었을 때만 실행
                {
                    isLive = false; // 플레이어 사망 상태로 변경
                    anim.SetTrigger("Dead");
                    GameManager.instance.GameOver();
                }
            }
        }
    }
}
