// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Bullet : MonoBehaviour
// {
//     public float damage;
//     public int per;

//     public void Init(float damage, int per)
//     {
//         this.damage = damage;
//         this.per = per;
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage; // 총알의 데미지
    public float speed; // 총알의 속도
    private Vector3 direction; // 총알이 이동하는 방향
    private float maxDistance; // 최대 이동 거리
    private Vector3 startPosition; // 발사 시작 위치
    private bool isInitialized = false; // 초기화 여부

    // 총알 초기화
    public void Init(float damage, Vector3 direction, float speed, float maxDistance)
    {
        this.damage = damage; // 데미지 설정
        this.direction = direction.normalized; // 방향 벡터를 정규화
        this.speed = speed; // 속도 설정
        this.maxDistance = maxDistance; // 최대 거리 설정
        startPosition = transform.position; // 시작 위치 저장
        isInitialized = true; // 초기화 완료

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        if (!isInitialized) return; // 초기화되지 않은 경우 이동하지 않음
        transform.position += direction * speed * Time.deltaTime; // 설정된 방향으로 이동

        // 최대 거리 초과 시 비활성화
        if (Vector3.Distance(startPosition, transform.position) > maxDistance)
        {
            gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            return; // 플레이어와의 충돌 무시
        }

        //적과 충돌 처리
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null && enemy.isLive)
            {
                enemy.health -= damage; // 적의 체력 감소
                enemy.StartCoroutine(enemy.KnockBack()); // 넉백 효과
                if (enemy.health > 0)
                {
                    enemy.anim.SetTrigger("Hit"); // 적 피격 애니메이션
                }
                else
                {
                    enemy.isLive = false; // 적 사망 처리
                    enemy.anim.SetBool("Dead", true); // 적 사망 애니메이션
                    enemy.coll.enabled = false; // 충돌 해제
                    enemy.rigid.simulated = false; // 물리 비활성화
                    GameManager.instance.GetExp(); // 경험치 증가
                }
            }
            gameObject.SetActive(false); // 총알 비활성화
        }

        //벽이나 다른 충돌체와 충돌 처리
        //if (collision.CompareTag("Wall"))
        //{
        //    gameObject.SetActive(false); // 벽에 충돌 시 총알 비활성화
        //}
    }

    private void OnBecameInvisible()
    {
        //화면 밖으로 나가면 총알 비활성화
        gameObject.SetActive(false);
    }
}