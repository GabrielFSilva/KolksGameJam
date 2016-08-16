using UnityEngine;
using System.Collections;

public class EnemyStandard : Enemy 
{
	void Start () 
	{
		EnemySetUp ();
		enemyType = EnemyType.STANDARD;
		helloEffect = ActionEffectType.STANDARD;
		excuseMeEffect = ActionEffectType.STANDARD;
	}
}
