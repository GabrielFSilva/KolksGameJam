using UnityEngine;
using System.Collections;

public class EnemyNasty : Enemy
{
    void Start()
    {
        EnemySetUp();
        enemyType = EnemyType.NASTY;
        helloEffect = ActionEffectType.STANDARD;
        excuseMeEffect = ActionEffectType.NONE;
    }
    public override bool GetActionCostEnergy(ActionType p_type)
    {
        if (p_type == ActionType.EXCUSE_ME)
            return false;
        return true;
    }
    
    public override bool GetShowActionDeniedIcon(ActionType p_type)
    {
        if (p_type == ActionType.EXCUSE_ME)
            return true;
        return false;
    }
}
