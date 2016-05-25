using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	MapManager mapManager;
	SnakeManager snakeManager;
	ScoreManager scoreManager;
	SpawnManager spawnManager;

	public float deadScreenDuration = 2;

	[Range(0, 1)]
	public float fadeMaxOpacity = .80f;
	public RawImage transitionBlack;
	public RawImage transitionWhite;
	public Text levelCompleteText;
	public Text tapToStartNextLevelText;
	public RawImage levelCompleteBG;
	public Button startNextLevelBtn;
	public Button restartBtn;

	public Text youDiedText;
	public Text tapToTryAgainText;

	public Button upBtn;
	public Button leftBtn;
	public Button downBtn;
	public Button rightBtn;
	private List<Button> buttons;

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

		transitionBlack.gameObject.SetActive(false);
		transitionWhite.gameObject.SetActive(false);
		startNextLevelBtn.gameObject.SetActive(false);

		buttons = new List<Button>();
		buttons.Add(upBtn);
		buttons.Add(rightBtn);
		buttons.Add(downBtn);
		buttons.Add(leftBtn);
	}

	private void ScoreManager_OnScoreMilestoneReach()
	{
		snakeManager.gameState = GameState.Transition;
		snakeManager.state = SnakeState.Winning;
		StartCoroutine(WinAnimations());
	}

	private IEnumerator WinAnimations()
	{
		DisableControls();
		Color targetColor = new Color(transitionWhite.color.r, transitionWhite.color.g, transitionWhite.color.b, fadeMaxOpacity);
		StartCoroutine(Fade(transitionWhite, targetColor));
		yield return new WaitForSeconds(1.5f);

		levelCompleteText.gameObject.SetActive(true);
		levelCompleteBG.gameObject.SetActive(true);
		tapToStartNextLevelText.gameObject.SetActive(true);
	}

	private IEnumerator YouDiedAnimations()
	{
		DisableControls();
		Color targetColor = new Color(transitionBlack.color.r, transitionBlack.color.g, transitionBlack.color.b, fadeMaxOpacity);
		StartCoroutine(Fade(transitionBlack, targetColor));
		yield return new WaitForSeconds(1.5f);

		youDiedText.gameObject.SetActive(true);
		levelCompleteBG.gameObject.SetActive(true);
		tapToTryAgainText.gameObject.SetActive(true);
		restartBtn.gameObject.SetActive(true);
	}

	public void OnTapToStartNextLevelButtonClick()
	{
		snakeManager.gameState = GameState.Countdown;
		mapManager.NextMapLevel();
		snakeManager.ResetSnake();
		spawnManager.DestroyAllActiveCollectables();
		spawnManager.SpawnCollectable();
		startNextLevelBtn.gameObject.SetActive(false);

		transitionWhite.color = new Color(transitionWhite.color.r, transitionWhite.color.g, transitionWhite.color.b, 0);
		levelCompleteText.gameObject.SetActive(false);
		levelCompleteBG.gameObject.SetActive(false);
		tapToStartNextLevelText.gameObject.SetActive(false);
	}

	private void Head_OnOutOfBoundsCollision()
	{
		DisableControls();

		snakeManager.state = SnakeState.Falling;
		snakeManager.gameState = GameState.Transition;
		foreach (SnakeNode node in snakeManager.snakeBody)
		{
			node.GetComponent<Rigidbody>().isKinematic = false;
		}

		StartCoroutine(YouDiedAnimations());
	}

	private void Head_OnObstacleCollision()
	{
		DisableControls();

		snakeManager.state = SnakeState.Crushing;
		snakeManager.gameState = GameState.Transition;

		foreach (SnakeNode node in snakeManager.snakeBody)
		{
			node.GetComponent<Rigidbody>().isKinematic = false;
		}

		StartCoroutine(YouDiedAnimations());
	}

	public void DisableControls()
	{
		foreach (Button btn in buttons)
		{
			btn.interactable = false;
		}
	}

	public void EnableControls()
	{
		foreach (Button btn in buttons)
		{
			btn.interactable = true;
		}
	}

	public IEnumerator Fade(RawImage transitionImage, Color targetColor)
	{
		transitionImage.gameObject.SetActive(true);
		Color from = transitionImage.color;
		float time = deadScreenDuration;

		float speed = 1 / time;
		float percent = 0;

		while (percent < 1)
		{
			percent += Time.deltaTime * speed;
			transitionImage.color = Color.Lerp(from, targetColor, percent);
			yield return null;
		}

		// if the target color is white we know that the fade is for winning
		// then on complete we set startnextlevelbutton to be active. 
		if (targetColor.r == 1)
		{
			startNextLevelBtn.gameObject.SetActive(true);
		}
	}

	public void RestartLevel()
	{
		SceneManager.LoadScene("Main");
	}
}
