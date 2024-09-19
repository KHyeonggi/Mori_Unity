using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public Text nameText; // 캐릭터 이름을 표시할 Text 컴포넌트
    public Text talkText; // 대사를 표시할 Text 컴포넌트

    public void SetNameText(string name)
    {
        if (nameText != null)
        {
            nameText.text = name; // 이름 설정
        }
    }

    public void SetTalkText(string talk)
    {
        if (talkText != null)
        {
            talkText.text = talk; // 대사 설정
        }
    }
}


