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
        List<GameObject> __childs;
		foreach (Button __button in levelButtons) 
		{
            __childs = new List<GameObject>();
            foreach (Transform __trans in __button.transform.parent)
                __childs.Add(__trans.gameObject);
			__button.transform.parent.name = "Level_" + (levelButtons.IndexOf (__button) + 1).ToString () + "_Button";
			if (levelButtons.IndexOf (__button) + 1 > unlockedLevel) 
			{
				__button.transition = Selectable.Transition.None;
                __childs[0].SetActive (false);
                __childs[1].SetActive (false);
                __childs[1].SetActive (false);
                __childs[5].GetComponent<Text> ().color = Color.gray;
                __childs[5].GetComponent<Text> ().text = (levelButtons.IndexOf (__button) + 1).ToString ();
				__button.GetComponent<Image> ().color = new Color(0.8f,0.8f,0.8f);
			}
			else 
			{
				__button.transform.parent.transform.GetChild (5).GetComponent<Text> ().text = 
					(levelButtons.IndexOf (__button) + 1).ToString ();
                //Played the level
                //3 Stars don't change the state (all stars start enabled)
                if (PlayerPrefs.HasKey ("Level_" + (levelButtons.IndexOf (__button) + 1).ToString () + "_Stars")) 
				{
					__stars = PlayerPrefs.GetInt ("Level_" + (levelButtons.IndexOf (__button) + 1).ToString () + "_Stars");
                    __totalStarCount += __stars;
                    //1 Star
                    if (__stars == 1) 
					{
                        __childs[0].SetActive (false);
                        __childs[2].SetActive (false);
					}
                    //2 Stars
                    //Change 0 and 1 position, disable 2
                    else if (__stars == 2) 
					{
                        __childs[0].GetComponent<RectTransform> ().anchoredPosition = __childs[3].GetComponent<RectTransform> ().anchoredPosition;
                        __childs[1].GetComponent<RectTransform> ().anchoredPosition = __childs[4].GetComponent<RectTransform> ().anchoredPosition;
                        __childs[2].SetActive (false);
					}
                    //0 Stars
                    else if (__stars == 0)
					{
                        __childs[0].SetActive (false);
                        __childs[1].SetActive (false);
                        __childs[2].SetActive (false);
					}
				}
                //Didn't player the level
				else
				{
                    __childs[0].SetActive (false);
                    __childs[1].SetActive (false);
                    __childs[2].SetActive (false);
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
