using System.Collections;
using System.Collections.Generic;
using BT_and_RL.Behaviour_Tree;
using UnityEngine;

public class EarthAttack : AttackClass {

    public EarthAttack()
    {
        baseDamage = 50;
        damageType = "Earth";
        taskName = "EarthAttack";
        poolable = true;
    }

    public override StatusValue Tick(Blackboard blackboard)
    {
        damageDealt = LaunchAttack(blackboard);
        return status = StatusValue.SUCCESS;
    }
}
