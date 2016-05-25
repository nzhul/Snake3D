using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

	LevelManager levelManager;
	SnakeManager snakeManager;
	public GameObject playBtn;
	public GameObject pauseBtn;
	public Button overloadBtn;
	public Text chargedText;

	// Confirm quit
	public GameObject confirmQuit;
	public RawImage transitionBlack;

	void Start()
	{
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		snakeManager = GameObject.FindObjectOfType<SnakeManager>();
		snakeManager.OnChargingComplete += SnakeManager_OnChargingComplete;
		snakeManager.OnOverloadEnd += SnakeManager_OnOverloadEnd;
		overloadBtn.interactable = false;
		chargedText.gameObject.SetActive(false);
		transitionBlack.gameObject.SetActive(false);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			this.OnOverloadBtnPress();
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			OnPauseBtnPress();
			transitionBlack.gameObject.SetActive(true);
			Color targetColor = new Color(transitionBlack.color.r, transitionBlack.color.g, transitionBlack.color.b, levelManager.fadeMaxOpacity);
			StartCoroutine(levelManager.Fade(transitionBlack, targetColor));

			confirmQuit.SetActive(true);
			
		}
	}

	public void OnNoPress()
	{
		confirmQuit.SetActive(false);
		transitionBlack.gameObject.SetActive(false);
		transitionBlack.color = Color.clear;
		levelManager.EnableControls();
	}

	public void OnYesPress()
	{
		Application.Quit();
	}

	private void SnakeManager_OnOverloadEnd()
	{
		chargedText.gameObject.SetActive(false);
	}

	private void SnakeManager_OnChargingComplete()
	{
		//this.OverloadTime = snakeManager.overloadCooldownLeft;
		// TODO: Enable the overload button click event and change his animation
		// Show the "CHARGED!" text
		chargedText.text = "Charged!";
		overloadBtn.interactable = true;
		chargedText.gameObject.SetActive(true);
	}

	public void OnOverloadBtnPress()
	{
		if (overloadBtn.interactable)
		{
			snakeManager.state = SnakeState.Overloaded;
			chargedText.text = "Overloaded!";
			overloadBtn.interactable = false;
		}
	}

	public void OnPauseBtnPress()
	{
		levelManager.DisableControls();
		snakeManager.gameState = GameState.Paused;
		playBtn.SetActive(true);
		pauseBtn.SetActive(false);
	}

	public void OnPlayBtnPress()
	{
		levelManager.EnableControls();
		snakeManager.gameState = GameState.Playing;
		playBtn.SetActive(false);
		pauseBtn.SetActive(true);
	}
}
