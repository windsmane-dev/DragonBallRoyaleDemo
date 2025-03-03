using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour {
	public enum MazeGenerationAlgorithm{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
	public bool FullRandom = false;
	public int RandomSeed = 12345;
	public GameObject Floor = null;
	public GameObject Wall = null;
	public GameObject Pillar = null;
	public int Rows = 5;
	public int Columns = 5;
	public float CellWidth = 5;
	public float CellHeight = 5;
	public bool AddGaps = true;
	public GameObject GoalPrefab = null;

	public GameObject mazeParent;
	private BasicMazeGenerator mMazeGenerator = null;

	private List<Vector2> goalCells;
    public void GenerateMaze (GameObject player) 
	{
		if (!FullRandom) {
			Random.seed = RandomSeed;
		}

		goalCells = new List<Vector2>();
		switch (Algorithm) {
		case MazeGenerationAlgorithm.PureRecursive:
			mMazeGenerator = new RecursiveMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RecursiveTree:
			mMazeGenerator = new RecursiveTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RandomTree:
			mMazeGenerator = new RandomTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.OldestTree:
			mMazeGenerator = new OldestTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RecursiveDivision:
			mMazeGenerator = new DivisionMazeGenerator (Rows, Columns);
			break;
		}
		mMazeGenerator.GenerateMaze ();
		for (int row = 0; row < Rows; row++) 
		{
			for(int column = 0; column < Columns; column++)
			{
				float x = column*(CellWidth+(AddGaps?.2f:0));
				float z = row*(CellHeight+(AddGaps?.2f:0));
				MazeCell cell = mMazeGenerator.GetMazeCell(row,column);
				GameObject tmp;
				tmp = Instantiate(Floor,new Vector3(x,0,z), Quaternion.Euler(0,0,0)) as GameObject;
				tmp.transform.parent = mazeParent.transform;
				tmp.transform.localPosition = new Vector3(x, 0, z);
				
				if (cell.WallRight)
				{
					tmp = Instantiate(Wall, mazeParent.transform) as GameObject;// right
					tmp.transform.localPosition = new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position;
					tmp.transform.localRotation = Quaternion.Euler(0, 90, 0);
				}
				if(cell.WallFront){
					tmp = Instantiate(Wall, mazeParent.transform) as GameObject;// front
					tmp.transform.localPosition = new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position;
					tmp.transform.localRotation = Quaternion.Euler(0, 0, 0);
				}
				if(cell.WallLeft){
					tmp = Instantiate(Wall, mazeParent.transform) as GameObject;// left
					tmp.transform.localPosition = new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position;
					tmp.transform.localRotation = Quaternion.Euler(0, 270, 0);
				}
				if(cell.WallBack){
					tmp = Instantiate(Wall, mazeParent.transform) as GameObject;// back
					tmp.transform.localPosition = new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position;
					tmp.transform.localRotation = Quaternion.Euler(0, 180, 0);
				}
				if(cell.IsGoal && GoalPrefab != null)
				{
					//tmp = Instantiate(GoalPrefab,new Vector3(x,1,z), Quaternion.Euler(0,0,0)) as GameObject;
					//tmp.transform.parent = transform;

					goalCells.Add(new Vector2(x, z));
				}
			}
		}

		//instantiating the player and the ball
		if(goalCells.Count > 0)
        {
			int randomIndex = Random.Range(0, goalCells.Count);
			Vector3 pos = goalCells[randomIndex];
			pos.z = pos.y;
			pos.y = 0;
			GameObject obj = Instantiate(GoalPrefab, mazeParent.transform) as GameObject;
			obj.transform.localPosition = pos;
			goalCells.RemoveAt(randomIndex);




			randomIndex = Random.Range(0, goalCells.Count);
			pos = goalCells[randomIndex];
			pos.z = pos.y;
			pos.y = 0;

			obj = Instantiate(player, mazeParent.transform);
			obj.transform.localPosition = pos;

		}


		//this is currently hardcoded to save time.
		mazeParent.transform.localScale = new Vector3(0.35f, 0.35f, 0.233625f);
		mazeParent.transform.localPosition = new Vector3(-3.64f, 0, -4.69f);
	}
}
