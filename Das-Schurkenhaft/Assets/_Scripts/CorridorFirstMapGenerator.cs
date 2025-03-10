using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
public class CorridorFirstMapGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;
    private HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
    public Transform player;
    HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
        Debug.Log("Spawn Position: " + GetSpawnPosition());
        Debug.Log("Exit Position: " + GetExitPosition());
        if (player != null)
        {
            player.position = (Vector3Int)GetSpawnPosition();
        }
        FogOfWar fogOfWar = FindAnyObjectByType<FogOfWar>();
        fogOfWar.Start();

        List<Vector2Int> EnemySpawnPoints = GetEnemySpawnPoints();
    }

    public List<Vector2Int> GetEnemySpawnPoints()
    {
        List<Vector2Int> enemySpawnPoints = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            var spawnPoint = Random.Range(1, 200);
            if (spawnPoint == 1)
            {
                enemySpawnPoints.Add(position);
            }
        }
        return enemySpawnPoints;
    }

    private void CorridorFirstGeneration()
    {
        
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);

        roomPositions = CreateRooms(potentialRoomPositions);

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnds(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);

        for (int i = 0; i < corridors.Count; i++)
        {
            corridors[i] = IncreaseCorridorBrush3by3(corridors[i]);
            floorPositions.UnionWith(corridors[i]);
        }

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    public Vector2Int GetSpawnPosition()
    {
        Vector2Int spawnPosition = Vector2Int.zero;
        int minDistance = int.MaxValue;
        foreach (var position in roomPositions)
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
        int maxDistance = int.MinValue;
        foreach (var position in roomPositions)
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

    private void CreateRoomsAtDeadEnds(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomPositions)
    {
        foreach (var position in deadEnds)
        {
            if (roomPositions.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomPositions.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            int neighborsCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if (floorPositions.Contains(position + direction))
                {
                    neighborsCount++;
                }
            }
            if (neighborsCount == 1)
            {
                deadEnds.Add(position);
            }
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            corridors.Add(corridor);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
        return corridors;
    }
}
