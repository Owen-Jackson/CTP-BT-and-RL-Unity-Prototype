using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

//gets the position of the next target enemy
public class GetTarget : BTAction {

	public GetTarget()
    {
        taskName = "GetTarget";
    }

    public override StatusValue Tick(Blackboard blackboard)
    {
        AIPerceptor senses = (AIPerceptor)blackboard.GetValue("senses");

        Enemy target = senses.GetCurrentTarget();
        Vector3 targetPos;
        if (target == null)
        {
            List<GameObject> attackers = (List<GameObject>)blackboard.GetValue("attackingEnemies");
            if (attackers.Count > 0)
            {
                targetPos = attackers[0].transform.position;
            }
            else
            {
                Debug.Log("targetting failed");
                return StatusValue.FAILED;
            }
        }
        else
        {
            targetPos = target.transform.position;
        }
        Debug.Log("getting target enemy to attack");
        blackboard.SetValue("moveToPos", targetPos);
        blackboard.SetValue("distanceToArrival", 2.0f);

        return StatusValue.SUCCESS;
    }
}
