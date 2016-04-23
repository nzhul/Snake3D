using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GridManager : MonoBehaviour {

	List<GridTile> allTileCoords;
	public int mapWidth;
	public int mapHeight;
	public float tileSize;
	public Transform tilePrefab;

	[Range(0, 1)]
	public float outlinePercent;

	void Awake()
	{
		GenerateGrid();
	}

	public void GenerateGrid()
	{
		allTileCoords = new List<GridTile>();
		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				GridTile newGridTile = new GridTile();
				newGridTile.position = new Coord(x, y);
                allTileCoords.Add(newGridTile);
			}
		}

		string holderName = "GeneratedMap";
		if (transform.FindChild(holderName))
		{
			DestroyImmediate(transform.FindChild(holderName).gameObject);
		}

		Transform mapHolder = new GameObject(holderName).transform;
		mapHolder.parent = transform;

		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				Vector3 tilePosition = CoordToPosition(x, y);
				Transform newTileTransform = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
				newTileTransform.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
				newTileTransform.parent = mapHolder;
				GridTile newTile = newTileTransform.GetComponent<GridTile>();
				newTile.position.x = x;
				newTile.position.y = y;
			}
		}
	}

	public Vector3 CoordToPosition(Coord coord)
	{
		return new Vector3(-mapWidth / 2 + .5f + coord.x, 0, -mapHeight / 2 + .5f + coord.y) * tileSize;
	}

	public Vector3 CoordToPosition(int x, int y)
	{
		return new Vector3(-mapWidth / 2 + .5f + x, 0, -mapHeight / 2 + .5f + y) * tileSize;
	}
}
