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

    private bool isLive = true; // �÷��̾� ���� ����

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isLive) return; // �÷��̾ ���� ��� �ൿ ����

        if (Input.GetKey(KeyCode.W) || Input.GetKey("up")) // ���� �ִ� ������Ʈ �ν�
            dirVec = Vector3.up;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey("down")) // �Ʒ��� �ִ� ������Ʈ �ν�
            dirVec = Vector3.down;
        else if (Input.GetKey(KeyCode.A) || Input.GetKey("left")) // ���ʿ� �ִ� ������Ʈ �ν�
            dirVec = Vector3.left;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey("right")) // �����ʿ� �ִ� ������Ʈ �ν�
            dirVec = Vector3.right;
    }

    private void OnInspect() // ������Ʈ �ν�
    {
        if (!isLive) return; // �÷��̾ ���� ��� �ൿ ����

        if (scanObject != null)
        {
            manager.Action(scanObject);
        }
    }

    void FixedUpdate()
    {
        if (!isLive) return; // �÷��̾ ���� ��� �̵� ����

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
        if (!isLive) return; // �÷��̾ ���� ��� �̵� ����

        inputVec = value.Get<Vector2>();
    }

    void LateUpdate()
    {
        if (!isLive) return; // �÷��̾ ���� ��� �ִϸ��̼� ����

        anim.SetFloat("Speed", inputVec.magnitude);
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!isLive) return; // �̹� ���� ��� ���� �������� ����

        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("BossAI"))
        {
            GameManager.instance.health -= Time.deltaTime * 10; // ���� ������ 10������
            if (GameManager.instance.health <= 0)
            {
                if (isLive) // ó�� �׾��� ���� ����
                {
                    isLive = false; // �÷��̾� ��� ���·� ����
                    anim.SetTrigger("Dead");
                    GameManager.instance.GameOver();
                }
            }
        }
    }
}
