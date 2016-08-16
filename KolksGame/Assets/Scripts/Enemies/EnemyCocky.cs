using UnityEngine;
using System.Collections;

public class EnemyCocky : Enemy 
{
	void Start()
	{
		EnemySetUp ();
		enemyType = EnemyType.COCKY;
		helloEffect = ActionEffectType.NONE;
		excuseMeEffect = ActionEffectType.NONE;
	}
}
