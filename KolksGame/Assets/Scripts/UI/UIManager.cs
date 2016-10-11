using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour 
{
	public UIEndLevelManager 			endLevelPanelManager;
	public UIActionButtonsManager		actionButtonsManager;
	public UIExtraButtonsManager		extraButtonsManager;
	public UIEnergyBarManager			energyBarManager;
	public UICoinLabelManager			coinLabelManager;
	public UIYawnLineManager	yawnLineFeedbackManager;

	public Text		levelNameLabel;

	void Start()
	{
		extraButtonsManager.OnMuteButtonClicked += ExtraButtonsManager_OnMuteButtonClicked;
		levelNameLabel.text = "LEVEL #" + (GameSceneManager.currentLevelIndex + 1).ToString ();
	}
	void ExtraButtonsManager_OnMuteButtonClicked ()
	{
		extraButtonsManager.UpdateMuteButtonSprite ();
	}
}
