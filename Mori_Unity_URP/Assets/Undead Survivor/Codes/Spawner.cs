using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float spawnInterval = 5f; // ���� ����
    public int spawnAmount = 1; // �� ���� ������ ���� ��
    public float spawnDuration = 30f; // ������ Ȱ��ȭ�� �ð�(�� ����)
    
    private float timer = 0f;
    private float spawnTimer = 0f;
    private int level = 0;
    private bool isSpawning = true; // ���� Ȱ��ȭ ����


    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();   
        
    }
    
    // Update is called once per frame
    public void Update()
    {
        if (!isSpawning) return; // ������ Ȱ��ȭ���� �ʾҴٸ� �ƹ� �۾��� �������� ����

        timer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnDuration)
        {
            isSpawning = false; // ������ ���� �ð��� ������ ������ ����
            return;
        }

        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);

        if (timer > spawnInterval)
        {
            timer = 0;
            Spawn();
        }
    }
    public void Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            // ��ü Ǯ���� �� ������Ʈ ��������
            GameObject enemy = GameManager.instance.pool.Get(0);

            // ���� ������ ���� (-1���� 1 ������ ��)
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f), // X�� ���� ������
                Random.Range(-1f, 1f),                    // Y���� ���� (2D ȯ���̶��)
                0 // Z�� ���� ������
            );

            // ���� �������� �����Ͽ� ���� ��ġ ����
            Vector3 spawnPosition = spawnPoint[Random.Range(0, spawnPoint.Length)].position + randomOffset;

            // ���� ��ġ ����
            enemy.transform.position = spawnPosition;

            // �� �ʱ�ȭ
            enemy.GetComponent<Enemy>().Init(spawnData[level]);
        }
    }

}
[System.Serializable]
public class SpawnData
{  
    public int spritType;
    public float moveDistance;
    public int health;
    public float speed;
    public float spawnTime;
}