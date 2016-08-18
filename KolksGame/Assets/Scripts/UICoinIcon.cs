using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class UICoinIcon : MonoBehaviour 
{
	public event Action<UICoinIcon>	OnMovementEnd;

	public RectTransform	coinTransform;
	public Animator 		coinAnimator;

	public Vector3 			startPos;
	public Vector3 			endPos;
	public float 			timerCount;
	public float 			speed;

	void Start () 
	{
		startPos = coinTransform.anchoredPosition;
		timerCount = -0.5f;
	}

	void FixedUpdate () 
	{
		timerCount += Time.deltaTime * speed;
		coinTransform.anchoredPosition = Vector3.Lerp (startPos, endPos, timerCount);
		if (timerCount >= 1.6f) 
		{
			OnMovementEnd (this);
			gameObject.SetActive (false);
			Destroy(gameObject);
		}
	}
}
