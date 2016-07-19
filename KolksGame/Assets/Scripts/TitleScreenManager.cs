using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreenManager : MonoBehaviour 
{
	void Start()
	{
		SoundManager.GetInstance ().PlayBGM ();
	}
	public void PlayButtonClicked()
	{
		//if (!PlayerPrefs.HasKey ("UnlockedLevel"))
		PlayerPrefs.SetInt ("UnlockedLevel", 15);
		
		SoundManager.GetInstance ().PlayClickSFX ();
		GameSceneManager.currentLevelIndex = 0;
		SceneManager.LoadScene ("LevelSelectScene");
	}
}
