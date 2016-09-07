using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICoinLabelManager : MonoBehaviour 
{
	public Text				coinLabel;
	public GameObject 		uiCoinPrefab;
	public RectTransform	canvasRect;
	public RectTransform	coinsIconRect;

	public void UpdateCoinLabel(int p_collectedCoins, int p_coinsOnStage)
	{
		coinLabel.text = p_collectedCoins.ToString() + "/" + p_coinsOnStage.ToString ();
	}
	public void CreateUICoin(Coin p_coin)
	{
		//Create UI Coin
		GameObject __go = (GameObject)GameObject.Instantiate (uiCoinPrefab);
		__go.transform.SetParent(canvasRect);
		__go.transform.localScale = Vector3.one;
		__go.name = "UICoin";

		//Move to the UI Canvas
		RectTransform __coinRect = __go.GetComponent<RectTransform>();
		Vector2 __viewportPosition= Camera.main.WorldToViewportPoint(p_coin.transform.position);
		__coinRect.anchoredPosition = new Vector2(
			((__viewportPosition.x*canvasRect.sizeDelta.x)-(canvasRect.sizeDelta.x*0.5f)),
			(__viewportPosition.y*canvasRect.sizeDelta.y));

		//Lerp SetUp
		__go.GetComponent<UICoinIcon>().endPos = coinsIconRect.anchoredPosition;
	}

}
