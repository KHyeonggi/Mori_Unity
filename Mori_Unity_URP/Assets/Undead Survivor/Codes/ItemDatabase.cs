using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
  public static ItemDatabase instance;
    private void Awake()
    {
        instance = this;
    }
    public List<Item>itemDB=new List<Item>();

    public GameObject fieldItemPrefab; //fieldItemPrefab복제를 위한 객체
    public Vector3[] pos; //생성위치
    private void Start()
    {
        for(int i = 0; i < 6; i++) {//아이템 6개 생성 임시
           GameObject go= Instantiate(fieldItemPrefab, pos[i], Quaternion.identity);
            go.GetComponent<FieldItem>().SetItem(itemDB[Random.Range(0, 4)]);
        }
    }
    public void DropItem(Vector3 pos)
    {
        int i = Random.Range(0, 10);
        if(i == 0) 
        {
            GameObject go = Instantiate(fieldItemPrefab, pos, Quaternion.identity);
            go.GetComponent<FieldItem>().SetItem(itemDB[Random.Range(0, 4)]);
        }
        
    }
}
