using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

public class FleeSequence : BTSequence {

    public FleeSequence()
    {
        taskName = "FleeSequence";
        children = new List<BTTask>()
        {
            new Flee(), //calculate the position to run to
            new MoveTo()    //run to the calculated position
        };

    }
	
}