using UnityEngine; // Unity�� �⺻ ��� (Transform, GameObject ��)
using UnityEngine.SceneManagement; // �� ���� ���� ��� ���

public class SceneLoader : MonoBehaviour
{
    // Ư�� ������ ��ȯ�ϴ� �Լ�
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // �� �̸��� ���� �� ��ȯ
    }

    // ���� �� �ٽ� �ε�
    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name; // ���� Ȱ��ȭ�� �� �̸� ��������
        SceneManager.LoadScene(currentSceneName); // ���� �� �ٽ� �ε�
    }
}
