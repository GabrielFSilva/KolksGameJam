using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;



public class UIExtraButtonsManager : MonoBehaviour 
{
	public event Action OnMuteButtonClicked;

	public Image muteButtonImage;
	public Sprite soundOnSprite;
	public Sprite soundOffSprite;

	public SoundManager soundManager;
	// Use this for initialization
	void Start()
	{
		soundManager = SoundManager.GetInstance ();
	}
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.R))
			RestartLevelButtonClicked ();
	}
	public void RestartLevelButtonClicked()
	{
		soundManager.PlayClickSFX ();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	public void HomeButtonClicked()
	{
		soundManager.PlayClickSFX ();
		SceneManager.LoadScene("LevelSelectScene");
	}
	public void MuteButtonClicked()
	{
		soundManager.PlayClickSFX ();
		AudioListener.volume = 1f - AudioListener.volume;
		OnMuteButtonClicked();
	}
}
