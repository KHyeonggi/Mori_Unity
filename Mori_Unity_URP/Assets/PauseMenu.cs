using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseCanvas;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//escŰ �Է½� ȣ��
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
    public void Resume()//����ϱ�
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()//���߱�
    {
        PauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ToSettingMenu()//���ø޴�
    {
        Debug.Log("���� �̱����Դϴ�...");
    }

    public void ToMain()//��������
    {
        Debug.Log("���� �̱����Դϴ�...");
    }

    public void QuitGame()//��������
    {
        Debug.Log("���� �̱����Դϴ�...");
        Application.Quit();
    }
}
