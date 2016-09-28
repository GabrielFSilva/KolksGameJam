using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectScreenManager : MonoBehaviour 
{
	public List<Button> levelButtons;
	public int unlockedLevel;

    public UITopBarReferences topBarReferences;

	void Start()
	{
		unlockedLevel = PlayerPrefs.GetInt ("UnlockedLevel");
		int __stars = 0;
        int __totalStarCount = 0;
		foreach (Button __button in levelButtons) 
		{
			__button.gameObject.name = "Level_" + (levelButtons.IndexOf (__button) + 1).ToString () + "_Button";
			if (levelButtons.IndexOf (__button) + 1 > unlockedLevel) 
			{
				__button.transition = Selectable.Transition.None;
				__button.transform.GetChild (0).gameObject.SetActive (false);
				__button.transform.GetChild (1).gameObject.SetActive (false);
				__button.transform.GetChild (2).gameObject.SetActive (false);
				__button.transform.GetChild (5).GetComponent<Text> ().color = Color.gray; 
				__button.transform.GetChild (5).GetComponent<Text> ().text = 
					(levelButtons.IndexOf (__button) + 1).ToString ();
				__button.GetComponent<Image> ().color = new Color(0.8f,0.8f,0.8f);
			}
			else 
			{
				__button.transform.GetChild (5).GetComponent<Text> ().text = 
					(levelButtons.IndexOf (__button) + 1).ToString ();
				if (PlayerPrefs.HasKey ("Level_" + (levelButtons.IndexOf (__button) + 1).ToString () + "_Stars")) 
				{
					__stars = PlayerPrefs.GetInt ("Level_" + (levelButtons.IndexOf (__button) + 1).ToString () + "_Stars");
                    __totalStarCount += __stars;
                    if (__stars == 1) 
					{
						__button.transform.GetChild (0).gameObject.SetActive (false);
						__button.transform.GetChild (2).gameObject.SetActive (false);
					} 
					else if (__stars == 2) 
					{
						__button.transform.GetChild (0).GetComponent<RectTransform> ().anchoredPosition = 
							__button.transform.GetChild (3).GetComponent<RectTransform> ().anchoredPosition;
						__button.transform.GetChild (1).GetComponent<RectTransform> ().anchoredPosition = 
							__button.transform.GetChild (4).GetComponent<RectTransform> ().anchoredPosition;
						__button.transform.GetChild (2).gameObject.SetActive (false);
					}
					else if (__stars == 0)
					{
						__button.transform.GetChild (0).gameObject.SetActive (false);
						__button.transform.GetChild (1).gameObject.SetActive (false);
						__button.transform.GetChild (2).gameObject.SetActive (false);
					}
				}
				else
				{
					__button.transform.GetChild (0).gameObject.SetActive (false);
					__button.transform.GetChild (1).gameObject.SetActive (false);
					__button.transform.GetChild (2).gameObject.SetActive (false);
				}
			}
		}
        topBarReferences.UpdateStarCountLabel(__totalStarCount);
        topBarReferences.UpdateCoinCountLabel(PrefsUtil.GetAllCollectedCoins());
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
