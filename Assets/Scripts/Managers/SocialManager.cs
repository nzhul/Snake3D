using GooglePlayGames;
using UnityEngine;

public class SocialManager : MonoBehaviour
{
	ScoreManager scoreManager;

	void Start()
	{
		PlayGamesPlatform.Activate();
		PlayGamesPlatform.DebugLogEnabled = true;
		Social.localUser.Authenticate((bool success) => { });

		scoreManager = GameObject.FindObjectOfType<ScoreManager>();
		scoreManager.OnScoreMilestoneReach += ScoreManager_OnScoreMilestoneReach;
	}

	private void ScoreManager_OnScoreMilestoneReach()
	{
		// Report score
		Social.ReportScore(scoreManager.Score, GooglePlayConstants.leaderboard_highscores, (bool success) => { });

		// Report achievements
		if (scoreManager.Score >= 100 && scoreManager.Score < 200)
		{
			Social.ReportProgress(GooglePlayConstants.achievement_100pt, 100.0f, (bool success) => { });
		}

		if (scoreManager.Score >= 200 && scoreManager.Score < 300)
		{
			Social.ReportProgress(GooglePlayConstants.achievement_200pt, 100.0f, (bool success) => { });
		}

		if (scoreManager.Score >= 300 && scoreManager.Score < 400)
		{
			Social.ReportProgress(GooglePlayConstants.achievement_300pt, 100.0f, (bool success) => { });
		}

		if (scoreManager.Score >= 400 && scoreManager.Score < 500)
		{
			Social.ReportProgress(GooglePlayConstants.achievement_400pt, 100.0f, (bool success) => { });
		}

		if (scoreManager.Score >= 500 && scoreManager.Score < 600)
		{
			Social.ReportProgress(GooglePlayConstants.achievement_500pt, 100.0f, (bool success) => { });
		}

		if (scoreManager.Score >= 600 && scoreManager.Score < 700)
		{
			Social.ReportProgress(GooglePlayConstants.achievement_600pt, 100.0f, (bool success) => { });
		}

		if (scoreManager.Score >= 700 && scoreManager.Score < 800)
		{
			Social.ReportProgress(GooglePlayConstants.achievement_700pt, 100.0f, (bool success) => { });
		}

		if (scoreManager.Score >= 800 && scoreManager.Score < 900)
		{
			Social.ReportProgress(GooglePlayConstants.achievement_800pt, 100.0f, (bool success) => { });
		}

		if (scoreManager.Score >= 900 && scoreManager.Score < 1000)
		{
			Social.ReportProgress(GooglePlayConstants.achievement_900pt, 100.0f, (bool success) => { });
		}

		if (scoreManager.Score >= 1000)
		{
			Social.ReportProgress(GooglePlayConstants.achievement_1000pt, 100.0f, (bool success) => { });
		}
	}

	public void OnShowLeaderboardBtnPress()
	{
		PlayGamesPlatform.Instance.ShowLeaderboardUI(GooglePlayConstants.leaderboard_highscores);
	}

	public void OnShowAchievementsBtnPress()
	{
		Social.ShowAchievementsUI();
	}
}
