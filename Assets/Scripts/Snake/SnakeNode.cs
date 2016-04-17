using UnityEngine;
using System.Collections;
using System;

public class SnakeNode : MonoBehaviour {

	public SnakeNode prevNode;
	public Coord position;
	public Coord previousPosition;
	private bool isMoving;
	public float moveSpeed = 1f;
	public Direction nextDirection;
	public bool isHead;
	MapManager mapManager;
	SnakeManager snakeManager;
	SpawnManager spawnManager;

	public event Action OnObstacleCollision;
	public event Action OnOutOfBoundsCollision;
	public event Action<int> OnCollectableCollision;

	void Start()
	{
		spawnManager = GameObject.FindObjectOfType<SpawnManager>();
		mapManager = GameObject.FindObjectOfType<MapManager>();
		snakeManager = GameObject.FindObjectOfType<SnakeManager>();
		this.previousPosition = position;
	}

	void Update()
	{
		if (!snakeManager.IsPaused)
		{
			PerformMove();
		}
	}

	public void PerformMove()
	{
		if (!isMoving)
		{
			if (isHead)
			{
				switch (nextDirection)
				{
					case Direction.Up:
						this.previousPosition = position;
						this.position.y++;
						break;
					case Direction.Right:
						this.previousPosition = position;
						this.position.x++;
						break;
					case Direction.Down:
						this.previousPosition = position;
						this.position.y--;
						break;
					case Direction.Left:
						this.previousPosition = position;
						this.position.x--;
						break;
					default:
						break;
				}
			}

			Vector3 newPosition = mapManager.CoordToPosition(this.position);
			StartCoroutine(AnimateMove(transform.position, newPosition, mapManager.GetCurrentMap()));
		}
	}

	public IEnumerator AnimateMove(Vector3 initialPosition, Vector3 targetPosition, Map currentMap)
	{
		isMoving = true;
		this.previousPosition = this.position;
		float percent = 0;

		while (percent < 1)
		{
			percent += Time.deltaTime * moveSpeed;
			transform.position = Vector3.MoveTowards(transform.position, targetPosition + Vector3.up * .5f, percent);

			yield return null;
		}
		// if there is a bug with the visualisation of the new node - put the collision check at the end of the animation(inside of it)
		if (isHead)
		{
			CheckCollectableCollisions();
			CheckObstacleCollisions();
			CheckOutOfBoundsCollision(currentMap);
		}
		isMoving = false;
	}

	private void CheckOutOfBoundsCollision(Map currentMap)
	{
		//Map currentMap = mapManager.GetCurrentMap();
		if (this.position.x < 0 || 
			this.position.x > currentMap.mapWidth - 1 || 
			this.position.y < 0 || 
			this.position.y > currentMap.mapHeight - 1)
		{
			Debug.Log("Out of map!!");
			if (OnOutOfBoundsCollision != null)
			{
				OnOutOfBoundsCollision();
			}
		}
	}

	private void CheckObstacleCollisions()
	{
		for (int col = 0; col < mapManager.obstacleMap.GetLength(0); col++)
		{
			for (int row = 0; row < mapManager.obstacleMap.GetLength(1); row++)
			{
				if (mapManager.obstacleMap[col, row] == true && this.position.x == col && this.position.y == row)
				{
					Debug.Log("Wall Colision!");
					if (OnObstacleCollision != null)
					{
						OnObstacleCollision();
					}
				}
			}
		}

	}

	private void CheckCollectableCollisions()
	{
		for (int i = 0; i < spawnManager.allActiveCollectables.Count; i++)
		{
			Collectable collectable = spawnManager.allActiveCollectables[i];
			if (this.position == collectable.position)
			{
				// TODO: Extract spawning logic in the SpawnManager
				// You may use event subscribe
				Debug.Log("Collectable collision");
				SnakeNode lastNode = snakeManager.snakeBody[snakeManager.snakeBody.Count - 1];
				Coord spawnPosition = new Coord(lastNode.previousPosition.x, lastNode.previousPosition.y);
				Vector3 nodeSpawnPosition = mapManager.CoordToPosition(spawnPosition);
				SnakeNode spawnedNode = Instantiate(snakeManager.nodePrefab, nodeSpawnPosition + Vector3.up * .5f, Quaternion.identity) as SnakeNode;
				spawnedNode.transform.localScale = Vector3.one * (1 - mapManager.outlinePercent) * mapManager.tileSize;
				spawnedNode.position = spawnPosition;
				spawnedNode.moveSpeed = moveSpeed;
				spawnedNode.prevNode = lastNode;
				snakeManager.snakeBody.Add(spawnedNode);

				spawnManager.DestroyCollectableAtPosition(collectable.position);
				spawnManager.SpawnCollectable();

				if (OnCollectableCollision != null)
				{
					OnCollectableCollision(collectable.LootValue);
				}
			}
		}
	}
}

public enum Direction
{
	Up,
	Right,
	Down,
	Left
}
