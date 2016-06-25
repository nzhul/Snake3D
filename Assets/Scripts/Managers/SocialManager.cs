using UnityEngine;
using System.Collections;
using GooglePlayGames;

public class SocialManager : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		PlayGamesPlatform.Activate();
		PlayGamesPlatform.DebugLogEnabled = true;
		Social.localUser.Authenticate((bool success) =>
		{
			if (success)
			{
				Debug.Log("Success login");
			}
			else
			{
				Debug.Log("Failed login");
			}
		});
	}

	// Update is called once per frame
	void Update()
	{

	}
}
