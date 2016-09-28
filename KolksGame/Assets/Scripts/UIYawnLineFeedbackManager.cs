using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIYawnLineFeedbackManager : MonoBehaviour 
{
	public GameObject 	yawnLinePrefab;
	public Transform  	yawnLineContainer;

	public List<SpriteRenderer>		yawnLines = new List<SpriteRenderer> ();
	private List<SpriteRenderer> 	_linesToRemove;

	public float yawnLineDelay;
	public float lineDuration;

    public int lineRendererOrder = 200;

	void Update () 
	{
        bool __hasPositiveLine = false;
        if (yawnLines.Count == 0)
            return;
		_linesToRemove = new List<SpriteRenderer> ();
		//Reduce alpha for each line
        for (int i = 0; i < yawnLines.Count; i++)
        {
            yawnLines[i].color = new Color ( 1f, yawnLines[i].color.g, 
                yawnLines[i].color.b, yawnLines[i].color.a - (Time.deltaTime / lineDuration));
			if (yawnLines[i].color.a <= 0.05f)
				_linesToRemove.Add (yawnLines[i]);
            if (yawnLines[i].color.r >= 0.9f && yawnLines[i].color.g >= 0.9f)
                __hasPositiveLine = true;
		}
        //Remove red lines if no white ones exist
        if (!__hasPositiveLine)
            foreach (SpriteRenderer __sr in yawnLines)
                if (__sr.color.r >= 0.9f && __sr.color.g <= 0.1f && !_linesToRemove.Contains(__sr))
                    _linesToRemove.Add(__sr);
        //destroy invisible lines
        for (int i = 0; i < _linesToRemove.Count; i++)
        {
			yawnLines.Remove (_linesToRemove[i]);
			Destroy (_linesToRemove[i].gameObject);
		}
        for (int i = 0; i < yawnLines.Count; i++)
            yawnLines[i].sortingOrder = lineRendererOrder;
	}

	public void CreateYawnLine(Vector3 p_pos1, Vector3 p_pos2, bool p_isPositive)
	{
        if (p_isPositive)
		    StartCoroutine (SpawnLine (p_pos1, p_pos2, p_isPositive));
	}
	IEnumerator SpawnLine(Vector3 p_pos1, Vector3 p_pos2, bool p_isPositive)
	{
		yield return new WaitForSeconds (yawnLineDelay);
		//Create the line in the mid point
		Vector3 __midPoint = (p_pos1 + p_pos2) / 2f;
		GameObject __go = (GameObject)GameObject.Instantiate (yawnLinePrefab);
		__go.transform.parent = yawnLineContainer.transform;
		__go.transform.position = __midPoint;
		yawnLines.Add(__go.GetComponent<SpriteRenderer>());

		if (!p_isPositive)
			yawnLines [yawnLines.Count - 1].color = Color.red;
		//Horizontal line
		if (Mathf.Abs (p_pos1.x - p_pos2.x) > Mathf.Abs (p_pos1.y - p_pos2.y)) 
		{
			__go.transform.localScale = new Vector3 (2.2f * Mathf.Abs (p_pos1.x - p_pos2.x), 2f, 1f);
			__go.transform.Translate (Vector3.up * -0.1f);
		}
		//Vertical line
		else
		{
			__go.transform.localScale = new Vector3 (2.2f * Mathf.Abs (p_pos1.y - p_pos2.y), 2f, 1f);
			__go.transform.localRotation = Quaternion.Euler (0f, 0f, 90f);
		}
	}
}
