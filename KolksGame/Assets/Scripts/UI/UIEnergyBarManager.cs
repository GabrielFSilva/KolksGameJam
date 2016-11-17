using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIEnergyBarManager : MonoBehaviour 
{
    public Animator         energyBarAnimator;

	public RectTransform    energyBar;
	public RectTransform    energyBarFull;
	public RectTransform    energyBarEmpty;

	public Text	energyCurrentLabel;
	public Text	energyMaxLabel;

	public void UpdateEnergyBar(int p_movesAvailable, int p_playerMovCount)
	{
		float __t = 1f - ((float)p_playerMovCount / (float)p_movesAvailable);
		if (p_movesAvailable == 0) 
		{
			energyBar.anchoredPosition = energyBarEmpty.anchoredPosition;
			energyBar.sizeDelta = energyBarEmpty.sizeDelta;
		}
		else 
		{
			energyBar.anchoredPosition = Vector2.Lerp (energyBarEmpty.anchoredPosition, 
				energyBarFull.anchoredPosition, __t);
			energyBar.sizeDelta = Vector2.Lerp (energyBarEmpty.sizeDelta, 
				energyBarFull.sizeDelta, __t);
		}
		
		energyCurrentLabel.text = (p_movesAvailable-p_playerMovCount).ToString ();
		energyMaxLabel.text = p_movesAvailable.ToString ();
	}

    public void PlayShakeAnimation()
    {
        energyBarAnimator.SetTrigger("Play");
    }
}
