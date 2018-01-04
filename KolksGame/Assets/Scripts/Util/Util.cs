using UnityEngine;
using System.Collections;

public class Util : MonoBehaviour 
{
	public static bool IsOpposeOrientation(Orientation p_ori1, Orientation p_ori2)
	{
		int __orientation = (int) p_ori1 + 2;
		if (__orientation >= 4)
			__orientation -= 4;
		return __orientation == (int)p_ori2 ? true : false;
	}
    public static int GridDistance(TupleInt p_pos1, TupleInt p_pos2)
    {
        return Mathf.Abs(p_pos1.Item1 - p_pos2.Item1) + Mathf.Abs(p_pos1.Item2 - p_pos2.Item2);
    }
}
