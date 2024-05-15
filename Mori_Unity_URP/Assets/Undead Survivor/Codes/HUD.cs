using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    public Vector2 inputVec;
    public float speed;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    public enum InfoType { Exp, Level, Health, HealthInfo }
    public InfoType type;

    Text myText;
    Slider mySlider;
    void Awake()
    {
        mySlider = GetComponent<Slider>();
        myText = GetComponent<Text>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp; //현재 경험치
                float maxExp = GameManager.instance.nextExp[GameManager.instance.level];  //최대 경험치
                mySlider.value = curExp/maxExp; //경험치바에 표시
                break;

            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level+1);
                break;

            case InfoType.Health:
                float curHealth = GameManager.instance.health; //현재 체력
                float maxHealth = GameManager.instance.maxHealth;  //최대 체력
                mySlider.value = curHealth / maxHealth ; //체력바에 표시
                break;
            case InfoType.HealthInfo://체력바 체력 수치
                myText.text = myText.text = string.Format("{0:F0}/{1:F0}", GameManager.instance.health, GameManager.instance.maxHealth);
                break;
        }
    }
}
