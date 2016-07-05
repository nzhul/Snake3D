using GooglePlayGames;
using UnityEngine;
using UnityEngine.UI;

public class SocialManager : MonoBehaviour
{
	ScoreManager scoreManager;
	public Button howToPlayModalBtn;
	public Button noAdsForNowModalBtn;

	void Start()
	{
#if UNITY_ANDROID
		if (LevelManager.restartsCount <= 1)
		{
			PlayGamesPlatform.Activate();
			PlayGamesPlatform.DebugLogEnabled = true;
			Social.localUser.Authenticate((bool success) => { });
		}
#endif

		scoreManager = GameObject.FindObjectOfType<ScoreManager>();
		scoreManager.OnScoreChanged += ScoreManager_OnScoreChanged;
	}

	private void ScoreManager_OnScoreChanged()
	{
		if (scoreManager.Score >= 50)
		{
			// Report score
			Social.ReportScore(scoreManager.Score, GooglePlayConstants.leaderboard_highscores, (bool success) => { });

			// Report achievements
			if (scoreManager.Score >= 100 && scoreManager.Score < 200)
			{
				Social.ReportProgress(GooglePlayConstants.achievement_good_old_times, 100.0f, (bool success) => { });
			}

			if (scoreManager.Score >= 200 && scoreManager.Score < 300)
			{
				Social.ReportProgress(GooglePlayConstants.achievement_im_just_warming_up, 100.0f, (bool success) => { });
			}

			if (scoreManager.Score >= 300 && scoreManager.Score < 400)
			{
				Social.ReportProgress(GooglePlayConstants.achievement_im_still_good_at_this, 100.0f, (bool success) => { });
			}

			if (scoreManager.Score >= 400 && scoreManager.Score < 500)
			{
				Social.ReportProgress(GooglePlayConstants.achievement_fast_and_furious, 100.0f, (bool success) => { });
			}

			if (scoreManager.Score >= 500 && scoreManager.Score < 600)
			{
				Social.ReportProgress(GooglePlayConstants.achievement_breaking_the_sound_barrier, 100.0f, (bool success) => { });
			}

			if (scoreManager.Score >= 600 && scoreManager.Score < 700)
			{
				Social.ReportProgress(GooglePlayConstants.achievement_skill_level_asian_kid, 100.0f, (bool success) => { });
			}

			if (scoreManager.Score >= 700 && scoreManager.Score < 800)
			{
				Social.ReportProgress(GooglePlayConstants.achievement_skill_level_starcraft_pro, 100.0f, (bool success) => { });
			}

			if (scoreManager.Score >= 800 && scoreManager.Score < 900)
			{
				Social.ReportProgress(GooglePlayConstants.achievement_skill_level_asian_starcraft_pro, 100.0f, (bool success) => { });
			}

			if (scoreManager.Score >= 900 && scoreManager.Score < 1000)
			{
				Social.ReportProgress(GooglePlayConstants.achievement_with_the_speed_of_light, 100.0f, (bool success) => { });
			}

			if (scoreManager.Score >= 1000)
			{
				Social.ReportProgress(GooglePlayConstants.achievement_godlike, 100.0f, (bool success) => { });
			}
		}
	}

	public void OnShowLeaderboardBtnPress()
	{
#if UNITY_ANDROID
		PlayGamesPlatform.Instance.ShowLeaderboardUI(GooglePlayConstants.leaderboard_highscores);
#endif
	}

	public void OnShowAchievementsBtnPress()
	{
		Social.ShowAchievementsUI();
	}

	public void OnHowToPlayBtnPress()
	{
		howToPlayModalBtn.gameObject.SetActive(true);
	}

	public void OnHowToPlayModalPress()
	{
		howToPlayModalBtn.gameObject.SetActive(false);
	}

	public void OnRateUsBtnPress()
	{
		Application.OpenURL("market://details?id=com.DidoGames.SnakeOverload");
	}

	public void OnNoAdsForNowModalPress()
	{
		noAdsForNowModalBtn.gameObject.SetActive(false);
	}

	public void OnRemoveAdsBtnPress()
	{
		noAdsForNowModalBtn.gameObject.SetActive(true);
	}

	public void ShareText()
	{
		string subject = "Square Snake";
		string body = "https://play.google.com/store/apps/details?id=com.DidoGames.SnakeOverload&hl=en";

		//execute the below lines if being run on a Android device
#if UNITY_ANDROID
		//Reference of AndroidJavaClass class for intent
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		//Reference of AndroidJavaObject class for intent
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
		//call setAction method of the Intent object created
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		//set the type of sharing that is happening
		intentObject.Call<AndroidJavaObject>("setType", "text/plain");
		//add data to be passed to the other activity i.e., the data to be sent
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
		//get the current activity
		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		//start the activity by sending the intent data
		currentActivity.Call("startActivity", intentObject);
#endif
	}
}