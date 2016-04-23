using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SnakeManager : MonoBehaviour
{
	public List<SnakeNode> snakeBody;
	public int snakeLength = 3;
	public float snakeSpeed = 0;
	public bool IsPaused = true;
	public SnakeNode nodePrefab;
	public Material headMaterial;
	MapManager mapManager;
	SpawnManager spawnManager;

	void Awake()
	{
		spawnManager = GameObject.FindObjectOfType<SpawnManager>();
		mapManager = GameObject.FindObjectOfType<MapManager>();
		snakeBody = new List<SnakeNode>();
		InstantiateSnake();
		InstantiateFirstCollectable();
	}

	private void InstantiateFirstCollectable()
	{
		spawnManager.SpawnCollectable();
	}

	void Update()
	{
		HandlePause();
		if (!IsPaused)
		{
			HandleUserInput();
		}
	}

	private void HandlePause()
	{
		if (IsPaused)
		{
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				snakeBody[0].nextDirection = Direction.Down;
				snakeBody[0].moveSpeed = 2;
				IsPaused = false;
			}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				snakeBody[0].nextDirection = Direction.Up;
				snakeBody[0].moveSpeed = 2;
				IsPaused = false;
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				snakeBody[0].nextDirection = Direction.Right;
				snakeBody[0].moveSpeed = 2;
				IsPaused = false;
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				snakeBody[0].nextDirection = Direction.Left;
				snakeBody[0].moveSpeed = 2;
				IsPaused = false;
			}
		}
	}

	public void ResetSnake()
	{
		SnakeNode oldHead = snakeBody[0];
		for (int i = 1; i < snakeBody.Count; i++)
		{
			Destroy(snakeBody[i].gameObject);
		}

		snakeBody.Clear();
		snakeBody.Add(oldHead);

		snakeBody[0].position = mapManager.GetCurrentMap().mapCenter;
		snakeBody[0].previousPosition = snakeBody[0].position;
		snakeBody[0].transform.position = mapManager.CoordToPosition(snakeBody[0].position) + Vector3.up * .5f;
	}

	private void HandleUserInput()
	{
		foreach (SnakeNode node in snakeBody)
		{
			if (node.prevNode == null)
			{
				// Node is the head because he do not have any previous nodes
				if (Input.GetKeyDown(KeyCode.UpArrow))
				{
					node.nextDirection = Direction.Up;
				}
				if (Input.GetKeyDown(KeyCode.RightArrow))
				{
					node.nextDirection = Direction.Right;
				}
				if (Input.GetKeyDown(KeyCode.DownArrow))
				{
					node.nextDirection = Direction.Down;
				}
				if (Input.GetKeyDown(KeyCode.LeftArrow))
				{
					node.nextDirection = Direction.Left;
				}
			}
			else
			{
				// Node is body part
				node.position = node.prevNode.previousPosition;
			}

			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				node.moveSpeed++;
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				node.moveSpeed--;
			}
		}
	}

	private void InstantiateSnake()
	{
		Map currentMap = mapManager.GetCurrentMap();

		for (int i = 0; i < snakeLength; i++)
		{
			Coord spawnPosition = new Coord(currentMap.mapCenter.x - i, currentMap.mapCenter.y);
			Vector3 nodeSpawnPosition = mapManager.CoordToPosition(spawnPosition);
			SnakeNode spawnedNode = Instantiate(nodePrefab, nodeSpawnPosition + Vector3.up * .65f, Quaternion.identity) as SnakeNode;
			spawnedNode.transform.localScale = Vector3.one * (1 - mapManager.outlinePercent) * mapManager.tileSize * (1 - mapManager.outlinePercent);
			spawnedNode.position = spawnPosition;
			spawnedNode.moveSpeed = snakeSpeed;
			snakeBody.Add(spawnedNode);

			if (i != 0)
			{
				spawnedNode.isHead = false;
				spawnedNode.prevNode = snakeBody[i - 1];
			}

			if (i == 0)
			{
				spawnedNode.isHead = true;
				Renderer rend = spawnedNode.GetComponent<Renderer>();
				rend.sharedMaterial = headMaterial;
			}
		}
	}

	public SnakeNode GetSnakeHead()
	{
		return snakeBody[0];
	}
}