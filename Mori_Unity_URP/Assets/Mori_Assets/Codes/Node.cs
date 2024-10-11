using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Node leftNode;
    public Node rightNode;
    public Node parNode;
    public RectInt nodeRect; // 분리된 공간의 Rect 정보
    public RectInt roomRect; // 분리된 공간 속 방의 Rect 정보

    public Vector2Int center
    {
        get
        {
            // roomRect가 유효하지 않으면 nodeRect의 중심을 반환
            RectInt rect = (roomRect.width > 0 && roomRect.height > 0) ? roomRect : nodeRect;
            return new Vector2Int(rect.x + rect.width / 2, rect.y + rect.height / 2);
        }
    }

    public Node(RectInt rect)
    {
        this.nodeRect = rect;
    }
}
