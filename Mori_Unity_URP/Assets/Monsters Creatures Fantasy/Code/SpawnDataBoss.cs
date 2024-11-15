using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDataBoss
{
    public int spritType; // 보스의 스프라이트 타입
    public float speed; // 보스의 이동 속도
    public float moveDistance; // 보스가 플레이어를 추적할 수 있는 거리
    public float health; // 보스의 최대 체력
    public float attackRange; // 보스의 공격 범위 추가 (신규 필드)

    // 생성자를 통해 데이터를 초기화할 수도 있습니다.
    public SpawnDataBoss(int spritType, float speed, float moveDistance, float health, float attackRange)
    {
        this.spritType = spritType;
        this.speed = speed;
        this.moveDistance = moveDistance;
        this.health = health;
        this.attackRange = attackRange; // 새로운 변수 초기화
    }
}
