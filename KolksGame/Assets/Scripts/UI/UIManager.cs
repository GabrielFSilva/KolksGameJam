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
		levelNameLabel.text = "LEVEL #" + (GameSceneManager.currentLevelIndex + 1).ToString ();
    }

    public void CheckActionButtonsAnimations()
    {
        if (GameSceneManager.currentLevelIndex == 11)
            actionButtonsManager.PlayButtonEntryAnimation(true);
        else if (GameSceneManager.currentLevelIndex == 17)
            actionButtonsManager.PlayButtonEntryAnimation(false);
    }
    public void PlayBarShakeAnimation()
    {
        energyBarManager.PlayShakeAnimation();
    }
}
