using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

//do nothing, always succeeds
public class Idle : BTAction
{
    public Idle()
    {
        taskName = "Idle";
        poolable = true;
    }

    //float timer = 0;

    public override StatusValue Tick(Blackboard blackboard)
    {
        return StatusValue.SUCCESS;

        //used to have a timer but it took too long during testing
        /*
        if (Mathf.FloorToInt(timer) % 2 == 0)
        {
            Debug.Log("I am Idle");
        }

        if(timer >= 5.0f)
        {
            timer = 0;
            Debug.Log("success");
            return StatusValue.SUCCESS;
        }

        timer += Time.deltaTime;
        return StatusValue.RUNNING;
        */
    }
}
