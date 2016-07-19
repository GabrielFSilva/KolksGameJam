using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectScreenManager : MonoBehaviour 
{
	public List<Button> levelButtons;
	public Sprite levelButtonOff;
	public Sprite starOn;
	public int unlockedLevel;
	void Start()
	{
		unlockedLevel = PlayerPrefs.GetInt ("UnlockedLevel");
		int __stars = 0;
		foreach (Button __button in levelButtons) 
		{
			if (levelButtons.IndexOf (__button) + 1 > unlockedLevel) {
				__button.GetComponent<Image> ().sprite = levelButtonOff;
				__button.transition = Selectable.Transition.None;
				__button.transform.GetChild (0).gameObject.SetActive (false);
				__button.transform.GetChild (1).gameObject.SetActive (false);
				__button.transform.GetChild (2).gameObject.SetActive (false);
			} 
			else 
			{
				if (PlayerPrefs.HasKey ("Level_" + (levelButtons.IndexOf (__button) + 1).ToString () + "_Stars")) 
				{
					__stars = PlayerPrefs.GetInt ("Level_" + (levelButtons.IndexOf (__button) + 1).ToString () + "_Stars");
					if (__stars >= 1)
						__button.transform.GetChild (0).GetComponent<Image> ().sprite = starOn;
					if (__stars >= 2)
						__button.transform.GetChild (1).GetComponent<Image> ().sprite = starOn;
					if (__stars >= 3)
						__button.transform.GetChild (2).GetComponent<Image> ().sprite = starOn;
				}
			}
		}
		
	}
	public void HomeButtonClicked()
	{
		SoundManager.GetInstance ().PlayClickSFX ();
		SceneManager.LoadScene ("TitleScreen");
	}
	public void LevelButtonClicked(int p_level)
	{
		if (p_level > unlockedLevel)
			return;
		GameSceneManager.currentLevelIndex = p_level - 1;
		SoundManager.GetInstance ().PlayClickSFX ();
		SceneManager.LoadScene ("GameScene");
	}
}
