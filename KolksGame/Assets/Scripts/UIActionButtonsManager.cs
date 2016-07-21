using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIActionButtonsManager : MonoBehaviour 
{
	public Button yawnButton;
	public Button helloButton;
	public Button excuseMeButton;

	public Animator yawnButtonAnimator;

	public Text	helloHint;
	public Text	excuseMeHint;

	void Start () 
	{
	
	}
	public void EnableActionButtons(GameSceneManager.ActionsAvailable p_actions)
	{
		if (p_actions < GameSceneManager.ActionsAvailable.YAWN_HELLO) 
		{
			helloButton.gameObject.SetActive (false);
			helloHint.gameObject.SetActive (false);
		}
		if (p_actions < GameSceneManager.ActionsAvailable.YAWN_HELLO_EXCUSE) 
		{
			excuseMeButton.gameObject.SetActive (false);
			excuseMeHint.gameObject.SetActive (false);
		}
		if (Application.platform == RuntimePlatform.Android) 
		{
			excuseMeHint.gameObject.SetActive (false);
			helloHint.gameObject.SetActive (false);
			yawnButton.transform.GetChild (0).GetComponent<Text> ().text = "YAAAWN";
		}
	}
	public void SetYawnButtonGlow()
	{
		yawnButtonAnimator.SetBool ("Blinking", true);
		DisableYawnButtonTransition ();
	}
	public void DisableYawnButtonTransition()
	{
		yawnButton.transition = Selectable.Transition.None;
	}
}
