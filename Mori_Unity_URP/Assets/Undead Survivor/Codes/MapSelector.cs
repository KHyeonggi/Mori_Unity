using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelector : MonoBehaviour
{
    // 스토리 맵 버튼 클릭 시 호출
    public void LoadStoryMap()
    {
        Debug.Log("Story Map Button Clicked");
        SceneManager.LoadScene("Demo (1.4)"); // "Demo (1.4)" 씬 이름이 정확한지 확인
    }

    // 랜덤 맵 버튼 클릭 시 호출
    public void LoadRandomMap()
    {
        Debug.Log("Random Map Button Clicked");
        SceneManager.LoadScene("RandommapDemo(1.0)"); // "RandommapDemo(1.0)" 씬 이름이 정확한지 확인
    }
}
