using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICoinLabelManager : MonoBehaviour 
{
	public Text	coinLabel;

	public void UpdateCoinLabel(int p_collectedCoins, int p_coinsOnStage)
	{
		coinLabel.text = p_collectedCoins.ToString() + "/" + p_coinsOnStage.ToString ();
	}
}
