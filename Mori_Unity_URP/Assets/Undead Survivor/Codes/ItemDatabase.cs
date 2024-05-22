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

    public GameObject fieldItemPrefab; //fieldItemPrefab������ ���� ��ü
    public Vector3[] pos; //������ġ
    private void Start()
    {
        for(int i = 0; i < 6; i++) {//������ 6�� ���� �ӽ�
           GameObject go= Instantiate(fieldItemPrefab, pos[i], Quaternion.identity);
            go.GetComponent<FieldItem>().SetItem(itemDB[Random.Range(0, 3)]);
        }
    }
}