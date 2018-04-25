using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

public class GetPlayerPosition : BTAction {

	public GetPlayerPosition()
    {
        taskName = "GetPlayerPosition";
    }

    public override StatusValue Tick(Blackboard blackboard)
    {
        //retrieve the player's position and set it as the move to position
        PlayerController player = (PlayerController)blackboard.GetValue("player");

        if(player == null)
        {
            return StatusValue.FAILED;
        }
        //set the target position to where the player is, set the arrival distance so that the agent stops when they get near enough
        Vector3 pos = player.transform.position;
        blackboard.SetValue("moveToPos", pos);
        blackboard.SetValue("distanceToArrival", 5.0f);

        return StatusValue.SUCCESS;
    }
}
