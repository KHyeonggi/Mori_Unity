using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;          // �ؽ�Ʈ ������Ʈ
    public float typingSpeed = 0.05f;  // Ÿ���� �ӵ� (�� ����)
    private string fullText;           // ����� ��ü �ؽ�Ʈ
    private Coroutine typingCoroutine; // �ڷ�ƾ ���� ����

    // ��ȭ ���� �� ȣ��� �Լ�
    public void StartDialogue(string text)
    {
        fullText = text;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText());
    }

    // Ÿ���� ����Ʈ �ڷ�ƾ
    private IEnumerator TypeText()
    {
        dialogueText.text = "";  // ���� �ؽ�Ʈ �ʱ�ȭ
        foreach (char letter in fullText.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);  // Ÿ���� �ӵ���ŭ ���
        }
        typingCoroutine = null;  // �ڷ�ƾ ����
    }
}
