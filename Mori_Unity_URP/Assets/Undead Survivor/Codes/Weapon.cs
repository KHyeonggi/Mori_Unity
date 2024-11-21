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

    private bool isSwinging = false; // 현재 휘두르는 중인지 판별하는 플래그
    private bool isReturning = false; // 원래 자리로 돌아가는 중인지 판별하는 플래그
    private float swingDuration = 0.1f; // 휘두르는 동작의 지속 시간
    private float swingTimer = 0f; // 휘두르는 동작을 위한 타이머
    private Quaternion startRotation; // 시작 회전값
    private Quaternion midRotation; // 중간 목표 회전값
    private Quaternion endRotation; // 최종 목표 회전값

    private Hand hand; // Hand 컴포넌트 참조

    void Awake()
    {
        hand = GetComponentInParent<Hand>(); // 부모 오브젝트에서 Hand 컴포넌트 가져오기
    }

    void Start()
    {
        Init(); // 초기 설정 실행
    }

    void Update()
    {
        // Hand 스크립트의 방향 정보를 가져옴
        bool isReverse = hand.spriter.flipX;

        // 기본 각도 설정 (Hand 스크립트의 방향에 따라 조정)
        float initialAngle = isReverse ? -35f : 35f;

        // 마우스 왼쪽 버튼을 클릭하고, 현재 휘두르지 않으며, 원래 자리로 돌아가지 않는 경우
        if (Input.GetMouseButtonDown(0) && !isSwinging && !isReturning)
        {
            isSwinging = true; // 휘두르기 시작
            swingTimer = 0f; // 타이머 초기화
            startRotation = transform.localRotation; // 현재 로컬 회전값을 시작 회전값으로 설정
            midRotation = Quaternion.Euler(0, 0, 0); // 중간 목표 회전값 (각도 0도)
            endRotation = Quaternion.Euler(0, 0, initialAngle); // 최종 목표 회전값
        }

        // 휘두르는 동작 중
        if (isSwinging)
        {
            swingTimer += Time.deltaTime; // 타이머 업데이트
            float fraction = swingTimer / swingDuration; // 진행 비율 계산
            transform.localRotation = Quaternion.Lerp(startRotation, midRotation, fraction); // 시작 회전값에서 중간 목표 회전값으로 부드럽게 회전

            // 휘두르는 동작이 완료되면
            if (swingTimer >= swingDuration)
            {
                isSwinging = false; // 휘두르기 중지
                isReturning = true; // 원래 자리로 돌아가기 시작
                swingTimer = 0f; // 타이머 초기화
            }
        }

        // 원래 자리로 돌아가는 동작 중
        if (isReturning)
        {
            swingTimer += Time.deltaTime; // 타이머 업데이트
            float fraction = swingTimer / swingDuration; // 진행 비율 계산
            transform.localRotation = Quaternion.Lerp(midRotation, endRotation, fraction); // 중간 목표 회전값에서 최종 목표 회전값으로 부드럽게 회전

            // 원래 자리로 돌아가기가 완료되면
            if (swingTimer >= swingDuration)
            {
                isReturning = false; // 원래 자리로 돌아가기 중지
            }
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
            default:
                break;
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
