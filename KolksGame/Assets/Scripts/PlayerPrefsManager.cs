using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour 
{
	public static void SetLevelStars(int p_currentLevelIndex, int p_stars)
	{
		int __currentStars = 0;

		if (PlayerPrefs.HasKey("Level_" + (p_currentLevelIndex + 1).ToString () + "_Stars"))
			__currentStars = PlayerPrefs.GetInt ("Level_" + (p_currentLevelIndex + 1).ToString () + "_Stars");

		if (p_stars <= __currentStars)
			return;

		PlayerPrefs.SetInt ("Level_" + (p_currentLevelIndex + 1).ToString () + "_Stars",p_stars);
	}
	public static void SetUnlockedLevel(int p_level)
	{
		int __currentLevel = 0;

		if (PlayerPrefs.HasKey("UnlockedLevel"))
			__currentLevel = PlayerPrefs.GetInt ("UnlockedLevel");

		if (p_level <= __currentLevel)
			return;
		PlayerPrefs.SetInt ("UnlockedLevel",p_level);
	}
}
