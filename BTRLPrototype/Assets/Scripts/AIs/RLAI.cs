using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using BT_and_RL.Behaviour_Tree;

public class RLAI : Actor
{
    public BTTree tree;

    //[SerializeField]
    //private int stepsToMinEps = 1000; //number of steps to go from starting epsilon to minEpsilon

    //[SerializeField]
    //private int episodeNum;

    [SerializeField]
    private AIPerceptor senses;

    [SerializeField]
    private List<GameObject> targetPositions;

    [SerializeField]
    private Text BTTreeStatusText;

    [SerializeField]
    private Text QLearningValuesText;

    // Use this for initialization
    void Awake()
    {
        tree.Blackboard = new Blackboard();
        senses = GetComponentInChildren<AIPerceptor>();

        //create the behaviour tree
        tree = new BTTree(
            new BTSelector(new List<BTTask>()
            {
                new Timer(
                    new BTSequence(
                        new List<BTTask>()
                        {
                            new MoveTo(),
                            new SelectNextLocation()
                        }),
                    5f),
                
                new RLAttack(
                    new List<BTTask>()
                    {
                        new FireballAttack(),
                        new WaterAttack(),
                        new AirAttack(),
                        new EarthAttack()
                    })
            }));

        tree.Blackboard.SetValue("owner", this.gameObject);
        tree.Blackboard.SetValue("patrolPoints", GetComponent<Patrol>().GetPatrolPoints());
        tree.Blackboard.SetValue("moveToPos", GetComponent<Patrol>().GetPatrolPoints()[0].transform.position);
        tree.Blackboard.SetValue("distanceToArrival", 0.5f);
        tree.Blackboard.SetValue("senses", senses);
        tree.BeginTree();
    }

    public AIPerceptor GetSenses()
    {
        return senses;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(tree.GetStatus().ToString());
        if (Time.timeScale != 0.0f)
        {
            tree.Blackboard.SetValue("MyHealthAsPercentage", GetHealthAsPercentage());
            if (tree.GetStatus() == StatusValue.RUNNING)
            {
                tree.Tick();
            }
            //restart the tree when all of the nodes have returned
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
            QLearningValuesText.text = (string)tree.Blackboard.GetValue("QValueDebugString");
        }
    }
}

