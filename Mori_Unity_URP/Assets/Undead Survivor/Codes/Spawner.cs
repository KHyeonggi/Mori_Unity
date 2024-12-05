using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float spawnInterval = 5f; // 스폰 간격
    public int spawnAmount = 1; // 한 번에 스폰할 적의 수
    public float spawnDuration = 30f; // 스폰이 활성화될 시간(초 단위)
    
    private float timer = 0f;
    private float spawnTimer = 0f;
    private int level = 0;
    private bool isSpawning = true; // 스폰 활성화 여부


    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();   
        
    }
    
    // Update is called once per frame
    public void Update()
    {
        if (!isSpawning) return; // 스폰이 활성화되지 않았다면 아무 작업도 수행하지 않음

        timer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnDuration)
        {
            isSpawning = false; // 정해진 스폰 시간이 지나면 스폰을 중지
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
            // 객체 풀에서 적 오브젝트 가져오기
            GameObject enemy = GameManager.instance.pool.Get(0);

            // 랜덤 오프셋 생성 (-1에서 1 사이의 값)
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f), // X축 랜덤 오프셋
                Random.Range(-1f, 1f),                    // Y축은 고정 (2D 환경이라면)
                0 // Z축 랜덤 오프셋
            );

            // 랜덤 오프셋을 적용하여 스폰 위치 결정
            Vector3 spawnPosition = spawnPoint[Random.Range(0, spawnPoint.Length)].position + randomOffset;

            // 적의 위치 설정
            enemy.transform.position = spawnPosition;

            // 적 초기화
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