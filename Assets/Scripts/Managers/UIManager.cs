using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GooglePlayGames;

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
	public Slider[] volumeSliders;

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

		volumeSliders[0].onValueChanged.AddListener(SetMasterVolume);
		volumeSliders[1].onValueChanged.AddListener(SetMusicVolume);
		volumeSliders[2].onValueChanged.AddListener(SetSfxVolume);

		volumeSliders[0].value = AudioManager.instance.masterVolumePercent;
		volumeSliders[1].value = AudioManager.instance.musicVolumePercent;
		volumeSliders[2].value = AudioManager.instance.sfxVolumePercent;

		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
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

	#region GooglePlayServices
	public void OnLoginBtnPress()
	{
		Social.localUser.Authenticate((bool success)=> {
			if (success)
			{
				Debug.Log("Successfully authenticated!");
			}
			else
			{
				Debug.Log("Error on authentication!");
			}
		});
	}
	#endregion

	#region OptionsMenu
	public void SetMasterVolume(float value)
	{
		AudioManager.instance.PlaySound2D("ButtonClick");
		AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
	}

	public void SetMusicVolume(float value)
	{
		AudioManager.instance.PlaySound2D("ButtonClick");
		AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);

	}

	public void SetSfxVolume(float value)
	{
		AudioManager.instance.PlaySound2D("ButtonClick");
		AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);

	}

	public void OnOptionsBtnPress()
	{
		AudioManager.instance.PlaySound2D("ButtonClick");
		OnPauseBtnPress();
		transitionBlack.gameObject.SetActive(true);
		Color targetColor = new Color(transitionBlack.color.r, transitionBlack.color.g, transitionBlack.color.b, levelManager.fadeMaxOpacity);
		StartCoroutine(levelManager.Fade(transitionBlack, targetColor));

		optionsModal.gameObject.SetActive(true);

	}

	public void OnCloseOptionsBtnPress()
	{
		AudioManager.instance.PlaySound2D("ButtonClick");
		OnNoPress();
	}

	public void OnControlModeChange()
	{
		AudioManager.instance.PlaySound2D("ButtonClick");
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
	#endregion

	public void OnNoPress()
	{
		AudioManager.instance.PlaySound2D("ButtonClick");
		confirmQuit.SetActive(false);
		optionsModal.SetActive(false);
		transitionBlack.gameObject.SetActive(false);
		transitionBlack.color = Color.clear;
		levelManager.EnableControls();
	}

	public void OnYesPress()
	{
		AudioManager.instance.PlaySound2D("ButtonClick");
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
		AudioManager.instance.PlaySound2D("ButtonClick");
		if (overloadBtn.interactable)
		{
			snakeManager.state = SnakeState.Overloaded;
			chargedText.text = "Overloaded!";
			overloadBtn.interactable = false;
		}
	}

	public void OnPauseBtnPress()
	{
		AudioManager.instance.PlaySound2D("ButtonClick");
		levelManager.DisableControls();
		snakeManager.gameState = GameState.Paused;
		playBtn.SetActive(true);
		pauseBtn.SetActive(false);
	}

	public void OnPlayBtnPress()
	{
		AudioManager.instance.PlaySound2D("ButtonClick");
		levelManager.EnableControls();
		snakeManager.gameState = GameState.Playing;
		playBtn.SetActive(false);
		pauseBtn.SetActive(true);
	}
}
