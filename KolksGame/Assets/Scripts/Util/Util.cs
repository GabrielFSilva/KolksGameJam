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
}
