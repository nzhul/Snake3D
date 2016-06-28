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

	public void OnShowLeaderboardBtnPress()
	{
		PlayGamesPlatform.Instance.ShowLeaderboardUI(GooglePlayConstants.leaderboard_highscores);
	}

	public void OnShowAchievementsBtnPress()
	{
		Social.ShowAchievementsUI();
	}
}
