using UnityEngine;
using System.Collections;

public class EnemyStandard : Enemy 
{
	void Start () 
	{
		enemyType = EnemyType.STANDARD;
		helloEffect = ActionEffectType.STANDARD;
		excuseMeEffect = ActionEffectType.STANDARD;
	}
}
