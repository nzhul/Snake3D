using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	SnakeManager snakeManager;
	public GameObject playBtn;
	public GameObject pauseBtn;

	void Start()
	{
		snakeManager = GameObject.FindObjectOfType<SnakeManager>();
	}

	public void OnPauseBtnPress()
	{
		snakeManager.gameState = GameState.Paused;
		playBtn.SetActive(true);
		pauseBtn.SetActive(false);
	}

	public void OnPlayBtnPress()
	{
		snakeManager.gameState = GameState.Playing;
		playBtn.SetActive(false);
		pauseBtn.SetActive(true);
	}
}
