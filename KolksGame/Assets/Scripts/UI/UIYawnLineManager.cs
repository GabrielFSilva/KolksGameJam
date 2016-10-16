using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIYawnLineManager : MonoBehaviour 
{
	public GameObject 	yawnLinePrefab;
	public Transform  	yawnLineContainer;
    
    public List<YawnLine> yawnLines = new List<YawnLine>();

	public float yawnLineDelay;
	public float lineDuration;
    
	public void CreateYawnLine(Vector3 p_pos1, Vector3 p_pos2, Orientation p_ori, bool p_isPositive)
	{
        if (p_isPositive)
		    StartCoroutine (SpawnLine (p_pos1, p_pos2, p_ori, p_isPositive));
	}
	IEnumerator SpawnLine(Vector3 p_pos1, Vector3 p_pos2, Orientation p_ori, bool p_isPositive)
	{
		yield return new WaitForSeconds (yawnLineDelay);
       
		GameObject __go = Instantiate (yawnLinePrefab);
		__go.transform.parent = yawnLineContainer.transform;
		__go.transform.position = p_pos2;

        YawnLine __line = __go.GetComponent<YawnLine>();
        __line.SetPoints(p_pos2, p_pos1, p_ori);
        __line.OnRemoveYawnLine += RemoveYawnLine;
        yawnLines.Add(__line);

        if (!p_isPositive)
            __line.GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void RemoveYawnLine(YawnLine p_line)
    {
        yawnLines.Remove(p_line);
        Destroy(p_line.gameObject);
    }
}
