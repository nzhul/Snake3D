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
	public float deadScreenMaxDarkness = .80f;
	public RawImage transitionBlack;
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

		buttons = new List<Button>();
		buttons.Add(upBtn);
		buttons.Add(rightBtn);
		buttons.Add(downBtn);
		buttons.Add(leftBtn);
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
		DisableControls();

		snakeManager.state = SnakeState.Falling;
		snakeManager.gameState = GameState.Transition;
		foreach (SnakeNode node in snakeManager.snakeBody)
		{
			node.GetComponent<Rigidbody>().isKinematic = false;
		}

		StartCoroutine(Fade());
	}

	private void Head_OnObstacleCollision()
	{
		DisableControls();

		snakeManager.state = SnakeState.Crushing;
		snakeManager.gameState = GameState.Transition;
		//RestartLevel();

		foreach (SnakeNode node in snakeManager.snakeBody)
		{
			node.GetComponent<Rigidbody>().isKinematic = false;
		}

		StartCoroutine(Fade());
		// After 1-2 seconds delay - StartCoroutine for showing
		// the "You Died!" + "Tip: Try avoiding the obstacles"
	}

	private void DisableControls()
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

	IEnumerator Fade()
	{
		transitionBlack.gameObject.SetActive(true);
		Color from = Color.clear;
		Color to = new Color(0, 0, 0, deadScreenMaxDarkness);
		float time = deadScreenDuration;

		float speed = 1 / time;
		float percent = 0;

		while (percent < 1)
		{
			percent += Time.deltaTime * speed;
			transitionBlack.color = Color.Lerp(from, to, percent);
			yield return null;
		}

		// TODO: Instead of waiting for seconds - show "Tab to continue" label
		yield return new WaitForSeconds(1);

		RestartLevel();
	}

	private void RestartLevel()
	{
		SceneManager.LoadScene("Main");
	}
}
