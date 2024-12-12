using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelector : MonoBehaviour
{
    // ���丮 �� ��ư Ŭ�� �� ȣ��
    public void LoadStoryMap()
    {
        Debug.Log("Story Map Button Clicked");
        SceneManager.LoadScene("Demo (1.4)"); // "Demo (1.4)" �� �̸��� ��Ȯ���� Ȯ��
    }

    // ���� �� ��ư Ŭ�� �� ȣ��
    public void LoadRandomMap()
    {
        Debug.Log("Random Map Button Clicked");
        SceneManager.LoadScene("RandommapDemo(1.0)"); // "RandommapDemo(1.0)" �� �̸��� ��Ȯ���� Ȯ��
    }
}
