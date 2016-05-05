using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SnakeManager : MonoBehaviour
{
	public List<SnakeNode> snakeBody;
	public int snakeLength = 3;
	public float snakeSpeed = 0;
	public float snakeSpeedIncrease = .2f;
	public float currentSpeed = 2;
	public GameState gameState;
	public SnakeNode nodePrefab;
	public Material headMaterial;

	int overloadCurrent = 0;
	public int overloadTreshhold = 10;
	public SnakeState state;
	public float overloadCooldown = 5f;
	public float overloadCooldownLeft = 0f;
	public event Action OnOverloadEnter;

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

	void Start()
	{
		SnakeNode head = this.GetSnakeHead();
		head.OnCollectableCollision += Head_OnCollectableCollision;
	}

	void Update()
	{
		HandlePause();
		if (gameState == GameState.Playing)
		{
			HandleUserInput();

			//HandleSnakeState();
			state = SnakeState.Overloaded;
		}
	}

	private void HandleSnakeState()
	{
		if (state == SnakeState.Overloaded)
		{
			overloadCooldownLeft -= Time.deltaTime;
			if (overloadCooldownLeft <= 0)
			{
				state = SnakeState.Normal;
				overloadCurrent = 0;
			}
		}
	}

	private void Head_OnCollectableCollision(int obj)
	{
		this.ChangeSpeed(snakeSpeedIncrease);

		overloadCurrent++;

		if (overloadCurrent == overloadTreshhold && state != SnakeState.Overloaded)
		{
			state = SnakeState.Overloaded;
			overloadCooldownLeft = overloadCooldown;

			if (OnOverloadEnter != null)
			{
				OnOverloadEnter();
			}
		}
	}

	private void InstantiateFirstCollectable()
	{
		spawnManager.SpawnCollectable();
	}

	private void HandlePause()
	{
		if (gameState == GameState.Paused)
		{
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				snakeBody[0].nextDirection = Direction.Down;
				snakeBody[0].moveSpeed = currentSpeed;
				gameState = GameState.Playing;
			}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				snakeBody[0].nextDirection = Direction.Up;
				snakeBody[0].moveSpeed = currentSpeed;
				gameState = GameState.Playing;
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				snakeBody[0].nextDirection = Direction.Right;
				snakeBody[0].moveSpeed = currentSpeed;
				gameState = GameState.Playing;
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				snakeBody[0].nextDirection = Direction.Left;
				snakeBody[0].moveSpeed = currentSpeed;
				gameState = GameState.Playing;
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

	public void OnLeftBtnPress()
	{
		snakeBody[0].nextDirection = Direction.Left;
		snakeBody[0].moveSpeed = currentSpeed;
		gameState = GameState.Playing;
	}

	public void OnRightBtnPress()
	{
		snakeBody[0].nextDirection = Direction.Right;
		snakeBody[0].moveSpeed = currentSpeed;
		gameState = GameState.Playing;
	}

	public void OnUpBtnPress()
	{
		snakeBody[0].nextDirection = Direction.Up;
		snakeBody[0].moveSpeed = currentSpeed;
		gameState = GameState.Playing;
	}

	public void OnDownBtnPress()
	{
		snakeBody[0].nextDirection = Direction.Down;
		snakeBody[0].moveSpeed = currentSpeed;
		gameState = GameState.Playing;
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

	private void ChangeSpeed(float value)
	{
		foreach (var node in this.snakeBody)
		{
			node.moveSpeed += value;
		}

		currentSpeed = snakeBody[0].moveSpeed;
	}

	public SnakeNode GetSnakeHead()
	{
		return snakeBody[0];
	}
}