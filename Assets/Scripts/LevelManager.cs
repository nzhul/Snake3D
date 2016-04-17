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
		snakeManager.IsPaused = true;
		mapManager.NextMapLevel();
		snakeManager.ResetSnake();
		spawnManager.DestroyAllActiveCollectables();
		spawnManager.SpawnCollectable();

	}

	private void Head_OnOutOfBoundsCollision()
	{
		RestartLevel();
	}

	private void Head_OnObstacleCollision()
	{
		RestartLevel();
	}

	private void RestartLevel()
	{
		SceneManager.LoadScene("Main");
	}
}
