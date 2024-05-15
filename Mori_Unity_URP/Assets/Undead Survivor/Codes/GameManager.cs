using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float gameTime;
    public float maxGameTime = 2 *10f;
    public bool isLive = true;

    public PoolManeger pool;
    public Player player;
    public GameObject CoverImage;
    public GameObject IconImage;
    public Spawner spawner;
    public GameObject Player;
    public PauseMenu pauseMenu;
    public GameObject uiResult;
    Animation anim;

    [Header("# Player Info")]
    public int level; //레벨
    public int exp; //경험치
    public int[] nextExp = { 10, 30, 70, 150, 310 }; //경험치 최대량
    public float health;
    public float maxHealth = 100;

    public void OnClickButton()
    {
        
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth;
    }

    public GameObject pauseMenuCanvas;
    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

   public void GetExp()
    {
        exp++;//몬스터 처치시 경험치 1제공
        if(exp == nextExp[level]) { //경험치가 현제 레벨 최대 경험치와 같다면
            level++; //레벨 1증가
            exp = 0; //경험치 초기화
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive=true;
        Time.timeScale = 1;
    }


    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.2f);

        uiResult.SetActive(true);
        Stop();
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
