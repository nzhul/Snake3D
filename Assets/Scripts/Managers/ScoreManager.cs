using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ScoreManager : MonoBehaviour {

	private int score = 0;
	public Text scoreText;

	private float overloadTime = 0;
	private float overloadFillStep;
	public Text overloadStatusText;
	public Image overloadFiller;

	public Text countDownText;

	public int levelTreshhold;
	private int currentLevelTreshhold;

	SnakeManager snakeManager;
	public event Action OnScoreMilestoneReach;

	void Start()
	{
		snakeManager = GameObject.FindObjectOfType<SnakeManager>();
		SnakeNode head = snakeManager.GetSnakeHead();
		head.OnCollectableCollision += Head_OnCollectableCollision;
		head.OnOverloadedObstacleCollision += Head_OnOverloadedObstacleCollision;
		this.Score = 0;

		float floatValue = snakeManager.overloadTreshhold;
		overloadFillStep = (floatValue / (floatValue * floatValue));
		currentLevelTreshhold = levelTreshhold;
    }

	private void Head_OnOverloadedObstacleCollision()
	{
		this.Score += 5;
	}

	void Update()
	{

		HandleOverloadFiller();

		if (snakeManager.gameState == GameState.Countdown)
		{
			StartCoroutine(ResumeAfterSeconds(4));
			//snakeManager.gameState = GameState.Paused;
			//this.countDownText.gameObject.SetActive(false);
		}
	}

	private void HandleOverloadFiller()
	{
		if (snakeManager.state == SnakeState.Overloaded)
		{
			float currentFillPercent = snakeManager.overloadCooldownLeft / snakeManager.overloadCooldown;
			this.overloadFiller.fillAmount = currentFillPercent;
			this.OverloadTime = snakeManager.overloadCooldownLeft;
		}
	}

	private IEnumerator ResumeAfterSeconds(int resumetime)
	{
		//Time.timeScale = 0.0001f;
		Camera.main.orthographicSize = 14;

		countDownText.gameObject.SetActive(true);
		float pauseEndTime = Time.realtimeSinceStartup + resumetime; 

		float number3 = Time.realtimeSinceStartup + 1;
		float number2 = Time.realtimeSinceStartup + 2;
		float number1 = Time.realtimeSinceStartup + 3;
		float goText = Time.realtimeSinceStartup + 4;

		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			// Animate mainCamera
			Camera.main.orthographicSize -= .000612f;

			// Change countdown numbers
			if (Time.realtimeSinceStartup <= number3)
			{
				countDownText.text = "3";
			}
			else if (Time.realtimeSinceStartup <= number2)
			{
				countDownText.text = "2";
				snakeManager.gameState = GameState.Paused;
			}
			else if (Time.realtimeSinceStartup <= number1)
			{
				countDownText.text = "1";
			}
			else if (Time.realtimeSinceStartup <= goText)
			{
				countDownText.text = "GO!";
			}

			yield return null;
		}
		countDownText.gameObject.SetActive(false);


		//Time.timeScale = 1;
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
			if (this.score >= currentLevelTreshhold)
			{
				currentLevelTreshhold += levelTreshhold;

				if (OnScoreMilestoneReach != null)
				{
					OnScoreMilestoneReach();
				}
			}
		}
	}

}