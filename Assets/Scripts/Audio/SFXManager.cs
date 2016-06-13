using UnityEngine;
using System.Collections;

public class SFXManager : MonoBehaviour
{
	SnakeManager snakeManager;
	ScoreManager scoreManager;
	SnakeNode head;

	void Start()
	{
		snakeManager = FindObjectOfType<SnakeManager>();
		head = snakeManager.GetSnakeHead();
		head.OnCollectableCollision += Head_OnCollectableCollision;
		head.OnObstacleCollision += Head_OnObstacleCollision;
		//head.OnOutOfBoundsCollision += Head_OnOutOfBoundsCollision;
		head.OnOverloadedObstacleCollision += Head_OnOverloadedObstacleCollision;
		head.OnMovePerformed += Head_OnMovePerformed;

		//snakeManager.OnChargingComplete += SnakeManager_OnChargingComplete;
		//snakeManager.OnOverloadEnd += SnakeManager_OnOverloadEnd;

		scoreManager = FindObjectOfType<ScoreManager>();
		scoreManager.OnScoreMilestoneReach += ScoreManager_OnScoreMilestoneReach;

	}

	private void Head_OnMovePerformed()
	{
		Vector3 headPosition = head.gameObject.transform.position;
		AudioManager.instance.PlaySound("Move", headPosition);
	}

	private void SnakeManager_OnOverloadEnd()
	{
		throw new System.NotImplementedException();
	}

	private void SnakeManager_OnChargingComplete()
	{
		throw new System.NotImplementedException();
	}

	private void ScoreManager_OnScoreMilestoneReach()
	{
		if (scoreManager.Score != 0)
		{
			AudioManager.instance.PlaySound2D("LevelComplete");
		}
	}

	private void Head_OnOverloadedObstacleCollision()
	{
		Vector3 headPosition = head.gameObject.transform.position;
		AudioManager.instance.PlaySound("ObstacleDestruction", headPosition);
	}

	private void Head_OnOutOfBoundsCollision()
	{
		throw new System.NotImplementedException();
	}

	private void Head_OnObstacleCollision()
	{
		Vector3 headPosition = head.gameObject.transform.position;
		AudioManager.instance.PlaySound("ObstacleCollision", headPosition);
	}

	private void Head_OnCollectableCollision(int obj)
	{
		Vector3 headPosition = head.gameObject.transform.position;
		AudioManager.instance.PlaySound("Collect", headPosition);
	}
}
