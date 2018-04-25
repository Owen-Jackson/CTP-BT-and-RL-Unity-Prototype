using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

//This action selects the next target position for the AI to move towards
public class SelectNextLocation : BTAction {
    int currentIndex = 0;

    public SelectNextLocation()
    {
        taskName = "Select Next Location";
        poolable = true;
    }

    public override StatusValue Tick(Blackboard blackboard)
    {
        List<GameObject> targets = (List<GameObject>)blackboard.GetValue("patrolPoints");
        if (targets != null)
        {
            if (targets.Count == 0)
            {
                return status = StatusValue.FAILED;
            }
            if (currentIndex == targets.Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            blackboard.SetValue("moveToPos", targets[currentIndex].transform.position);
            return status = StatusValue.SUCCESS;
        }
        return status = StatusValue.FAILED;
    }
}
