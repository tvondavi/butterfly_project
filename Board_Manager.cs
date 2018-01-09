using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Board_Manager : MonoBehaviour {

	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 10;
	public int rows = 10;
	public Count wallCount = new Count (6, 9);
	public GameObject portal;
	public GameObject flower1;
	public GameObject flower2;
	public GameObject flower3;
	public GameObject flower4;
	public GameObject[] floorTiles;
	public GameObject[] outerWallTiles;
	public GameObject[] wallTiles;
	public GameObject[] enemyTiles;

	private Transform boardHolder;
	private List <Vector3> gridPositions = new List<Vector3>();


	//Use this code to place enemies, inner walls, items, and it prevents the level from becoming impossible
	void InitialiseList()
	{
		gridPositions.Clear ();

		for (int x = 1; x < columns - 1; x++) 
		{
			for (int y = 1; y < rows - 1; y++) 
			{
				gridPositions.Add(new Vector3(x,y,0f));
			}
		}
	}
	// This code gets called by the board_manager to actually create the tiles for the board
	void BoardSetup()
	{
		boardHolder = new GameObject ("Board").transform;

		for (int x = -1; x < columns + 1; x++) 
		{
			for (int y = -1; y < rows + 1; y++) 
			{
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];

				if (x == -1 || x == columns || y == -1 || y == rows)
					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];

				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				instance.transform.SetParent (boardHolder);
			}
		}
	}

	Vector3 CheckPosition()
	{
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 checkPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return checkPosition;
	}

	void RandomObjectLayout(GameObject[] tileArray, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum + 1);

		for (int i = 0; i < objectCount; i++) 
		{
			Vector3 checkPosition = CheckPosition ();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, checkPosition, Quaternion.identity);
		}
	}

	public void SetupLevel (int level)
	{
		BoardSetup ();
		InitialiseList ();
		RandomObjectLayout (wallTiles, wallCount.minimum, wallCount.maximum);
		int enemyCount = (int)Mathf.Log (level, 2f);
		RandomObjectLayout (enemyTiles, enemyCount, enemyCount);
		Instantiate (portal, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);

		if (level == 3)
		{
			Instantiate (flower1, new Vector3 (columns - 3, rows - 3, 0f), Quaternion.identity);
		}
		if (level == 5) 
		{
			Instantiate (flower2, new Vector3 (columns - 3, rows - 3, 0f), Quaternion.identity);
		}
		if (level == 6) 
		{
			Instantiate (flower3, new Vector3 (columns - 3, rows - 3, 0f), Quaternion.identity);
		}
		if (level == 8) 
		{
			Instantiate (flower4, new Vector3 (columns - 3, rows - 3, 0f), Quaternion.identity);
		}
	}
}
