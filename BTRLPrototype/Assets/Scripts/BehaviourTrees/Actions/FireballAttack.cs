using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

public class FireballAttack : AttackClass {

    public FireballAttack()
    {
        baseDamage = 50;
        damageType = "Fire";
        taskName = "FireballAttack";
        poolable = true;
    }

    public override StatusValue Tick(Blackboard blackboard)
    {
        damageDealt = LaunchAttack(blackboard);
        return status = StatusValue.SUCCESS;
    }
}
