using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelector : MonoBehaviour
{
    // 스토리 맵 버튼 클릭 시 호출
    public void LoadStoryMap()
    {
        SceneManager.LoadScene("Demo (1.4)"); // "StoryMapScene" 이름의 씬 로드
        Debug.Log("stroybutton");
    }

    // 랜덤 맵 버튼 클릭 시 호출
    public void LoadRandomMap()
    {
        SceneManager.LoadScene("RandommapDemo(1.0)"); // "RandomMapScene" 이름의 씬 로드
        Debug.Log("Random");
    }
}
