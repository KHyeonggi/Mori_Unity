using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelector : MonoBehaviour
{
    // ���丮 �� ��ư Ŭ�� �� ȣ��
    public void LoadStoryMap()
    {
        SceneManager.LoadScene("Demo (1.4)"); // "StoryMapScene" �̸��� �� �ε�
        Debug.Log("stroybutton");
    }

    // ���� �� ��ư Ŭ�� �� ȣ��
    public void LoadRandomMap()
    {
        SceneManager.LoadScene("RandommapDemo(1.0)"); // "RandomMapScene" �̸��� �� �ε�
        Debug.Log("Random");
    }
}
