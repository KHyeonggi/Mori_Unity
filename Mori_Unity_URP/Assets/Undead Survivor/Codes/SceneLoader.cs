using UnityEngine; // Unity의 기본 기능 (Transform, GameObject 등)
using UnityEngine.SceneManagement; // 씬 관리 관련 기능 사용

public class SceneLoader : MonoBehaviour
{
    // 특정 씬으로 전환하는 함수
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // 씬 이름을 통해 씬 전환
    }

    // 현재 씬 다시 로드
    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name; // 현재 활성화된 씬 이름 가져오기
        SceneManager.LoadScene(currentSceneName); // 현재 씬 다시 로드
    }
}
