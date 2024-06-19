using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;          // 텍스트 컴포넌트
    public float typingSpeed = 0.05f;  // 타이핑 속도 (초 단위)
    private string fullText;           // 출력할 전체 텍스트
    private Coroutine typingCoroutine; // 코루틴 저장 변수

    // 대화 시작 시 호출될 함수
    public void StartDialogue(string text)
    {
        fullText = text;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText());
    }

    // 타이핑 이펙트 코루틴
    private IEnumerator TypeText()
    {
        dialogueText.text = "";  // 기존 텍스트 초기화
        foreach (char letter in fullText.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);  // 타이핑 속도만큼 대기
        }
        typingCoroutine = null;  // 코루틴 종료
    }
}
