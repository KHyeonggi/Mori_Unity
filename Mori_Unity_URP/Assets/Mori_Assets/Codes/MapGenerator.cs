using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int mapSize;
    [SerializeField] float minimumDivideRate; // ������ �������� �ּ� ����
    [SerializeField] float maximumDivideRate; // ������ �������� �ִ� ����
    [SerializeField] private GameObject line; // LineRenderer�� ����ؼ� ������ ������ ���� �ð������� �����ֱ� ����
    [SerializeField] private GameObject map; // LineRenderer�� ����ؼ� ù ���� ����� �����ֱ� ����
    [SerializeField] private GameObject roomLine; // LineRenderer�� ����ؼ� ���� ����� �����ֱ� ����
    [SerializeField] private int maximumDepth; // Ʈ���� ����, �������� ���� �� �ڼ��� ������ ��
    [SerializeField] Tilemap tileMap;
    [SerializeField] Tile roomTile; // ���� �����ϴ� Ÿ��
    [SerializeField] Tile wallTile; // ��� �ܺθ� ���������� �� Ÿ��
    [SerializeField] Tile outTile; // �� �ܺ��� Ÿ��

    [SerializeField] private GameObject[] monsterPrefabs; // ���� ������ �迭
    [SerializeField] private GameObject[] itemPrefabs;    // ������ ������ �迭

    [SerializeField] private GameObject playerPrefab; // �÷��̾� ������
    [SerializeField] private GameObject bossPrefab;

    void Start()
    {
        FillBackground(); // �� �ε� �� ���� �� �ٱ� Ÿ�Ϸ� ����
        Node root = new Node(new RectInt(0, 0, mapSize.x, mapSize.y));
        Divide(root, 0);
        List<Node> leafNodes = new List<Node>();
        GetLeafNodes(root, leafNodes);

        Node bossRoomNode = SelectBossRoom(leafNodes);
        GenerateRoom(root, bossRoomNode);
        GenerateLoad(root);
        FillWall(); // �ٱ��� ���� ������ ������ ������ ĥ���ִ� �Լ�

        SpawnPlayer(root); //**�÷��̾� ���� �Լ� ȣ��**
        SpawnBoss(bossRoomNode); // ���� ����

        FillWall(); // �ٱ��� ���� ������ ������ ������ ĥ���ִ� �Լ�
    }

    void Divide(Node tree, int depth)
    {
        if (depth == maximumDepth) return;

        int maxLength = Mathf.Max(tree.nodeRect.width, tree.nodeRect.height);
        int minSplit = Mathf.RoundToInt(maxLength * minimumDivideRate);
        int maxSplit = Mathf.RoundToInt(maxLength * maximumDivideRate);

        if (maxSplit <= minSplit)
        {
            maxSplit = minSplit + 1; // �ּ����� ������ ����
        }

        int split = Random.Range(minSplit, maxSplit);

        if (tree.nodeRect.width >= tree.nodeRect.height)
        {
            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, split, tree.nodeRect.height));
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x + split, tree.nodeRect.y, tree.nodeRect.width - split, tree.nodeRect.height));
        }
        else
        {
            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, tree.nodeRect.width, split));
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y + split, tree.nodeRect.width, tree.nodeRect.height - split));
        }

        tree.leftNode.parNode = tree;
        tree.rightNode.parNode = tree;

        Divide(tree.leftNode, depth + 1);
        Divide(tree.rightNode, depth + 1);
    }

    private void GenerateRoom(Node tree, Node bossRoomNode)
    {
        if (tree.leftNode == null && tree.rightNode == null) // ���� ����� ���
        {
            RectInt rect = tree.nodeRect;
            int width = Random.Range(rect.width / 2, rect.width - 1);
            int height = Random.Range(rect.height / 2, rect.height - 1);

            width = Mathf.Clamp(width, 3, rect.width - 2);   // �ּ� ũ�� ����
            height = Mathf.Clamp(height, 3, rect.height - 2); // �ּ� ũ�� ����

            int x = rect.x + Random.Range(1, rect.width - width);
            int y = rect.y + Random.Range(1, rect.height - height);

            tree.roomRect = new RectInt(x, y, width, height);
            FillRoom(tree.roomRect);

            // **���Ϳ� ������ ��ġ �߰�**
            if (tree != bossRoomNode)
            {
                PlaceContentInRoom(tree.roomRect);
            }
        }
        else
        {
            if (tree.leftNode != null)
                GenerateRoom(tree.leftNode, bossRoomNode);
            if (tree.rightNode != null)
                GenerateRoom(tree.rightNode, bossRoomNode);
        }
    }
    private Node SelectBossRoom(List<Node> leafNodes)
    {
        // ���� ū ���� ��带 ���� ������ ����
        Node bossRoomNode = leafNodes[0];
        foreach (Node node in leafNodes)
        {
            if (node.nodeRect.size.magnitude > bossRoomNode.nodeRect.size.magnitude)
            {
                bossRoomNode = node;
            }
        }
        return bossRoomNode;
    }

    private void SpawnBoss(Node bossRoomNode)
    {
        if (bossRoomNode != null)
        {
            Vector3 spawnPosition = GetRandomPositionInRoom(bossRoomNode.roomRect);
            Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        }
    }
    private void PlaceContentInRoom(RectInt roomRect)
    {
        // ���Ϳ� �������� ������ �����ϰ� �����մϴ�.
        int monsterCount = Random.Range(4, 9); // �� �濡 4~9������ ���͸� ��ġ
        int itemCount = Random.Range(0, 2);    // �� �濡 0~2���� �������� ��ġ

        // ���� ��ġ
        for (int i = 0; i < monsterCount; i++)
        {
            // ���� ������ �� �ϳ��� �����ϰ� ����
            GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];

            // �� ���� ������ ��ġ�� ����
            Vector3 position = GetRandomPositionInRoom(roomRect);

            // ���� ����
            Instantiate(monsterPrefab, position, Quaternion.identity);
        }

        // ������ ��ġ
        for (int i = 0; i < itemCount; i++)
        {
            // ������ ������ �� �ϳ��� �����ϰ� ����
            GameObject itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

            // �� ���� ������ ��ġ�� ����
            Vector3 position = GetRandomPositionInRoom(roomRect);

            // ������ ����
            Instantiate(itemPrefab, position, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPositionInRoom(RectInt roomRect)
    {
        int x = Random.Range(roomRect.x + 1, roomRect.x + roomRect.width - 1);
        int y = Random.Range(roomRect.y + 1, roomRect.y + roomRect.height - 1);

        // Ÿ�ϸ� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3Int tilePosition = GetTilePosition(x, y);
        Vector3 worldPosition = tileMap.CellToWorld(tilePosition);

        // �ణ�� �������� �߰��Ͽ� �߾ӿ� ��ġ
        worldPosition += tileMap.tileAnchor;

        return worldPosition;
    }

    //private void GenerateLoad(Node tree)
    //{
    //    if (tree.leftNode == null || tree.rightNode == null)
    //        return;

    //    Vector2Int leftCenter = tree.leftNode.center;
    //    Vector2Int rightCenter = tree.rightNode.center;

    //    // ���� �̵�
    //    for (int x = Mathf.Min(leftCenter.x, rightCenter.x); x <= Mathf.Max(leftCenter.x, rightCenter.x); x++)
    //    {
    //        tileMap.SetTile(GetTilePosition(x, leftCenter.y), roomTile);
    //    }

    //    // ���� �̵�
    //    for (int y = Mathf.Min(leftCenter.y, rightCenter.y); y <= Mathf.Max(leftCenter.y, rightCenter.y); y++)
    //    {
    //        tileMap.SetTile(GetTilePosition(rightCenter.x, y), roomTile);
    //    }

    //    GenerateLoad(tree.leftNode);
    //    GenerateLoad(tree.rightNode);
    //}
    private void GenerateLoad(Node tree)
    {
        // �켱 ��� ���� ��带 �����ɴϴ�.
        List<Node> leafNodes = new List<Node>();
        GetLeafNodes(tree, leafNodes);

        // ��� ���� �ּ����� ����� �����մϴ�.
        for (int i = 0; i < leafNodes.Count - 1; i++)
        {
            ConnectRooms(leafNodes[i], leafNodes[i + 1]);
        }

        // �߰����� ���� ��θ� ������ Ȯ���� ����ϴ� (15% Ȯ���� �߰� ��� ����)
        float additionalPathChance = 0.15f; // �߰����� ��� ���� Ȯ��
        for (int i = 0; i < leafNodes.Count; i++)
        {
            if (Random.value < additionalPathChance)
            {
                Node roomA = leafNodes[Random.Range(0, leafNodes.Count)];
                Node roomB = leafNodes[Random.Range(0, leafNodes.Count)];

                if (roomA != roomB && !AreRoomsConnected(roomA, roomB))
                {
                    ConnectRooms(roomA, roomB);
                }
            }
        }
    }
    private void ConnectRooms(Node roomA, Node roomB)
    {
        Vector2Int pointA = GetRandomPointInRoom(roomA.roomRect);
        Vector2Int pointB = GetRandomPointInRoom(roomB.roomRect);

        // ���� �Ǵ� ���� �̵��� �������� �����Ͽ� �����մϴ�.
        if (Random.value > 0.5f)
        {
            // ���� �̵� �� ���� �̵�
            for (int x = Mathf.Min(pointA.x, pointB.x); x <= Mathf.Max(pointA.x, pointB.x); x++)
            {
                tileMap.SetTile(GetTilePosition(x, pointA.y), roomTile);
            }
            for (int y = Mathf.Min(pointA.y, pointB.y); y <= Mathf.Max(pointA.y, pointB.y); y++)
            {
                tileMap.SetTile(GetTilePosition(pointB.x, y), roomTile);
            }
        }
        else
        {
            // ���� �̵� �� ���� �̵�
            for (int y = Mathf.Min(pointA.y, pointB.y); y <= Mathf.Max(pointA.y, pointB.y); y++)
            {
                tileMap.SetTile(GetTilePosition(pointA.x, y), roomTile);
            }
            for (int x = Mathf.Min(pointA.x, pointB.x); x <= Mathf.Max(pointA.x, pointB.x); x++)
            {
                tileMap.SetTile(GetTilePosition(x, pointB.y), roomTile);
            }
        }
    }
    private bool AreRoomsConnected(Node roomA, Node roomB)
    {
        // Ư�� ����� �̹� ����Ǿ����� ���θ� Ȯ���ϴ� ������ �ʿ��մϴ�.
        // �� ���ÿ����� �ܼ��ϰ� ������ ������� ���� ���θ� üũ�ϴ� �κ��� �����߽��ϴ�.
        return false; // �⺻������ false ��ȯ
    }
    private Vector2Int GetRandomPointInRoom(RectInt roomRect)
    {
        // �� ������ ������ ��ġ�� ��ȯ�մϴ�.
        int x = Random.Range(roomRect.x + 1, roomRect.xMax - 1);
        int y = Random.Range(roomRect.y + 1, roomRect.yMax - 1);
        return new Vector2Int(x, y);
    }

    private void ShuffleList<T>(List<T> list)
    {
        // Fisher-Yates Shuffle�� �̿��� ����Ʈ�� �������� �����ϴ�.
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void FillBackground() // ����� ä��� �Լ�, �� �ε� �� ���� ���� ���ش�.
    {
        for (int i = -10; i < mapSize.x + 10; i++)
        {
            for (int j = -10; j < mapSize.y + 10; j++)
            {
                tileMap.SetTile(GetTilePosition(i, j), outTile);
            }
        }
    }

    void FillWall() // �� Ÿ�ϰ� �ٱ� Ÿ���� ������ �κ��� ������ ä���.
    {
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                Vector3Int currentPos = GetTilePosition(i, j);
                if (tileMap.GetTile(currentPos) == outTile)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            if (x == 0 && y == 0) continue;

                            int checkX = i + x;
                            int checkY = j + y;

                            if (checkX >= 0 && checkX < mapSize.x && checkY >= 0 && checkY < mapSize.y)
                            {
                                Vector3Int neighborPos = GetTilePosition(checkX, checkY);
                                if (tileMap.GetTile(neighborPos) == roomTile)
                                {
                                    tileMap.SetTile(currentPos, wallTile);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void FillRoom(RectInt rect)
    { // room�� Rect ������ �޾Ƽ� Ÿ���� �������ִ� �Լ�
        for (int i = rect.x; i < rect.x + rect.width; i++)
        {
            for (int j = rect.y; j < rect.y + rect.height; j++)
            {
                tileMap.SetTile(GetTilePosition(i, j), roomTile);
            }
        }
    }

    private Vector3Int GetTilePosition(int x, int y)
    {
        return new Vector3Int(x - mapSize.x / 2, y - mapSize.y / 2, 0);
    }
    private void SpawnPlayer(Node tree)
    {
        List<Node> leafNodes = new List<Node>();
        GetLeafNodes(tree, leafNodes);

        if (leafNodes.Count > 0)
        {
            // ������ ���� ���� ��� �� �ϳ��� �����ϰ� ����
            Node spawnRoom = leafNodes[Random.Range(0, leafNodes.Count)];

            // ���� �߽� ��ġ�� ���
            Vector3 spawnPosition = GetRandomPositionInRoom(spawnRoom.roomRect);

            // �÷��̾� ������ �����ϰ� GameManager�� ����
            GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            GameManager.instance.SetPlayer(player);
        }
    }

    private void GetLeafNodes(Node tree, List<Node> leafNodes)
    {
        if (tree.leftNode == null && tree.rightNode == null)
        {
            leafNodes.Add(tree);
        }
        else
        {
            if (tree.leftNode != null)
                GetLeafNodes(tree.leftNode, leafNodes);
            if (tree.rightNode != null)
                GetLeafNodes(tree.rightNode, leafNodes);
        }
    }

}

