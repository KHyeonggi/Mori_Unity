using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Node leftNode;
    public Node rightNode;
    public Node parNode;
    public RectInt nodeRect; // �и��� ������ Rect ����
    public RectInt roomRect; // �и��� ���� �� ���� Rect ����

    public Vector2Int center
    {
        get
        {
            // roomRect�� ��ȿ���� ������ nodeRect�� �߽��� ��ȯ
            RectInt rect = (roomRect.width > 0 && roomRect.height > 0) ? roomRect : nodeRect;
            return new Vector2Int(rect.x + rect.width / 2, rect.y + rect.height / 2);
        }
    }

    public Node(RectInt rect)
    {
        this.nodeRect = rect;
    }
}
