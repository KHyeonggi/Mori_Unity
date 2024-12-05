using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy 클래스는 적의 행동 및 상태를 관리하는 스크립트입니다.
public class Enemy : MonoBehaviour
{
    // public 변수들: 외부에서 설정 가능한 적의 속성들
    public float speed; // 적의 이동 속도
    public float moveDistance; // 플레이어를 추격할 거리 범위
    public float health; // 적의 현재 체력
    public float maxHealth; // 적의 최대 체력
    public RuntimeAnimatorController[] animCon; // 애니메이션 컨트롤러 배열
    public Rigidbody2D target; // 목표가 되는 플레이어의 Rigidbody2D

    // 상태 관리 변수들
    public bool isLive; // 적이 살아있는지 여부

    // Unity 컴포넌트 참조 변수들
    public Rigidbody2D rigid; // 적의 Rigidbody2D
    public Collider2D coll; // 적의 충돌 체계
    public Animator anim; // 적의 애니메이터
    public SpriteRenderer spriter; // 적의 스프라이트 렌더러
    public WaitForFixedUpdate wait; // FixedUpdate와 함께 사용하는 Wait 객체

    // Awake 메서드: 컴포넌트 초기화
    void Awake()
    {
        coll = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    // FixedUpdate: 물리 관련 로직 실행
    void FixedUpdate()
    {
        // 적이 살아있지 않거나, Hit 애니메이션 상태라면 실행 중단
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") || !GameManager.instance.gameStarted)
        {
            return;
        }

        // 타겟이 설정되지 않은 경우 경고를 출력하고 중단
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned for the enemy.");
            return;
        }

        // 타겟과의 거리 계산
        float distanceToTarget = Vector2.Distance(rigid.position, target.position);

        // 타겟이 moveDistance 내에 있을 경우 적이 이동
        if (distanceToTarget <= moveDistance)
        {
            Vector2 dirVec = target.position - rigid.position; // 이동 방향 계산
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; // 이동 벡터 계산
            rigid.MovePosition(rigid.position + nextVec); // 위치 이동
        }
        rigid.velocity = Vector2.zero; // 이동 후 속도 초기화
    }

    // LateUpdate: 렌더링 직전 로직
    void LateUpdate()
    {
        if (!isLive || target == null)
            return;

        // 적이 플레이어를 바라보게끔 스프라이트 반전 설정
        spriter.flipX = target.position.x < rigid.position.x;
    }

    // OnEnable: 적이 활성화될 때 초기화
    void OnEnable()
    {
        // GameManager로부터 플레이어 타겟 설정
        if (GameManager.instance != null && GameManager.instance.player != null)
        {
            target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogWarning("Player target not found for the enemy.");
        }

        // 적 상태 초기화
        isLive = true;
        health = maxHealth;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
    }

    // Init: 적의 데이터 초기화
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spritType]; // 애니메이션 컨트롤러 설정
        speed = data.speed; // 이동 속도 설정
        moveDistance = data.moveDistance; // 이동 거리 설정
        maxHealth = data.health; // 최대 체력 설정
        health = data.health; // 현재 체력 설정
    }

    // OnTriggerEnter2D: 충돌 감지 처리
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 총알과의 충돌만 처리
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        // 총알의 데미지만큼 체력 감소
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack()); // 넉백 효과 처리

        if (health > 0)
        {
            anim.SetTrigger("Hit"); // Hit 애니메이션 실행
        }
        else
        {
            // 적이 죽었을 때 처리
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.GetExp(); // 경험치 획득
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            StartCoroutine(HandleDeath()); // 죽음 처리 코루틴 실행
        }
    }

    // HandleDeath: 적의 죽음 처리
    IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(1.0f); // 1초 대기
        Dead(); // Dead 메서드 호출
    }

    // KnockBack: 플레이어로부터 반동 효과
    public IEnumerator KnockBack()
    {
        yield return wait;

        if (GameManager.instance.player != null)
        {
            Vector3 playerPos = GameManager.instance.player.transform.position; // 플레이어 위치
            Vector3 dirVec = transform.position - playerPos; // 반동 방향 계산
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); // 힘 추가
        }
    }

    // EnemyDeathHandler: 적의 죽음을 알리는 델리게이트
    public delegate void EnemyDeathHandler(GameObject enemy);
    public event EnemyDeathHandler OnEnemyDeath;

    // Dead: 적 제거 로직
    void Dead()
    {
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath(gameObject); // 적의 죽음을 알림
        }

        gameObject.SetActive(false); // 적 비활성화
    }

    // OnDestroy: 적이 파괴될 때 호출
    void OnDestroy()
    {
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath(gameObject); // 적의 죽음을 알림
        }
    }

    // SetTarget: 플레이어 타겟 설정
    public void SetTarget(Rigidbody2D playerTarget)
    {
        target = playerTarget;
        Debug.Log($"Enemy target set: {name} -> {target.name}");
    }
}
