using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

//TEST CLASS, NOT USED IN BUILD
public class CompanionTest : Actor {
    public List<GameObject> targetPositions;

    [SerializeField]
    private AITree testTree;
    [SerializeField]
    private StatusValue treeStatus;
    //[SerializeField]
    //private float timer = 0;

    // Use this for initialization
    new void Start()
    {
        testTree = new AITree(new BTSequence(new List<BTTask>() { new MoveTo(), new Idle() }));
        if(testTree.Blackboard == null)
        {
            Debug.Log("blackboard not setup");
        }
        testTree.Blackboard.SetValue("patrolPoints", targetPositions);
        testTree.Blackboard.SetValue("owner", gameObject);
        testTree.BeginTree();
    }

    // Update is called once per frame
    void Update()
    {
        treeStatus = testTree.GetStatus();
        if(treeStatus == StatusValue.RUNNING)
        {
            testTree.Tick();
        }
        //timer += Time.deltaTime;
    }
}
