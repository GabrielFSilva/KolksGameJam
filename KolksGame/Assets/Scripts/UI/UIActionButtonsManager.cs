using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIActionButtonsManager : MonoBehaviour 
{
	public Animator yawnButtonAnimator;
	public RectTransform 	yawnButton;
	public RectTransform 	helloButton;
	public RectTransform 	excuseMeButton;

	public RectTransform	yawnButtonRectTransform;
	public RectTransform	yawnSize1;
	public RectTransform	yawnSize2;

	public void EnableActionButtons(GameSceneManager.ActionsAvailable p_actions)
	{
		if (p_actions == GameSceneManager.ActionsAvailable.YAWN_HELLO) 
		{
			excuseMeButton.gameObject.SetActive (false);
			yawnButtonRectTransform.anchoredPosition = yawnSize1.anchoredPosition;
			yawnButtonRectTransform.sizeDelta = yawnSize1.sizeDelta;
			helloButton.anchoredPosition = excuseMeButton.anchoredPosition;
		}
		else if (p_actions == GameSceneManager.ActionsAvailable.YAWN) 
		{
			helloButton.gameObject.SetActive (false);
			excuseMeButton.gameObject.SetActive (false);
			yawnButtonRectTransform.anchoredPosition = yawnSize2.anchoredPosition;
			yawnButtonRectTransform.sizeDelta = yawnSize2.sizeDelta;
		}
		
	}
	public void SetYawnButtonGlow()
	{
		if (!yawnButtonAnimator.GetBool("Blinking")) 
		{
			yawnButtonAnimator.SetBool ("Blinking", true);
			DisableYawnButtonTransition ();
		}
	}
	public void DisableYawnButtonTransition()
	{
		yawnButton.GetComponent<Button>().transition = Selectable.Transition.None;
	}
}
