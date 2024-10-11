using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2Int position; // 방의 좌표
    public List<Room> connectedRooms; // 연결된 방들

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
    public int mapWidth = 10; // 맵의 너비
    public int mapHeight = 10; // 맵의 높이
    public int maxRooms = 20; // 생성할 최대 방 수

    public GameObject roomPrefab; // 방 프리팹
    public GameObject corridorPrefab; // 복도 프리팹
    public GameObject[] monsterPrefabs; // 몬스터 프리팹 배열
    public GameObject[] itemPrefabs; // 아이템 프리팹 배열

    private List<Room> rooms; // 생성된 방 목록
    private List<Vector2Int> possibleDirections = new List<Vector2Int> // 가능한 방향
    {
        new Vector2Int(1, 0),  // 동쪽
        new Vector2Int(-1, 0), // 서쪽
        new Vector2Int(0, 1),  // 북쪽
        new Vector2Int(0, -1)  // 남쪽
    };

    void Start()
    {
        GenerateDungeon(); // 던전 생성 시작
    }

    void GenerateDungeon()
    {
        rooms = new List<Room>();

        // 시작 방 생성 (0, 0 좌표)
        Room startRoom = new Room(Vector2Int.zero);
        rooms.Add(startRoom);

        // 방들을 생성하고 연결하기 위한 BFS 방식 큐
        Queue<Room> roomQueue = new Queue<Room>();
        roomQueue.Enqueue(startRoom);

        while (roomQueue.Count > 0 && rooms.Count < maxRooms)
        {
            Room currentRoom = roomQueue.Dequeue();

            foreach (var direction in possibleDirections)
            {
                Vector2Int newPosition = currentRoom.position + direction;

                // 방이 맵 범위를 넘지 않게 하고, 이미 생성된 방이 있는지 확인
                if (newPosition.x >= -mapWidth / 2 && newPosition.x <= mapWidth / 2 &&
                    newPosition.y >= -mapHeight / 2 && newPosition.y <= mapHeight / 2 &&
                    !RoomExistsAt(newPosition))
                {
                    Room newRoom = new Room(newPosition);
                    currentRoom.ConnectRoom(newRoom);
                    rooms.Add(newRoom);
                    roomQueue.Enqueue(newRoom);

                    // 방을 맵에 생성
                    Instantiate(roomPrefab, new Vector3(newPosition.x * 5f, 0, newPosition.y * 5f), Quaternion.identity);

                    // 방 사이에 복도를 생성
                    Vector2 corridorPosition = ((Vector2)currentRoom.position + (Vector2)newRoom.position) / 2f;
                    Instantiate(corridorPrefab, new Vector3(corridorPosition.x * 5f, 0, corridorPosition.y * 5f), Quaternion.identity);


                    // 방 안에 몬스터와 아이템을 랜덤으로 추가
                    AddContentToRoom(newRoom);
                }
            }
        }
    }

    // 해당 위치에 방이 있는지 확인하는 함수
    bool RoomExistsAt(Vector2Int position)
    {
        foreach (var room in rooms)
        {
            if (room.position == position)
                return true;
        }
        return false;
    }

    // 방에 콘텐츠(몬스터, 아이템)를 추가하는 함수
    void AddContentToRoom(Room room)
    {
        // 몬스터를 랜덤으로 추가
        if (monsterPrefabs.Length > 0)
        {
            int randomMonsterIndex = Random.Range(0, monsterPrefabs.Length);
            Vector3 roomPosition = new Vector3(room.position.x * 5f, 0, room.position.y * 5f);
            Instantiate(monsterPrefabs[randomMonsterIndex], roomPosition + new Vector3(0.5f, 0, 0.5f), Quaternion.identity);
        }

        // 아이템을 랜덤으로 추가
        if (itemPrefabs.Length > 0)
        {
            int randomItemIndex = Random.Range(0, itemPrefabs.Length);
            Vector3 roomPosition = new Vector3(room.position.x * 5f, 0, room.position.y * 5f);
            Instantiate(itemPrefabs[randomItemIndex], roomPosition + new Vector3(-0.5f, 0, -0.5f), Quaternion.identity);
        }
    }
}
