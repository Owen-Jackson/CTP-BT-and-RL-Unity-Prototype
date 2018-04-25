using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;
using BT_and_RL.QLearning;

//This selector uses Reinforcement Learning to reconstruct itself based on what type of enemy the current target is
[System.Serializable]
public class RLAttack : RLSelector {

    AIPerceptor senses;

    public RLAttack()
    {
        taskName = "RLAttack";
        isNodeDynamic = true;
    }

    public RLAttack(List<BTTask> tasks) : base(tasks)
    {
        taskName = "RLAttack";
        isNodeDynamic = true;
    }    

    private bool CheckIfRejected(BTTask task)
    {
        if (learner.GetPreviousState().CheckIfRejected(task.GetName()))
        {
            return true;
        }
        return false;
    }

    public override void FirstTimeInit(Blackboard blackboard)
    {
        senses = (AIPerceptor)blackboard.GetValue("senses");
    }

    public override string GetState(Blackboard blackboard)
    {
        //return the name of the current target as the state
        if(senses.GetCurrentTarget() != null)
        {
            return senses.GetCurrentTarget().GetName();
        }
        return "NO TARGET";
    }

    public override float GetReward(StateClass state, BTTask action)
    {
        if (children[indexOfSelectedAction].GetType().IsSubclassOf(typeof(AttackClass)))
        {
            Debug.Log("attacking");
            return ((AttackClass)children[indexOfSelectedAction]).GetDamageDealt();
        }
        return -1;
    }

    public override bool ShouldActionBeRejected(Blackboard blackboard)
    {
        if (learner.GetPreviousState().GetScore(learner.CurrentActionName) / learner.GetHighScore(learner.PreviousStateName) <= 0.8f)
        {
            return true;
        }
        return false;
    }
}


//OLD CODE
/*
public override StatusValue Tick(Blackboard blackboard)
{
    AIPerceptor senses = (AIPerceptor)blackboard.GetValue("senses");
    GameObject caster = (GameObject)blackboard.GetValue("owner");
    if (senses.GetCurrentTarget() == null)
    {
        if(!senses.SelectEnemy())
        {
            return StatusValue.FAILED;
        }
    }

    //Get which enemy type we are attacking
    CurrentState = senses.GetCurrentTarget().GetName();
    if (!states.ContainsKey(CurrentState))
    {
        states.Add(CurrentState, new StateClass(CurrentState, children, MaxEpsilon));
    }

    //if the list is empty or by random chance add a new action to try
    if (children.Count == 0 || Random.Range(0f, 1f) < states[CurrentState].Epsilon)
    {
        //get a random task from the action pool
        System.Type addType = ActionPool.Instance.GetRandomAction();
        object addInstance = new object();
        //Debug.Log("adding new " + toAdd.Name);
        //check that we haven't rejected it before
        if (!states[CurrentState].CheckIfRejected(addType))
        {
            if (children.Find(t => t.GetType().Name == addType.Name) == null)
            {
                //Debug.Log("GOT IT ADDING NOW: " + ActionPool.Instance.actionPool[newTask].Name);
                addInstance = System.Activator.CreateInstance(ActionPool.Instance.actionPool[addType.Name]);
                children.Add((BTTask)addInstance);
            }

            //check that this new action is not already in the current state's list of possible actions
            if (!states[CurrentState].GetScoresList().ContainsKey(addType.Name))
            {
                states[CurrentState].AddAction(addType.Name);
                Debug.Log("adding new state for " + addType.Name);
            }
        }
    }

    //Select the action to perform
    //Check whether we are using a random action or the max value one
    if (Random.Range(0f, 1f) < states[CurrentState].Epsilon)
    {
        do
        {
            CurrentActionIndex = Random.Range(0, children.Count);
            CurrentActionName = children[CurrentActionIndex].GetType().Name;
            //Debug.Log("random current action: " + CurrentActionName);
            //Debug.Log("random current Action index: " + CurrentActionIndex);
        } while (CheckIfRejected(children[CurrentActionIndex]));
    }
    else
    {
        //Find the best action from the Q value table
        CurrentActionName = states[CurrentState].GetScoresList().Where(x => x.Value == states[CurrentState].GetScoresList().Max(y => y.Value)).Select(z => z.Key).FirstOrDefault(); //max value
        CurrentActionIndex = children.IndexOf(children.Find(x => x.GetType().Name == CurrentActionName));
        //Debug.Log("current best action: " + CurrentActionName);
        //Debug.Log("current Action index: " + CurrentActionIndex);
    }
    //decrease the epsilon value to reduce future probability
    if (states[CurrentState].Epsilon > MinEpsilon)
    {
        states[CurrentState].Epsilon -= (1f-MinEpsilon) / StepsInEpisode;
        //Debug.Log("current epsilon: " + Epsilon);
    }
    //Debug.Log("This time i used: " + CurrentActionName);

    //Perform the action (Tick it)
    //Debug.Log("attacking enemy");
    //Debug.Log("current action index: " + CurrentActionIndex);
    status = children[CurrentActionIndex].Tick(blackboard);
    if (states[CurrentState].EpisodeCount >= 0)
    {
        //increment how many times this action has been used for this state
        /*
        if(blackboard.GetValue(children[CurrentActionIndex].GetType().Name) == null)
        {
            blackboard.SetValue(children[CurrentActionIndex].GetType().Name, 0);
        }
        int temp = (int)blackboard.GetValue(children[CurrentActionIndex].GetType().Name) + 1;
        blackboard.SetValue(children[CurrentActionIndex].GetType().Name, temp);

        if (children[CurrentActionIndex].GetType().IsSubclassOf(typeof(AttackClass)))
        {
            Environment.Instance.AddToDamageDictionary(senses.GetCurrentTarget().GetName(), (children[CurrentActionIndex] as AttackClass).GetElementType());
        }
    }
    if (!senses.GetCurrentTarget().IsAlive())
    {
        senses.RemoveTargetFromList();
    }

    //update the state
    PreviousState = CurrentState;

    //Get the reward
    float reward = 0;
    if (children[CurrentActionIndex].GetType().IsSubclassOf(typeof(AttackClass)))
    {
        if(((AttackClass)children[CurrentActionIndex]).GetCooldown() > 0)
        {
            return StatusValue.FAILED;
        }
        reward = ((AttackClass)children[CurrentActionIndex]).GetDamageDealt();
    }

    //Debug.Log(CurrentActionName + " reward received: " + reward);

    //Update the q value table with the reward value
    //Q-Value of action a in state s = (1 - learningRate) * Q(s, a) + learningRate * observed Q(s,a)
    //observed Q(s, a) = reward(s, a) + discountFactor * max Q(s + 1, a)
    //max Q(s + 1, a) = estimate of optimal future value
    string maxArg = FindBestAction();
    //Debug.Log("best action to take is: " + maxArg);
    float newQ = 0;
    //try
    //{
    newQ = (1 - LearningRate) * states[PreviousState].GetScoresList()[CurrentActionName] + LearningRate * (reward + GammaDiscountFactor * states[CurrentState].GetScoresList()[maxArg]);
    //}
    //catch
    //{
    //Debug.Log("previous state: " + PreviousState);
    //Debug.Log("current state: " + CurrentState);
    //Debug.Log("maxArg: " + maxArg);
    //Debug.Log("action at the time of error: " + CurrentActionName);
    //}
    states[PreviousState].GetScoresList()[CurrentActionName] = newQ;
    if (states[PreviousState].EpisodeCount >= 0)
    {
        if (children[CurrentActionIndex].GetType().IsSubclassOf(typeof(AttackClass)))
        {
            Environment.Instance.SetQValue(states[PreviousState].GetName(), (children[CurrentActionIndex] as AttackClass).GetElementType(), newQ);
        }
    }

    //Debug.Log("Updated scores from state: " + PreviousState);
    states[PreviousState].EpisodeCount--;
    if (states[PreviousState].EpisodeCount <= 0 && !states[PreviousState].DisplayedFinalResults)
    {
        Debug.Log("Completed learning for how to fight a " + PreviousState + "results by attack type: ");
        float maxQ = states[PreviousState].GetScoresList().Max(x => x.Value);
        Debug.Log("max value is: " + maxQ);
        foreach (KeyValuePair<string, float> innerPair in states[PreviousState].GetScoresList())
        {
            Debug.Log(innerPair.Key + " : " + innerPair.Value/maxQ);
        }
        states[PreviousState].DisplayedFinalResults = true;
        /*
        foreach (KeyValuePair<string, StateClass> outerPair in states)
        {
            Debug.Log("My values for state " + outerPair.Key + " are:");
            foreach (KeyValuePair<string, float> innerPair in states[outerPair.Key].GetScoresList())
            {
                Debug.Log(innerPair.Key + " : " + innerPair.Value);
            }
        }

        Debug.Log("rejected pile for " + PreviousState + ":");
        foreach (System.Type type in states[PreviousState].GetRejectedActions())
        {
            Debug.Log(type.Name);
        }
    }

    if(newQ / states[PreviousState].GetScoresList()[maxArg] <= 0.8f && reward <= 0)
    {
        states[PreviousState].RejectAction(children[CurrentActionIndex].GetType());
        Debug.Log("adding " + children[CurrentActionIndex].GetType() + " to the rejected pile");
        //RemoveTask(children[CurrentActionIndex]);
    }

    //Debug.Log("number of actions attached: " + children.Count);
    return status;
}
*/
