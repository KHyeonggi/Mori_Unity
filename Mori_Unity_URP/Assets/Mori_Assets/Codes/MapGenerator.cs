using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int mapSize;
    [SerializeField] float minimumDivideRate; // 공간이 나눠지는 최소 비율
    [SerializeField] float maximumDivideRate; // 공간이 나눠지는 최대 비율
    [SerializeField] private GameObject line; // LineRenderer를 사용해서 공간이 나눠진 것을 시각적으로 보여주기 위함
    [SerializeField] private GameObject map; // LineRenderer를 사용해서 첫 맵의 사이즈를 보여주기 위함
    [SerializeField] private GameObject roomLine; // LineRenderer를 사용해서 방의 사이즈를 보여주기 위함
    [SerializeField] private int maximumDepth; // 트리의 높이, 높을수록 방을 더 자세히 나누게 됨
    [SerializeField] Tilemap tileMap;
    [SerializeField] Tile roomTile; // 방을 구성하는 타일
    [SerializeField] Tile wallTile; // 방과 외부를 구분지어줄 벽 타일
    [SerializeField] Tile outTile; // 방 외부의 타일

    [SerializeField] private GameObject[] monsterPrefabs; // 몬스터 프리팹 배열
    [SerializeField] private GameObject[] itemPrefabs;    // 아이템 프리팹 배열

    [SerializeField] private GameObject playerPrefab; // 플레이어 프리팹
    [SerializeField] private GameObject bossPrefab;

    void Start()
    {
        FillBackground(); // 씬 로드 시 전부 다 바깥 타일로 덮음
        Node root = new Node(new RectInt(0, 0, mapSize.x, mapSize.y));
        Divide(root, 0);
        List<Node> leafNodes = new List<Node>();
        GetLeafNodes(root, leafNodes);

        Node bossRoomNode = SelectBossRoom(leafNodes);
        GenerateRoom(root, bossRoomNode);
        GenerateLoad(root);
        FillWall(); // 바깥과 방이 만나는 지점을 벽으로 칠해주는 함수

        SpawnPlayer(root); //**플레이어 스폰 함수 호출**
        SpawnBoss(bossRoomNode); // 보스 스폰

        FillWall(); // 바깥과 방이 만나는 지점을 벽으로 칠해주는 함수
    }

    void Divide(Node tree, int depth)
    {
        if (depth == maximumDepth) return;

        int maxLength = Mathf.Max(tree.nodeRect.width, tree.nodeRect.height);
        int minSplit = Mathf.RoundToInt(maxLength * minimumDivideRate);
        int maxSplit = Mathf.RoundToInt(maxLength * maximumDivideRate);

        if (maxSplit <= minSplit)
        {
            maxSplit = minSplit + 1; // 최소한의 분할을 보장
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
        if (tree.leftNode == null && tree.rightNode == null) // 리프 노드인 경우
        {
            RectInt rect = tree.nodeRect;
            int width = Random.Range(rect.width / 2, rect.width - 1);
            int height = Random.Range(rect.height / 2, rect.height - 1);

            width = Mathf.Clamp(width, 3, rect.width - 2);   // 최소 크기 보장
            height = Mathf.Clamp(height, 3, rect.height - 2); // 최소 크기 보장

            int x = rect.x + Random.Range(1, rect.width - width);
            int y = rect.y + Random.Range(1, rect.height - height);

            tree.roomRect = new RectInt(x, y, width, height);
            FillRoom(tree.roomRect);

            // **몬스터와 아이템 배치 추가**
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
        // 가장 큰 리프 노드를 보스 방으로 선택
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
        // 몬스터와 아이템의 개수를 랜덤하게 결정합니다.
        int monsterCount = Random.Range(4, 9); // 각 방에 4~9마리의 몬스터를 배치
        int itemCount = Random.Range(0, 2);    // 각 방에 0~2개의 아이템을 배치

        // 몬스터 배치
        for (int i = 0; i < monsterCount; i++)
        {
            // 몬스터 프리팹 중 하나를 랜덤하게 선택
            GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];

            // 방 안의 랜덤한 위치를 선택
            Vector3 position = GetRandomPositionInRoom(roomRect);

            // 몬스터 생성
            Instantiate(monsterPrefab, position, Quaternion.identity);
        }

        // 아이템 배치
        for (int i = 0; i < itemCount; i++)
        {
            // 아이템 프리팹 중 하나를 랜덤하게 선택
            GameObject itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

            // 방 안의 랜덤한 위치를 선택
            Vector3 position = GetRandomPositionInRoom(roomRect);

            // 아이템 생성
            Instantiate(itemPrefab, position, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPositionInRoom(RectInt roomRect)
    {
        int x = Random.Range(roomRect.x + 1, roomRect.x + roomRect.width - 1);
        int y = Random.Range(roomRect.y + 1, roomRect.y + roomRect.height - 1);

        // 타일맵 좌표를 월드 좌표로 변환
        Vector3Int tilePosition = GetTilePosition(x, y);
        Vector3 worldPosition = tileMap.CellToWorld(tilePosition);

        // 약간의 오프셋을 추가하여 중앙에 배치
        worldPosition += tileMap.tileAnchor;

        return worldPosition;
    }

    //private void GenerateLoad(Node tree)
    //{
    //    if (tree.leftNode == null || tree.rightNode == null)
    //        return;

    //    Vector2Int leftCenter = tree.leftNode.center;
    //    Vector2Int rightCenter = tree.rightNode.center;

    //    // 수평 이동
    //    for (int x = Mathf.Min(leftCenter.x, rightCenter.x); x <= Mathf.Max(leftCenter.x, rightCenter.x); x++)
    //    {
    //        tileMap.SetTile(GetTilePosition(x, leftCenter.y), roomTile);
    //    }

    //    // 수직 이동
    //    for (int y = Mathf.Min(leftCenter.y, rightCenter.y); y <= Mathf.Max(leftCenter.y, rightCenter.y); y++)
    //    {
    //        tileMap.SetTile(GetTilePosition(rightCenter.x, y), roomTile);
    //    }

    //    GenerateLoad(tree.leftNode);
    //    GenerateLoad(tree.rightNode);
    //}
    private void GenerateLoad(Node tree)
    {
        // 우선 모든 리프 노드를 가져옵니다.
        List<Node> leafNodes = new List<Node>();
        GetLeafNodes(tree, leafNodes);

        // 모든 방을 최소한의 연결로 연결합니다.
        for (int i = 0; i < leafNodes.Count - 1; i++)
        {
            ConnectRooms(leafNodes[i], leafNodes[i + 1]);
        }

        // 추가적인 연결 경로를 생성할 확률을 낮춥니다 (15% 확률로 추가 경로 생성)
        float additionalPathChance = 0.15f; // 추가적인 경로 생성 확률
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

        // 수평 또는 수직 이동을 무작위로 선택하여 연결합니다.
        if (Random.value > 0.5f)
        {
            // 수평 이동 후 수직 이동
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
            // 수직 이동 후 수평 이동
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
        // 특정 방들이 이미 연결되었는지 여부를 확인하는 로직이 필요합니다.
        // 이 예시에서는 단순하게 임의의 방식으로 연결 여부를 체크하는 부분을 생략했습니다.
        return false; // 기본적으로 false 반환
    }
    private Vector2Int GetRandomPointInRoom(RectInt roomRect)
    {
        // 방 내부의 임의의 위치를 반환합니다.
        int x = Random.Range(roomRect.x + 1, roomRect.xMax - 1);
        int y = Random.Range(roomRect.y + 1, roomRect.yMax - 1);
        return new Vector2Int(x, y);
    }

    private void ShuffleList<T>(List<T> list)
    {
        // Fisher-Yates Shuffle을 이용해 리스트를 무작위로 섞습니다.
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void FillBackground() // 배경을 채우는 함수, 씬 로드 시 가장 먼저 해준다.
    {
        for (int i = -10; i < mapSize.x + 10; i++)
        {
            for (int j = -10; j < mapSize.y + 10; j++)
            {
                tileMap.SetTile(GetTilePosition(i, j), outTile);
            }
        }
    }

    void FillWall() // 룸 타일과 바깥 타일이 만나는 부분을 벽으로 채운다.
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
    { // room의 Rect 정보를 받아서 타일을 설정해주는 함수
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
            // 스폰할 방을 리프 노드 중 하나로 랜덤하게 선택
            Node spawnRoom = leafNodes[Random.Range(0, leafNodes.Count)];

            // 방의 중심 위치를 계산
            Vector3 spawnPosition = GetRandomPositionInRoom(spawnRoom.roomRect);

            // 플레이어 프리팹 생성하고 GameManager에 설정
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

