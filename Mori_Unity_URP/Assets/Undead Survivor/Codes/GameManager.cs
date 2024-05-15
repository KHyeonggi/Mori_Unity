using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Header("# Player Info")]
    public int level; //����
    public int exp; //����ġ
    public int[] nextExp = { 10, 30, 70, 150, 310 }; //����ġ �ִ뷮
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
        exp++;//���� óġ�� ����ġ 1����
        if(exp == nextExp[level]) { //����ġ�� ���� ���� �ִ� ����ġ�� ���ٸ�
            level++; //���� 1����
            exp = 0; //����ġ �ʱ�ȭ
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
}
