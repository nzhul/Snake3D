using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	[Header("Maps:")]
	public List<Map> maps;

	[Header("Common Settings:")]
	public int mapIndex;
	public Transform tilePrefab;
	public Transform obstaclePrefab;
	public float tileSize;
	public bool[,] obstacleMap;
	[Range(0, 1)]
	public float outlinePercent;

	List<Coord> allTileCoords;
	List<GameObject> allObstacles;
	Queue<Coord> shuffledTileCoords;

	Map currentMap;

	[Header("Random Map Limits:")]
	public int maxWidth = 15;
	public int minWidth = 5;
	public int maxHeight = 15;
	public int minHeight = 5;
	public float maxObstaclePercent = .15f;
	public float minObstaclePercent = .02f;
	public float minObstacleHeight = .5f;
	public float maxObstacleHeight = 2.5f;
	private System.Random rng;

	void Awake()
	{
		rng = new System.Random();
		maps.Add(CreateRandomMap());
		maps.Add(CreateRandomMap());
		GenerateMap();
	}

	public Map CreateRandomMap()
	{
		Map newMap = new Map();
		newMap.mapHeight = rng.Next(minHeight, maxHeight);
		newMap.mapWidth = rng.Next(minWidth, maxWidth);
		newMap.minObstacleHeight = minObstacleHeight;
		newMap.maxObstacleHeight = maxObstacleHeight;
		newMap.obstaclePercent = (float)rng.NextDouble() * (maxObstaclePercent - minObstaclePercent) + minObstaclePercent;

		return newMap;
	}

	public void NextMapLevel()
	{
		mapIndex++;
		GenerateMap();
		maps.Add(CreateRandomMap());
    }

	public Map GetCurrentMap()
	{
		return currentMap;
	}

	public void GenerateMap()
	{
		currentMap = maps[mapIndex];
		System.Random prng = new System.Random(currentMap.seed);

		allTileCoords = new List<Coord>();
		obstacleMap = new bool[currentMap.mapWidth, currentMap.mapHeight];

		for (int x = 0; x < currentMap.mapWidth; x++)
		{
			for (int y = 0; y < currentMap.mapHeight; y++)
			{
				allTileCoords.Add(new Coord(x, y));
			}
		}

		List<Coord> allTileCoordsWithoutMapEdge = TrimMapEdge(allTileCoords);

		shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoordsWithoutMapEdge.ToArray(), currentMap.seed));

		string holderName = "GeneratedMap";
		if (transform.FindChild(holderName))
		{
			DestroyImmediate(transform.FindChild(holderName).gameObject);
		}

		Transform mapHolder = new GameObject(holderName).transform;
		mapHolder.parent = transform;

		for (int x = 0; x < currentMap.mapWidth; x++)
		{
			for (int y = 0; y < currentMap.mapHeight; y++)
			{
				Vector3 tilePosition = CoordToPosition(x, y);
				Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
				newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
				newTile.parent = mapHolder;
			}
		}

		allObstacles = new List<GameObject>();
        int obstaclecount = (int)(currentMap.mapWidth * currentMap.mapHeight * currentMap.obstaclePercent);
		for (int i = 0; i < obstaclecount; i++)
		{
			Coord randomCoord = GetRandomCoord();

			//// Ensure the obstacle is not mapEdge
			//while (IsEdgeOftheMap(randomCoord))
			//{
			//	randomCoord = GetRandomCoord();
			//}

			if (randomCoord != currentMap.mapCenter)
			{
				float obstacleHeight = Mathf.Lerp(currentMap.minObstacleHeight, currentMap.maxObstacleHeight, (float)prng.NextDouble());

				obstacleMap[randomCoord.x, randomCoord.y] = true;
				Vector3 obstaclePosition = CoordToPosition(randomCoord);
				Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * obstacleHeight / 2, Quaternion.identity) as Transform;
				allObstacles.Add(newObstacle.gameObject);
				newObstacle.parent = mapHolder;
				newObstacle.localScale = new Vector3((1 - outlinePercent) * tileSize * .9f, obstacleHeight, (1 - outlinePercent) * tileSize * .9f);
			}
		}
	}

	private List<Coord> TrimMapEdge(List<Coord> allTileCoords)
	{
		List<Coord> outputList = new List<Coord>();

		for (int i = 0; i < allTileCoords.Count; i++)
		{
			Coord currentTile = allTileCoords[i];
			if (IsEdgeOftheMap(currentTile))
			{
				continue;
			}

			outputList.Add(currentTile);
		}

		return outputList;
	}

	public void DestroyObstacleAtPosition(Coord coord)
	{
		Vector3 targetPosition = this.CoordToPosition(coord);
		GameObject theObstacle = this.allObstacles.FirstOrDefault(o => o.transform.position.x == targetPosition.x && o.transform.position.z == targetPosition.z);
		this.allObstacles.Remove(theObstacle);
		Destroy(theObstacle.gameObject);

		obstacleMap[coord.x, coord.y] = false;
	}

	public bool IsEdgeOftheMap(Coord randomCoord)
	{
		return randomCoord.x == 0 || randomCoord.x == currentMap.mapWidth - 1 || randomCoord.y == 0 || randomCoord.y == currentMap.mapHeight - 1;
	}

	private void Print2DArray(bool[,] obstacleMap)
	{
		string output = "";
		for (int i = 0; i < obstacleMap.GetLength(0); i++)
		{
			for (int y = 0; y < obstacleMap.GetLength(1); y++)
			{
				if (obstacleMap[i, y])
				{
					output += "1 ";
				}
				else
				{
					output += "0 ";
				}
			}
			output += "\n";
		}

		Debug.Log(output);
	}

	public Vector3 CoordToPosition(int x, int y)
	{
		return new Vector3(-currentMap.mapWidth / 2 + .5f + x, 0, -currentMap.mapHeight / 2 + .5f + y) * tileSize;
	}

	public Vector3 CoordToPosition(Coord coord)
	{
		return new Vector3(-currentMap.mapWidth / 2 + .5f + coord.x, 0, -currentMap.mapHeight / 2 + .5f + coord.y) * tileSize;
	}

	public Coord GetRandomCoord()
	{
		Coord randomCoord = shuffledTileCoords.Dequeue();
		shuffledTileCoords.Enqueue(randomCoord);

		return randomCoord;
	}
}