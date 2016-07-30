using UnityEngine;
using System.Collections;

public class EnemyShy : Enemy 
{
	void Start () 
	{
		enemyType = EnemyType.SHY;
		helloEffect = ActionEffectType.NONE;
		excuseMeEffect = ActionEffectType.STANDARD;
	}
}
