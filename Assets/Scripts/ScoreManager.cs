using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ScoreManager : MonoBehaviour {

	private int score = 0;
	private float overloadTime = 0;
	public Text scoreText;
	public Text overloadStatusText;
	public Image overloadFiller;
	public int nextLevelTreshhold;

	private float overloadFillStep;

	SnakeManager snakeManager;
	public event Action OnScoreMilestoneReach;

	void Start()
	{
		snakeManager = GameObject.FindObjectOfType<SnakeManager>();
		snakeManager.GetSnakeHead();
		SnakeNode head = snakeManager.GetSnakeHead();
		head.OnCollectableCollision += Head_OnCollectableCollision;
		this.Score = 0;

		snakeManager.OnOverloadEnter += SnakeManager_OnOverloadEnter;

		float floatValue = snakeManager.overloadTreshhold;
		overloadFillStep = (floatValue / (floatValue * floatValue));
	}

	void Update()
	{
		if (snakeManager.state == SnakeState.Overloaded)
		{
			this.OverloadTime = snakeManager.overloadCooldownLeft;
		}
	}

	private void SnakeManager_OnOverloadEnter()
	{
		this.OverloadTime = snakeManager.overloadCooldownLeft;
	}

	private void Head_OnCollectableCollision(int lootValue)
	{
		this.Score += lootValue;
		this.overloadFiller.fillAmount += overloadFillStep;
	}

	public float OverloadTime
	{
		get
		{
			return overloadTime;
		}
		set
		{
			this.overloadTime = value;
			overloadStatusText.text = "Overload: " + overloadTime;
        }
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