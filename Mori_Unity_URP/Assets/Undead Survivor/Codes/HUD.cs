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
                float curExp = GameManager.instance.exp; //���� ����ġ
                float maxExp = GameManager.instance.nextExp[GameManager.instance.level];  //�ִ� ����ġ
                mySlider.value = curExp/maxExp; //����ġ�ٿ� ǥ��
                break;

            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level+1);
                break;

            case InfoType.Health:
                float curHealth = GameManager.instance.health; //���� ü��
                float maxHealth = GameManager.instance.maxHealth;  //�ִ� ü��
                mySlider.value = curHealth / maxHealth ; //ü�¹ٿ� ǥ��
                break;
            case InfoType.HealthInfo://ü�¹� ü�� ��ġ
                myText.text = myText.text = string.Format("{0:F0}/{1:F0}", GameManager.instance.health, GameManager.instance.maxHealth);
                break;
        }
    }
}
