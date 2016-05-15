using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	MapManager mapManager;
	SnakeManager snakeManager;
	ScoreManager scoreManager;
	SpawnManager spawnManager;

	void Start()
	{
		mapManager = GameObject.FindObjectOfType<MapManager>();
		spawnManager = GameObject.FindObjectOfType<SpawnManager>();

		scoreManager = GameObject.FindObjectOfType<ScoreManager>();
		scoreManager.OnScoreMilestoneReach += ScoreManager_OnScoreMilestoneReach;

		snakeManager = GameObject.FindObjectOfType<SnakeManager>();
		SnakeNode head = snakeManager.GetSnakeHead();
		head.OnObstacleCollision += Head_OnObstacleCollision;
		head.OnOutOfBoundsCollision += Head_OnOutOfBoundsCollision;
	}

	private void ScoreManager_OnScoreMilestoneReach()
	{
		snakeManager.gameState = GameState.Countdown;
		mapManager.NextMapLevel();
		snakeManager.ResetSnake();
		spawnManager.DestroyAllActiveCollectables();
		spawnManager.SpawnCollectable();

	}

	private void Head_OnOutOfBoundsCollision()
	{
		snakeManager.state = SnakeState.Falling;
		snakeManager.gameState = GameState.Transition;
		foreach (SnakeNode node in snakeManager.snakeBody)
		{
			node.GetComponent<Rigidbody>().isKinematic = false;
		}
		//RestartLevel();
	}

	private void Head_OnObstacleCollision()
	{
		snakeManager.state = SnakeState.Crushing;
		snakeManager.gameState = GameState.Transition;
		//RestartLevel();

		foreach (SnakeNode node in snakeManager.snakeBody)
		{
			node.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	private void RestartLevel()
	{
		SceneManager.LoadScene("Main");
	}
}
