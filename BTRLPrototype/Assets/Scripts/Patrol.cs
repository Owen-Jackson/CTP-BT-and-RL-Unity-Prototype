using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour {

    public List<GameObject> patrolPoints;

    private void Start()
    {
        if(patrolPoints == null)
        {
            patrolPoints = new List<GameObject>();
        }
    }

    public List<GameObject> GetPatrolPoints()
    {
        return patrolPoints;
    }
}
