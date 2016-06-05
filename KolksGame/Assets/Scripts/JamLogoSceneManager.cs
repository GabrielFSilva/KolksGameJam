using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class JamLogoSceneManager : MonoBehaviour 
{
	public Image fade;

	void Start () 
	{
		SoundManager.GetInstance ().PlayBGM ();
		StartCoroutine (Fade (true));
	}
	IEnumerator Fade(bool p_fadeIn)
	{
		if (!p_fadeIn)
			yield return new WaitForSeconds (2.0f);
		else
			yield return new WaitForSeconds (0.5f);

		fade.gameObject.SetActive (fade);
		//fading = true;
		Color __oldColor = fade.color;
		Color __newColor = new Color (0f, 0f, 0f, 0f);

		if (!p_fadeIn) 
			__newColor = new Color (0f, 0f, 0f, 1f);

		float __t = 0f;

		while (__t < 1f) 
		{
			__t += Time.deltaTime * 0.7f;
			fade.color = Color.Lerp (__oldColor, __newColor, __t);
			yield return null;
		}
		fade.gameObject.SetActive (!p_fadeIn);
		if (p_fadeIn)
			StartCoroutine (Fade (false));
		else 
		{
			yield return new WaitForSeconds (1.0f);
			SceneManager.LoadScene ("TitleScreen");
		}
		yield break;
	}
}
