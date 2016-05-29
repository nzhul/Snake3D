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

	// Confirm quit modal
	public GameObject confirmQuit;
	public RawImage transitionBlack;

	// Options Modal
	public GameObject optionsModal;
	public GameObject leftRightControls;
	public GameObject joystickControls;
	public Toggle joystickToggle;
	public Toggle leftRightToggle;

	void Start()
	{
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		snakeManager = GameObject.FindObjectOfType<SnakeManager>();
		snakeManager.OnChargingComplete += SnakeManager_OnChargingComplete;
		snakeManager.OnOverloadEnd += SnakeManager_OnOverloadEnd;
		overloadBtn.interactable = false;
		chargedText.gameObject.SetActive(false);
		transitionBlack.gameObject.SetActive(false);

		SetControlModeTogglesInitialState();
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

	public void OnControlModeChange()
	{
		int controlMode = 1;

		if (joystickToggle.isOn)
		{
			controlMode = 1;
			joystickControls.gameObject.SetActive(true);
			leftRightControls.gameObject.SetActive(false);
		}

		if (leftRightToggle.isOn)
		{
			controlMode = 2;
			joystickControls.gameObject.SetActive(false);
			leftRightControls.gameObject.SetActive(true);
		}

		PlayerPrefs.SetInt("controlModeType", controlMode);
		PlayerPrefs.Save();
	}

	public void SetControlModeTogglesInitialState()
	{
		int controlType = PlayerPrefs.GetInt("controlModeType", 1);
		switch (controlType)
		{
			case 1:
				joystickToggle.isOn = true;
				leftRightToggle.isOn = false;
				break;
			case 2:
				joystickToggle.isOn = false;
				leftRightToggle.isOn = true;
				break;
			default:
				break;
		}
	}

	public void OnNoPress()
	{
		confirmQuit.SetActive(false);
		optionsModal.SetActive(false);
		transitionBlack.gameObject.SetActive(false);
		transitionBlack.color = Color.clear;
		levelManager.EnableControls();
	}

	public void OnYesPress()
	{
		Application.Quit();
	}

	public void OnOptionsBtnPress()
	{
		OnPauseBtnPress();
		transitionBlack.gameObject.SetActive(true);
		Color targetColor = new Color(transitionBlack.color.r, transitionBlack.color.g, transitionBlack.color.b, levelManager.fadeMaxOpacity);
		StartCoroutine(levelManager.Fade(transitionBlack, targetColor));

		optionsModal.gameObject.SetActive(true);

	}

	public void OnCloseOptionsBtnPress()
	{
		OnNoPress();
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
