using UnityEngine;
using System.Collections;

public class EnemyPolite : Enemy 
{
	void Start () 
	{
		EnemySetUp ();
		enemyType = EnemyType.POLITE;
		helloEffect = ActionEffectType.STANDARD;
		excuseMeEffect = ActionEffectType.GLIDE;
	}
}
