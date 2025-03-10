using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class RoomFirstMapGenerator : SimpleRandomWalkMapGenerator
{
    public static RoomFirstMapGenerator instance { get; private set; }
    
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int mapWidth = 20, mapHeight = 20;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;
    List<Vector2Int> roomCenters = new List<Vector2Int>();
    public Transform player;

    HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

    [SerializeField]
    private float npcHolderOffsetDistance = 1f;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
        Debug.Log("Spawn Position: " + GetSpawnPosition());
        Debug.Log("Exit Position: " + GetExitPosition());
        
        if (player != null)
        {
            Vector3 spawnPos = (Vector3Int)GetSpawnPosition();
            player.position = spawnPos;
            
            GameObject npcHolder = GameObject.Find("NPC holder");
            if (npcHolder == null)
            {
                npcHolder = new GameObject("NPC holder");
            }
            
            npcHolder.transform.position = player.position + player.right * npcHolderOffsetDistance;
        }
        
        FogOfWar fogOfWar = FindAnyObjectByType<FogOfWar>();
        fogOfWar.Start();

        List<Vector2Int> EnemySpawnPoints = GetEnemySpawnPoints();
    }
    

    public List<Vector2Int> GetEnemySpawnPoints()
    {
        List<Vector2Int> enemySpawnPoints = new List<Vector2Int>();
        foreach (var position in floor)
        {
            var spawnPoint = Random.Range(1, 200);
            if (spawnPoint == 1)
            {
                enemySpawnPoints.Add(position);
            }
        }
        return enemySpawnPoints;
    }
    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(mapWidth, mapHeight, 0)), minRoomWidth, minRoomHeight);
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        List<Vector2Int> roomCentersConnect = new List<Vector2Int>();

        if (randomWalkRooms)
        {
            floor = CreateRandomRooms(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }

        foreach (var room in roomsList)
        {
            roomCentersConnect.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        List<List<Vector2Int>> corridors = ConnectRooms(roomCentersConnect);
        foreach (var corridor in corridors)
        {
            floor.UnionWith(corridor);
        }

        for (int i = 0; i < corridors.Count; i++)
        {
            corridors[i] = IncreaseCorridorBrush3by3(corridors[i]);
            floor.UnionWith(corridors[i]);
        }

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }

    public Vector2Int GetSpawnPosition()
    {
        Vector2Int spawnPosition = Vector2Int.zero;
        int minDistance = int.MaxValue;
        foreach (var position in roomCenters)
        {
            int distance = Mathf.Abs(position.x) + Mathf.Abs(position.y);
            if (distance < minDistance)
            {
                minDistance = distance;
                spawnPosition = position;
            }
        }
        return spawnPosition;
    }

    public Vector2Int GetExitPosition()
    {
        Vector2Int exitPosition = Vector2Int.zero;
        int maxDistance = 0;
        foreach (var position in roomCenters)
        {
            int distance = Mathf.Abs(position.x) + Mathf.Abs(position.y);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                exitPosition = position;
            }
        }
        return exitPosition;
    }

    private List<Vector2Int> IncreaseCorridorBrush3by3(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        for (int i = 1; i < corridor.Count; i++)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                }
            }
        }
        return newCorridor;
    }

    private HashSet<Vector2Int> CreateRandomRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset) && 
                    position.x <= (roomBounds.xMax - offset) && 
                    position.y >= (roomBounds.yMin + offset) && 
                    position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private List<List<Vector2Int>> ConnectRooms(List<Vector2Int> roomCentersConnect)
    {
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();
        var currentRoomCenter = roomCentersConnect[Random.Range(0, roomCentersConnect.Count)];
        roomCentersConnect.Remove(currentRoomCenter);

        while (roomCentersConnect.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCentersConnect);
            roomCentersConnect.Remove(closest);
            List<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.Add(newCorridor);
        }
        return corridors;
    }

    private List<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.executeOnLoad == 0)
        {
            tilemapVisualizer.Clear();
            RunProceduralGeneration();
            GameManager.Instance.executeOnLoad = 2;
        }
    }
}
