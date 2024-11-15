using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDataBoss
{
    public int spritType; // ������ ��������Ʈ Ÿ��
    public float speed; // ������ �̵� �ӵ�
    public float moveDistance; // ������ �÷��̾ ������ �� �ִ� �Ÿ�
    public float health; // ������ �ִ� ü��
    public float attackRange; // ������ ���� ���� �߰� (�ű� �ʵ�)

    // �����ڸ� ���� �����͸� �ʱ�ȭ�� ���� �ֽ��ϴ�.
    public SpawnDataBoss(int spritType, float speed, float moveDistance, float health, float attackRange)
    {
        this.spritType = spritType;
        this.speed = speed;
        this.moveDistance = moveDistance;
        this.health = health;
        this.attackRange = attackRange; // ���ο� ���� �ʱ�ȭ
    }
}
