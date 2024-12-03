using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManeger : MonoBehaviour
{
    public GameObject[] prefabs;

    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        // �ε��� ��ȿ�� �˻� �߰�
        if (index < 0 || index >= pools.Length)
        {
            Debug.LogError($"Invalid index: {index}. Index must be between 0 and {pools.Length - 1}.");
            return null;
        }

        GameObject select = null;

        // ���� Ǯ���� ��Ȱ��ȭ�� ������Ʈ ã��
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // ���� Ǯ�� ��� ������ ������Ʈ�� ������ ���� ����
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
