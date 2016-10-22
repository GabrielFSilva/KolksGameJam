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

    public override bool GetActionCostEnergy(ActionType p_type)
    {
        if (p_type == ActionType.HELLO || p_type == ActionType.EXCUSE_ME)
            return false;
        return true;
    }

    public override bool GetShowActionDeniedIcon(ActionType p_type)
    {
        if (p_type == ActionType.HELLO || p_type == ActionType.EXCUSE_ME)
            return true;
        return false;
    }
}
