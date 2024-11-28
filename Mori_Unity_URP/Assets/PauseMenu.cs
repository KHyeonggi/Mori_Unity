using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseCanvas;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//esc키 입력시 호출
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()//계속하기
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()//멈추기
    {
        PauseCanvas.SetActive(true);
        Time.timeScale = 0f; //시간 정지
        GameIsPaused = true;
    }

    public void ToSettingMenu()//셋팅메뉴
    {
        Debug.Log("아직 미구현입니다...");
    }

    public void ToMain()//메인으로
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//씬 다시 불러오기
        Time.timeScale = 1;
    }

    public void QuitGame()//게임종료
    {
        Application.Quit();
    }
}
