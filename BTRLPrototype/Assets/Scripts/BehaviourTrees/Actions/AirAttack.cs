using System.Collections;
using System.Collections.Generic;
using BT_and_RL.Behaviour_Tree;
using UnityEngine;

//performs a basic air attack
public class AirAttack : AttackClass {

    public AirAttack()
    {
        baseDamage = 50;
        damageType = "Air";
        taskName = "AirAttack";
        poolable = true;
    }

    public override StatusValue Tick(Blackboard blackboard)
    {
        damageDealt = LaunchAttack(blackboard);
        return status = StatusValue.SUCCESS;
    }
}
