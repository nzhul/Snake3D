using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	SnakeManager snakeManager;
	public GameObject playBtn;
	public GameObject pauseBtn;
	public Button overloadBtn;
	public Text chargedText;

	void Start()
	{
		snakeManager = GameObject.FindObjectOfType<SnakeManager>();
		snakeManager.OnChargingComplete += SnakeManager_OnChargingComplete;
		snakeManager.OnOverloadEnd += SnakeManager_OnOverloadEnd;
		overloadBtn.interactable = false;
		chargedText.gameObject.SetActive(false);
    }

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			this.OnOverloadBtnPress();
		}
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
