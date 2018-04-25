using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

public class WaterAttack : AttackClass {

    public WaterAttack()
    {
        baseDamage = 50;
        damageType = "Water";
        taskName = "WaterAttack";
        poolable = true;
    }

    public override StatusValue Tick(Blackboard blackboard)
    {
        damageDealt = LaunchAttack(blackboard);
        return status = StatusValue.SUCCESS;
    }
}
