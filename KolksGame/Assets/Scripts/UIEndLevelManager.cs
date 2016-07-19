using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIEndLevelManager : MonoBehaviour 
{
	public GameObject endLevelPanel;

	public RectTransform starBar;
	public RectTransform starBarFull;
	public RectTransform starBarEmpty;

	public Image star0;
	public Image star1;
	public Image star2;

	public Sprite starOn;
	public Sprite starOff;

	public Button nextButton;
	public Button replay2Button;

	public SoundManager soundManager;
	void Start()
	{
		UpdateStarBarPosition (0f, 0f);
		EnableEndLevelPanel (false);
		nextButton.gameObject.SetActive (false);
		replay2Button.gameObject.SetActive (false);
		soundManager = SoundManager.GetInstance ();
	}
	public void EnableEndLevelPanel(bool p_enable)
	{
		endLevelPanel.gameObject.SetActive (p_enable);
	}
	public void EnableEndLevelButtons(float p_value)
	{
		if (p_value >= 0.5f)
			nextButton.gameObject.SetActive (true);
		else
			replay2Button.gameObject.SetActive (true);
	}
	public void UpdateStarBarPosition(float p_value, float p_limit)
	{
		starBar.anchoredPosition = Vector2.Lerp (starBarEmpty.anchoredPosition, starBarFull.anchoredPosition,
			Mathf.Clamp(p_value,0f,p_limit));
	}
	public void UpdateStarSprites(float p_value)
	{
		if (p_value >= 0.5f && star0.sprite == starOff) 
		{
			star0.sprite = starOn;
			soundManager.PlayEndOfLevelSFX ();
		}
		if (p_value >= 0.75f && star1.sprite == starOff)
		{
			star1.sprite = starOn;
			soundManager.PlayEndOfLevelSFX ();
		}
		if (p_value >= 0.98f && star2.sprite == starOff)
		{
			star2.sprite = starOn;
			soundManager.PlayEndOfLevelSFX ();
		}
	}
}
