using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreenManager : MonoBehaviour 
{
	void Update () 
	{
		GameSceneManager.currentLevelIndex = 0;
		if (Input.anyKeyDown && Time.timeSinceLevelLoad >= 1f)
			SceneManager.LoadScene ("GameScene");
	}
}
