using UnityEngine;
using System.Collections;

public class EnemyCocky : Enemy 
{
	void Start () 
	{
		enemyType = EnemyType.COCKY;
		helloEffect = ActionEffectType.NONE;
		excuseMeEffect = ActionEffectType.NONE;
	}
}
