// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Weapon : MonoBehaviour
// {
//     public int id; // 오브젝트 ID
//     public int prefabid; // 프리팹 ID
//     public float damage; // 데미지
//     public int count; // 카운트 (발사될 총알의 수)
//     public float speed; // 속도

//     void Start()
//     {
//         Init(); // 초기 설정 실행
//     }

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             // 마우스 클릭 시 동작 추가 가능
//         }
//     }

//     public void Init()
//     {
//         // ID에 따라 다른 설정을 할 수 있습니다.
//         switch (id)
//         {
//             case 0:
//                 speed = -150; // 속도 설정
//                 Batch(); // Batch 함수 실행
//                 break;
//             // default:
//                 // Sbreak;
//         }
//     }

//     // 총알 배치를 위한 함수
//     void Batch()
//     {
//         for (int index = 0; index < count; index++)
//         {
//             Transform bullet = GameManager.instance.pool.Get(prefabid).transform; // 총알 가져오기
//             bullet.parent = transform; // 부모 설정

//             Bullet bulletComponent = bullet.GetComponent<Bullet>(); // Bullet 컴포넌트 가져오기
//             if (bulletComponent == null)
//             {
//                 bulletComponent = bullet.gameObject.AddComponent<Bullet>(); // 없다면 Bullet 컴포넌트 추가
//             }
//             bulletComponent.Init(damage, -1); // Bullet 초기화
//         }
//     }
// }


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Weapon : MonoBehaviour
// {
//     public int id; // 오브젝트 ID
//     public int prefabid; // 프리팹 ID
//     public float damage; // 데미지
//     public int count; // 발사될 총알의 수
//     public float speed; // 투사체 속도

//     void Start()
//     {
//         Init(); // 초기 설정 실행
//     }

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭 시
//         {
//             Fire(); // 발사 동작 실행
//         }
//     }

//     public void Init()
//     {
//         // ID에 따라 다른 설정을 할 수 있습니다.
//         switch (id)
//         {
//             case 0:
//                 speed = 10; // 속도 설정
//                 break;
//             default:
//                 break;
//         }
//     }

//     // 총알 발사 함수
//     void Fire()
//     {
//         Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 마우스의 월드 좌표
//         mousePosition.z = 0; // 2D 환경이므로 Z 값을 0으로 설정

//         Vector3 direction = (mousePosition - transform.position).normalized; // 무기에서 마우스 방향으로의 단위 벡터 계산

//         for (int i = 0; i < count; i++)
//         {
//             GameObject bulletObject = GameManager.instance.pool.Get(prefabid); // 풀에서 총알 가져오기
//             if (bulletObject == null)
//                 continue;

//             bulletObject.transform.position = transform.position; // 총알의 초기 위치 설정
//             bulletObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction); // 방향에 맞게 회전 설정

//             Bullet bulletComponent = bulletObject.GetComponent<Bullet>(); // 총알의 Bullet 컴포넌트 가져오기
//             if (bulletComponent == null)
//             {
//                 bulletComponent = bulletObject.AddComponent<Bullet>(); // 없다면 Bullet 컴포넌트 추가
//             }

//             bulletComponent.Init(damage, direction, speed); // 총알 초기화
//         }
//     }
// }





// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Weapon : MonoBehaviour
// {
//     public int id; // 무기의 고유 ID
//     public int prefabId; // 발사체의 프리팹 ID
//     public float damage; // 무기 데미지
//     public int count; // 발사체 수
//     public float speed; // 무기 속도 (발사 간격)

//     float timer; // 발사 타이머
//     Player player; // 플레이어 객체 참조

//     private void Awake()
//     {
//         // GameManager에서 플레이어 객체 참조
//         player = GameManager.instance.player;
//     }

//     void Update()
//     {
//         // 게임 오버 상태에서는 무기 동작 중지
//         if (!GameManager.instance.isLive)
//             return;

//         // 무기 종류별 동작 처리
//         switch (id)
//         {
//             //case 0: // 회전형 무기 (주석 처리)
//             //    transform.Rotate(Vector3.back * speed * Time.deltaTime);
//             //    break;

//             default: // 발사형 무기
//                 timer += Time.deltaTime;

//                 if (timer > speed) // 발사 간격에 따라 발사
//                 {
//                     timer = 0f;
//                     Fire();
//                 }
//                 break;
//         }

//         // ..Test Code..
//         //if (Input.GetButtonDown("Jump"))
//         //{
//         //    LevelUp(10, 1);
//         //}
//     }

//     // 무기 레벨 업 (주석 처리)
//     //public void LevelUp(float damage, int count)
//     //{
//     //    this.damage = damage * Character.Damage; // 데미지 증가
//     //    this.count += count; // 발사체 수 증가

//     //    if (id == 0) // 회전형 무기는 발사체 재배치
//     //    {
//     //        Batch();
//     //    }

//     //    // 플레이어에게 장비 변경 알림
//     //    player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
//     //}

//     public void Init(ItemData data)
//     {
//         // 기본 설정
//         name = "Weapon " + data.itemId;
//         transform.parent = player.transform; // 무기를 플레이어에 붙임
//         transform.localPosition = Vector3.zero; // 플레이어의 중심에 위치

//         // 속성 설정
//         id = data.itemId;
//         damage = data.baseDamage * Character.Damage; // 데미지 초기화
//         count = data.baseCount + Character.Count; // 발사체 수 초기화

//         // 발사체 프리팹 ID 설정
//         for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
//         {
//             if (data.projectile == GameManager.instance.pool.prefabs[index])
//             {
//                 prefabId = index;
//                 break;
//             }
//         }

//         // 무기 유형에 따른 속도 설정
//         switch (id)
//         {
//             //case 0: // 회전형 무기 (주석 처리)
//             //    speed = 150 * Character.WeaponSpeed;
//             //    Batch(); // 발사체 배치
//             //    break;

//             default: // 발사형 무기
//                 speed = 0.3f * Character.WeaponRate;
//                 break;
//         }

//         // 무기 아이콘(손) 설정
//         Hand hand = player.hands[(int)data.itemType];
//         hand.spriter.sprite = data.hand;
//         hand.gameObject.SetActive(true);

//         // 플레이어에게 장비 변경 알림
//         player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
//     }

//     // 회전형 무기의 발사체 배치 (주석 처리)
//     //void Batch()
//     //{
//     //    for (int index = 0; index < count; index++)
//     //    {
//     //        Transform bullet;

//     //        if (index < transform.childCount) // 이미 존재하는 발사체 재사용
//     //        {
//     //            bullet = transform.GetChild(index);
//     //        }
//     //        else // 새 발사체 생성
//     //        {
//     //            bullet = GameManager.instance.pool.Get(prefabId).transform;
//     //            bullet.parent = transform;
//     //        }

//     //        bullet.parent = transform;
//     //        bullet.localPosition = Vector3.zero; // 발사체 초기 위치
//     //        bullet.localRotation = Quaternion.identity; // 발사체 초기 회전

//     //        // 발사체 회전 배치
//     //        Vector3 rotVec = Vector3.forward * 360 * index / count;
//     //        bullet.Rotate(rotVec);
//     //        bullet.Translate(bullet.up * 1.5f, Space.World); // 발사체를 일정 거리로 배치

//     //        // 발사체 초기화
//     //        bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1은 무한 지속 시간
//     //    }
//     //}

//     // 발사형 무기 발사
//     void Fire()
//     {
//         // 가장 가까운 적이 없으면 발사하지 않음
//         // if (!player.scanner.nearestTarget)
//         //     return;

//         // 타겟 위치 계산
//         Vector3 targetPos = player.scanner.nearestTarget.position;
//         Vector3 dir = (targetPos - transform.position).normalized;

//         // 발사체 생성 및 초기화
//         Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
//         bullet.position = transform.position;
//         bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
//         bullet.GetComponent<Bullet>().Init(damage, count, dir);

//         // 발사 사운드 재생
//         //AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
//     }
// }



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab; // 발사체 프리팹
    public float bulletSpeed = 10f; // 발사체 속도
    public float bulletRange = 5f; // 발사체의 최대 이동 거리
    public float damage = 10f; // 무기 데미지
    public float fireRate = 0.5f; // 발사 간격

    private float fireTimer; // 발사 타이머

    private void Update()
    {
        // 타이머 업데이트
        fireTimer += Time.deltaTime;

        // 마우스 클릭 시 발사
        if (Input.GetMouseButtonDown(0) && fireTimer > fireRate)
        {
            fireTimer = 0f;
            Fire();
        }
    }

    void Fire()
    {
        // 마우스 위치 가져오기
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // 2D 환경에서 Z축 제거

        // 발사 방향 계산
        Vector3 direction = (mousePosition - transform.position).normalized;

        // 발사체 생성 및 초기화
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.Init(damage, direction, bulletSpeed, bulletRange);
        }
    }
}
