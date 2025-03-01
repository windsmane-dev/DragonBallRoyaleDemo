using UnityEngine;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    public MazeSpawner spawner;
    protected GameObject mazePlayer;
    private void Start()
    {
        EventHolder.OnDrawMatch += GenerateMaze;
    }

    public void SetPlayer(GameObject obj)
    {
        mazePlayer = obj;
    }

    void GenerateMaze()
    {
        spawner.GenerateMaze(mazePlayer);
    }

    private void OnDestroy()
    {
        EventHolder.OnDrawMatch -= GenerateMaze;
    }
}