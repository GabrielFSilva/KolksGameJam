using UnityEngine;
using System.Collections;

public class PrefsUtil
{
	public static bool GetCoinCollected(int p_levelIndex, int p_coinIndex)
	{
		return PlayerPrefs.HasKey ("Level_" + p_levelIndex.ToString () + "_" + p_coinIndex.ToString ());
	}
	public static void SetCoinCollected(int p_levelIndex, int p_coinIndex)
	{
		PlayerPrefs.SetInt ("Level_" + p_levelIndex.ToString () + "_" + p_coinIndex.ToString (), 1);
	}
}
