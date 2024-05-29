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
            GameObject enemy = GameManager.instance.pool.Get(0);
            enemy.transform.position = spawnPoint[Random.Range(0, spawnPoint.Length)].position;
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