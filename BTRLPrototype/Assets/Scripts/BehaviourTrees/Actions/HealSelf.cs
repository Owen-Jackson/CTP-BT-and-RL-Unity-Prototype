using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

public class HealSelf : BTAction {

    float healRate = 0.5f;          //how many seconds until the next heal tick
    float healPerTick = 5.0f;       //how much health healed per tick
    float healTimer = 0.0f;         //counts so that the heal at the correct rate
    float totalHealTimer = 0.0f;    //how long this action should run for, locks the agent into this action
    float maxTimeForHealing = 2.0f; //limits how long this action lasts

	public HealSelf()
    {
        taskName = "HealSelf";
    }

    public override StatusValue Tick(Blackboard blackboard)
    {
        healTimer += Time.deltaTime;
        totalHealTimer += Time.deltaTime;
        if (healTimer >= healRate)
        {
            //get this agent's class
            GameObject owner = (GameObject)blackboard.GetValue("owner");
            if (owner == null)
            {
                return StatusValue.FAILED;
            }
            owner.GetComponent<Actor>().Heal(healPerTick);
            healTimer = 0.0f;
            Debug.Log("healing");
        }
        if(totalHealTimer >= maxTimeForHealing)
        {
            totalHealTimer = 0.0f;
            return StatusValue.SUCCESS;
        }

        return StatusValue.RUNNING;
    }
}
