using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public Transform nearestTarget; // 마우스 위치를 저장할 변수

    private void FixedUpdate()
    {
        TrackMousePosition();
    }

    void TrackMousePosition()
    {
        // 마우스의 화면 좌표를 월드 좌표로 변환
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Z축 값은 0으로 고정 (2D 환경에서는 필요 없음)
        mousePos.z = 0;

        // 마우스 위치를 nearestTarget으로 저장
        if (nearestTarget == null)
        {
            // nearestTarget이 없으면 새로운 빈 객체 생성
            GameObject mouseTracker = new GameObject("MouseTarget");
            nearestTarget = mouseTracker.transform;
        }

        nearestTarget.position = mousePos; // 마우스 위치 업데이트
    }
}
