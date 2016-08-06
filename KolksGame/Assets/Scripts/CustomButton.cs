using UnityEngine;
using System;
using System.Collections;
public class CustomButton : MonoBehaviour 
{
	public event Action <CustomButton> onMouseDown;

	void OnMouseDown()
	{
		if (onMouseDown != null)
			onMouseDown (this);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
