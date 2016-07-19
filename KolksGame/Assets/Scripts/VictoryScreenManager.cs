using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class VictoryScreenManager : MonoBehaviour 
{
	public void HomeButtonClicked()
	{
		SoundManager.GetInstance ().PlayClickSFX ();
		SceneManager.LoadScene ("TitleScreen");
	}
}
