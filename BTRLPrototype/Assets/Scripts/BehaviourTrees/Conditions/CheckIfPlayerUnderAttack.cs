using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

public class CheckIfPlayerUnderAttack : BTAction {
    public CheckIfPlayerUnderAttack()
    {
        taskName = "CheckIfPlayerUnderAttack";
    }

    public override StatusValue Tick(Blackboard blackboard)
    {
        PlayerController player = (PlayerController)blackboard.GetValue("player");
        if(player.IsUnderAttack)
        {
            return StatusValue.SUCCESS;
        }
        else
        {
            return StatusValue.FAILED;
        }
    }
}
