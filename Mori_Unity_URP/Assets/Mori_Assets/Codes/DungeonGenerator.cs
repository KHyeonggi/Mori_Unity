using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2Int position; // ���� ��ǥ
    public List<Room> connectedRooms; // ����� ���

    public Room(Vector2Int pos)
    {
        position = pos;
        connectedRooms = new List<Room>();
    }

    public void ConnectRoom(Room otherRoom)
    {
        connectedRooms.Add(otherRoom);
        otherRoom.connectedRooms.Add(this);
    }
}

public class DungeonGenerator : MonoBehaviour
{
    public int mapWidth = 10; // ���� �ʺ�
    public int mapHeight = 10; // ���� ����
    public int maxRooms = 20; // ������ �ִ� �� ��

    public GameObject roomPrefab; // �� ������
    public GameObject corridorPrefab; // ���� ������
    public GameObject[] monsterPrefabs; // ���� ������ �迭
    public GameObject[] itemPrefabs; // ������ ������ �迭

    private List<Room> rooms; // ������ �� ���
    private List<Vector2Int> possibleDirections = new List<Vector2Int> // ������ ����
    {
        new Vector2Int(1, 0),  // ����
        new Vector2Int(-1, 0), // ����
        new Vector2Int(0, 1),  // ����
        new Vector2Int(0, -1)  // ����
    };

    void Start()
    {
        GenerateDungeon(); // ���� ���� ����
    }

    void GenerateDungeon()
    {
        rooms = new List<Room>();

        // ���� �� ���� (0, 0 ��ǥ)
        Room startRoom = new Room(Vector2Int.zero);
        rooms.Add(startRoom);

        // ����� �����ϰ� �����ϱ� ���� BFS ��� ť
        Queue<Room> roomQueue = new Queue<Room>();
        roomQueue.Enqueue(startRoom);

        while (roomQueue.Count > 0 && rooms.Count < maxRooms)
        {
            Room currentRoom = roomQueue.Dequeue();

            foreach (var direction in possibleDirections)
            {
                Vector2Int newPosition = currentRoom.position + direction;

                // ���� �� ������ ���� �ʰ� �ϰ�, �̹� ������ ���� �ִ��� Ȯ��
                if (newPosition.x >= -mapWidth / 2 && newPosition.x <= mapWidth / 2 &&
                    newPosition.y >= -mapHeight / 2 && newPosition.y <= mapHeight / 2 &&
                    !RoomExistsAt(newPosition))
                {
                    Room newRoom = new Room(newPosition);
                    currentRoom.ConnectRoom(newRoom);
                    rooms.Add(newRoom);
                    roomQueue.Enqueue(newRoom);

                    // ���� �ʿ� ����
                    Instantiate(roomPrefab, new Vector3(newPosition.x * 5f, 0, newPosition.y * 5f), Quaternion.identity);

                    // �� ���̿� ������ ����
                    Vector2 corridorPosition = ((Vector2)currentRoom.position + (Vector2)newRoom.position) / 2f;
                    Instantiate(corridorPrefab, new Vector3(corridorPosition.x * 5f, 0, corridorPosition.y * 5f), Quaternion.identity);


                    // �� �ȿ� ���Ϳ� �������� �������� �߰�
                    AddContentToRoom(newRoom);
                }
            }
        }
    }

    // �ش� ��ġ�� ���� �ִ��� Ȯ���ϴ� �Լ�
    bool RoomExistsAt(Vector2Int position)
    {
        foreach (var room in rooms)
        {
            if (room.position == position)
                return true;
        }
        return false;
    }

    // �濡 ������(����, ������)�� �߰��ϴ� �Լ�
    void AddContentToRoom(Room room)
    {
        // ���͸� �������� �߰�
        if (monsterPrefabs.Length > 0)
        {
            int randomMonsterIndex = Random.Range(0, monsterPrefabs.Length);
            Vector3 roomPosition = new Vector3(room.position.x * 5f, 0, room.position.y * 5f);
            Instantiate(monsterPrefabs[randomMonsterIndex], roomPosition + new Vector3(0.5f, 0, 0.5f), Quaternion.identity);
        }

        // �������� �������� �߰�
        if (itemPrefabs.Length > 0)
        {
            int randomItemIndex = Random.Range(0, itemPrefabs.Length);
            Vector3 roomPosition = new Vector3(room.position.x * 5f, 0, room.position.y * 5f);
            Instantiate(itemPrefabs[randomItemIndex], roomPosition + new Vector3(-0.5f, 0, -0.5f), Quaternion.identity);
        }
    }
}
