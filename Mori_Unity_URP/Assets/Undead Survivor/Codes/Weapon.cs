using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id; // 오브젝트 ID
    public int prefabid; // 프리팹 ID
    public float damage; // 데미지
    public int count; // 카운트 (발사될 총알의 수)
    public float speed; // 속도

    void Start()
    {
        Init(); // 초기 설정 실행
    }

    void Update()
    {
        // 추가적인 회전 로직 없음
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 클릭 시 동작 추가 가능
        }
    }

    public void Init()
    {
        // ID에 따라 다른 설정을 할 수 있습니다.
        switch (id)
        {
            case 0:
                speed = -150; // 속도 설정
                Batch(); // Batch 함수 실행
                break;
            // default:
                // Sbreak;
        }
    }

    // 총알 배치를 위한 함수
    void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet = GameManager.instance.pool.Get(prefabid).transform; // 총알 가져오기
            bullet.parent = transform; // 부모 설정

            Bullet bulletComponent = bullet.GetComponent<Bullet>(); // Bullet 컴포넌트 가져오기
            if (bulletComponent == null)
            {
                bulletComponent = bullet.gameObject.AddComponent<Bullet>(); // 없다면 Bullet 컴포넌트 추가
            }
            bulletComponent.Init(damage, -1); // Bullet 초기화
        }
    }
}
