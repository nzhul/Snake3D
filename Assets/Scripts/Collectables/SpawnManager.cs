using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class SpawnManager : MonoBehaviour
{

	MapManager mapManager;
	SnakeManager snakeManager;
	public Collectable collectablePrefab;
	public List<Collectable> allActiveCollectables;
	public GameObject spawnEffectPrefab;

	void Awake()
	{
		mapManager = GameObject.FindObjectOfType<MapManager>();
		snakeManager = GameObject.FindObjectOfType<SnakeManager>();
		allActiveCollectables = new List<Collectable>();
	}

	public void SpawnCollectable()
	{
		Coord spawnPosition = mapManager.GetRandomCoord();
		while (CollideWithObstacleOrSnake(spawnPosition))
		{
			spawnPosition = mapManager.GetRandomCoord();
		}

		Collectable newCollectable = Instantiate(collectablePrefab, mapManager.CoordToPosition(spawnPosition) + Vector3.up * .65f, Quaternion.identity) as Collectable;
		newCollectable.transform.localScale = Vector3.one * (1 - mapManager.outlinePercent) * mapManager.tileSize * (1 - mapManager.outlinePercent * 2);
		newCollectable.position = spawnPosition;
		allActiveCollectables.Add(newCollectable);

		Destroy(Instantiate(spawnEffectPrefab, newCollectable.transform.position, newCollectable.transform.rotation), 3);
	}

	public void DestroyCollectableAtPosition(Coord collectablePosition)
	{
		Collectable theCollectable = this.allActiveCollectables.FirstOrDefault(c => c.position == collectablePosition);
		this.allActiveCollectables.Remove(theCollectable);
		Destroy(theCollectable.gameObject);
	}

	public void DestroyAllActiveCollectables()
	{
		for (int i = 0; i < this.allActiveCollectables.Count; i++)
		{
			Collectable col = allActiveCollectables[i];
			allActiveCollectables.Remove(col);
			Destroy(col.gameObject);
		}
	}

	private bool CollideWithObstacleOrSnake(Coord spawnPosition)
	{
		for (int x = 0; x < mapManager.obstacleMap.GetLength(0); x++)
		{
			for (int y = 0; y < mapManager.obstacleMap.GetLength(1); y++)
			{
				if (mapManager.obstacleMap[x, y] == true && (spawnPosition.x == x || spawnPosition.y == y))
				{
					return true;
				}
			}
		}

		for (int i = 0; i < snakeManager.snakeBody.Count; i++)
		{
			if (snakeManager.snakeBody[i].position == spawnPosition)
			{
				return true;
			}
		}

		return false;
	}
}