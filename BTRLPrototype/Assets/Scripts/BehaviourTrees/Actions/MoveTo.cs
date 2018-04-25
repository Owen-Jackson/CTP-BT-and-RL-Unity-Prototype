using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

//used for most of the AI's movement logic
//the other nodes write to the blackboard to tell this node where to make the NPC move to
public class MoveTo : BTAction {
    GameObject owner;

    public MoveTo()
    {
        taskName = "MoveTo";
    }

    public override StatusValue Tick(Blackboard blackboard)
    {
        if(owner == null)
        {
            owner = (GameObject)blackboard.GetValue("owner");
        }
        Vector3 target = (Vector3)blackboard.GetValue("moveToPos");

        //get how far from the target we need to be before we say we have arrived
        float distanceToArrival = (float)blackboard.GetValue("distanceToArrival");

        if (Vector3.Distance(owner.transform.position, target) <= distanceToArrival)
        {
            return status = StatusValue.SUCCESS;
        }
        MoveToPosition(owner, target);

        return status = StatusValue.RUNNING;
    }

    void MoveToPosition(GameObject character, Vector3 pos)
    {
        Vector3 move_dir = Vector3.Normalize(pos - character.transform.position);
        move_dir.y = 0; //prevent from flying up
        character.transform.position += move_dir * character.GetComponent<Actor>().MoveSpeed * Time.deltaTime;
    }
}
