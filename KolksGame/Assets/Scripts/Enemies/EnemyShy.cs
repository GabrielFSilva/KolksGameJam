using UnityEngine;
using System.Collections;

public class EnemyShy : Enemy 
{
	void Start () 
	{
		EnemySetUp ();
		enemyType = EnemyType.SHY;
		helloEffect = ActionEffectType.NONE;
		excuseMeEffect = ActionEffectType.STANDARD;
	}
}
