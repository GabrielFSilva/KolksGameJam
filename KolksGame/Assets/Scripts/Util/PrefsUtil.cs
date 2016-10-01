using UnityEngine;
using System.Collections;

public class PrefsUtil
{
	public static bool GetCoinCollected(int p_levelIndex, int p_coinIndex)
	{
		return PlayerPrefs.HasKey ("Level_" + p_levelIndex.ToString () + "_" + p_coinIndex.ToString ());
	}
    public static int GetAllCollectedCoins()
    {
        int __coinCount = 0;
        int __coinIndex = 0;
        //loop for each level
        for(int i = 1; i < 60; i++)
        {
            //loop for each coin
            while (true)
            {
                if (GetCoinCollected(i, __coinIndex))
                {
                    __coinCount++;
                    __coinIndex++;
                }
                else
                    break;
            }
            __coinIndex = 0;
        }
        return __coinCount;
    }
	public static void SetCoinCollected(int p_levelIndex, int p_coinIndex)
	{
		PlayerPrefs.SetInt ("Level_" + p_levelIndex.ToString () + "_" + p_coinIndex.ToString (), 1);
	}
}
