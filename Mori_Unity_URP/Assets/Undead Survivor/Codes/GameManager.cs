using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float gameTime;
    public float maxGameTime = 2 *10f;

    public PoolManeger pool;
    public Player player;
    public GameObject CoverImage;
    public GameObject IconImage;
    public Spawner spawner;

    public bool isLive=false;

    public void StartBBBB()
    {
        CoverImage.SetActive(false);//배경이미지 비활
        IconImage.SetActive(false);//로고이미지 비활
        //spawner.Spawn(); //몬스터 생성
        isLive=true;
    }

    

    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if (isLive==true)
        {
            spawner.Spawn();
        }
        else if (isLive == false)
        {
            return;
        }
        gameTime += Time.deltaTime;

        if (gameTime> maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

}
