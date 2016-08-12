using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour 
{
	public UIEndLevelManager 			endLevelPanelManager;
	public UIActionButtonsManager		actionButtonsManager;
	public UIExtraButtonsManager		extraButtonsManager;
	public UIEnergyBarManager			energyBarManager;
	public UICoinLabelManager			coinLabelManager;

	void Start()
	{
		extraButtonsManager.OnMuteButtonClicked += ExtraButtonsManager_OnMuteButtonClicked;
	}
	void ExtraButtonsManager_OnMuteButtonClicked ()
	{
		extraButtonsManager.UpdateMuteButtonSprite ();
	}
}
