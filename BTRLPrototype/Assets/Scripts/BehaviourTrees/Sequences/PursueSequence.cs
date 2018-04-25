using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

public class PursueSequence : BTSequence {

    public PursueSequence()
    {
        taskName = "PursueSequence";
        children = new List<BTTask>()
        {
            new GetTarget(),    //get the target to chase
            new MoveTo()        //move to pursue them
        };        
    }
}
