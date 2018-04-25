using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;
using BT_and_RL.QLearning;

public class RLFightOrFlight : RLSelector {

    int[] stateArray;   //stores the values associated with each state's condition
    GameObject owner;
    AIPerceptor senses;

    public RLFightOrFlight()
    {
        taskName = "RLFightOrFlight";
        //initialise the state array, in this case the indices represent: this NPC's health percentage, number of enemies attacking, and current enemy health percentage respectively
        stateArray = new int[3] { 0, 0, 0 };
        children = new List<BTTask>()
        {
            new AttackSequence(),
            new FleeSequence(),
            new HealSelf()
        };
    }

    public override void FirstTimeInit(Blackboard blackboard)
    {
        //initialise values for future runs
        if (owner == null)
        {
            owner = (GameObject)blackboard.GetValue("owner");
        }
        if (senses == null)
        {
            senses = (AIPerceptor)blackboard.GetValue("senses");
        }
    }

    public override string GetState(Blackboard blackboard)
    {
        //check for my health
        float healthPercent = owner.GetComponent<Actor>().GetHealthAsPercentage();
        if(healthPercent <= 0.25f)  // <25%
        {
            stateArray[0] = 1;
        }
        else if(healthPercent > 0.25f && healthPercent <= 0.50f) //25% - 50%
        {
            stateArray[0] = 2;
        }
        else if(healthPercent > 0.5f && healthPercent <= 0.75f) //50% - 75%
        {
            stateArray[0] = 3;
        }
        else if(healthPercent > 0.75f)  // >75% 
        {
            stateArray[0] = 4;
        }

        //check how many enemies are nearby
        int numberOfAttackingEnemies = 0;
        
        if(numberOfAttackingEnemies <= 1)
        {
            stateArray[1] = 1;
        }
        else if(numberOfAttackingEnemies == 2)
        {
            stateArray[1] = 2;
        }
        else if(numberOfAttackingEnemies >= 3)
        {
            stateArray[1] = 3;
        }

        //get current enemy's health as percentage
        Enemy currentEnemy = senses.GetCurrentTarget();
        if (currentEnemy != null)
        {
            healthPercent = currentEnemy.GetHealthAsPercentage();
            if (healthPercent <= 0.25f)  // <25%
            {
                stateArray[2] = 1;
            }
            else if (healthPercent > 0.25f && healthPercent <= 0.50f) //25% - 50%
            {
                stateArray[2] = 2;
            }
            else if (healthPercent > 0.5f && healthPercent <= 0.75f) //50% - 75%
            {
                stateArray[2] = 3;
            }
            else if (healthPercent > 0.75f)  // >75% 
            {
                stateArray[2] = 4;
            }
        }
        else
        {
            stateArray[2] = 5;  //no enemy to attack
        }

        //create the final output string. use a dash to differentiate where each state's number starts/ends
        string output = "";
        for(int i = 0; i < stateArray.Length; i++)
        {
            output += stateArray[i] + "-";
        }

        return output;
    }

    //INCOMPLETE
    public override float GetReward(StateClass state, BTTask action)
    {
        float reward = 0.0f;
        //prepare for a veeeeery long switch statement to match several state-action pairs to their rewards
        switch (action.GetName())
        {
            case "AttackSequence":
                //negative rewards for low health and high enemy numbers
                if(stateArray[0] == 1)  //low health
                {
                    reward = -50.0f;
                    if (stateArray[1] == 1)  //one or no enemies
                    {
                        if(stateArray[2] == 1)  //current enemy is low health, get the last hit in
                        {
                            reward = 10.0f;
                        }
                    }
                    if (stateArray[1] == 2)  //some enemies
                    {
                        reward = -75.0f;
                    }
                    if(stateArray[1] == 3)  //lots of enemies
                    {
                        reward = -100.0f;
                    }
                }
                else if (stateArray[0] == 2)    //medium-low health
                {
                    reward = -25.0f;
                    if(stateArray[1] == 1)  //only one enemy, can probably attack
                    {
                        reward = 10.0f;
                        if(stateArray[2] <= 2)  //enemy quite low on health
                        {
                            reward = 30.0f;
                        }
                    }
                    if (stateArray[1] == 2)  //some enemies
                    {
                        reward = -30.0f;
                        if (stateArray[2] == 1)  //current enemy is low health, get the last hit in
                        {
                            reward = 10.0f;
                        }
                    }
                    if (stateArray[1] == 3)  //lots of enemies
                    {
                        reward = -50.0f;
                    }
                }
                else if(stateArray[0] == 3) //medium high health
                {
                    if(stateArray[1] == 1)  //one enemy or fewer
                    {
                        reward = 30.0f;
                    }
                    else if(stateArray[1] == 2)
                    {
                        reward = 25.0f;
                    }
                }
                else if (stateArray[0] == 4) //high or full health
                {
                    reward = 100.0f;    //it is always safe to attack at this health
                }
                if (stateArray[2] == 5)
                {
                    reward = -100.0f;   //no enemy to attack
                }
                break;
            case "FleeSequnece":    //not implemented in time
                break;
            case "HealSelf":
                //healing should only be done when there are few or no enemies nearby and when not at full health
                if(stateArray[1] == 3)  //lots of enemies, don't heal at all
                {
                    reward = -100.0f;
                    
                }
                else if(stateArray[0] == 4) //full health already so don't bother
                {
                    reward = -100.0f; 
                }

                else if (stateArray[1] == 2)    //some enemies, reward depends on current health
                {
                    if(stateArray[0] == 1)
                    {
                        reward = -10.0f;    //negative since they should be trying to flee at this stage
                    }
                    if(stateArray[0] == 2)
                    {
                        reward = 0.0f;
                    }
                    if (stateArray[0] == 3)
                    {
                        reward = 10.0f;
                    }
                }
                else if(stateArray[1] == 1) //1 or no enemies nearby, should be safe to heal
                {
                    if (stateArray[0] == 1)
                    {
                        reward = 60.0f;    //definitely try and heal
                    }
                    if (stateArray[0] == 2)
                    {
                        reward = 30.0f;
                    }
                    if (stateArray[0] == 3)
                    {
                        reward = 10.0f;
                    }
                }
                break;
        }
        return reward;
    }
}
