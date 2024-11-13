using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ItemEft/Consumable/Health")]
public class ItemHealingEft : ItemEffect
{
    public GameManager gameManager;
    public int healingPoint = 0;

    public override bool ExecuteRole()
    {
        if(GameManager.instance.health + healingPoint> GameManager.instance.maxHealth)
        {
            GameManager.instance.health = GameManager.instance.maxHealth;
        }
        else
        {
            GameManager.instance.health = GameManager.instance.health + healingPoint;
        }
        
        Debug.Log("PlayerHp Add: " + healingPoint);
        return true;
    }
}
