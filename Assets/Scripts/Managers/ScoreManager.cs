using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ScoreManager : MonoBehaviour
{
	private int score = 0;
	public Text scoreText;
	public Text speedText;

	private float overloadTime = 0;
	private float overloadFillStep;
	public Image overloadFiller;

	public Text countDownText;

	public int levelTreshhold;
	private int currentLevelTreshhold;

	SnakeManager snakeManager;
	LevelManager levelManager;
	public event Action OnScoreMilestoneReach;
	public event Action OnGo;

	private float cameraShakeDuration = .5f;
	private bool isCameraShaking;

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
		levelManager = FindObjectOfType<LevelManager>();
	}

	private void Head_OnOverloadedObstacleCollision()
	{
		this.Score += 5;
		if (!isCameraShaking)
		{
			StartCoroutine(Shake());
		}
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
				if (OnGo != null)
				{
					OnGo();
				}
			}

			yield return null;
		}

		levelManager.EnableControls();
		countDownText.gameObject.SetActive(false);


		//Time.timeScale = 1;
	}

	IEnumerator Shake()
	{
		isCameraShaking = true;

		float elapsed = 0.0f;

		Vector3 originalCamPos = Camera.main.transform.position;

		while (elapsed < cameraShakeDuration)
		{

			elapsed += Time.deltaTime;

			float percentComplete = elapsed / cameraShakeDuration;
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

			// map value to [-1, 1]
			float x = UnityEngine.Random.value * 2.0f - 1.0f;
			float z = UnityEngine.Random.value * 2.0f - 1.0f;
			x *= .5f * damper;
			z *= .5f * damper;

			x += originalCamPos.x;
			z += originalCamPos.z;

			Camera.main.transform.position = new Vector3(x, originalCamPos.y, z);

			yield return null;
		}

		Camera.main.transform.position = originalCamPos;
		isCameraShaking = false;
	}

	private void Head_OnCollectableCollision(int lootValue)
	{
		this.Score += (int)(lootValue * (snakeManager.currentSpeed / 4));
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
				currentLevelTreshhold += levelTreshhold + ((int)(snakeManager.currentSpeed / 2) * 10);

				if (OnScoreMilestoneReach != null)
				{
					OnScoreMilestoneReach();
				}
			}
		}
	}
}