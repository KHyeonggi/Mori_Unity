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
        // 인덱스 유효성 검사 추가
        if (index < 0 || index >= pools.Length)
        {
            Debug.LogError($"Invalid index: {index}. Index must be between 0 and {pools.Length - 1}.");
            return null;
        }

        GameObject select = null;

        // 기존 풀에서 비활성화된 오브젝트 찾기
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 기존 풀에 사용 가능한 오브젝트가 없으면 새로 생성
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
