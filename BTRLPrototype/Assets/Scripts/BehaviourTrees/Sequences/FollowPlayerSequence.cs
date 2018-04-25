using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

public class FollowPlayerSequence : BTSequence {

	public FollowPlayerSequence()
    {
        taskName = "FollowPlayerSequence";

        children = new List<BTTask>()
        {
            new GetPlayerPosition(),
            new MoveTo()
        };
    }
}
