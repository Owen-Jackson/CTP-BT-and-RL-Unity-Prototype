using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

public class AttackSequence : BTSequence {

    public AttackSequence()
    {
        taskName = "AttackSequence";
        children = new List<BTTask>()
        {
            new GetTarget(),
            new MoveTo(),
            new RLAttack(new List<BTTask>()
            {
                new AirAttack(),
                new WaterAttack(),
                new FireballAttack(), 
                new EarthAttack()
            })               
        };
    }

	public AttackSequence(List<BTTask> tasks) : base (tasks)
    {
        taskName = "Attack Sequence";
    }
}
