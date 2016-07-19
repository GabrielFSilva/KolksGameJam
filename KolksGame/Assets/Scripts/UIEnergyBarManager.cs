using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIEnergyBarManager : MonoBehaviour 
{
	
	public RectTransform fillBar;
	public RectTransform fillBarFull;
	public RectTransform fillBarEmpty;

	public Text	energyLabel;

	public void UpdateEnergyBar(int p_movesAvailable, int p_playerMovCount)
	{
		if (p_movesAvailable == 0)
			fillBar.anchoredPosition = fillBarEmpty.anchoredPosition;
		else
			fillBar.anchoredPosition = Vector2.Lerp (fillBarEmpty.anchoredPosition, 
				fillBarFull.anchoredPosition,
				1f - ((float)p_playerMovCount/(float)p_movesAvailable));
		
		energyLabel.text = (p_movesAvailable-p_playerMovCount).ToString () 
			+ "/" + p_movesAvailable.ToString();
	}
}
