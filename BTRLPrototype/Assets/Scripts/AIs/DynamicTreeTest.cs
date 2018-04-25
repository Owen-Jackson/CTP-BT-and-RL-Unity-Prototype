using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BT_and_RL.Behaviour_Tree;

public class DynamicTreeTest : Actor {
    [SerializeField]
    private BTTree tree;    //this actor's behaviour tree

    [SerializeField]
    private AIPerceptor senses;

    [SerializeField]
    private Text BTTreeStatusText;

    [SerializeField]
    private Text QLearningValuesText;

    [SerializeField]
    private PlayerController player;

    // Use this for initialization
    void Awake () {
        //create a tree with just the attack node
        senses = GetComponentInChildren<AIPerceptor>();
        tree = new BTTree
            (
            new BTSelector(new List<BTTask>()
            {
                new BTSequence(new List<BTTask>()
                {
                    new CheckIfPlayerUnderAttack(),
                    new RLFightOrFlight()
                }),
                new FollowPlayerSequence(),
                new Idle()
            }
            ));

        //setup the blackboard values
        tree.Blackboard = new Blackboard();
        senses = GetComponentInChildren<AIPerceptor>();
        senses.SetBlackboard(tree.Blackboard);
        tree.Blackboard.SetValue("owner", this.gameObject);
        tree.Blackboard.SetValue("senses", senses);
        if(player)
        {
            tree.Blackboard.SetValue("player", player);
        }
        tree.BeginTree();
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.timeScale != 0.0f)
        {
            if (tree.GetStatus() == StatusValue.RUNNING)
            {
                tree.Tick();
            }
            if (tree.GetStatus() != StatusValue.RUNNING)
            {
                tree.BeginTree();
            }
        }
        UpdateDebugText();
    }

    //Displays the layout of the tree and it's q learning values
    void UpdateDebugText()
    {
        string outputString = "test failed";
        if (BTTreeStatusText)
        {
            outputString = tree.BuildDebugString();
            BTTreeStatusText.text = outputString;
        }
        if (QLearningValuesText)
        {
            string text = (string)tree.Blackboard.GetValue("QValueDebugString");
            if (text != "")
            {
                QLearningValuesText.text = text;
            }
        }
    }

    public Blackboard GetBlackboard()
    {
        if(tree.Blackboard != null)
        {
            return tree.Blackboard;
        }
        return null;
    }
}
