using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    private int mazeWidth;
    private int mazeHeight;
    private GameObject floorObject;
    private GameObject wallPrefab;
    private GameObject playerPrefab;
    private GameObject ballPrefab;
    private Transform startPoint;
    private Transform exitPoint;

    private int[,] mazeGrid;
    private List<Vector2Int> pathCells = new List<Vector2Int>();

    public void Initialize(GameObject floor, GameObject wall, GameObject player, GameObject ball)
    {
        this.floorObject = floor;
        this.wallPrefab = wall;
        this.playerPrefab = player;
        this.ballPrefab = ball;

        AdjustMazeSize();
    }

    private void AdjustMazeSize()
    {
        Vector3 floorSize = floorObject.GetComponent<Collider>().bounds.size;
        mazeWidth = Mathf.FloorToInt(floorSize.x);
        mazeHeight = Mathf.FloorToInt(floorSize.z);
    }

    public void GenerateMaze(Transform start, Transform exit)
    {
        startPoint = start;
        exitPoint = exit;

        Vector2Int startCell = ConvertWorldToGrid(startPoint.position);
        Vector2Int exitCell = ConvertWorldToGrid(exitPoint.position);

        mazeGrid = new int[mazeWidth, mazeHeight];
        GeneratePath(startCell, exitCell);
        FillMazeWithWalls();
        SpawnPlayerAndBall();
    }

    private Vector2Int ConvertWorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.Clamp(Mathf.RoundToInt(worldPos.x), 0, mazeWidth - 1),
                              Mathf.Clamp(Mathf.RoundToInt(worldPos.z), 0, mazeHeight - 1));
    }

    private void GeneratePath(Vector2Int start, Vector2Int exit)
    {
        pathCells.Clear();
        pathCells.Add(start);
        mazeGrid[start.x, start.y] = 0; // Open path

        Vector2Int current = start;
        while (current != exit)
        {
            List<Vector2Int> neighbors = GetValidNeighbors(current);
            if (neighbors.Count > 0)
            {
                Vector2Int next = neighbors[Random.Range(0, neighbors.Count)];
                mazeGrid[next.x, next.y] = 0; // Mark as path
                pathCells.Add(next);
                current = next;
            }
        }
    }

    private List<Vector2Int> GetValidNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighbor = cell + dir;
            if (IsWithinBounds(neighbor) && !pathCells.Contains(neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private bool IsWithinBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < mazeWidth && pos.y >= 0 && pos.y < mazeHeight;
    }

    private void FillMazeWithWalls()
    {
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                if (mazeGrid[x, y] == 1)
                {
                    Vector3 wallPos = new Vector3(x, 0, y);
                    Instantiate(wallPrefab, wallPos, Quaternion.identity);
                }
            }
        }
    }

    private void SpawnPlayerAndBall()
    {
        Vector2Int playerPos = pathCells[Random.Range(1, pathCells.Count - 1)];
        Vector2Int ballPos = pathCells[Random.Range(1, pathCells.Count - 1)];

        Vector3 playerSpawnPosition = new Vector3(playerPos.x, 0, playerPos.y);
        Vector3 ballSpawnPosition = new Vector3(ballPos.x, 0, ballPos.y);

        Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
        Instantiate(ballPrefab, ballSpawnPosition, Quaternion.identity);
    }
}
