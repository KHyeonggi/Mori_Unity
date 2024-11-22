using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Vector3 normalPos = new Vector3(0, -0.3f, 0); // 기본 위치
    Vector3 reversePos = new Vector3(0, -0.3f, 0); // 반전된 위치

    Quaternion normalRot = Quaternion.Euler(0, 0, 50); // 기본 회전 (35도에서 50도로 변경)
    Quaternion reverseRot = Quaternion.Euler(0, 0, -50); // 반전된 회전 (-35도에서 -50도로 변경)

    private bool isSwinging = false; // 휘두르는 중인지 확인하는 플래그
    private float swingDuration = 0.1f; // 휘두르기 지속 시간
    private float swingTimer = 0f; // 휘두르기 타이머

    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;

        // 마우스 클릭 시 휘두르기 시작
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            isSwinging = true; // 휘두르기 활성화
            swingTimer = 0f; // 타이머 초기화
        }

        if (isSwinging)
        {
            Swing(isReverse); // 휘두르는 로직 실행
        }
        else
        {
            // 기본 위치 및 회전 유지
            transform.localPosition = isReverse ? reversePos : normalPos;
            transform.localRotation = isReverse ? reverseRot : normalRot;
            spriter.flipX = isReverse;
            //spriter.sortingOrder = isReverse ? 4 : 6;
        }
    }

    private void Swing(bool isReverse)
    {
        swingTimer += Time.deltaTime; // 타이머 업데이트

        if (swingTimer <= swingDuration / 2)
        {
            // 각도를 0도로 이동 (휘두르기 진행 중)
            transform.localRotation = Quaternion.Lerp(
                isReverse ? reverseRot : normalRot,
                Quaternion.Euler(0, 0, 0),
                swingTimer / (swingDuration / 2)
            );
        }
        else
        {
            // 각도를 원래 상태로 복구
            transform.localRotation = Quaternion.Lerp(
                Quaternion.Euler(0, 0, 0),
                isReverse ? reverseRot : normalRot,
                (swingTimer - swingDuration / 2) / (swingDuration / 2)
            );
        }

        // 휘두르기 동작 종료
        if (swingTimer >= swingDuration)
        {
            isSwinging = false; // 휘두르기 중지
            swingTimer = 0f; // 타이머 초기화
        }
    }
}
    