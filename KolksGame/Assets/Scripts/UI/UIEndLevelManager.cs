using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIEndLevelManager : MonoBehaviour 
{
    public GameObject buttonsPanel;
	public GameObject endLevelPanel;

    public List<Animator> starAnimators;

    public RectTransform starBar;
	public RectTransform starBarFull;
	public RectTransform starBarEmpty;
    public RectTransform success;

    public Image star1;
	public Image star2;
	public Image star3;
	public Color starOffColor;
	public Color starOnColor;

	public Button nextButton;
	public Button replay2Button;
	public RectTransform replayButtonCenterRef;

	public SoundManager soundManager;
    public int activeStarsCount = 0;

	void Start()
	{
		star1.color = starOffColor;
		star2.color = starOffColor;
		star3.color = starOffColor;
		UpdateStarBarPosition (0f, 0f);
		EnableEndLevelPanel (false);
		nextButton.gameObject.SetActive (false);
		replay2Button.gameObject.SetActive (false);
        success.gameObject.SetActive(false);
        soundManager = SoundManager.GetInstance ();
	}
	public void EnableEndLevelPanel(bool p_enable)
	{
		endLevelPanel.gameObject.SetActive (p_enable);
		buttonsPanel.gameObject.SetActive (!p_enable);
	}
	public void EnableEndLevelButtons(float p_value)
	{
		if (p_value >= 0.5f) 
		{
			nextButton.gameObject.SetActive (true);
			replay2Button.gameObject.SetActive (true);
            success.gameObject.SetActive(true);
        }
        else 
		{
			replay2Button.gameObject.SetActive (true);
			replay2Button.GetComponent<RectTransform> ().anchoredPosition = replayButtonCenterRef.anchoredPosition;
		}
	}
	public void UpdateStarBarPosition(float p_value, float p_limit)
	{
		starBar.anchoredPosition = Vector2.Lerp (starBarEmpty.anchoredPosition, starBarFull.anchoredPosition,
			Mathf.Clamp(p_value,0f,p_limit));
		starBar.sizeDelta =  Vector2.Lerp (starBarEmpty.sizeDelta, starBarFull.sizeDelta,
		Mathf.Clamp(p_value,0f,p_limit));
	}
    public void EnableStars(int p_stars)
    {
        if (p_stars > 0)
            star1.color = starOnColor;
        if (p_stars > 1)
            star2.color = starOnColor;
        if (p_stars > 2)
            star3.color = starOnColor;
    }
    private void PlayStarAnimation(int p_starIndex, bool p_firstTime)
    {
        starAnimators[p_starIndex].SetTrigger(p_firstTime ? "Play" : "PlayAgain");
        soundManager.PlayEndOfLevelSFX(p_firstTime ? 1f : 0.75f);
    }
	public void UpdateStarSprites(float p_value)
	{
		if (p_value >= 0.5f && activeStarsCount < 1) 
		{
            activeStarsCount = 1;
            if (star1.color == starOffColor)
            {
                PlayStarAnimation(0, true);
                star1.color = starOnColor;
            }
            else
                PlayStarAnimation(0, false);
        }
        if (p_value >= 0.75f && activeStarsCount < 2)
		{
            activeStarsCount = 2;
            if (star2.color == starOffColor)
            {
                PlayStarAnimation(1, true);
                star2.color = starOnColor;
            }
            else
                PlayStarAnimation(1, false);
        }
		if (p_value >= 0.99f && activeStarsCount < 3)
		{
            activeStarsCount = 3;
            if (star3.color == starOffColor)
            {
                PlayStarAnimation(2, true);
                star3.color = starOnColor;
            }
            else
                PlayStarAnimation(2, false);
        }
	}
}
