using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIYawnLineFeedbackManager : MonoBehaviour 
{
	public GameObject 	yawnLinePrefab;
	public Transform  	yawnLineContainer;

	public List<SpriteRenderer>		yawnLines = new List<SpriteRenderer> ();
	private List<SpriteRenderer> 	_linesToRemove;

	void Update () 
	{
		_linesToRemove = new List<SpriteRenderer> ();
		//Reduce alpha for each line
		foreach (SpriteRenderer __sr in yawnLines) 
		{
			__sr.color = new Color (1f, 1f, 1f, __sr.color.a - (Time.deltaTime / 0.8f));
			if (__sr.color.a <= 0.05f)
				_linesToRemove.Add (__sr);
		}
		//destroy invisible lines
		foreach (SpriteRenderer __sr in _linesToRemove) 
		{
			yawnLines.Remove (__sr);
			Destroy (__sr.gameObject);
		}
	}

	public void CreateYawnLine(Vector3 p_pos1, Vector3 p_pos2)
	{
		//Create the line in the mid point
		Vector3 __midPoint = (p_pos1 + p_pos2) / 2f;
		GameObject __go = (GameObject)GameObject.Instantiate (yawnLinePrefab);
		__go.transform.parent = yawnLineContainer.transform;
		__go.transform.position = __midPoint;
		yawnLines.Add(__go.GetComponent<SpriteRenderer>());

		//Horizontal line
		if (Mathf.Abs (p_pos1.x - p_pos2.x) > Mathf.Abs (p_pos1.y - p_pos2.y))
			__go.transform.localScale = new Vector3 (2.2f * Mathf.Abs (p_pos1.x - p_pos2.x), 2f, 1f);
		//Vertical line
		else
		{
			__go.transform.localScale = new Vector3 (2.2f * Mathf.Abs (p_pos1.y - p_pos2.y), 2f, 1f);
			__go.transform.localRotation = Quaternion.Euler (0f, 0f, 90f);
		}

	}
}
