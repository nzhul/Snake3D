using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ScoreManager : MonoBehaviour {

	private int score = 0;
	public Text scoreText;

	public int nextLevelTreshhold;

	SnakeManager snakeManager;
	public event Action OnScoreMilestoneReach;

	void Start()
	{
		snakeManager = GameObject.FindObjectOfType<SnakeManager>();
		snakeManager.GetSnakeHead();
		SnakeNode head = snakeManager.GetSnakeHead();
		head.OnCollectableCollision += Head_OnCollectableCollision;
		this.Score = 0;
	}

	private void Head_OnCollectableCollision(int lootValue)
	{
		this.Score += lootValue;
	}

	public int Score
	{
		get
		{
			return score;
		}

		set
		{
			this.score = value;
			scoreText.text = score.ToString().PadLeft(7, '0');
			if (this.score % nextLevelTreshhold == 0)
			{
				if (OnScoreMilestoneReach != null)
				{
					OnScoreMilestoneReach();
				}
			}
		}
	}

}