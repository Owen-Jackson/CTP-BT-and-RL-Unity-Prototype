using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

//run away from enemies
public class Flee : BTAction {
    GameObject owner;

	public Flee()
    {
        taskName = "Flee";
    }

    public override StatusValue Tick(Blackboard blackboard)
    {
        if(owner == null)
        {
            owner = (GameObject)blackboard.GetValue("owner");
        }

        //get the list of enemies that are nearby
        List<GameObject> runningFrom = (List<GameObject>)blackboard.GetValue("enemies");
        if (runningFrom != null)
        {
            //get the direction to run to, opposite to the average position of the enemies
            Vector3 fleeDir = Vector3.zero;
            foreach (GameObject obj in runningFrom)
            {
                fleeDir += (owner.transform.position - obj.transform.position);
            }
            fleeDir.y = 0;
            fleeDir.Normalize();
            Vector3 posToFleeTo = owner.transform.position + fleeDir * owner.GetComponent<Actor>().MoveSpeed * Time.deltaTime;

            //set the value for the move to action
            blackboard.SetValue("moveToPos", posToFleeTo);
            blackboard.SetValue("distanceToArrival", 0.1f);
            Debug.Log("running");
        }
        else
        {
            return StatusValue.FAILED;
        }

        return StatusValue.SUCCESS;
    }
}
